using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms.ObjectChange
{
    internal static class ClassicRequiredFields
    {
        private const string RequiredSuffix = " *";
        private static readonly Color RequiredColor = Color.FromArgb(153, 35, 35);
        private static readonly ConditionalWeakTable<BaseLayoutItem, CaptionStyle> OriginalStyles =
            new ConditionalWeakTable<BaseLayoutItem, CaptionStyle>();

        internal static void HighlightExisting(Control container)
        {
            if (container is LayoutControl layout)
            {
                foreach (BaseLayoutItem item in layout.Items)
                    if (HasMarker(item.Text)) UpdateAppearance(item, true);
            }
            foreach (Control child in container.Controls) HighlightExisting(child);
        }

        internal static void Mark(params Control[] controls)
        {
            foreach (var control in controls) SetRequired(GetItem(control), true);
        }

        internal static void Unmark(params Control[] controls)
        {
            foreach (var control in controls) SetRequired(GetItem(control), false);
        }

        internal static void MarkTabs(params XtraTabPage[] tabs)
        {
            foreach (var tab in tabs)
            {
                if (!HasMarker(tab.Text)) tab.Text = tab.Text.TrimEnd() + RequiredSuffix;
            }
        }

        internal static void MarkGroups(params BaseLayoutItem[] items)
        {
            foreach (var item in items) SetRequired(item, true);
        }

        internal static void MarkWhen(BaseEdit trigger, Func<bool> required, params Control[] controls)
        {
            void Update()
            {
                var mark = required();
                foreach (var control in controls) SetRequired(GetItem(control), mark);
            }

            trigger.EditValueChanged += (_, __) => Update();
            Update();
        }

        internal static void Focus(Control control)
        {
            for (var parent = control.Parent; parent != null; parent = parent.Parent)
            {
                if (parent is XtraTabPage page && page.Parent is XtraTabControl tabs && page.PageVisible)
                    tabs.SelectedTabPage = page;
            }

            for (var item = GetItem(control); item != null; item = item.Parent)
            {
                if (item is LayoutControlGroup group)
                {
                    var tabs = group.ParentTabbedGroup;
                    if (tabs != null) tabs.SelectedTabPage = group;
                    group.Expanded = true;
                }
            }
            control.Focus();
        }

        private static BaseLayoutItem GetItem(Control control)
        {
            for (var child = control; child.Parent != null; child = child.Parent)
            {
                if (child.Parent is LayoutControl layout) return layout.GetItemByControl(child);
            }
            return null;
        }

        private static void SetRequired(BaseLayoutItem item, bool required)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Text)) return;
            var text = item.Text.TrimEnd();
            var marked = HasMarker(text);
            if (required && !marked) item.Text = text + RequiredSuffix;
            if (!required && marked) item.Text = text.Substring(0, text.Length - 1).TrimEnd();
            UpdateAppearance(item, required);
        }

        private static bool HasMarker(string text)
            => text?.TrimEnd().EndsWith("*", StringComparison.Ordinal) == true;

        private static void UpdateAppearance(BaseLayoutItem item, bool required)
        {
            var caption = item.AppearanceItemCaption;
            if (required && !SystemInformation.HighContrast)
            {
                OriginalStyles.GetValue(item, _ => new CaptionStyle(caption.ForeColor, caption.Options.UseForeColor));
                caption.ForeColor = RequiredColor;
                caption.Options.UseForeColor = true;
            }
            else if (OriginalStyles.TryGetValue(item, out var original))
            {
                caption.ForeColor = original.Color;
                caption.Options.UseForeColor = original.UseColor;
                OriginalStyles.Remove(item);
            }
        }

        private sealed class CaptionStyle
        {
            internal CaptionStyle(Color color, bool useColor)
            {
                Color = color;
                UseColor = useColor;
            }

            internal Color Color { get; }
            internal bool UseColor { get; }
        }
    }
}
