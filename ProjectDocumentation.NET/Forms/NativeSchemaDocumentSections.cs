using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProjectDocumentation.NET.Forms
{
    internal partial class SchemaDocumentForm
    {
        private readonly XtraTabControl sectionTabs = new XtraTabControl { Dock = DockStyle.Fill };
        private readonly ListBoxControl sectionBlocks = new ListBoxControl { Dock = DockStyle.Fill, HorizontalScrollbar = true };
        private readonly LabelControl sectionLocation = WrappedLabelFor("");
        private readonly SimpleButton sectionBack = ActionButton("Назад");
        private readonly SimpleButton sectionOpen = ActionButton("Вложенные поля");
        private readonly Dictionary<string, XtraTabPage> sectionByField = new Dictionary<string, XtraTabPage>();
        private SplitContainerControl sectionBody;
        private XtraTabPage previousFieldsPage;
        private XtraTabPage otherSection;
        private XElement sectionParent;
        private bool updatingSections;

        private bool IsSectionView => tabs.SelectedTabPage == sectionsPage;

        private Control CreateSectionsPanel()
        {
            if (family == "explanatorynote")
            {
                AddSection("Реквизиты", "ExplanatoryNoteNumber", "ExplanatoryNoteYear", "ExplanatoryNoteModifications");
                AddSection("Авторы и участники", "IssueAuthor", "ProjectDocumentationAuthors", "Signers", "Developer", "TechnicalCustomer");
                AddSection("Объект строительства", "NonIndustrialObject", "IndustrialObject", "LinearObject");
                AddSection("Документы и нормы", "UsedNorms", "ProjectDecisionDocuments", "ProjectInitialDocuments", "EngineeringSurveyDocuments", "AdditionalData");
                AddSection("Финансирование", "FinanceSources");
                AddSection("Пояснения и заверение", "UsedAbbreviations", "DesignerNote", "DesignerAssurance");
            }
            else
            {
                AddSection("Общие сведения", "ExpertOrganization", "Approver", "ExaminationObject", "Declarant", "EcologyExpertise");
                AddSection("Объект и условия", "Object", "ClimateConditions", "ClimateConditionsNote", "CadastralNumber");
                AddSection("Представленные документы", "Documents", "PreviousConclusions", "PreviousSimpleConclusions", "EEPDUse");
                AddSection("Участники", "Designer", "ProjectDocumentsDeveloper", "ProjectDocumentsTechnicalCustomer",
                    "ProjectDocumentsOtherOrganization", "EngineeringSurveyDeveloper", "EngineeringSurveyTechnicalCustomer",
                    "EngineeringSurveyOtherOrganization", "EngineeringSurveyAddress");
                AddSection("Финансирование", "Finance", "FinanceComment", "EstimatedCost");
                AddSection("Рассмотрение", "ExpertEngineeringSurveys", "ExpertProjectDocuments", "ExpertEstimate");
                AddSection("Выводы и эксперты", "Summary", "Experts");
            }

            sectionBody = CreateEditorSplit();
            sectionTabs.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold;
            sectionBlocks.AccessibleName = "Блоки раздела";
            sectionBlocks.ItemHeight = 27;
            sectionBlocks.ToolTipController = fieldToolTips;
            sectionLocation.Appearance.FontStyleDelta = FontStyle.Bold;

            var list = CreateLayout(Padding.Empty);
            AddLayoutItem(list, sectionLocation, AddLayoutRow(list));
            AddLayoutItem(list, sectionBlocks, AddLayoutRow(list, SizeType.Percent, 100));
            AddLayoutItem(list, Toolbar(sectionBack, sectionOpen), AddLayoutRow(list));
            sectionBody.Panel1.Controls.Add(list);
            sectionTabs.SelectedPageChanged += (_, __) =>
            {
                MountSectionBody();
                if (!loaded || updatingSections) return;
                sectionParent = document.Root;
                SelectTreeElement(sectionParent);
            };
            sectionBlocks.SelectedIndexChanged += (_, __) => SelectSectionBlock();
            sectionBlocks.DoubleClick += (_, __) => OpenSectionBlock();
            sectionOpen.Click += (_, __) => OpenSectionBlock();
            sectionBack.Click += (_, __) => BackFromSectionBlock();
            MountSectionBody();
            return sectionTabs;
        }

        private void AddSection(string name, params string[] fields)
        {
            var page = new XtraTabPage { Text = name };
            sectionTabs.TabPages.Add(page);
            foreach (var field in fields) sectionByField.Add(field, page);
        }

        private XtraTabPage SectionFor(string field)
        {
            if (sectionByField.TryGetValue(field, out var section)) return section;
            if (otherSection == null)
            {
                otherSection = new XtraTabPage { Text = "Прочие поля" };
                sectionTabs.TabPages.Add(otherSection);
            }
            return otherSection;
        }

        private void MountSectionBody()
        {
            var page = sectionTabs.SelectedTabPage;
            if (page != null && sectionBody.Parent != page) page.Controls.Add(sectionBody);
        }

        private void ShowFieldsView()
        {
            previousFieldsPage = tabs.SelectedTabPage;
            var host = IsSectionView ? sectionBody.Panel2 : treeDetailsHost;
            if (details.Parent != host) host.Controls.Add(details);
            if (IsSectionView) MountSectionBody();
            ShowSelectedFields(xmlTree.FocusedNode);
        }

        private void ShowSelectedFields(TreeListNode item)
        {
            if (!(item?.Tag is NativeSchemaNode node)) return;
            if (IsSectionView && !updatingSections)
            {
                var top = node.Element.AncestorsAndSelf().FirstOrDefault(element => element.Parent == document.Root);
                if (top != null) SelectSectionTab(SectionFor(top.Name.LocalName));
                if (!ReferenceEquals(sectionParent, node.Element))
                    sectionParent = node.Element.Parent ?? document.Root;
                RefreshSectionBlocks(node.Element.Parent == sectionParent ? node.Element : null);
            }
            ShowFields(item);
        }

        private void SelectSectionTab(XtraTabPage page)
        {
            if (sectionTabs.SelectedTabPage == page) return;
            var previous = updatingSections;
            updatingSections = true;
            try { sectionTabs.SelectedTabPage = page; }
            finally { updatingSections = previous; }
        }

        private void RefreshSectionBlocks(XElement selected = null)
        {
            var parentItem = FindTreeElement(sectionParent);
            if (!(parentItem?.Tag is NativeSchemaNode parent)) return;
            var root = parent.Element == document.Root;
            var previous = updatingSections;
            updatingSections = true;
            var selectedIndex = -1;
            sectionBlocks.BeginUpdate();
            try
            {
                sectionBlocks.Items.Clear();
                sectionLocation.Text = root ? sectionTabs.SelectedTabPage.Text : XmlPath(parent.Element);
                foreach (var element in parent.Element.Elements().Where(element => !root || SectionFor(element.Name.LocalName) == sectionTabs.SelectedTabPage))
                {
                    var node = profile.ChildNode(parent, element);
                    var label = SectionCaption(node.Field, element.Name.LocalName);
                    var occurrence = element.ElementsBeforeSelf(element.Name).Count();
                    if (occurrence > 0 || element.ElementsAfterSelf(element.Name).Any()) label += $" [{occurrence + 1}]";
                    var block = new SectionBlock(label, node, null);
                    sectionBlocks.Items.Add(block);
                    if (ReferenceEquals(element, selected)) selectedIndex = sectionBlocks.Items.Count - 1;
                }

                if (!ReadOnlyDocument)
                {
                    foreach (var field in VisibleChildren(parent).GroupBy(candidate => candidate.Field.Name).Select(group => group.First()))
                    {
                        if (parent.Element.Element(field.Field.Name) == null)
                            sectionBlocks.Items.Add(new SectionBlock("Добавить: " + SectionCaption(field.Field, field.Field.Name), null, field));
                    }
                }
            }
            finally
            {
                sectionBlocks.EndUpdate();
                sectionBlocks.SelectedIndex = selectedIndex;
                updatingSections = previous;
            }
            UpdateSectionButtons();
        }

        private void UpdateSectionButtons()
        {
            var selectedElement = (xmlTree.FocusedNode?.Tag as NativeSchemaNode)?.Element;
            sectionBack.Enabled = sectionParent != null && (sectionParent.Parent != null || !ReferenceEquals(selectedElement, sectionParent));
            var selected = (sectionBlocks.SelectedItem as SectionBlock)?.Node;
            sectionOpen.Enabled = selected != null && (selected.Type?.Particle != null || selected.Element.HasElements);
        }

        private void BackFromSectionBlock()
        {
            if (sectionParent == null) return;
            var selected = (xmlTree.FocusedNode?.Tag as NativeSchemaNode)?.Element;
            if (!ReferenceEquals(selected, sectionParent))
            {
                SelectTreeElement(sectionParent);
                return;
            }
            if (sectionParent.Parent == null) return;
            var previous = sectionParent;
            sectionParent = previous.Parent;
            SelectTreeElement(previous);
        }

        private void SelectSectionBlock()
        {
            if (updatingSections || !(sectionBlocks.SelectedItem is SectionBlock block)) return;
            if (block.Node != null)
            {
                SelectTreeElement(block.Node.Element);
            }
            else
            {
                updatingSections = true;
                try
                {
                    SelectTreeElement(sectionParent);
                    FocusChildEditor(block.Field.Field.Name);
                }
                finally
                {
                    updatingSections = false;
                }
            }
            UpdateSectionButtons();
        }

        private void OpenSectionBlock()
        {
            if (!sectionOpen.Enabled || !(sectionBlocks.SelectedItem is SectionBlock block) || block.Node == null) return;
            sectionParent = block.Node.Element;
            SelectTreeElement(sectionParent);
        }

        private void SelectTreeElement(XElement element)
        {
            var item = FindTreeElement(element);
            if (item == null) return;
            if (xmlTree.FocusedNode != item) xmlTree.FocusedNode = item;
            else ShowSelectedFields(item);
            xmlTree.MakeNodeVisible(item);
        }

        private TreeListNode FindTreeElement(XElement element)
        {
            if (element == null || xmlTree.Nodes.Count == 0) return null;
            var item = xmlTree.Nodes[0];
            if (!ReferenceEquals(element.Document, document)) return null;
            foreach (var child in element.AncestorsAndSelf().Reverse().Skip(1))
            {
                PopulateTree(item);
                item = item.Nodes.Cast<TreeListNode>().FirstOrDefault(candidate => ReferenceEquals((candidate.Tag as NativeSchemaNode)?.Element, child));
                if (item == null) return null;
            }
            return item;
        }

        private IEnumerable<NativeSchemaChild> VisibleChildren(NativeSchemaNode node)
            => IsNil(node.Element) ? Enumerable.Empty<NativeSchemaChild>() : IsSectionView && node.Element == document.Root
                ? node.Children.Where(child => SectionFor(child.Field.Name) == sectionTabs.SelectedTabPage)
                : node.Children;

        private bool ShowRootAttributes(NativeSchemaNode node)
            => !IsSectionView || node.Element != document.Root || sectionTabs.SelectedTabPageIndex == 0;

        private void SelectMissingField(string name)
        {
            if (IsSectionView && xmlTree.FocusedNode?.ParentNode == null)
            {
                SelectSectionTab(name.StartsWith("@", StringComparison.Ordinal) ? sectionTabs.TabPages[0] : SectionFor(name));
                ShowSelectedFields(xmlTree.FocusedNode);
            }
            FocusChildEditor(name);
        }

        private void FocusChildEditor(string name)
        {
            if (fieldEditors.TryGetValue(name, out var editor)) editor.Focus();
            else SelectChild(name);
        }

        private void AddSectionValues(LayoutControl layout, TreeListNode item, NativeSchemaNode node)
        {
            foreach (var field in VisibleChildren(node).Where(child => child.Capacity == 1 && !child.InsideChoice))
            {
                var type = profile.TypeOf(field.Field.Type);
                if (type?.Kind != "simple" || type.Base == "base64Binary") continue;
                var element = node.Element.Element(field.Field.Name);
                if (element?.HasElements == true || (element == null && ReadOnlyDocument)) continue;
                var value = element?.Value ?? "";
                var label = SectionCaption(field.Field, field.Field.Name) + (field.Required ? " *" : "");
                var editor = ValueEditor(value, type, ReadOnlyDocument || field.Field.Fixed != null,
                    type.Base == "string" && (value.Length > 100 || field.Field.Name == "Text"), changed =>
                    {
                        var newElement = element == null;
                        if (newElement && changed.Length == 0) return;
                        if (newElement) element = node.AddChild(field);
                        SetText(element, changed);
                        UpdateInlineTreeNode(item, node, element);
                        if (newElement && ReferenceEquals(sectionParent, node.Element)) RefreshSectionBlocks();
                        MarkChanged();
                    });
                fieldEditors[field.Field.Name] = editor;
                AddField(layout, label, editor, null);
            }
        }

        private void UpdateInlineTreeNode(TreeListNode parent, NativeSchemaNode node, XElement element)
        {
            PopulateTree(parent);
            var target = parent.Nodes.Cast<TreeListNode>().FirstOrDefault(item => ReferenceEquals((item.Tag as NativeSchemaNode)?.Element, element));
            if (target != null)
            {
                target.SetValue(0, TreeLabel((NativeSchemaNode)target.Tag));
                return;
            }
            var following = element.ElementsAfterSelf().FirstOrDefault();
            var next = parent.Nodes.Cast<TreeListNode>().FirstOrDefault(item => ReferenceEquals((item.Tag as NativeSchemaNode)?.Element, following));
            target = AppendTreeNode(profile.ChildNode(node, element), parent);
            if (next != null) xmlTree.SetNodeIndex(target, xmlTree.GetNodeIndex(next));
        }

        private static string SectionCaption(NativeSchemaParticle field, string fallback)
        {
            var text = field?.Documentation?.Trim();
            if (string.IsNullOrEmpty(text)) return fallback;
            var sentence = text.IndexOf(". ", StringComparison.Ordinal);
            if (sentence > 0) text = text.Substring(0, sentence);
            return text.Length > 82 ? text.Substring(0, 81) + "…" : text;
        }

        private sealed class SectionBlock
        {
            private readonly string label;

            public SectionBlock(string label, NativeSchemaNode node, NativeSchemaChild field)
            {
                this.label = label;
                Node = node;
                Field = field;
            }

            public NativeSchemaNode Node { get; }
            public NativeSchemaChild Field { get; }
            public override string ToString() => label;
        }
    }
}
