using Core.NET.Models.XPO;
using DevExpress.XtraEditors;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectDocumentation.NET.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    internal sealed class Conclusion0104Form : XtraForm
    {
        private readonly ConclusionSchemaService service;
        private readonly Func<string, string, Task<ConclusionXPO>> save;
        private readonly string initialXml;
        private readonly string initialName;
        private readonly string section;
        private readonly IReadOnlyList<string> warnings;
        private readonly WebView2 browser = new WebView2 { Dock = DockStyle.Fill, Visible = false };
        private readonly Label loading = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
            Text = "Загрузка редактора заключения…" };

        private Uri address;
        private bool ready;
        private bool dirty;
        private bool saving;
        private bool lookingUp;

        public ConclusionXPO Document { get; private set; }

        public Conclusion0104Form(ConclusionSchemaService service, string xml, string name,
            Func<string, string, Task<ConclusionXPO>> save = null, string section = null, IReadOnlyList<string> warnings = null)
        {
            this.service = service;
            initialXml = xml;
            initialName = name;
            this.save = save;
            this.section = section;
            this.warnings = warnings;
            Text = save == null ? "Просмотр заключения экспертизы 01.04" : "Заключение экспертизы 01.04";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(760, 520);
            ClientSize = new Size(1200, 800);
            Controls.Add(browser);
            Controls.Add(loading);
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                address = await service.GetAddressAsync();
                if (IsDisposed || Disposing)
                {
                    return;
                }

                var profile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ProjectDocumentation", "ConclusionEditor");
                var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: profile);
                if (IsDisposed || Disposing)
                {
                    return;
                }

                await browser.EnsureCoreWebView2Async(environment);
                if (IsDisposed || Disposing)
                {
                    return;
                }

                var web = browser.CoreWebView2;
                web.Settings.AreDevToolsEnabled = false;
                web.Settings.AreHostObjectsAllowed = false;
                web.Settings.IsPasswordAutosaveEnabled = false;
                web.Settings.IsGeneralAutofillEnabled = false;
                web.NewWindowRequested += (_, request) => request.Handled = true;
                web.PermissionRequested += (_, request) => request.State = CoreWebView2PermissionState.Deny;
                web.NavigationStarting += (_, navigation) => navigation.Cancel = !IsEditorPage(navigation.Uri);
                web.NavigationCompleted += (_, navigation) =>
                {
                    if (!navigation.IsSuccess && navigation.WebErrorStatus != CoreWebView2WebErrorStatus.OperationCanceled)
                    {
                        loading.Text = "Не удалось загрузить редактор заключения. Проверьте комплект установки.";
                    }
                };
                web.WebMessageReceived += HandleMessage;
                web.Navigate(address.AbsoluteUri);
            }
            catch (Exception error)
            {
                if (!IsDisposed && !Disposing)
                {
                    loading.Text = "Не удалось запустить редактор заключения." + Environment.NewLine + error.Message;
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && DialogResult != DialogResult.OK)
            {
                e.Cancel = saving || (dirty && XtraMessageBox.Show(this, "Закрыть редактор и отбросить несохранённые изменения?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes);
            }

            base.OnFormClosing(e);
        }

        private bool IsEditorPage(string source)
        {
            return address != null && Uri.TryCreate(source, UriKind.Absolute, out var uri)
                && uri.Scheme == address.Scheme && uri.Host == address.Host && uri.Port == address.Port
                && uri.AbsolutePath == "/" && uri.UserInfo.Length == 0 && uri.Query.Length == 0;
        }

        private async void HandleMessage(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (!IsEditorPage(e.Source) || IsDisposed || Disposing)
            {
                return;
            }

            JObject message;
            try
            {
                if (e.WebMessageAsJson.Length > Conclusion0104Document.MaxXmlCharacters * 6L + 65536)
                {
                    return;
                }

                var token = JToken.Parse(e.WebMessageAsJson);
                message = token.Type == JTokenType.String ? JObject.Parse(token.Value<string>()) : token as JObject;
            }
            catch (JsonException)
            {
                return;
            }

            if (message?["type"]?.Type != JTokenType.String)
            {
                return;
            }

            switch (message.Value<string>("type"))
            {
                case "ready":
                    if (!ready)
                    {
                        ready = true;
                        Send(new { type = "load", xml = initialXml, name = initialName, readOnly = save == null, section, warnings });
                        loading.Visible = false;
                        browser.Visible = true;
                    }
                    break;

                case "dirty":
                    if (ready && save != null && message["dirty"]?.Type == JTokenType.Boolean)
                    {
                        dirty = message.Value<bool>("dirty");
                    }
                    break;

                case "lookup-organization":
                    await HandleLookupAsync(message, true);
                    break;

                case "normalize-address":
                    await HandleLookupAsync(message, false);
                    break;

                case "save":
                    if (!ready || save == null || saving || message["xml"]?.Type != JTokenType.String || message["name"]?.Type != JTokenType.String)
                    {
                        return;
                    }

                    saving = true;
                    try
                    {
                        Document = await save(message.Value<string>("xml"), message.Value<string>("name"));
                        if (!IsDisposed && !Disposing)
                        {
                            dirty = false;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                    catch (Exception error)
                    {
                        if (!IsDisposed && !Disposing)
                        {
                            Send(new { type = "error", message = error.Message });
                        }
                    }
                    finally
                    {
                        saving = false;
                    }
                    break;
            }
        }

        private async Task HandleLookupAsync(JObject message, bool organization)
        {
            if (!ready || save == null || message["requestId"]?.Type != JTokenType.String)
            {
                return;
            }

            var requestId = message.Value<string>("requestId");
            if (string.IsNullOrWhiteSpace(requestId) || requestId.Length > 128)
            {
                return;
            }

            var field = organization ? "inn" : "query";
            var value = message[field]?.Type == JTokenType.String ? message.Value<string>(field)?.Trim() : null;
            if (organization ? !ConclusionDaData.IsDigits(value, 10) : (value == null || value.Length < 3 || value.Length > 400))
            {
                SendLookup(requestId, null, organization ? "Введите ИНН юридического лица из 10 цифр." : "Введите российский адрес от 3 до 400 символов.");
                return;
            }

            if (lookingUp || saving)
            {
                SendLookup(requestId, null, "Дождитесь завершения текущего действия.");
                return;
            }

            lookingUp = true;
            try
            {
                var fields = organization ? await ConclusionDaData.FindOrganizationAsync(value) : await ConclusionDaData.NormalizeAddressAsync(value);
                var error = fields != null ? null : organization ? "Организация с этим ИНН не найдена." : "DaData не удалось однозначно распознать российский адрес.";
                SendLookup(requestId, fields, error);
            }
            catch (Exception)
            {
                SendLookup(requestId, null, "DaData не ответила. Проверьте подключение или повторите запрос.");
            }
            finally
            {
                lookingUp = false;
            }
        }

        private void SendLookup(string requestId, Dictionary<string, string> fields, string error)
        {
            if (!IsDisposed && !Disposing)
            {
                Send(new { type = "lookup-result", requestId, fields = fields ?? new Dictionary<string, string>(), error });
            }
        }

        private void Send(object payload)
        {
            browser.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(payload));
        }
    }
}
