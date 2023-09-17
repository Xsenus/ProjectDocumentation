using ProjectDocumentation.Forms;
using ProjectDocumentation.Forms.ObjectChange;
using System;
using System.Windows.Forms;

namespace ProjectDocumentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConclusionEdit());
        }
    }
}