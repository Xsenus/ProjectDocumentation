using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectDocumentation.NET.Services
{
    internal sealed class ConclusionSchemaService : IDisposable
    {
        private const string WorkerArguments = "--stdio";
        private const string WorkerExecutable = "ProjectDocumentation.Schema.exe";
        private const string BrowserPortalAddress = "http://127.0.0.1:8765";
        private static readonly Encoding Utf8 = new UTF8Encoding(false);
        private readonly SemaphoreSlim requests = new SemaphoreSlim(1, 1);
        private readonly object engineLock = new object();
        private Process engine;
        private StreamWriter input;
        private bool disposed;

        private static string BundledEngineDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "engine");

        public static bool HasBundledPortal => File.Exists(Path.Combine(BundledEngineDirectory, WorkerExecutable))
            && File.Exists(Path.Combine(BundledEngineDirectory, "_internal", "portal", "dist", "index.html"));

        public async Task<string> StartBrowserPortalAsync()
        {
            var result = await RequestAsync<JObject>("start-portal", new JObject());
            if (result.Value<string>("url") != BrowserPortalAddress)
                throw new InvalidOperationException("Не удалось определить адрес локального веб-редактора.");
            return BrowserPortalAddress;
        }

        public Task<JObject> GetSchemaAsync(string family, string version = null)
            => RequestAsync<JObject>("schema", new JObject { ["family"] = family, ["version"] = version });

        public Task<string> CreateAsync(string family = null) => CreateAsync(family, null);

        public async Task<string> CreateAsync(string family, string version)
        {
            var document = await RequestAsync<JObject>("new", new JObject { ["family"] = family, ["version"] = version });
            return document.Value<string>("xml");
        }

        public Task<ConclusionValidation> ValidateAsync(string xml)
            => RequestAsync<ConclusionValidation>("validate", new JObject { ["xml"] = xml });

        public Task<ConclusionMigration> MigrateAsync(string xml, string targetVersion)
            => RequestAsync<ConclusionMigration>("migrate", new JObject { ["xml"] = xml, ["targetVersion"] = targetVersion });

        public async Task<string> NormalizeLegacyConclusionAsync(string xml)
        {
            var document = await RequestAsync<JObject>("normalize-legacy-conclusion", new JObject { ["xml"] = xml });
            return document.Value<string>("xml") ?? throw new InvalidOperationException("Не удалось подготовить заключение 01.03.");
        }

        public async Task<string> PreviewAsync(string xml)
        {
            var preview = await RequestAsync<JObject>("preview", new JObject { ["xml"] = xml, ["layout"] = "rich-edit" });
            return preview.Value<string>("html");
        }

        public async Task<byte[]> GetSchemaAssetAsync(string filename)
        {
            var asset = await RequestAsync<JObject>("asset", new JObject { ["name"] = filename });
            return Convert.FromBase64String(asset.Value<string>("base64"));
        }

        private async Task<T> RequestAsync<T>(string operation, JObject request) where T : class
        {
            await requests.WaitAsync().ConfigureAwait(false);
            try
            {
                var process = EnsureWorker(out var writer);
                request["operation"] = operation;
                var response = await ReceiveAsync(process, writer, request.ToString(Formatting.None)).ConfigureAwait(false);
                if (response.Value<string>("error") is string error)
                {
                    throw new InvalidOperationException(error);
                }

                return response["result"]?.ToObject<T>()
                    ?? throw new InvalidOperationException("Комплектный валидатор XML вернул пустой ответ.");
            }
            finally
            {
                requests.Release();
            }
        }

        private async Task<JObject> ReceiveAsync(Process process, StreamWriter writer, string request)
        {
            try
            {
                using (var timer = new CancellationTokenSource())
                {
                    var transfer = ExchangeAsync(process, writer, request);
                    var timeout = Task.Delay(TimeSpan.FromSeconds(60), timer.Token);
                    if (await Task.WhenAny(transfer, timeout).ConfigureAwait(false) != transfer)
                    {
                        StopWorker(process);
                        _ = transfer.ContinueWith(task => { _ = task.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
                        throw new TimeoutException("Комплектный валидатор XML не ответил за 60 секунд.");
                    }

                    timer.Cancel();
                    return await transfer.ConfigureAwait(false);
                }
            }
            catch (Exception error) when (error is IOException || error is JsonException)
            {
                StopWorker(process);
                throw new InvalidOperationException("Комплектный валидатор XML завершился или вернул повреждённый ответ. Повторите действие.", error);
            }
        }

        private static async Task<JObject> ExchangeAsync(Process process, StreamWriter writer, string request)
        {
            await writer.WriteLineAsync(request).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
            var line = await process.StandardOutput.ReadLineAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new EndOfStreamException();
            }

            return JObject.Parse(line);
        }

        private Process EnsureWorker(out StreamWriter writer)
        {
            lock (engineLock)
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(ConclusionSchemaService));
                }

                if (engine == null || engine.HasExited)
                {
                    StopWorker();
                    var process = new Process { StartInfo = GetStartInfo() };
                    process.ErrorDataReceived += (_, __) => { };
                    engine = process;
                    try
                    {
                        process.Start();
                        process.BeginErrorReadLine();
                        input = new StreamWriter(process.StandardInput.BaseStream, Utf8, 4096, leaveOpen: true);
                    }
                    catch
                    {
                        StopWorker(process);
                        throw;
                    }
                }

                writer = input;
                return engine;
            }
        }

        private static ProcessStartInfo GetStartInfo()
        {
            var directory = BundledEngineDirectory;
            var executable = Path.Combine(directory, WorkerExecutable);
            var arguments = WorkerArguments;
            if (!File.Exists(executable))
            {
                directory = AppDomain.CurrentDomain.BaseDirectory;
                executable = Path.Combine(directory, WorkerExecutable);
            }
            if (!File.Exists(executable))
            {
                var source = FindSource(directory);
                if (source == null)
                {
                    throw new FileNotFoundException("В комплекте установки отсутствует ProjectDocumentation.Schema.exe.");
                }

                directory = source;
                var pythonOverride = Environment.GetEnvironmentVariable("PROJECTDOCUMENTATION_SCHEMA_PYTHON");
                var localPython = Path.Combine(directory, ".venv", "Scripts", "python.exe");
                executable = !string.IsNullOrWhiteSpace(pythonOverride) ? pythonOverride
                    : File.Exists(localPython) ? localPython : "python.exe";
                arguments = "-m schema_engine " + WorkerArguments;
            }

            return new ProcessStartInfo
            {
                FileName = executable,
                WorkingDirectory = directory,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Utf8
            };
        }

        private static string FindSource(string directory)
        {
            for (var parent = new DirectoryInfo(directory); parent != null; parent = parent.Parent)
            {
                if (File.Exists(Path.Combine(parent.FullName, "ProjectDocumentation.sln"))
                    && File.Exists(Path.Combine(parent.FullName, "schema_engine", "__main__.py")))
                {
                    return parent.FullName;
                }
            }

            return null;
        }

        private void StopWorker(Process expected = null)
        {
            lock (engineLock)
            {
                var process = engine;
                if (process == null || (expected != null && process != expected))
                {
                    return;
                }

                engine = null;
                input = null;
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch (InvalidOperationException) { }
                catch (System.ComponentModel.Win32Exception) { }
                finally
                {
                    process.Dispose();
                }
            }
        }

        public void Dispose()
        {
            lock (engineLock)
            {
                disposed = true;
                StopWorker();
            }
        }
    }

    internal sealed class ConclusionValidation
    {
        public bool Valid { get; set; }
        public string Version { get; set; }
        public string ValidationBasis { get; set; }
        public List<ConclusionValidationError> Errors { get; set; }
    }

    internal sealed class ConclusionValidationError
    {
        public string Path { get; set; }
        public string Message { get; set; }
    }

    internal sealed class ConclusionMigration
    {
        public string Xml { get; set; }
        public List<string> Warnings { get; set; }
    }
}
