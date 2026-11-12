using Core.NET.DaDataObj;
using Dadata.Model;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    internal sealed class AddressLookupDialog : XtraForm
    {
        private readonly Func<string, CancellationToken, Task<SuggestResponse<Address>>> findAddresses;
        private readonly Func<string, CancellationToken, Task<Address>> normalizeAddress;
        private readonly TextEdit query = new TextEdit { Dock = DockStyle.Fill, AccessibleName = "Российский адрес" };
        private readonly SimpleButton find = new SimpleButton { Text = "Найти", AutoSize = true };
        private readonly SimpleButton standardize = new SimpleButton { Text = "Стандартизировать (платно)", AutoSize = true };
        private readonly SimpleButton apply = new SimpleButton { Text = "Применить адрес", AutoSize = true, Enabled = false };
        private readonly ListBoxControl results = new ListBoxControl
        {
            Dock = DockStyle.Fill,
            HorizontalScrollbar = true,
            AccessibleName = "Найденные адреса"
        };
        private readonly LabelControl status = new LabelControl { Dock = DockStyle.Fill, AutoSizeMode = LabelAutoSizeMode.None };
        private CancellationTokenSource request;
        private bool closing;

        public Address SelectedAddress { get; private set; }

        internal AddressLookupDialog(string initialQuery,
            Func<string, CancellationToken, Task<SuggestResponse<Address>>> findAddresses = null,
            Func<string, CancellationToken, Task<Address>> normalizeAddress = null)
        {
            this.findAddresses = findAddresses ?? DaDataObj.SuggestAddressesAsync;
            this.normalizeAddress = normalizeAddress ?? DaDataObj.GetAddressAsync;
            Text = "Поиск адреса в DaData";
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(690, 345);
            MinimumSize = new Size(530, 320);

            query.Properties.MaxLength = 400;
            query.Text = initialQuery ?? "";
            status.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
            status.Text = "Введите адрес и выберите подходящий вариант из списка.";

            var close = new SimpleButton { Text = "Отмена", AutoSize = true, DialogResult = DialogResult.Cancel };
            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            actions.Controls.Add(find);
            actions.Controls.Add(standardize);
            var confirmation = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.RightToLeft
            };
            confirmation.Controls.Add(close);
            confirmation.Controls.Add(apply);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 6
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new LabelControl { Text = "Российский адрес", Margin = new Padding(3, 0, 3, 4) }, 0, 0);
            layout.Controls.Add(query, 0, 1);
            layout.Controls.Add(actions, 0, 2);
            layout.Controls.Add(results, 0, 3);
            layout.Controls.Add(status, 0, 4);
            layout.Controls.Add(confirmation, 0, 5);
            Controls.Add(layout);

            find.Click += async (_, __) => await SearchAsync(standardization: false);
            standardize.Click += async (_, __) => await SearchAsync(standardization: true);
            apply.Click += (_, __) => ApplySelection();
            results.SelectedIndexChanged += (_, __) => UpdateApply();
            query.TextChanged += (_, __) =>
            {
                results.Items.Clear();
                UpdateApply();
            };
            AcceptButton = find;
            CancelButton = close;
            ActiveControl = query;
        }

        internal static Address ShowFor(IWin32Window owner, string initialQuery = null)
        {
            using (var dialog = new AddressLookupDialog(initialQuery))
                return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.SelectedAddress : null;
        }

        private async Task SearchAsync(bool standardization)
        {
            if (request != null || closing) return;
            var text = query.Text.Trim();
            var limit = standardization ? 400 : 300;
            if (text.Length < 3 || text.Length > limit)
            {
                status.Text = $"Введите адрес от 3 до {limit} символов.";
                return;
            }

            using (var current = new CancellationTokenSource())
            {
                request = current;
                results.Items.Clear();
                SetBusy(true);
                status.Text = standardization ? "Стандартизирую адрес…" : "Ищу варианты адреса…";
                try
                {
                    if (standardization)
                    {
                        var address = await normalizeAddress(text, current.Token);
                        if (IsAvailable(current.Token) && DaDataObj.IsRussianAddress(address) && address.qc == "0")
                            results.Items.Add(new AddressOption(address, address.result ?? text));
                    }
                    else
                    {
                        var response = await findAddresses(text, current.Token);
                        if (IsAvailable(current.Token) && response?.suggestions != null)
                        {
                            foreach (var suggestion in response.suggestions)
                            {
                                var address = suggestion?.data;
                                var label = string.IsNullOrWhiteSpace(suggestion?.unrestricted_value)
                                    ? suggestion?.value : suggestion.unrestricted_value;
                                if (DaDataObj.IsRussianAddress(address) && !string.IsNullOrWhiteSpace(label))
                                    results.Items.Add(new AddressOption(address, label));
                            }
                        }
                    }
                    if (!IsAvailable(current.Token)) return;
                    results.SelectedIndex = -1;
                    status.Text = results.Items.Count > 0 ? "Выберите адрес и нажмите «Применить адрес»."
                        : "Подходящих адресов нет. Уточните запрос или заполните поля вручную.";
                }
                catch (OperationCanceledException) when (current.IsCancellationRequested) { }
                catch (InvalidOperationException error)
                {
                    if (IsAvailable(current.Token)) status.Text = error.Message;
                }
                finally
                {
                    request = null;
                    if (IsAvailable(current.Token)) SetBusy(false);
                }
            }
        }

        private bool IsAvailable(CancellationToken cancellationToken)
            => !closing && !IsDisposed && !Disposing && !cancellationToken.IsCancellationRequested;

        private void SetBusy(bool busy)
        {
            query.Enabled = find.Enabled = standardize.Enabled = results.Enabled = !busy;
            UpdateApply();
        }

        private void UpdateApply()
        {
            apply.Enabled = request == null && results.SelectedItem is AddressOption;
            AcceptButton = apply.Enabled ? apply : find;
        }

        private void ApplySelection()
        {
            if (closing || request != null || !(results.SelectedItem is AddressOption option)) return;
            SelectedAddress = option.Address;
            if (string.IsNullOrWhiteSpace(SelectedAddress.result)) SelectedAddress.result = option.Label;
            DialogResult = DialogResult.OK;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.Cancel) return;
            closing = true;
            request?.Cancel();
        }

        private sealed class AddressOption
        {
            internal AddressOption(Address address, string label)
            {
                Address = address;
                Label = label;
            }

            internal Address Address { get; }
            internal string Label { get; }
            public override string ToString() => Label;
        }
    }
}
