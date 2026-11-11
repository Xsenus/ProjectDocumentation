using Core.NET.Controllers;
using DevExpress.LookAndFeel;
using ProjectDocumentation.NET.Forms;
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
            if (!SystemInformation.HighContrast)
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.WXI);
            Application.Run(new MainForm());
        }
    }
}
