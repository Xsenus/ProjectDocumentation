using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Forms
{
    internal static class GridContextMenus
    {
        internal static void Attach(GridView view, PopupMenu menu, BarItem add = null)
        {
            Attach(view, hasRow =>
            {
                if (!hasRow && add == null) return null;
                foreach (BarItemLink link in menu.ItemLinks)
                    link.Visible = hasRow || link.Item == add;
                return menu;
            });
        }

        internal static void Attach(GridView view, Func<bool, PopupMenu> menuForRow)
        {
            var grid = view.GridControl;
            view.PopupMenuShowing += (_, e) =>
            {
                var hit = e.HitInfo;
                if (hit == null) return;

                var onRow = hit.InDataRow && view.IsDataRow(hit.RowHandle);
                var onBackground = e.MenuType == GridMenuType.User &&
                    (hit.HitTest == GridHitTest.None || hit.HitTest == GridHitTest.EmptyRow);
                if (!onRow && !onBackground) return;
                if (onRow) view.FocusedRowHandle = hit.RowHandle;

                var menu = menuForRow(onRow && view.FocusedRowHandle == hit.RowHandle);
                if (menu != null) e.ShowCustomMenu(menu);
            };
            grid.ProcessGridKey += (_, e) =>
            {
                if (grid.FocusedView != view || (e.KeyData != Keys.Apps && e.KeyData != (Keys.Shift | Keys.F10))) return;

                var row = view.FocusedRowHandle;
                var menu = menuForRow(view.IsDataRow(row));
                if (menu == null) return;

                var info = (view.GetViewInfo() as GridViewInfo)?.GetGridRowInfo(row);
                var location = info == null ? new Point(12, 30) : new Point(info.Bounds.Left + 12, info.Bounds.Bottom);
                menu.ShowPopup(grid.PointToScreen(location));
                e.Handled = true;
            };
        }
    }
}
