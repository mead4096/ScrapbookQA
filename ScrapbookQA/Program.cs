using System;
using System.Windows.Forms;

namespace ScrapbookQA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application-wide settings
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Allows for renaming of buttons on Windows Forms' defaults msg boxes
            MessageBoxManager.Yes = "Quit";
            MessageBoxManager.No = "New Message";
            MessageBoxManager.Register();

            FormsManager manager = new FormsManager();
            manager.MainLoop(); //Begins showing forms
        }
    }
}
