using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectDocumentation.NET.Services
{
    internal sealed class ConclusionSchemaService : IDisposable
    {
        private const string ServerArguments = "--host 127.0.0.1 --port 0";
        private readonly SemaphoreSlim startup = new SemaphoreSlim(1, 1);
        private readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseProxy = false,
            UseCookies = false
        }) { Timeout = TimeSpan.FromSeconds(45) };

        private Process engine;
        private Uri address;
        private bool disposed;

        public async Task<Uri> GetAddressAsync()
        {
            await startup.WaitAsync();
            try
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(ConclusionSchemaService));
                }

                if (engine != null && !engine.HasExited && address != null)
                {
                    return address;
                }

                StopEngine();
                var process = new Process { StartInfo = GetStartInfo() };
                process.ErrorDataReceived += (_, __) => { };
                engine = process;
                try
                {
                    process.Start();
                    process.BeginErrorReadLine();
                    var read = process.StandardOutput.ReadLineAsync();
                    if (await Task.WhenAny(read, Task.Delay(TimeSpan.FromSeconds(30))) != read)
                    {
                        throw new InvalidOperationException("Служба проверки XML не запустилась за 30 секунд.");
                    }

                    var line = await read;
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        throw new InvalidOperationException("Служба проверки XML завершилась при запуске. Проверьте комплект установки.");
                    }

                    var url = JObject.Parse(line).Value<string>("url");
                    if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttp
                        || uri.Host != "127.0.0.1" || uri.IsDefaultPort || uri.AbsolutePath != "/"
                        || uri.UserInfo.Length != 0 || uri.Query.Length != 0 || uri.Fragment.Length != 0)
                    {
                        throw new InvalidOperationException("Служба проверки XML вернула недопустимый локальный адрес.");
                    }

                    if (disposed)
                    {
                        throw new ObjectDisposedException(nameof(ConclusionSchemaService));
                    }

                    address = uri;
                    return uri;
                }
                catch
                {
                    StopEngine();
                    throw;
                }
            }
            finally
            {
                startup.Release();
            }
        }

        public async Task<string> CreateAsync()
        {
            var document = await RequestAsync<JObject>("api/new", null);
            return document.Value<string>("xml");
        }

        public Task<ConclusionValidation> ValidateAsync(string xml)
        {
            return RequestAsync<ConclusionValidation>("api/validate", new JObject { ["xml"] = xml });
        }

        public Task<ConclusionMigration> MigrateAsync(string xml, string targetVersion)
        {
            return RequestAsync<ConclusionMigration>("api/migrate", new JObject
            {
                ["xml"] = xml,
                ["targetVersion"] = targetVersion
            });
        }

        public async Task<byte[]> GetSchemaAssetAsync(string filename)
        {
            var origin = await GetAddressAsync();
            return await client.GetByteArrayAsync(new Uri(origin, "schemas/" + Uri.EscapeDataString(filename)));
        }

        private async Task<T> RequestAsync<T>(string route, JObject body)
        {
            var origin = await GetAddressAsync();
            using (var request = new HttpRequestMessage(body == null ? HttpMethod.Get : HttpMethod.Post, new Uri(origin, route)))
            {
                if (body != null)
                {
                    request.Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json");
                }

                using (var response = await client.SendAsync(request))
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        string detail = null;
                        try { detail = JObject.Parse(payload).Value<string>("error"); }
                        catch (JsonException) { }
                        catch (InvalidCastException) { }
                        throw new InvalidOperationException(detail ?? $"Служба проверки XML вернула HTTP {(int)response.StatusCode}.");
                    }

                    var result = JsonConvert.DeserializeObject<T>(payload);
                    if (result == null)
                    {
                        throw new InvalidOperationException("Служба проверки XML вернула пустой ответ.");
                    }

                    return result;
                }
            }
        }

        private static ProcessStartInfo GetStartInfo()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var executable = Path.Combine(directory, "ProjectDocumentation.Schema.exe");
            var arguments = ServerArguments;
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
                arguments = "-m schema_engine " + ServerArguments;
            }

            return new ProcessStartInfo
            {
                FileName = executable,
                WorkingDirectory = directory,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8
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

        private void StopEngine()
        {
            address = null;
            var process = engine;
            engine = null;
            if (process == null)
            {
                return;
            }

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

        public void Dispose()
        {
            disposed = true;
            StopEngine();
            client.Dispose();
        }
    }

    internal sealed class ConclusionValidation
    {
        public bool Valid { get; set; }
        public string Version { get; set; }
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
