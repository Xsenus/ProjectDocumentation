using Core.NET.Controllers;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using ProjectDocumentation.NET.Forms;
using ProjectDocumentation.NET.Services;
using System;
using System.Windows.Forms;

namespace ProjectDocumentation.NET
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SessionController.GetSession();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureAppearance();
            Application.Run(new MainForm());
        }

        internal static void ConfigureAppearance()
        {
            if (!SystemInformation.HighContrast)
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.WXI);

            var icon = Properties.Resources.icon;
            FormIconOptions.Default.Image = icon;
            FormIconOptions.Default.LargeImage = icon;
            FileDialogLocalization.Configure();
            WindowsFormsSettings.UseDXDialogs = DefaultBoolean.True;
        }
    }
}
