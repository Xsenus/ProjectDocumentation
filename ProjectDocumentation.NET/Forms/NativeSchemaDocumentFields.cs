using ProjectDocumentation.NET.Services;
using ComboBoxEdit = DevExpress.XtraEditors.ComboBoxEdit;
using MemoEdit = DevExpress.XtraEditors.MemoEdit;
using TextEdit = DevExpress.XtraEditors.TextEdit;
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
        private const string LookupInnConflict = "ИНН запроса отличается от ИНН в документе.";
        private static readonly XName NilAttribute = XName.Get("nil", "http://www.w3.org/2001/XMLSchema-instance");
        private static readonly HashSet<string> StructuredAddressFields = new HashSet<string>
        {
            "District", "City", "Settlement", "Street", "Building", "Room", "Landmark"
        };
        private readonly TreeView xmlTree = new TreeView { Dock = DockStyle.Fill, HideSelection = false, ShowNodeToolTips = true };
        private readonly Panel details = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10, 0, 10, 10) };
        private readonly Dictionary<string, Control> fieldEditors = new Dictionary<string, Control>();
        private ComboBox childSelector;
        private SplitterPanel treeDetailsHost;

        private Control CreateFieldsPanel()
        {
            var split = new SplitContainer { Dock = DockStyle.Fill, Size = new Size(1000, 530), FixedPanel = FixedPanel.Panel1 };
            split.Panel1MinSize = 210;
            split.Panel2MinSize = 350;
            split.SplitterDistance = 325;
            split.Panel1.Padding = new Padding(8);
            split.Panel2.Padding = new Padding(8);
            xmlTree.BeforeExpand += (_, e) => PopulateTree(e.Node);
            xmlTree.AfterSelect += (_, e) => ShowSelectedFields(e.Node);
            split.Panel1.Controls.Add(xmlTree);
            treeDetailsHost = split.Panel2;
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
                ShowSelectedFields(xmlTree.SelectedNode);
                if (section != null && xmlTree.SelectedNode == root) SelectMissingField(section);
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
            ShowSelectedFields(xmlTree.SelectedNode);
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
                var nil = IsNil(node.Element);
                if (node.Field?.Nillable == true) AddNilControl(layout, item, node.Element, nil);

                var attributes = !ShowRootAttributes(node) ? Enumerable.Empty<NativeSchemaAttribute>()
                    : node.Type?.Attributes ?? Enumerable.Empty<NativeSchemaAttribute>();
                foreach (var attribute in attributes)
                {
                    var current = (string)node.Element.Attribute(attribute.Name);
                    var identity = node.Element == document.Root && (attribute.Name == idAttribute || attribute.Name == "SchemaVersion");
                    var locked = ReadOnlyDocument || identity || attribute.Fixed != null;
                    var editor = ValueEditor(current, profile.TypeOf(attribute.Type), locked, false, value =>
                    {
                        node.Element.SetAttributeValue(attribute.Name, value.Length == 0 ? null : value);
                        MarkChanged();
                    });
                    fieldEditors["@" + attribute.Name] = editor;
                    AddField(layout, "@" + attribute.Name + (attribute.Required ? " *" : ""), editor, attribute.Documentation);
                }

                if (!nil && node.Type?.Particle == null && !node.Element.HasElements)
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

                if (!nil && IsSectionView) AddSectionValues(layout, item, node);

                if (!ReadOnlyDocument)
                {
                    if (!nil)
                    {
                        if (!legacyEditor && node.Field?.Type == "tTable")
                        {
                            var editTable = ActionButton("Редактировать таблицу…");
                            editTable.Click += (_, __) => EditStructuredTable(item, node);
                            AddWide(layout, editTable);
                        }
                        AddChildControls(layout, item, node);
                        AddFileAndLookupControls(layout, item, node);
                    }
                    if (item.Parent != null) AddTreeCommands(layout, item, node.Element);
                }
            }
            finally
            {
                details.ResumeLayout(true);
            }
        }

        private static bool IsNil(XElement element)
        {
            var value = ((string)element.Attribute(NilAttribute))?.Trim();
            return value == "true" || value == "1";
        }

        private void AddNilControl(TableLayoutPanel layout, TreeNode item, XElement element, bool nil)
        {
            var check = new CheckBox
            {
                Text = "Пустой элемент (xsi:nil)", AutoSize = true, Checked = nil, Enabled = !ReadOnlyDocument
            };
            var reverting = false;
            check.CheckedChanged += (_, __) =>
            {
                if (reverting || !CanEditFields()) return;
                var content = element.Nodes().Where(child => child is XElement || child is XText).ToList();
                if (check.Checked && content.Count != 0)
                {
                    if (XtraMessageBox.Show(this, "Очистить содержимое и отметить элемент пустым?", Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        reverting = true;
                        check.Checked = false;
                        reverting = false;
                        return;
                    }
                    content.Remove();
                }
                element.SetAttributeValue(NilAttribute, check.Checked ? "true" : null);
                MarkChanged();
                RebuildBranch(item);
            };
            fieldEditors["@xsi:nil"] = check;
            AddWide(layout, check);
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

        private Control ValueEditor(string value, NativeSchemaType type, bool readOnly, bool multiline, Action<string> changed)
        {
            value = value ?? "";
            if (IsSectionView) return SectionValueEditor(value, type, readOnly, multiline, changed);
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

        private static Control SectionValueEditor(string value, NativeSchemaType type, bool readOnly, bool multiline, Action<string> changed)
        {
            if (!readOnly && type?.Enumeration?.Count > 0)
            {
                var editor = new ComboBoxEdit { Dock = DockStyle.Top };
                editor.Properties.Items.AddRange(type.Enumeration.Cast<object>().ToArray());
                editor.Properties.DropDownRows = 14;
                editor.Properties.PopupFormSize = new Size(550, 0);
                editor.EditValue = (object)type.Enumeration.FirstOrDefault(option => option.Value == value) ?? value;
                editor.EditValueChanged += (_, __) => changed(
                    editor.EditValue is NativeSchemaOption option ? option.Value : editor.Text);
                return editor;
            }

            TextEdit text;
            if (multiline)
            {
                var memo = new MemoEdit { Height = 95 };
                memo.Properties.ScrollBars = ScrollBars.Vertical;
                text = memo;
            }
            else
            {
                text = new TextEdit();
            }
            text.Dock = DockStyle.Top;
            text.Properties.ReadOnly = readOnly;
            text.Properties.MaxLength = SchemaXmlDocument.MaxXmlBytes;
            text.Text = value;
            if (!readOnly) text.EditValueChanged += (_, __) => changed(text.Text);
            return text;
        }

        private void AddChildControls(TableLayoutPanel layout, TreeNode item, NativeSchemaNode node)
        {
            var candidates = VisibleChildren(node).GroupBy(child => child.Field.Name).Select(group => group.First())
                .Where(child => (!IsSectionView || !fieldEditors.ContainsKey(child.Field.Name)) &&
                    node.Element.Elements(child.Field.Name).LongCount() < child.Capacity).ToArray();
            if (candidates.Length == 0) return;
            childSelector = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, DropDownWidth = 650, MaxDropDownItems = 18 };
            childSelector.Items.AddRange(candidates.Cast<object>().ToArray());
            childSelector.SelectedItem = candidates.FirstOrDefault(child => child.Required && node.Element.Element(child.Field.Name) == null) ?? candidates[0];
            AddField(layout, "Вложенное поле", childSelector, null);
            var add = ActionButton("Добавить выбранное поле");
            add.Click += (_, __) =>
            {
                if (!(childSelector.SelectedItem is NativeSchemaChild child) || !CanEditFields()) return;
                if (node.Element.Elements(child.Field.Name).LongCount() >= child.Capacity)
                {
                    ShowFields(item);
                    return;
                }
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
            var parentNode = (NativeSchemaNode)item.Parent.Tag;
            up.Enabled = MoveTarget(parentNode, element, true) != null;
            down.Enabled = MoveTarget(parentNode, element, false) != null;
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
            var parent = item.Parent;
            var adjacent = MoveTarget((NativeSchemaNode)parent.Tag, element, up);
            if (adjacent == null) return;
            element.Remove();
            if (up) adjacent.AddBeforeSelf(element);
            else adjacent.AddAfterSelf(element);
            MarkChanged();
            RebuildBranch(parent, element);
        }

        private static XElement MoveTarget(NativeSchemaNode parent, XElement element, bool up)
        {
            var adjacent = up ? element.ElementsBeforeSelf().LastOrDefault() : element.ElementsAfterSelf().FirstOrDefault();
            if (adjacent == null) return null;
            var fields = parent.Children.ToList();
            var currentIndex = fields.FindIndex(child => XName.Get(child.Field.Name) == element.Name);
            var adjacentIndex = fields.FindIndex(child => XName.Get(child.Field.Name) == adjacent.Name);
            if (currentIndex < 0 || adjacentIndex < 0) return null;
            var group = parent.Children[currentIndex].RepeatedGroup;
            var sameChoice = group?.Kind == "choice" && ReferenceEquals(group, parent.Children[adjacentIndex].RepeatedGroup);
            var restoresOrder = up ? currentIndex < adjacentIndex : currentIndex > adjacentIndex;
            return currentIndex == adjacentIndex || sameChoice || restoresOrder ? adjacent : null;
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
            if (!CanUseDocument() || string.IsNullOrWhiteSpace(path) || xmlTree.Nodes.Count == 0) return;
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
            tabs.SelectedTab = previousFieldsPage ?? sectionsPage;
            SelectTreeElement(((NativeSchemaNode)item.Tag).Element);
            if (missing != null && fieldEditors.TryGetValue(missing, out var editor)) editor.Focus();
            else if (missing != null) SelectMissingField(missing);
            else if (fieldEditors.TryGetValue(".", out editor)) editor.Focus();
        }

        private void FindField()
        {
            if (!CanUseDocument()) return;
            using (var dialog = new Form
            {
                Text = "Найти поле", StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
                MinimumSize = new Size(690, 360), ClientSize = new Size(850, 480)
            })
            {
                var query = new TextBox { Width = 240, AccessibleName = "Имя, путь XML или описание поля" };
                var go = ActionButton("Перейти");
                var close = ActionButton("Закрыть");
                close.DialogResult = DialogResult.Cancel;
                dialog.AcceptButton = go;
                dialog.CancelButton = close;
                var hint = LabelFor("Введите имя поля, часть пути XML или описание.");
                var matches = new ListView
                {
                    Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, HideSelection = false,
                    MultiSelect = false, ShowItemToolTips = true, AccessibleName = "Найденные поля"
                };
                matches.Columns.Add("Поле", 165);
                matches.Columns.Add("Путь XML", 365);
                matches.Columns.Add("Описание", 295);
                var results = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
                results.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                results.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                results.Controls.Add(hint, 0, 0);
                results.Controls.Add(matches, 0, 1);
                dialog.Controls.Add(WithToolbar(results, LabelFor("Поиск"), query, go, close));
                go.Enabled = false;
                string target = null;
                void Choose()
                {
                    if (matches.SelectedItems.Count != 1) return;
                    target = (string)matches.SelectedItems[0].Tag;
                    dialog.DialogResult = DialogResult.OK;
                }
                go.Click += (_, __) => Choose();
                matches.ItemActivate += (_, __) => Choose();
                matches.SelectedIndexChanged += (_, __) => go.Enabled = matches.SelectedItems.Count == 1;
                query.TextChanged += (_, __) =>
                {
                    var terms = query.Text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    const int limit = 200;
                    var hasMore = false;
                    matches.BeginUpdate();
                    try
                    {
                        matches.Items.Clear();
                        if (terms.Length > 0)
                        {
                            foreach (var match in SearchFields(profile.Root, document.Root, "/" + profile.Root.Name,
                                terms, new HashSet<NativeSchemaType>()).Take(limit + 1))
                            {
                                if (matches.Items.Count == limit)
                                {
                                    hasMore = true;
                                    break;
                                }
                                matches.Items.Add(match);
                            }
                        }
                        hint.Text = terms.Length == 0 ? "Введите имя поля, часть пути XML или описание."
                            : matches.Items.Count == 0 ? "Совпадений нет."
                            : hasMore ? $"Показаны первые {limit} совпадений. Уточните запрос."
                            : $"Найдено полей: {matches.Items.Count}.";
                        if (matches.Items.Count > 0)
                        {
                            matches.Items[0].Selected = true;
                            matches.Items[0].Focused = true;
                        }
                    }
                    finally
                    {
                        matches.EndUpdate();
                    }
                };
                dialog.Shown += (_, __) => query.Focus();
                if (dialog.ShowDialog(this) == DialogResult.OK) NavigateToField(target);
            }
        }

        private IEnumerable<ListViewItem> SearchFields(NativeSchemaParticle field, XElement element, string path,
            string[] terms, HashSet<NativeSchemaType> ancestors)
        {
            var type = profile.TypeOf(field.Type);
            var documentation = string.IsNullOrWhiteSpace(field.Documentation) ? type?.Documentation : field.Documentation;
            var match = SearchMatch(field.Name, path, documentation, terms);
            if (match != null) yield return match;
            if (field.Nillable && (!ReadOnlyDocument || element?.Attribute(NilAttribute) != null))
            {
                match = SearchMatch("@xsi:nil", path + "/@xsi:nil", "Пустой элемент", terms);
                if (match != null) yield return match;
            }
            if (type == null) yield break;
            foreach (var attribute in type.Attributes ?? Enumerable.Empty<NativeSchemaAttribute>())
            {
                if (ReadOnlyDocument && element?.Attribute(attribute.Name) == null) continue;
                match = SearchMatch("@" + attribute.Name, path + "/@" + attribute.Name, attribute.Documentation, terms);
                if (match != null) yield return match;
            }
            var firstTypeVisit = ancestors.Add(type);
            try
            {
                // Schema types may be recursive. Expand a recursive branch only where the document has a node.
                if (!firstTypeVisit && element == null) yield break;
                foreach (var child in type.Particle?.ChildFields() ?? Enumerable.Empty<NativeSchemaChild>())
                {
                    var name = child.Field.Name;
                    var children = element?.Elements(name).ToArray() ?? Array.Empty<XElement>();
                    var childPath = path + "/" + name;
                    if (children.Length == 0 && !ReadOnlyDocument)
                    {
                        foreach (var nested in SearchFields(child.Field, null, childPath, terms, ancestors)) yield return nested;
                    }
                    for (var index = 0; index < children.Length; index++)
                    {
                        var occurrencePath = children.Length == 1 ? childPath : $"{childPath}[{index + 1}]";
                        foreach (var nested in SearchFields(child.Field, children[index], occurrencePath, terms, ancestors))
                            yield return nested;
                    }
                }
            }
            finally
            {
                if (firstTypeVisit) ancestors.Remove(type);
            }
        }

        private static ListViewItem SearchMatch(string name, string path, string documentation, string[] terms)
        {
            documentation = documentation ?? "";
            if (!terms.All(term => path.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                || documentation.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)) return null;
            return new ListViewItem(new[] { name, path, documentation }) { Tag = path, ToolTipText = path + "\n" + documentation };
        }

        private void SelectChild(string name)
        {
            var option = childSelector?.Items.OfType<NativeSchemaChild>().FirstOrDefault(child => child.Field.Name == name);
            if (option == null) return;
            childSelector.SelectedItem = option;
            childSelector.Focus();
        }

        private void EditStructuredTable(TreeNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            var edited = new XElement(node.Element);
            var alignment = profile.TypeOf("tAlignTypes")?.Enumeration ?? new List<NativeSchemaOption>();
            using (var dialog = new DevExpress.XtraEditors.XtraForm
            {
                Text = "Таблица пояснительной записки", StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
                MinimumSize = new Size(740, 420), ClientSize = new Size(900, 540)
            })
            {
                var rowList = new ListBox { Dock = DockStyle.Fill, AccessibleName = "Строки таблицы" };
                var cells = new DataGridView
                {
                    Dock = DockStyle.Fill, AccessibleName = "Ячейки выбранной строки", MultiSelect = false,
                    AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersWidth = 45,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    SelectionMode = DataGridViewSelectionMode.CellSelect
                };
                cells.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Значение", FillWeight = 72, SortMode = DataGridViewColumnSortMode.NotSortable });
                var alignColumn = new DataGridViewComboBoxColumn
                {
                    HeaderText = "Выравнивание", FillWeight = 28, DisplayMember = "Caption", ValueMember = "Value",
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                var options = new[] { new NativeSchemaOption { Value = "", Label = "По умолчанию" } }.Concat(alignment);
                alignColumn.DataSource = options.Select(option => new
                {
                    option.Value, Caption = string.IsNullOrWhiteSpace(option.Label) ? option.Value : option.Label
                }).ToList();
                cells.Columns.Add(alignColumn);

                var addTitle = ActionButton("Добавить заголовок");
                var addRow = ActionButton("Добавить строку");
                var removeRow = ActionButton("Удалить строку");
                var addCell = ActionButton("Добавить ячейку");
                var removeCell = ActionButton("Удалить ячейку");
                var saveTable = ActionButton("Применить");
                var cancel = ActionButton("Отмена");
                cancel.DialogResult = DialogResult.Cancel;
                dialog.AcceptButton = saveTable;
                dialog.CancelButton = cancel;
                var hint = LabelFor("");
                var rows = new List<XElement>();
                var rowCells = new List<XElement>();
                var rebuilding = false;

                void UpdateTableActions()
                {
                    removeRow.Enabled = addCell.Enabled = rowList.SelectedIndex >= 0;
                    removeCell.Enabled = cells.CurrentCell != null;
                    saveTable.Enabled = edited.Elements("Row").Any() && rows.All(row => row.Element("Cell") != null);
                    hint.Text = saveTable.Enabled ? ""
                        : "Нужна хотя бы одна строка данных и хотя бы одна ячейка в каждой строке.";
                }

                void LoadCells()
                {
                    rowCells.Clear();
                    if (rowList.SelectedIndex >= 0) rowCells.AddRange(rows[rowList.SelectedIndex].Elements("Cell"));
                    rebuilding = true;
                    try
                    {
                        cells.Rows.Clear();
                        foreach (var cell in rowCells)
                        {
                            var align = (string)cell.Attribute("align");
                            var displayAlign = alignment.Any(option => option.Value == align) ? align : "";
                            var index = cells.Rows.Add(cell.Value, displayAlign);
                            cells.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        }
                    }
                    finally
                    {
                        rebuilding = false;
                    }
                    UpdateTableActions();
                }

                void LoadRows(XElement selection = null)
                {
                    rows.Clear();
                    rows.AddRange(edited.Elements().Where(row => row.Name == "TitleRow" || row.Name == "Row"));
                    rowList.BeginUpdate();
                    try
                    {
                        rowList.Items.Clear();
                        var titles = 0;
                        var data = 0;
                        foreach (var row in rows)
                        {
                            var caption = row.Name == "TitleRow" ? $"Заголовок {++titles}" : $"Строка {++data}";
                            rowList.Items.Add(caption);
                        }
                        rowList.SelectedIndex = selection == null ? (rows.Count > 0 ? 0 : -1) : rows.IndexOf(selection);
                    }
                    finally
                    {
                        rowList.EndUpdate();
                    }
                    LoadCells();
                }

                void AddTableRow(bool title)
                {
                    if (!cells.EndEdit()) return;
                    var row = new XElement(title ? "TitleRow" : "Row", new XElement("Cell"));
                    var firstData = title ? edited.Element("Row") : null;
                    if (firstData == null) edited.Add(row);
                    else firstData.AddBeforeSelf(row);
                    LoadRows(row);
                }

                rowList.SelectedIndexChanged += (_, __) => LoadCells();
                cells.SelectionChanged += (_, __) => { if (!rebuilding) UpdateTableActions(); };
                cells.CurrentCellDirtyStateChanged += (_, __) =>
                {
                    if (cells.IsCurrentCellDirty && cells.CurrentCell is DataGridViewComboBoxCell)
                        cells.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
                cells.CellValueChanged += (_, e) =>
                {
                    if (rebuilding || e.RowIndex < 0 || e.RowIndex >= rowCells.Count || e.ColumnIndex < 0) return;
                    var cell = rowCells[e.RowIndex];
                    var value = Convert.ToString(cells[e.ColumnIndex, e.RowIndex].Value);
                    if (e.ColumnIndex == 0) SetText(cell, value);
                    else cell.SetAttributeValue("align", value.Length == 0 ? null : value);
                };
                addTitle.Click += (_, __) => AddTableRow(true);
                addRow.Click += (_, __) => AddTableRow(false);
                removeRow.Click += (_, __) =>
                {
                    if (rowList.SelectedIndex < 0 || !cells.EndEdit()) return;
                    var index = rowList.SelectedIndex;
                    rows[index].Remove();
                    var selection = rows.Count <= 1 ? null : rows[index == rows.Count - 1 ? index - 1 : index + 1];
                    LoadRows(selection);
                };
                addCell.Click += (_, __) =>
                {
                    if (rowList.SelectedIndex < 0 || !cells.EndEdit()) return;
                    rows[rowList.SelectedIndex].Add(new XElement("Cell"));
                    LoadCells();
                    cells.CurrentCell = cells[0, cells.Rows.Count - 1];
                    cells.BeginEdit(true);
                };
                removeCell.Click += (_, __) =>
                {
                    if (cells.CurrentCell == null || !cells.EndEdit()) return;
                    rowCells[cells.CurrentCell.RowIndex].Remove();
                    LoadCells();
                };
                saveTable.Click += (_, __) =>
                {
                    if (cells.EndEdit() && saveTable.Enabled) dialog.DialogResult = DialogResult.OK;
                };

                var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(8) };
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                body.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                body.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var rowTools = Toolbar(addTitle, addRow, removeRow);
                rowTools.WrapContents = true;
                body.Controls.Add(rowTools, 0, 0);
                body.Controls.Add(Toolbar(addCell, removeCell), 1, 0);
                body.Controls.Add(rowList, 0, 1);
                body.Controls.Add(cells, 1, 1);
                var footer = Toolbar(saveTable, cancel, hint);
                footer.WrapContents = true;
                body.Controls.Add(footer, 0, 2);
                body.SetColumnSpan(footer, 2);
                dialog.Controls.Add(body);

                if (!edited.Elements().Any()) edited.Add(new XElement("Row", new XElement("Cell")));
                LoadRows();
                if (dialog.ShowDialog(this) != DialogResult.OK || XNode.DeepEquals(node.Element, edited)) return;
                node.Element.ReplaceNodes(edited.Nodes());
                MarkChanged();
                RebuildBranch(item);
            }
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
                    var removedSignatures = UpdateAttachmentMetadata(node, filename, extension, checksum);
                    MarkChanged();
                    RebuildBranch(item);
                    if (removedSignatures)
                        status.Text = "Файл изменился. Сведения о прежних файлах подписи удалены. Добавьте подписи к новому файлу.";
                });
            }
        }

        private static bool UpdateAttachmentMetadata(NativeSchemaNode node, string name, string format, string checksum)
        {
            var previousChecksum = (string)node.Element.Element("FileChecksum");
            PutField(node, "FileName", name);
            PutField(node, "FileFormat", format);
            PutField(node, "FileChecksum", checksum);
            if (string.Equals(previousChecksum, checksum, StringComparison.OrdinalIgnoreCase)) return false;
            var oldSignatures = node.Element.Elements("SignFile").ToList();
            oldSignatures.Remove();
            return oldSignatures.Count > 0;
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
            if (organization && HasDifferentIdentifier(node.Element, "INN", query))
            {
                status.Text = LookupInnConflict;
                return;
            }
            await RunAsync("Поиск в DaData…", async () =>
            {
                var values = organization ? await ConclusionDaData.FindOrganizationAsync(query) : await ConclusionDaData.NormalizeAddressAsync(query);
                if (values == null) throw new InvalidOperationException("DaData не нашла однозначного совпадения.");
                var populated = FillLookupFields(node, values, profile);
                if (populated)
                {
                    MarkChanged();
                    RebuildBranch(item);
                }
                status.Text = populated ? "Пустые поля заполнены из DaData." : "Подходящих пустых полей нет.";
            });
        }

        private static bool HasDifferentIdentifier(XElement element, string name, string value)
        {
            var existing = ((string)element.Element(name))?.Trim();
            return !string.IsNullOrEmpty(existing) && existing != value;
        }

        private static bool FillLookupFields(NativeSchemaNode node, Dictionary<string, string> values, NativeSchemaProfile schema)
        {
            if (values.TryGetValue("INN", out var inn) && HasDifferentIdentifier(node.Element, "INN", inn))
                throw new InvalidOperationException(LookupInnConflict);
            if (values.TryGetValue("OGRN", out var ogrn) && HasDifferentIdentifier(node.Element, "OGRN", ogrn))
                throw new InvalidOperationException("ОГРН найденной организации отличается от ОГРН в документе.");

            var hasNote = node.Element.Element("Note") != null;
            var hasStructuredAddress = node.Element.Elements().Any(field => StructuredAddressFields.Contains(field.Name.LocalName));
            var populated = false;
            foreach (var value in values)
            {
                if ((hasNote && StructuredAddressFields.Contains(value.Key)) || (hasStructuredAddress && value.Key == "Note"))
                    continue;
                if (string.IsNullOrWhiteSpace((string)node.Element.Element(value.Key)) && PutField(node, value.Key, value.Value, schema))
                    populated = true;
            }
            return populated;
        }

        private static bool PutField(NativeSchemaNode node, string name, string value, NativeSchemaProfile schema = null)
        {
            var candidate = node.Children.FirstOrDefault(field => field.Field.Name == name);
            if (candidate == null) return false;
            var allowed = schema?.TypeOf(candidate.Field.Type)?.Enumeration;
            if (allowed?.Count > 0 && !allowed.Any(option => option.Value == value)) return false;
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
