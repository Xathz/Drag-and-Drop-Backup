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
                if (arg.Trim() == "--autocopy") {
                    mainForm.DoAutocopy = true;
                }
            }
  
            Application.Run(mainForm);
        }

    }

}
