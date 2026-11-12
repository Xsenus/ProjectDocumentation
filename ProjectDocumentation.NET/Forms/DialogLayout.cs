using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;
using Padding = System.Windows.Forms.Padding;

namespace ProjectDocumentation.NET.Forms
{
    internal static class DialogLayout
    {
        internal static void ShowRowFocus(GridView view)
        {
            if (!SystemInformation.HighContrast) view.FocusRectStyle = DrawFocusRectStyle.None;
            view.OptionsSelection.EnableAppearanceFocusedCell = false;
            view.OptionsHint.ShowCellHints = true;
        }

        internal static void AddActionFooter(XtraForm form, SimpleButton save, SimpleButton cancel)
        {
            var footer = new TablePanel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                Padding = new Padding(8, 6, 8, 6),
                TabIndex = 1,
                Name = "dialogActions"
            };
            footer.Columns.AddRange(new[]
            {
                new TablePanelColumn(TablePanelEntityStyle.Relative, 1),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 136),
                new TablePanelColumn(TablePanelEntityStyle.Absolute, 136)
            });
            footer.Rows.Add(new TablePanelRow(TablePanelEntityStyle.Relative, 1));
            footer.Controls.AddRange(new Control[] { save, cancel });
            PlaceAction(footer, save, 1);
            PlaceAction(footer, cancel, 2);
            form.Controls.Add(footer);
            footer.SendToBack();
        }

        private static void PlaceAction(TablePanel footer, SimpleButton button, int column)
        {
            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(6, 0, 0, 0);
            button.TabIndex = column - 1;
            footer.SetCell(button, 0, column);
        }
    }
}
