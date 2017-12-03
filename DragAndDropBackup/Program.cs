using System;
using System.Windows.Forms;

namespace DragAndDropBackup {

    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();

            foreach (string arg in args) {
                string currentArg = arg.Trim();
                if (currentArg == "--autocopy") {
                    mainForm.Settings.CurrentSettings.Arguments.DoAutocopy = true;
                }
                if (currentArg == "--elevate") {
                    mainForm.Settings.CurrentSettings.Arguments.Elevate = true;
                }
            }
  
            Application.Run(mainForm);
        }

    }

}
