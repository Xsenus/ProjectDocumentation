using ProjectDocumentation.NET.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using XtraInputBox = DevExpress.XtraEditors.XtraInputBox;
using XtraMessageBox = DevExpress.XtraEditors.XtraMessageBox;

namespace ProjectDocumentation.NET.Forms
{
    internal partial class SchemaDocumentForm
    {
        private readonly TreeView xmlTree = new TreeView { Dock = DockStyle.Fill, HideSelection = false, ShowNodeToolTips = true };
        private readonly Panel details = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10, 0, 10, 10) };
        private readonly Dictionary<string, Control> fieldEditors = new Dictionary<string, Control>();
        private ComboBox childSelector;

        private Control CreateFieldsPanel()
        {
            var split = new SplitContainer { Dock = DockStyle.Fill, Size = new Size(1000, 530), FixedPanel = FixedPanel.Panel1 };
            split.Panel1MinSize = 210;
            split.Panel2MinSize = 350;
            split.SplitterDistance = 325;
            split.Panel1.Padding = new Padding(8);
            split.Panel2.Padding = new Padding(8);
            xmlTree.BeforeExpand += (_, e) => PopulateTree(e.Node);
            xmlTree.AfterSelect += (_, e) => ShowFields(e.Node);
            split.Panel1.Controls.Add(xmlTree);
            split.Panel2.Controls.Add(details);
            return split;
        }

        private void BuildTree(string section = null)
        {
            xmlTree.BeginUpdate();
            try
            {
                xmlTree.Nodes.Clear();
                var root = TreeNodeFor(profile.RootNode(document.Root));
                xmlTree.Nodes.Add(root);
                root.Expand();
                xmlTree.SelectedNode = section == null ? root
                    : root.Nodes.Cast<TreeNode>().FirstOrDefault(item => (item.Tag as NativeSchemaNode)?.Element.Name.LocalName == section) ?? root;
                xmlTree.SelectedNode.EnsureVisible();
                if (section != null && xmlTree.SelectedNode == root) SelectChild(section);
            }
            finally
            {
                xmlTree.EndUpdate();
            }
        }

        private TreeNode TreeNodeFor(NativeSchemaNode node)
        {
            var result = new TreeNode(TreeLabel(node))
            {
                Tag = node,
                ToolTipText = XmlPath(node.Element) + Environment.NewLine + (node.Field?.Documentation ?? node.Type?.Documentation)
            };
            if (node.Element.HasElements) result.Nodes.Add(new TreeNode());
            return result;
        }

        private static string TreeLabel(NativeSchemaNode node)
        {
            var element = node.Element;
            if (element.HasElements) return element.Name.LocalName;
            var value = element.Value;
            if (value.Length == 0) return element.Name.LocalName;
            if (value.Length > 64) value = value.Substring(0, 61) + "…";
            value = value.Trim().Replace('\n', ' ').Replace('\r', ' ');
            return $"{element.Name.LocalName} · {value}";
        }

        private void PopulateTree(TreeNode item)
        {
            if (item.Tag is NativeSchemaNode node && item.Nodes.Count == 1 && item.Nodes[0].Tag == null)
            {
                item.Nodes.Clear();
                foreach (var element in node.Element.Elements()) item.Nodes.Add(TreeNodeFor(profile.ChildNode(node, element)));
            }
        }

        private void RebuildBranch(TreeNode item, XElement selected = null)
        {
            var node = (NativeSchemaNode)item.Tag;
            item.Nodes.Clear();
            foreach (var element in node.Element.Elements()) item.Nodes.Add(TreeNodeFor(profile.ChildNode(node, element)));
            item.Text = TreeLabel(node);
            item.Expand();
            xmlTree.SelectedNode = item.Nodes.Cast<TreeNode>().FirstOrDefault(n => ReferenceEquals(((NativeSchemaNode)n.Tag).Element, selected)) ?? item;
            xmlTree.SelectedNode.EnsureVisible();
            ShowFields(xmlTree.SelectedNode);
        }

        private void ShowFields(TreeNode item)
        {
            if (!(item?.Tag is NativeSchemaNode node)) return;
            details.SuspendLayout();
            try
            {
                while (details.Controls.Count > 0) details.Controls[0].Dispose();
                fieldEditors.Clear();
                childSelector = null;
                var layout = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 182));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                details.Controls.Add(layout);
                AddWide(layout, LabelFor(XmlPath(node.Element)));
                var documentation = node.Field?.Documentation;
                if (string.IsNullOrWhiteSpace(documentation)) documentation = node.Type?.Documentation;
                if (!string.IsNullOrWhiteSpace(documentation)) AddWide(layout, LabelFor(documentation));

                var attributes = node.Type?.Attributes ?? new List<NativeSchemaAttribute>();
                foreach (var attribute in attributes)
                {
                    var current = (string)node.Element.Attribute(attribute.Name);
                    var immutableId = node.Element == document.Root && attribute.Name == idAttribute;
                    var locked = ReadOnlyDocument || immutableId || attribute.Fixed != null;
                    var editor = ValueEditor(current, profile.TypeOf(attribute.Type), locked, false, value =>
                    {
                        node.Element.SetAttributeValue(attribute.Name, value.Length == 0 ? null : value);
                        MarkChanged();
                    });
                    fieldEditors["@" + attribute.Name] = editor;
                    AddField(layout, "@" + attribute.Name + (attribute.Required ? " *" : ""), editor, attribute.Documentation);
                }

                if (node.Type?.Particle == null && !node.Element.HasElements)
                {
                    var type = node.Type?.TextType == null ? node.Type : profile.TypeOf(node.Type.TextType);
                    var current = node.Element.Value;
                    var binary = type?.Base == "base64Binary" && current.Length > 8192;
                    var multiline = type?.Base == "string" && (current.Length > 100 || node.Element.Name.LocalName == "Text");
                    var editor = ValueEditor(binary ? $"Base64 · {current.Length:N0} символов" : current, type,
                        ReadOnlyDocument || binary || node.Field?.Fixed != null, multiline, value =>
                        {
                            SetText(node.Element, value);
                            item.Text = TreeLabel(node);
                            MarkChanged();
                        });
                    fieldEditors["."] = editor;
                    AddField(layout, "Значение", editor, null);
                }

                if (!ReadOnlyDocument)
                {
                    AddChildControls(layout, item, node);
                    AddFileAndLookupControls(layout, item, node);
                    if (item.Parent != null) AddTreeCommands(layout, item, node.Element);
                }
            }
            finally
            {
                details.ResumeLayout(true);
            }
        }

        private static Label LabelFor(string text)
            => new Label { Text = text, AutoSize = true, MaximumSize = new Size(690, 0), Margin = new Padding(3, 6, 3, 6) };

        private static void AddWide(TableLayoutPanel layout, Control control)
        {
            var row = layout.RowCount++;
            layout.Controls.Add(control, 0, row);
            layout.SetColumnSpan(control, 2);
        }

        private static void AddField(TableLayoutPanel layout, string label, Control editor, string documentation)
        {
            var row = layout.RowCount++;
            layout.Controls.Add(LabelFor(label), 0, row);
            layout.Controls.Add(editor, 1, row);
            if (!string.IsNullOrWhiteSpace(documentation))
            {
                layout.Controls.Add(LabelFor(documentation), 1, layout.RowCount++);
            }
        }

        private static Control ValueEditor(string value, NativeSchemaType type, bool readOnly, bool multiline, Action<string> changed)
        {
            value = value ?? "";
            if (!readOnly && type?.Enumeration?.Count > 0)
            {
                var options = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDown, DropDownWidth = 550 };
                options.Items.AddRange(type.Enumeration.Cast<object>().ToArray());
                var match = type.Enumeration.FirstOrDefault(option => option.Value == value);
                if (match != null) options.SelectedItem = match;
                else options.Text = value;
                options.SelectedIndexChanged += (_, __) => { if (options.SelectedItem is NativeSchemaOption option) changed(option.Value); };
                options.TextUpdate += (_, __) => changed(options.Text);
                return options;
            }

            var text = new TextBox
            {
                Dock = DockStyle.Top, Text = value, ReadOnly = readOnly, Multiline = multiline,
                MaxLength = SchemaXmlDocument.MaxXmlBytes
            };
            if (multiline)
            {
                text.ScrollBars = ScrollBars.Vertical;
                text.Height = 95;
            }
            text.TextChanged += (_, __) => { if (!readOnly) changed(text.Text); };
            return text;
        }

        private void AddChildControls(TableLayoutPanel layout, TreeNode item, NativeSchemaNode node)
        {
            var candidates = node.Children.GroupBy(child => child.Field.Name).Select(group => group.First())
                .Where(child => node.Element.Elements(child.Field.Name).LongCount() < child.Capacity).ToArray();
            if (candidates.Length == 0) return;
            childSelector = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, DropDownWidth = 650, MaxDropDownItems = 18 };
            childSelector.Items.AddRange(candidates.Cast<object>().ToArray());
            childSelector.SelectedItem = candidates.FirstOrDefault(child => child.Required && node.Element.Element(child.Field.Name) == null) ?? candidates[0];
            AddField(layout, "Вложенное поле", childSelector, null);
            var add = ActionButton("Добавить выбранное поле");
            add.Click += (_, __) =>
            {
                if (!(childSelector.SelectedItem is NativeSchemaChild child) || !CanEditFields()) return;
                var element = node.AddChild(child);
                foreach (var attribute in profile.TypeOf(child.Field.Type)?.Attributes ?? new List<NativeSchemaAttribute>())
                    if (attribute.Fixed != null) element.SetAttributeValue(attribute.Name, attribute.Fixed);
                MarkChanged();
                RebuildBranch(item, element);
                if (fieldEditors.TryGetValue(".", out var editor)) editor.Focus();
            };
            AddWide(layout, Toolbar(add));
        }

        private void AddTreeCommands(TableLayoutPanel layout, TreeNode item, XElement element)
        {
            var up = ActionButton("Вверх");
            var down = ActionButton("Вниз");
            var remove = ActionButton("Удалить поле");
            up.Enabled = element.ElementsBeforeSelf().Any();
            down.Enabled = element.ElementsAfterSelf().Any();
            up.Click += (_, __) => MoveField(item, true);
            down.Click += (_, __) => MoveField(item, false);
            remove.Click += (_, __) =>
            {
                if (!CanEditFields()) return;
                if (XtraMessageBox.Show(this, $"Удалить поле {element.Name.LocalName} и его содержимое?", Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                var parent = item.Parent;
                element.Remove();
                MarkChanged();
                RebuildBranch(parent);
            };
            AddWide(layout, Toolbar(up, down, remove));
        }

        private void MoveField(TreeNode item, bool up)
        {
            if (!CanEditFields()) return;
            var element = ((NativeSchemaNode)item.Tag).Element;
            var adjacent = up ? element.ElementsBeforeSelf().LastOrDefault() : element.ElementsAfterSelf().FirstOrDefault();
            if (adjacent == null) return;
            var parent = item.Parent;
            element.Remove();
            if (up) adjacent.AddBeforeSelf(element);
            else adjacent.AddAfterSelf(element);
            MarkChanged();
            RebuildBranch(parent, element);
        }

        private bool CanEditFields()
        {
            if (!rawDirty) return !busy && !ReadOnlyDocument;
            tabs.SelectedTab = xmlPage;
            status.Text = "Примените или отмените изменения исходного XML.";
            return false;
        }

        private static void SetText(XElement element, string value)
        {
            foreach (var text in element.Nodes().OfType<XText>().ToList()) text.Remove();
            if (value.Length > 0) element.AddFirst(new XText(value));
        }

        private void NavigateToField(string path)
        {
            if (xmlTree.Nodes.Count == 0) return;
            var item = xmlTree.Nodes[0];
            var steps = path.Trim('/').Split('/');
            string missing = null;
            foreach (var step in steps.Skip(1))
            {
                var name = step.Split('[')[0];
                if (name.StartsWith("@"))
                {
                    missing = name;
                    break;
                }
                var offset = step.Contains("[") && int.TryParse(step.Split('[').Last().TrimEnd(']'), out var number) ? number - 1 : 0;
                PopulateTree(item);
                var next = item.Nodes.Cast<TreeNode>().Where(child => (child.Tag as NativeSchemaNode)?.Element.Name.LocalName == name)
                    .Skip(Math.Max(0, offset)).FirstOrDefault();
                if (next == null)
                {
                    missing = name;
                    break;
                }
                item = next;
            }
            tabs.SelectedTab = fieldsPage;
            xmlTree.SelectedNode = item;
            item.EnsureVisible();
            if (missing != null && fieldEditors.TryGetValue(missing, out var editor)) editor.Focus();
            else if (missing != null) SelectChild(missing);
            else if (fieldEditors.TryGetValue(".", out editor)) editor.Focus();
        }

        private void SelectChild(string name)
        {
            var option = childSelector?.Items.OfType<NativeSchemaChild>().FirstOrDefault(child => child.Field.Name == name);
            if (option == null) return;
            childSelector.SelectedItem = option;
            childSelector.Focus();
        }

        private static string XmlPath(XElement element)
        {
            return "/" + string.Join("/", element.AncestorsAndSelf().Reverse().Select(item =>
            {
                var previous = item.ElementsBeforeSelf(item.Name).Count();
                var indexed = previous > 0 || item.ElementsAfterSelf(item.Name).Any();
                return item.Name.LocalName + (indexed ? $"[{previous + 1}]" : "");
            }));
        }

        private void AddFileAndLookupControls(TableLayoutPanel layout, TreeNode item, NativeSchemaNode node)
        {
            var fields = new HashSet<string>(node.Children.Select(child => child.Field.Name));
            var actions = new List<Control>();
            if (fields.IsSupersetOf(new[] { "FileName", "FileFormat", "FileChecksum" }))
            {
                var attachment = ActionButton("Выбрать файл…");
                attachment.Click += async (_, __) => await ChooseAttachmentAsync(item, node);
                actions.Add(attachment);
            }
            if (fields.Contains("ImageData") && node.Type?.Attributes?.Any(a => a.Name == "Type") == true)
            {
                var image = ActionButton("Выбрать изображение…");
                image.Click += async (_, __) => await ChooseImageAsync(item, node);
                actions.Add(image);
            }
            var organization = fields.IsSupersetOf(new[] { "FullName", "INN", "OGRN" });
            var address = fields.Contains("RegionCode") && node.Element.Name.LocalName.StartsWith("Russian", StringComparison.Ordinal);
            if (organization || address)
            {
                var lookup = ActionButton(organization ? "Найти по ИНН в DaData…" : "Заполнить адрес из DaData…");
                lookup.Click += async (_, __) => await LookupAsync(item, node, organization);
                actions.Add(lookup);
            }
            if (actions.Count > 0) AddWide(layout, Toolbar(actions.ToArray()));
        }

        private async Task ChooseAttachmentAsync(TreeNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            using (var picker = new OpenFileDialog { CheckFileExists = true })
            {
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                await RunAsync("Расчёт контрольной суммы…", async () =>
                {
                    var filename = Path.GetFileName(picker.FileName);
                    var extension = Path.GetExtension(filename).TrimStart('.').ToLowerInvariant();
                    if (extension.Length == 0 || extension.Length > 4 || family == "conclusion" &&
                        extension.Any(ch => !(ch >= 'a' && ch <= 'z') && !(ch >= '0' && ch <= '9')))
                        throw new InvalidOperationException("Расширение файла не соответствует формату схемы.");
                    var checksum = await ReadChecksumAsync(picker.FileName);
                    PutField(node, "FileName", filename);
                    PutField(node, "FileFormat", extension);
                    PutField(node, "FileChecksum", checksum);
                    MarkChanged();
                    RebuildBranch(item);
                });
            }
        }

        private async Task ChooseImageAsync(TreeNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            using (var picker = new OpenFileDialog { Filter = "Изображения|*.png;*.jpg;*.jpeg;*.gif", CheckFileExists = true })
            {
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                await RunAsync("Чтение изображения…", async () =>
                {
                    using (var stream = File.OpenRead(picker.FileName))
                    {
                        var previous = (string)node.Element.Element("ImageData") ?? "";
                        var targetSize = (stream.Length + 2) / 3 * 4 + Encoding.UTF8.GetByteCount(Snapshot()) - previous.Length + 128;
                        if (targetSize > SchemaXmlDocument.MaxXmlBytes)
                            throw new InvalidOperationException("Изображение превышает доступный объём XML.");
                        var data = new byte[(int)stream.Length];
                        for (var offset = 0; offset < data.Length;)
                        {
                            var count = await stream.ReadAsync(data, offset, data.Length - offset);
                            if (count == 0) throw new EndOfStreamException("Файл изображения прочитан не полностью.");
                            offset += count;
                        }
                        var format = ImageFormat(data);
                        var type = node.Type.Attributes.First(a => a.Name == "Type");
                        var allowed = profile.TypeOf(type.Type)?.Enumeration?.Select(option => option.Value).ToList();
                        if (format == "jpeg" && allowed != null && !allowed.Contains(format) && allowed.Contains("jpg")) format = "jpg";
                        if (format == null || allowed != null && !allowed.Contains(format))
                            throw new InvalidOperationException("Выберите изображение PNG, JPEG или GIF.");
                        node.Element.SetAttributeValue("Type", format);
                        PutField(node, "ImageData", Convert.ToBase64String(data));
                        MarkChanged();
                        RebuildBranch(item);
                    }
                });
            }
        }

        private static string ImageFormat(byte[] bytes)
        {
            if (bytes.Length >= 8 && bytes.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 })) return "png";
            if (bytes.Length >= 3 && bytes[0] == 255 && bytes[1] == 216 && bytes[2] == 255) return "jpeg";
            if (bytes.Length >= 6 && (Encoding.ASCII.GetString(bytes, 0, 6) == "GIF87a" || Encoding.ASCII.GetString(bytes, 0, 6) == "GIF89a")) return "gif";
            return null;
        }

        private async Task LookupAsync(TreeNode item, NativeSchemaNode node, bool organization)
        {
            if (!CanEditFields()) return;
            var query = XtraInputBox.Show(organization ? "Введите ИНН юридического лица" : "Введите российский адрес",
                "DaData", organization ? (string)node.Element.Element("INN") ?? "" : "");
            query = query?.Trim();
            if (string.IsNullOrWhiteSpace(query)) return;
            if (organization ? !ConclusionDaData.IsDigits(query, 10) : query.Length < 3 || query.Length > 400)
            {
                status.Text = organization ? "ИНН юридического лица должен содержать 10 цифр." : "Введите адрес от 3 до 400 символов.";
                return;
            }
            await RunAsync("Поиск в DaData…", async () =>
            {
                var values = organization ? await ConclusionDaData.FindOrganizationAsync(query) : await ConclusionDaData.NormalizeAddressAsync(query);
                if (values == null) throw new InvalidOperationException("DaData не нашла однозначного совпадения.");
                var populated = false;
                foreach (var value in values)
                {
                    if (string.IsNullOrWhiteSpace((string)node.Element.Element(value.Key)) && PutField(node, value.Key, value.Value))
                        populated = true;
                }
                if (populated)
                {
                    MarkChanged();
                    RebuildBranch(item);
                }
                status.Text = populated ? "Пустые поля заполнены из DaData." : "Подходящих пустых полей нет.";
            });
        }

        private static bool PutField(NativeSchemaNode node, string name, string value)
        {
            var candidate = node.Children.FirstOrDefault(field => field.Field.Name == name);
            if (candidate == null) return false;
            var target = node.Element.Element(name) ?? node.AddChild(candidate);
            SetText(target, value);
            return true;
        }

        private static readonly uint[] CrcTable = Enumerable.Range(0, 256).Select(index =>
        {
            var value = (uint)index;
            for (var bit = 0; bit < 8; bit++) value = (value & 1) != 0 ? (value >> 1) ^ 0xedb88320u : value >> 1;
            return value;
        }).ToArray();

        private static async Task<string> ReadChecksumAsync(string path)
        {
            var crc = uint.MaxValue;
            var buffer = new byte[64 * 1024];
            using (var file = File.OpenRead(path))
            {
                int read;
                while ((read = await file.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    for (var index = 0; index < read; index++) crc = (crc >> 8) ^ CrcTable[(crc ^ buffer[index]) & 0xff];
            }
            return (crc ^ uint.MaxValue).ToString("x8");
        }
    }
}
