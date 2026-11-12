using ProjectDocumentation.NET.Services;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
        private readonly TreeList xmlTree = new TreeList { Dock = DockStyle.Fill, AccessibleName = "Дерево XML" };
        private readonly XtraScrollableControl details = new XtraScrollableControl
        {
            Dock = DockStyle.Fill, Padding = new Padding(8, 2, 9, 8), AccessibleName = "Поля выбранного элемента"
        };
        private readonly Dictionary<string, Control> fieldEditors = new Dictionary<string, Control>();
        private ComboBoxEdit childSelector;
        private SplitGroupPanel treeDetailsHost;
        private DXPopupMenu treeMenu;
        private bool rebuildingTree;

        private Control CreateFieldsPanel()
        {
            var split = CreateEditorSplit();
            xmlTree.OptionsBehavior.Editable = false;
            xmlTree.OptionsBehavior.AutoPopulateColumns = false;
            xmlTree.OptionsView.ShowColumns = false;
            xmlTree.OptionsView.ShowIndicator = false;
            xmlTree.OptionsView.ShowHorzLines = false;
            xmlTree.OptionsView.ShowVertLines = false;
            xmlTree.OptionsSelection.EnableAppearanceFocusedCell = false;
            xmlTree.OptionsSelection.SelectNodesOnRightClick = true;
            xmlTree.RowHeight = 26;
            xmlTree.ToolTipController = fieldToolTips;
            var column = xmlTree.Columns.Add();
            column.Caption = "Элемент XML";
            column.VisibleIndex = 0;
            xmlTree.BeforeExpand += (_, e) => PopulateTree(e.Node);
            xmlTree.FocusedNodeChanged += (_, e) =>
            {
                if (!rebuildingTree) ShowSelectedFields(e.Node);
            };
            xmlTree.MouseUp += (_, e) =>
            {
                if (e.Button == MouseButtons.Right && xmlTree.CalcHitInfo(e.Location).Node is TreeListNode node)
                    ShowTreeMenu(node, e.Location);
            };
            split.Panel1.Controls.Add(xmlTree);
            treeDetailsHost = split.Panel2;
            return split;
        }

        private static SplitContainerControl CreateEditorSplit()
        {
            var split = new SplitContainerControl
            {
                Dock = DockStyle.Fill, Size = new Size(1000, 530), Horizontal = true, FixedPanel = SplitFixedPanel.Panel1,
                ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True
            };
            split.Panel1.MinSize = 205;
            split.Panel2.MinSize = 340;
            split.SplitterPosition = 322;
            split.Panel1.Padding = new Padding(8);
            split.Panel2.Padding = new Padding(5, 8, 8, 8);
            split.SizeChanged += (_, __) =>
            {
                var normal = (int)Math.Round(322 * split.DeviceDpi / 96d);
                var position = Math.Min(normal, Math.Max(split.Panel1.MinSize, (int)(split.ClientSize.Width * .30)));
                split.SplitterPosition = position;
            };
            return split;
        }

        private void BuildTree(string section = null)
        {
            rebuildingTree = true;
            xmlTree.BeginUpdate();
            try
            {
                xmlTree.Nodes.Clear();
                var root = AppendTreeNode(profile.RootNode(document.Root), null);
                PopulateTree(root);
                root.Expanded = true;
                xmlTree.FocusedNode = section == null ? root
                    : root.Nodes.Cast<TreeListNode>().FirstOrDefault(item => (item.Tag as NativeSchemaNode)?.Element.Name.LocalName == section) ?? root;
                xmlTree.MakeNodeVisible(xmlTree.FocusedNode);
                ShowSelectedFields(xmlTree.FocusedNode);
                if (section != null && xmlTree.FocusedNode == root) SelectMissingField(section);
            }
            finally
            {
                xmlTree.EndUpdate();
                rebuildingTree = false;
            }
        }

        private TreeListNode AppendTreeNode(NativeSchemaNode node, TreeListNode parent)
        {
            var result = xmlTree.AppendNode(new object[] { TreeLabel(node) }, parent, node);
            if (node.Element.HasElements) xmlTree.AppendNode(new object[] { "" }, result);
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

        private void PopulateTree(TreeListNode item)
        {
            if (item.Tag is NativeSchemaNode node && item.Nodes.Count == 1 && item.Nodes[0].Tag == null)
            {
                item.Nodes.Clear();
                foreach (var element in node.Element.Elements()) AppendTreeNode(profile.ChildNode(node, element), item);
            }
        }

        private void RebuildBranch(TreeListNode item, XElement selected = null)
        {
            var node = (NativeSchemaNode)item.Tag;
            rebuildingTree = true;
            xmlTree.BeginUpdate();
            try
            {
                item.Nodes.Clear();
                foreach (var element in node.Element.Elements()) AppendTreeNode(profile.ChildNode(node, element), item);
                item.SetValue(0, TreeLabel(node));
                item.Expanded = true;
                xmlTree.FocusedNode = item.Nodes.Cast<TreeListNode>().FirstOrDefault(n => ReferenceEquals(((NativeSchemaNode)n.Tag).Element, selected)) ?? item;
                xmlTree.MakeNodeVisible(xmlTree.FocusedNode);
            }
            finally
            {
                xmlTree.EndUpdate();
                rebuildingTree = false;
            }
            ShowSelectedFields(xmlTree.FocusedNode);
        }

        private void ShowFields(TreeListNode item)
        {
            if (!(item?.Tag is NativeSchemaNode node)) return;
            details.SuspendLayout();
            LayoutControl layout = null;
            try
            {
                while (details.Controls.Count > 0) details.Controls[0].Dispose();
                fieldEditors.Clear();
                childSelector = null;
                layout = CreateLayout(Padding.Empty,
                    new ColumnDefinition { SizeType = SizeType.Percent, Width = 34 },
                    new ColumnDefinition { SizeType = SizeType.Percent, Width = 66 });
                layout.BeginUpdate();
                details.Controls.Add(layout);
                var path = WrappedLabelFor(XmlPath(node.Element));
                path.Appearance.FontStyleDelta = FontStyle.Bold;
                AddWide(layout, path);
                var documentation = node.Field?.Documentation;
                if (string.IsNullOrWhiteSpace(documentation)) documentation = node.Type?.Documentation;
                if (!string.IsNullOrWhiteSpace(documentation)) AddWide(layout, WrappedLabelFor(documentation));
                var nil = IsNil(node.Element);
                if (node.Field?.Nillable == true) AddNilControl(layout, item, node.Element, nil);

                var attributes = (!ShowRootAttributes(node) ? Enumerable.Empty<NativeSchemaAttribute>()
                    : node.Type?.Attributes ?? Enumerable.Empty<NativeSchemaAttribute>()).ToArray();
                var summary = WrappedLabelFor("");
                summary.AccessibleName = "Обязательные поля и группы";
                UpdateRequirementSummary(summary, node, attributes, nil);
                if (summary.Text.Length > 0)
                    AddWide(layout, summary);
                else
                {
                    summary.Dispose();
                    summary = null;
                }
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
                    AddField(layout, "@" + attribute.Name, editor, attribute.Documentation,
                        attribute.Required ? (Func<bool>)(() => MissingAttribute(node.Element, attribute)) : null);
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
                            item.SetValue(0, TreeLabel(node));
                            MarkChanged();
                        });
                    fieldEditors["."] = editor;
                    AddField(layout, "Значение", editor, null,
                        RequiresValue(type) ? (Func<bool>)(() => string.IsNullOrWhiteSpace(node.Element.Value)) : null);
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
                            AddWide(layout, Toolbar(editTable));
                        }
                        AddChildControls(layout, item, node);
                        AddFileAndLookupControls(layout, item, node);
                    }
                    if (item.ParentNode != null) AddTreeCommands(layout, item, node.Element);
                }
                if (summary != null)
                    foreach (var editor in fieldEditors.Values.OfType<BaseEdit>())
                        editor.EditValueChanged += (_, __) => UpdateRequirementSummary(summary, node, attributes, nil);
            }
            finally
            {
                layout?.EndUpdate();
                details.ResumeLayout(true);
            }
        }

        private static bool IsNil(XElement element)
        {
            var value = ((string)element.Attribute(NilAttribute))?.Trim();
            return value == "true" || value == "1";
        }

        private void AddNilControl(LayoutControl layout, TreeListNode item, XElement element, bool nil)
        {
            var check = new CheckEdit
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

        private static LabelControl LabelFor(string text)
            => new LabelControl { Text = text, Margin = new Padding(3, 7, 3, 6) };

        private static LabelControl WrappedLabelFor(string text)
            => new LabelControl { Text = text, AutoSizeMode = LabelAutoSizeMode.Vertical, Dock = DockStyle.Top, Margin = new Padding(3, 6, 3, 6) };

        private static void AddWide(LayoutControl layout, Control control)
            => AddLayoutItem(layout, control, AddLayoutRow(layout), columnSpan: 2);

        private static void AddField(LayoutControl layout, string label, Control editor, string documentation, Func<bool> isMissing = null)
        {
            var row = AddLayoutRow(layout);
            editor.AccessibleName = label;
            if (editor is ComboBoxEdit combo)
            {
                void UpdateToolTip()
                {
                    combo.ToolTip = string.Join(Environment.NewLine, new[] { documentation, combo.EditValue?.ToString() }
                        .Where(text => !string.IsNullOrWhiteSpace(text)));
                }
                UpdateToolTip();
                combo.EditValueChanged += (_, __) => UpdateToolTip();
            }
            else if (editor is BaseControl control && !string.IsNullOrWhiteSpace(documentation))
                control.ToolTip = documentation;
            var caption = WrappedLabelFor(isMissing == null ? label : label + " *");
            AddLayoutItem(layout, caption, row);
            var item = AddLayoutItem(layout, editor, row, 1);
            if (editor is MemoEdit)
            {
                item.SizeConstraintsType = SizeConstraintsType.Custom;
                item.ControlMinSize = new Size(0, editor.Height);
                item.ControlMaxSize = new Size(0, editor.Height);
            }
            if (!string.IsNullOrWhiteSpace(documentation))
                AddLayoutItem(layout, WrappedLabelFor(documentation), AddLayoutRow(layout), 1);
            if (isMissing != null && editor is BaseEdit edit)
            {
                void Update()
                {
                    var missing = isMissing();
                    edit.AccessibleDescription = missing ? "Обязательное поле не заполнено." : "Обязательное поле заполнено.";
                    edit.ErrorText = missing ? edit.AccessibleDescription : "";
                    caption.Appearance.FontStyleDelta = missing ? FontStyle.Bold : FontStyle.Regular;
                }
                Update();
                edit.EditValueChanged += (_, __) => Update();
            }
        }

        private bool RequiresValue(NativeSchemaType type)
            => (type?.TextType == null ? type : profile.TypeOf(type.TextType))?.AllowsEmpty == false;

        private bool MissingAttribute(XElement element, NativeSchemaAttribute attribute)
        {
            var current = element.Attribute(attribute.Name);
            return current == null || RequiresValue(profile.TypeOf(attribute.Type)) && string.IsNullOrWhiteSpace(current.Value);
        }

        private bool MissingElementValue(XElement element, NativeSchemaParticle field)
            => !IsNil(element) && !element.HasElements && RequiresValue(profile.TypeOf(field.Type)) && string.IsNullOrWhiteSpace(element.Value);

        private static string RequirementCaption(string name, string documentation)
        {
            var caption = DocumentationCaption(documentation);
            return caption.Length == 0 || caption.Length > 64 || string.Equals(caption, name, StringComparison.OrdinalIgnoreCase)
                ? name : $"{caption} ({name})";
        }

        private void UpdateRequirementSummary(LabelControl summary, NativeSchemaNode node, NativeSchemaAttribute[] attributes, bool nil)
        {
            if (summary.IsDisposed) return;
            NativeSchemaParticle ChildField(string name) => node.Children.FirstOrDefault(child => child.Field.Name == name)?.Field;
            string ChildCaption(string name) => RequirementCaption(name, ChildField(name)?.Documentation);
            var requiredAttributes = attributes.Where(attribute => attribute.Required).ToArray();
            var missing = requiredAttributes.Where(attribute => MissingAttribute(node.Element, attribute))
                .Select(attribute => RequirementCaption("@" + attribute.Name, attribute.Documentation)).ToList();
            var choices = new List<NativeSchemaChoiceRequirement>();
            var hasRequirements = requiredAttributes.Length > 0;
            if (!nil)
            {
                var visible = new HashSet<string>(VisibleChildren(node).Select(child => child.Field.Name));
                var requirements = node.GetRequirements();
                var fields = requirements.Elements.Where(requirement => visible.Contains(requirement.Field.Name)).ToArray();
                foreach (var requirement in fields)
                {
                    var name = requirement.Field.Name;
                    if (requirement.IsMissing || node.Element.Elements(name).Any(element => MissingElementValue(element, requirement.Field)))
                        missing.Add(RequirementCaption(name, requirement.Field.Documentation));
                }
                choices.AddRange(requirements.Choices.Where(choice => choice.Alternatives.SelectMany(alternative => alternative).Any(visible.Contains)));
                foreach (var name in choices.SelectMany(choice => choice.Selected).Distinct())
                {
                    var field = ChildField(name);
                    if (field != null && node.Element.Elements(name).Any(element => MissingElementValue(element, field)))
                        missing.Add(RequirementCaption(name, field.Documentation));
                }
                var valueRequired = node.Type?.Particle == null && !node.Element.HasElements && RequiresValue(node.Type);
                if (valueRequired && string.IsNullOrWhiteSpace(node.Element.Value)) missing.Add("значение элемента");
                hasRequirements |= fields.Length > 0 || choices.Count > 0 || valueRequired;
            }

            var lines = new List<string>();
            if (missing.Count > 0)
                lines.Add("Не заполнены обязательные поля: " + string.Join(", ", missing.Distinct()) + ".");
            foreach (var choice in choices)
            {
                var alternatives = string.Join(" или ", choice.Alternatives.Select(branch => string.Join(" + ", branch.Select(ChildCaption))));
                if (choice.HasConflict)
                    lines.Add("Выберите один из вариантов: " + alternatives + ".");
                else if (choice.IsMissing && choice.Minimum > 1)
                    lines.Add($"Обязательная группа: добавлено {choice.Present} из {choice.Minimum}. Варианты: {alternatives}.");
                else if (choice.IsMissing)
                    lines.Add("Выберите обязательный вариант: " + alternatives + ".");
                else
                    lines.Add("Выбран вариант: " + string.Join(", ", choice.Selected.Select(ChildCaption)) + ".");
            }
            if (hasRequirements && lines.Count == 0)
                lines.Add("Обязательные поля этого уровня заполнены.");
            var text = string.Join(Environment.NewLine, lines);
            if (summary.Text != text) summary.Text = text;
            summary.Appearance.FontStyleDelta = missing.Count > 0 || choices.Any(choice => choice.IsMissing || choice.HasConflict)
                ? FontStyle.Bold : FontStyle.Regular;
        }

        private static Control ValueEditor(string value, NativeSchemaType type, bool readOnly, bool multiline, Action<string> changed)
        {
            value = value ?? "";
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

        private void AddChildControls(LayoutControl layout, TreeListNode item, NativeSchemaNode node)
        {
            var candidates = ChildCandidates(node).ToArray();
            if (candidates.Length == 0) return;
            childSelector = new ComboBoxEdit { Dock = DockStyle.Top };
            childSelector.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            childSelector.Properties.DropDownRows = 14;
            childSelector.Properties.PopupFormSize = new Size(650, 0);
            childSelector.Properties.Items.AddRange(candidates.Cast<object>().ToArray());
            var requirements = node.GetRequirements();
            var missingNames = new HashSet<string>(requirements.Elements.Where(requirement => requirement.IsMissing).Select(requirement => requirement.Field.Name)
                .Concat(requirements.Choices.Where(choice => choice.IsMissing).SelectMany(choice => choice.Alternatives.SelectMany(branch => branch))));
            childSelector.SelectedItem = candidates.FirstOrDefault(child => missingNames.Contains(child.Field.Name)) ?? candidates[0];
            AddField(layout, "Вложенное поле", childSelector, null);
            var add = ActionButton("Добавить выбранное поле");
            add.Click += (_, __) =>
            {
                if (childSelector.SelectedItem is NativeSchemaChild child) AddChildField(item, node, child);
            };
            AddWide(layout, Toolbar(add));
        }

        private IEnumerable<NativeSchemaChild> ChildCandidates(NativeSchemaNode node)
            => VisibleChildren(node).GroupBy(child => child.Field.Name).Select(group => group.First())
                .Where(child => (!IsSectionView || !fieldEditors.ContainsKey(child.Field.Name)) &&
                    node.Element.Elements(child.Field.Name).LongCount() < child.Capacity);

        private void AddChildField(TreeListNode item, NativeSchemaNode node, NativeSchemaChild child)
        {
            if (!CanEditFields()) return;
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
            if (fieldEditors.TryGetValue(".", out var editor)) FocusField(editor);
        }

        private void AddTreeCommands(LayoutControl layout, TreeListNode item, XElement element)
        {
            var up = ActionButton("Вверх");
            var down = ActionButton("Вниз");
            var remove = ActionButton("Удалить поле");
            var parentNode = (NativeSchemaNode)item.ParentNode.Tag;
            up.Enabled = MoveTarget(parentNode, element, true) != null;
            down.Enabled = MoveTarget(parentNode, element, false) != null;
            up.Click += (_, __) => MoveField(item, true);
            down.Click += (_, __) => MoveField(item, false);
            remove.Click += (_, __) => RemoveField(item, element);
            AddWide(layout, Toolbar(up, down, remove));
        }

        private void RemoveField(TreeListNode item, XElement element)
        {
            if (!CanEditFields() || item.ParentNode == null) return;
            if (XtraMessageBox.Show(this, $"Удалить поле {element.Name.LocalName} и его содержимое?", Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var parent = item.ParentNode;
            element.Remove();
            MarkChanged();
            RebuildBranch(parent);
        }

        private bool ShowTreeMenu(TreeListNode item, Point location)
        {
            if (!loaded || busy || !(item?.Tag is NativeSchemaNode node)) return false;
            xmlTree.FocusedNode = item;
            var canEdit = !ReadOnlyDocument && !rawDirty;
            var parent = item.ParentNode?.Tag as NativeSchemaNode;
            var menu = new DXPopupMenu(xmlTree);
            if (canEdit)
            {
                var add = new DXSubMenuItem("Добавить вложенное поле");
                foreach (var child in ChildCandidates(node))
                    add.Items.Add(new DXMenuItem(child.ToString(), (_, __) => AddChildField(item, node, child)));
                add.Enabled = add.Items.Count > 0;
                menu.Items.Add(add);
            }
            menu.Items.Add(new DXMenuItem("Копировать путь XML", (_, __) =>
            {
                try { Clipboard.SetText(XmlPath(node.Element)); }
                catch (ExternalException error)
                {
                    XtraMessageBox.Show(this, error.Message, "Буфер обмена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }));
            if (canEdit && parent != null)
            {
                menu.Items.Add(new DXMenuItem("Переместить вверх", (_, __) => MoveField(item, true))
                    { Enabled = MoveTarget(parent, node.Element, true) != null, BeginGroup = true });
                menu.Items.Add(new DXMenuItem("Переместить вниз", (_, __) => MoveField(item, false))
                    { Enabled = MoveTarget(parent, node.Element, false) != null });
                menu.Items.Add(new DXMenuItem("Удалить поле", (_, __) => RemoveField(item, node.Element)) { BeginGroup = true });
            }
            treeMenu?.Dispose();
            treeMenu = menu;
            menu.ShowPopup(xmlTree, location);
            return true;
        }

        private bool ShowFocusedTreeMenu()
        {
            var item = xmlTree.FocusedNode;
            if (item == null) return false;
            var row = xmlTree.GetVisibleIndexByNode(item) - xmlTree.TopVisibleNodeIndex;
            var y = Math.Max(0, Math.Min(xmlTree.Height - 1, (row + 1) * xmlTree.RowHeight));
            return ShowTreeMenu(item, new Point(18, y));
        }

        private void FieldToolTip(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == xmlTree && xmlTree.CalcHitInfo(e.ControlMousePosition).Node?.Tag is NativeSchemaNode node)
            {
                var path = XmlPath(node.Element);
                var description = node.Field?.Documentation ?? node.Type?.Documentation;
                e.Info = new ToolTipControlInfo(node, string.IsNullOrWhiteSpace(description) ? path : path + Environment.NewLine + description);
            }
            else if (e.SelectedControl == sectionBlocks)
            {
                var index = sectionBlocks.IndexFromPoint(e.ControlMousePosition);
                if (index < 0 || !(sectionBlocks.Items[index] is SectionBlock block)) return;
                var description = block.Node?.Field?.Documentation ?? block.Node?.Type?.Documentation ?? block.Field?.Field?.Documentation;
                e.Info = new ToolTipControlInfo(block, string.IsNullOrWhiteSpace(description) ? block.ToString() : description);
            }
        }

        private void MoveField(TreeListNode item, bool up)
        {
            if (!CanEditFields()) return;
            var element = ((NativeSchemaNode)item.Tag).Element;
            var parent = item.ParentNode;
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
            tabs.SelectedTabPage = xmlPage;
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
                var next = item.Nodes.Cast<TreeListNode>().Where(child => (child.Tag as NativeSchemaNode)?.Element.Name.LocalName == name)
                    .Skip(Math.Max(0, offset)).FirstOrDefault();
                if (next == null)
                {
                    missing = name;
                    break;
                }
                item = next;
            }
            tabs.SelectedTabPage = previousFieldsPage ?? sectionsPage;
            SelectTreeElement(((NativeSchemaNode)item.Tag).Element);
            if (missing != null && fieldEditors.TryGetValue(missing, out var editor)) FocusField(editor);
            else if (missing != null) SelectMissingField(missing);
            else if (fieldEditors.TryGetValue(".", out editor)) FocusField(editor);
            else if (childSelector != null) FocusField(childSelector);
        }

        private void FindField()
        {
            if (!CanUseDocument()) return;
            using (var dialog = new XtraForm
            {
                Text = "Найти поле", StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
                MinimumSize = new Size(690, 360), ClientSize = new Size(850, 480)
            })
            {
                var query = new TextEdit { Width = 270, Margin = new Padding(4, 3, 9, 3), AccessibleName = "Имя, путь XML или описание поля" };
                var go = ActionButton("Перейти");
                var close = ActionButton("Закрыть");
                close.DialogResult = DialogResult.Cancel;
                dialog.AcceptButton = go;
                dialog.CancelButton = close;
                var hint = WrappedLabelFor("Введите имя поля, часть пути XML или описание.");
                var matches = new GridControl { Dock = DockStyle.Fill, AccessibleName = "Найденные поля" };
                var view = ConfigureListGrid(matches);
                view.Columns.AddVisible(nameof(FieldSearchResult.Name), "Поле").Width = 165;
                view.Columns.AddVisible(nameof(FieldSearchResult.Path), "Путь XML").Width = 365;
                view.Columns.AddVisible(nameof(FieldSearchResult.Documentation), "Описание").Width = 295;
                WrapListColumns(view);
                var results = CreateLayout(Padding.Empty);
                AddLayoutItem(results, hint, AddLayoutRow(results));
                AddLayoutItem(results, matches, AddLayoutRow(results, SizeType.Percent, 100));
                dialog.Controls.Add(WithToolbar(results, LabelFor("Поиск"), query, go, close));
                go.Enabled = false;
                string target = null;
                void Choose()
                {
                    if (!(view.GetFocusedRow() is FieldSearchResult match)) return;
                    target = match.Path;
                    dialog.DialogResult = DialogResult.OK;
                }
                go.Click += (_, __) => Choose();
                OnGridActivate(view, Choose);
                view.FocusedRowChanged += (_, __) => go.Enabled = view.GetFocusedRow() is FieldSearchResult;
                query.TextChanged += (_, __) =>
                {
                    var terms = query.Text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    const int limit = 200;
                    var rows = terms.Length == 0 ? new List<FieldSearchResult>()
                        : SearchFields(profile.Root, document.Root, "/" + profile.Root.Name, terms,
                            new HashSet<NativeSchemaType>()).Take(limit + 1).ToList();
                    var hasMore = rows.Count > limit;
                    if (hasMore) rows.RemoveAt(limit);
                    matches.DataSource = rows;
                    if (rows.Count > 0) view.FocusedRowHandle = 0;
                    go.Enabled = rows.Count > 0;
                    hint.Text = terms.Length == 0 ? "Введите имя поля, часть пути XML или описание."
                        : rows.Count == 0 ? "Совпадений нет."
                        : hasMore ? $"Показаны первые {limit} совпадений. Уточните запрос."
                        : $"Найдено полей: {rows.Count}.";
                };
                dialog.Shown += (_, __) => query.Focus();
                if (dialog.ShowDialog(this) == DialogResult.OK) NavigateToField(target);
            }
        }

        private IEnumerable<FieldSearchResult> SearchFields(NativeSchemaParticle field, XElement element, string path,
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

        private static FieldSearchResult SearchMatch(string name, string path, string documentation, string[] terms)
        {
            documentation = documentation ?? "";
            if (!terms.All(term => path.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                || documentation.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)) return null;
            return new FieldSearchResult(name, path, documentation);
        }

        private sealed class FieldSearchResult
        {
            internal FieldSearchResult(string name, string path, string documentation)
            {
                Name = name;
                Path = path;
                Documentation = documentation;
            }
            public string Name { get; }
            public string Path { get; }
            public string Documentation { get; }
        }

        private void SelectChild(string name)
        {
            var option = childSelector?.Properties.Items.OfType<NativeSchemaChild>().FirstOrDefault(child => child.Field.Name == name);
            if (option == null) return;
            childSelector.SelectedItem = option;
            FocusField(childSelector);
        }

        private static void FocusField(Control editor)
        {
            if (editor.Parent is LayoutControl layout) layout.ScrollControlIntoView(editor);
            editor.Focus();
        }

        private void EditStructuredTable(TreeListNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            var edited = new XElement(node.Element);
            var alignment = profile.TypeOf("tAlignTypes")?.Enumeration ?? new List<NativeSchemaOption>();
            using (var dialog = new XtraForm
            {
                Text = "Таблица пояснительной записки", StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
                MinimumSize = new Size(740, 420), ClientSize = new Size(900, 540)
            })
            {
                var rowList = new ListBoxControl { Dock = DockStyle.Fill, AccessibleName = "Строки таблицы", ItemHeight = 27 };
                var cells = new GridControl { Dock = DockStyle.Fill, AccessibleName = "Ячейки выбранной строки" };
                var cellView = ConfigureListGrid(cells);
                cellView.OptionsBehavior.Editable = true;
                cellView.OptionsView.ShowIndicator = true;
                cellView.IndicatorWidth = 42;
                cellView.OptionsSelection.EnableAppearanceFocusedCell = true;
                cellView.OptionsCustomization.AllowSort = false;
                cellView.CustomDrawRowIndicator += (_, e) =>
                {
                    if (e.RowHandle >= 0) e.Info.DisplayText = (e.RowHandle + 1).ToString();
                };
                var valueColumn = cellView.Columns.AddVisible(nameof(TableCellRow.Value), "Значение");
                valueColumn.Width = 420;
                var alignColumn = cellView.Columns.AddVisible(nameof(TableCellRow.Alignment), "Выравнивание");
                alignColumn.Width = 175;
                var options = new[] { new NativeSchemaOption { Value = "", Label = "По умолчанию" } }.Concat(alignment);
                var alignEditor = new RepositoryItemLookUpEdit
                {
                    DataSource = options.Select(option => new
                    {
                        option.Value, Caption = string.IsNullOrWhiteSpace(option.Label) ? option.Value : option.Label
                    }).ToList(),
                    DisplayMember = "Caption",
                    ValueMember = nameof(NativeSchemaOption.Value),
                    NullText = "По умолчанию",
                    ShowHeader = false,
                    ShowFooter = false
                };
                alignEditor.Columns.Add(new LookUpColumnInfo("Caption"));
                cells.RepositoryItems.Add(alignEditor);
                alignColumn.ColumnEdit = alignEditor;

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
                var rebuildingRows = false;
                DXPopupMenu tableMenu = null;

                void UpdateTableActions()
                {
                    removeRow.Enabled = addCell.Enabled = rowList.SelectedIndex >= 0;
                    removeCell.Enabled = cellView.GetFocusedRow() is TableCellRow;
                    saveTable.Enabled = edited.Elements("Row").Any() && rows.All(row => row.Element("Cell") != null);
                    hint.Text = saveTable.Enabled ? ""
                        : "Нужна хотя бы одна строка данных и хотя бы одна ячейка в каждой строке.";
                }

                void LoadCells()
                {
                    var row = rowList.SelectedIndex < 0 ? null : rows[rowList.SelectedIndex];
                    cells.DataSource = row?.Elements("Cell").Select(cell =>
                    {
                        var align = (string)cell.Attribute("align");
                        return new TableCellRow(cell, alignment.Any(option => option.Value == align) ? align : "");
                    }).ToList() ?? new List<TableCellRow>();
                    UpdateTableActions();
                }

                void LoadRows(XElement selection = null)
                {
                    rows.Clear();
                    rows.AddRange(edited.Elements().Where(row => row.Name == "TitleRow" || row.Name == "Row"));
                    rebuildingRows = true;
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
                        rebuildingRows = false;
                    }
                    LoadCells();
                }

                void AddTableRow(bool title)
                {
                    if (!CommitCellEdit()) return;
                    var row = new XElement(title ? "TitleRow" : "Row", new XElement("Cell"));
                    var firstData = title ? edited.Element("Row") : null;
                    if (firstData == null) edited.Add(row);
                    else firstData.AddBeforeSelf(row);
                    LoadRows(row);
                }

                bool CommitCellEdit() => cellView.ActiveEditor == null || cellView.PostEditor() && cellView.UpdateCurrentRow();

                XElement AdjacentRow(bool up)
                {
                    var index = rowList.SelectedIndex;
                    if (index < 0) return null;
                    var adjacent = index + (up ? -1 : 1);
                    return adjacent >= 0 && adjacent < rows.Count && rows[index].Name == rows[adjacent].Name
                        ? rows[adjacent] : null;
                }

                void MoveRow(bool up)
                {
                    var target = AdjacentRow(up);
                    if (target == null || !CommitCellEdit()) return;
                    var row = rows[rowList.SelectedIndex];
                    row.Remove();
                    if (up) target.AddBeforeSelf(row);
                    else target.AddAfterSelf(row);
                    LoadRows(row);
                }

                XElement AdjacentCell(bool left)
                {
                    var cell = (cellView.GetFocusedRow() as TableCellRow)?.Element;
                    return left ? cell?.ElementsBeforeSelf("Cell").LastOrDefault() : cell?.ElementsAfterSelf("Cell").FirstOrDefault();
                }

                void MoveCell(bool left)
                {
                    var target = AdjacentCell(left);
                    if (target == null || !CommitCellEdit()) return;
                    var cell = ((TableCellRow)cellView.GetFocusedRow()).Element;
                    var index = cellView.GetDataSourceRowIndex(cellView.FocusedRowHandle);
                    cell.Remove();
                    if (left) target.AddBeforeSelf(cell);
                    else target.AddAfterSelf(cell);
                    LoadCells();
                    cellView.FocusedRowHandle = cellView.GetRowHandle(index + (left ? -1 : 1));
                }

                DXMenuItem ButtonCommand(SimpleButton button, bool beginGroup = false)
                    => new DXMenuItem(button.Text, (_, __) => button.PerformClick()) { Enabled = button.Enabled, BeginGroup = beginGroup };

                void ShowTableMenu(Control owner, Point location)
                {
                    tableMenu?.Dispose();
                    tableMenu = new DXPopupMenu(owner);
                    if (owner == rowList)
                    {
                        tableMenu.Items.Add(ButtonCommand(addTitle));
                        tableMenu.Items.Add(ButtonCommand(addRow));
                        tableMenu.Items.Add(new DXMenuItem("Переместить строку вверх", (_, __) => MoveRow(true))
                            { Enabled = AdjacentRow(true) != null, BeginGroup = true });
                        tableMenu.Items.Add(new DXMenuItem("Переместить строку вниз", (_, __) => MoveRow(false))
                            { Enabled = AdjacentRow(false) != null });
                        tableMenu.Items.Add(ButtonCommand(removeRow, true));
                    }
                    else
                    {
                        tableMenu.Items.Add(ButtonCommand(addCell));
                        tableMenu.Items.Add(new DXMenuItem("Переместить ячейку влево", (_, __) => MoveCell(true))
                            { Enabled = AdjacentCell(true) != null, BeginGroup = true });
                        tableMenu.Items.Add(new DXMenuItem("Переместить ячейку вправо", (_, __) => MoveCell(false))
                            { Enabled = AdjacentCell(false) != null });
                        tableMenu.Items.Add(ButtonCommand(removeCell, true));
                    }
                    tableMenu.ShowPopup(owner, location);
                }

                void TableMenuKey(Control owner, KeyEventArgs e)
                {
                    if (e.KeyData != Keys.Apps && e.KeyData != (Keys.Shift | Keys.F10)) return;
                    e.SuppressKeyPress = true;
                    var location = owner == rowList && rowList.SelectedIndex >= 0
                        ? rowList.GetItemRectangle(rowList.SelectedIndex).Location : new Point(15, 30);
                    ShowTableMenu(owner, location);
                }

                rowList.SelectedIndexChanged += (_, __) =>
                {
                    if (!rebuildingRows && CommitCellEdit()) LoadCells();
                };
                rowList.MouseUp += (_, e) =>
                {
                    if (e.Button != MouseButtons.Right) return;
                    var index = rowList.IndexFromPoint(e.Location);
                    if (index >= 0) rowList.SelectedIndex = index;
                    ShowTableMenu(rowList, e.Location);
                };
                cells.MouseUp += (_, e) =>
                {
                    if (e.Button != MouseButtons.Right) return;
                    var hit = cellView.CalcHitInfo(e.Location);
                    if (hit.InColumn) return;
                    if (hit.InRow) cellView.FocusedRowHandle = hit.RowHandle;
                    ShowTableMenu(cells, e.Location);
                };
                rowList.KeyDown += (_, e) => TableMenuKey(rowList, e);
                cellView.KeyDown += (_, e) => TableMenuKey(cells, e);
                dialog.FormClosed += (_, __) => tableMenu?.Dispose();
                cellView.FocusedRowChanged += (_, __) => UpdateTableActions();
                cellView.CellValueChanged += (_, e) =>
                {
                    if (!(cellView.GetRow(e.RowHandle) is TableCellRow row)) return;
                    var value = Convert.ToString(e.Value);
                    if (e.Column == valueColumn) SetText(row.Element, value);
                    else row.Element.SetAttributeValue("align", value.Length == 0 ? null : value);
                };
                addTitle.Click += (_, __) => AddTableRow(true);
                addRow.Click += (_, __) => AddTableRow(false);
                removeRow.Click += (_, __) =>
                {
                    if (rowList.SelectedIndex < 0 || !CommitCellEdit()) return;
                    var index = rowList.SelectedIndex;
                    rows[index].Remove();
                    var selection = rows.Count <= 1 ? null : rows[index == rows.Count - 1 ? index - 1 : index + 1];
                    LoadRows(selection);
                };
                addCell.Click += (_, __) =>
                {
                    if (rowList.SelectedIndex < 0 || !CommitCellEdit()) return;
                    rows[rowList.SelectedIndex].Add(new XElement("Cell"));
                    LoadCells();
                    cellView.FocusedRowHandle = cellView.GetRowHandle(cellView.DataRowCount - 1);
                    cellView.FocusedColumn = valueColumn;
                    cells.Focus();
                    cellView.ShowEditor();
                };
                removeCell.Click += (_, __) =>
                {
                    if (!(cellView.GetFocusedRow() is TableCellRow row) || !CommitCellEdit()) return;
                    row.Element.Remove();
                    LoadCells();
                };
                saveTable.Click += (_, __) =>
                {
                    if (CommitCellEdit() && saveTable.Enabled) dialog.DialogResult = DialogResult.OK;
                };

                var body = CreateLayout(new Padding(8),
                    new ColumnDefinition { SizeType = SizeType.Absolute, Width = 200 },
                    new ColumnDefinition { SizeType = SizeType.Percent, Width = 100 });
                AddLayoutItem(body, Toolbar(addTitle, addRow, removeRow, addCell, removeCell), AddLayoutRow(body), columnSpan: 2);
                var contentRow = AddLayoutRow(body, SizeType.Percent, 100);
                AddLayoutItem(body, rowList, contentRow);
                AddLayoutItem(body, cells, contentRow, 1);
                AddLayoutItem(body, Toolbar(saveTable, cancel, hint), AddLayoutRow(body), columnSpan: 2);
                dialog.Controls.Add(body);

                if (!edited.Elements().Any()) edited.Add(new XElement("Row", new XElement("Cell")));
                LoadRows();
                if (dialog.ShowDialog(this) != DialogResult.OK || XNode.DeepEquals(node.Element, edited)) return;
                node.Element.ReplaceNodes(edited.Nodes());
                MarkChanged();
                RebuildBranch(item);
            }
        }

        private sealed class TableCellRow
        {
            internal TableCellRow(XElement element, string alignment)
            {
                Element = element;
                Value = element.Value;
                Alignment = alignment;
            }
            internal XElement Element { get; }
            public string Value { get; set; }
            public string Alignment { get; set; }
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

        private void AddFileAndLookupControls(LayoutControl layout, TreeListNode item, NativeSchemaNode node)
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
                var lookup = ActionButton(organization ? "Найти по ИНН в DaData…" : "Найти адрес в DaData…");
                lookup.Click += async (_, __) => await LookupAsync(item, node, organization);
                actions.Add(lookup);
            }
            if (actions.Count > 0) AddWide(layout, Toolbar(actions.ToArray()));
        }

        private async Task ChooseAttachmentAsync(TreeListNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            using (var picker = new XtraOpenFileDialog { CheckFileExists = true })
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

        private async Task ChooseImageAsync(TreeListNode item, NativeSchemaNode node)
        {
            if (!CanEditFields()) return;
            using (var picker = new XtraOpenFileDialog { Filter = "Изображения|*.png;*.jpg;*.jpeg;*.gif", CheckFileExists = true })
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

        private async Task LookupAsync(TreeListNode item, NativeSchemaNode node, bool organization)
        {
            if (!CanEditFields()) return;
            if (!organization)
            {
                var address = AddressLookupDialog.ShowFor(this);
                if (address != null && !IsDisposed && !Disposing)
                    ApplyLookupResult(item, node, ConclusionDaData.MapAddress(address));
                return;
            }

            var query = XtraInputBox.Show("Введите ИНН юридического лица", "DaData", (string)node.Element.Element("INN") ?? "");
            query = query?.Trim();
            if (string.IsNullOrWhiteSpace(query)) return;
            if (!ConclusionDaData.IsDigits(query, 10))
            {
                status.Text = "ИНН юридического лица должен содержать 10 цифр.";
                return;
            }
            if (HasDifferentIdentifier(node.Element, "INN", query))
            {
                status.Text = LookupInnConflict;
                return;
            }
            using (var cancellation = new CancellationTokenSource())
            {
                EventHandler cancel = (_, __) => cancellation.Cancel();
                Disposed += cancel;
                try
                {
                    await RunAsync("Поиск в DaData…", async () =>
                    {
                        var values = await ConclusionDaData.FindOrganizationAsync(query, ((string)node.Element.Element("KPP"))?.Trim(), cancellation.Token);
                        if (cancellation.IsCancellationRequested || IsDisposed || Disposing) return;
                        ApplyLookupResult(item, node, values);
                    });
                }
                finally
                {
                    Disposed -= cancel;
                }
            }
        }

        private void ApplyLookupResult(TreeListNode item, NativeSchemaNode node, Dictionary<string, string> values)
        {
            if (values == null)
            {
                status.Text = "DaData не вернула подходящих данных. Заполните поля вручную.";
                return;
            }
            var populated = FillLookupFields(node, values, profile);
            if (populated)
            {
                MarkChanged();
                RebuildBranch(item);
            }
            status.Text = populated ? "Пустые поля заполнены из DaData." : "Подходящих пустых полей нет.";
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
            if (values.TryGetValue("KPP", out var kpp) && HasDifferentIdentifier(node.Element, "KPP", kpp))
                throw new InvalidOperationException("КПП найденной организации отличается от КПП в документе.");

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
