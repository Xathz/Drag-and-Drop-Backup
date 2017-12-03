using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace DragAndDropBackup {

    public partial class MainForm : Form {

        public Settings Settings = new Settings();
        private Point _FirstPoint;
        private bool _IsMouseDown = false;
        private string _TempFile;

        public MainForm() {
            InitializeComponent();

            // Icon from https://icons8.com/icon/1039/data-backup, https://icons8.com/license
            Icon = Properties.Resources.BackupIcon;
            Location = new Point(Settings.CurrentSettings.General.WindowLocation.X, Settings.CurrentSettings.General.WindowLocation.Y);
        }

        private void MainForm_Load(object sender, EventArgs e) {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Settings.SaveSettings();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e) {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (droppedFiles == null) { return; }
            string firstItem = droppedFiles[0];

            string workingFileList = Path.GetTempFileName();
            _TempFile = workingFileList;
            string workingDirectory = Path.GetDirectoryName(firstItem);
            string workingName = null;

            FileAttributes firstItem_Attributes = File.GetAttributes(firstItem);

            StringBuilder fileList = new StringBuilder();
            foreach (string file in droppedFiles) {
                fileList.AppendLine(file);
            }
            File.WriteAllText(workingFileList, fileList.ToString(), Encoding.UTF8);

            // Example Path: C:\TopDirectory\SubDirectory\SomeFile.txt
            if (droppedFiles.Length == 1) { // If there is only one dropped file or directory.

                if (firstItem_Attributes.HasFlag(FileAttributes.Directory)) { // If it is a directory.

                    // Returns: SubDirectory
                    workingName = new DirectoryInfo(firstItem).Name;

                } else { // If it is a file.

                    // Returns: SomeFile
                    workingName = Path.GetFileNameWithoutExtension(firstItem);

                }

            } else { // If there is more than one dropped file or directory.

                // If the first one in the list is a directory.
                if (firstItem_Attributes.HasFlag(FileAttributes.Directory)) {

                    // Returns: TopDirectory
                    workingName = new DirectoryInfo(Path.GetDirectoryName(firstItem)).Name;

                } else { // If the first one in the list is a file.

                    // Returns: SubDirectory
                    workingName = new DirectoryInfo(Path.GetDirectoryName(firstItem)).Name;

                }
            }

            if (workingName == null) {
                MessageBox.Show("workingName is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Dispose();
            }

#if DEBUG
            Debug.WriteLine(workingName);
            Debug.WriteLine(workingDirectory);
            Debug.WriteLine(workingFileList);
#endif

            DoBackupBackgroundWorker_Arguments workerArgs = new DoBackupBackgroundWorker_Arguments() {
                WorkingFileList = workingFileList,
                WorkingDirectory = workingDirectory,
                WorkingName = workingName
            };

            BackupWorkingProgressBar.BringToFront();
            TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.Indeterminate);

            DoBackupBackgroundWorker.RunWorkerAsync(workerArgs);

        }

        private void MainForm_LocationChanged(object sender, EventArgs e) {
            Settings.CurrentSettings.General.WindowLocation.X = Location.X;
            Settings.CurrentSettings.General.WindowLocation.Y = Location.Y;
        }

        private void InfoLabel_MouseDown(object sender, MouseEventArgs e) {
            _FirstPoint = e.Location;
            _IsMouseDown = true;
        }

        private void InfoLabel_MouseUp(object sender, MouseEventArgs e) {
            _IsMouseDown = false;
        }

        private void InfoLabel_MouseMove(object sender, MouseEventArgs e) {
            if (_IsMouseDown && e.Button == MouseButtons.Left) {
                int xDiff = _FirstPoint.X - e.Location.X;
                int yDiff = _FirstPoint.Y - e.Location.Y;

                int x = Location.X - xDiff;
                int y = Location.Y - yDiff;

                Location = new Point(x, y);
            }
        }

        private class DoBackupBackgroundWorker_Arguments {
            public string WorkingFileList { get; set; }
            public string WorkingDirectory { get; set; }
            public string WorkingName { get; set; }
        }

        private void DoBackupBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            DoBackupBackgroundWorker_Arguments workerArgs = (DoBackupBackgroundWorker_Arguments)e.Argument;
            BackupFiles(workerArgs.WorkingFileList, workerArgs.WorkingDirectory, workerArgs.WorkingName);
        }

        private void DoBackupBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == 100) {
                TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.NoProgress);
                Hide();
            }
        }

        private void BackupFiles(string workingFileList, string workingDirectory, string workingName) {
            string sevenZip = Settings.CurrentSettings.Backup.SevenZip;
            // M-d-yyyy
            string dateDate = DateTime.Now.ToString(Settings.CurrentSettings.Backup.DateFormat).ToLower();
            // h.mmtt
            string dateTime = DateTime.Now.ToString(Settings.CurrentSettings.Backup.TimeFormat).ToLower();

            string tempFileName = Settings.CurrentSettings.Backup.NameFormat;

            tempFileName = tempFileName.Replace("%F", workingName);
            tempFileName = tempFileName.Replace("%D", dateDate);
            tempFileName = tempFileName.Replace("%T", dateTime);

            workingName = Path.Combine(workingDirectory, tempFileName);

            if (File.Exists(workingName)) {
                MessageBox.Show($"There is a file where the backup zip is trying to be saved with the same name.\r\n\r\n{workingName}\r\n\r\nBackup will exit when you click OK.", "Backup zip already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            string sevenZipArguments = "a \"" + workingName + "\" " + $"@{workingFileList}";

#if DEBUG  
            Debug.WriteLine(dateTime);
            Debug.WriteLine(sevenZipArguments);
            Debug.WriteLine("====");
#endif

            try {
                if (!File.Exists(sevenZip)) { throw new FileNotFoundException("7-Zip was not found on the system!", sevenZip); }

                Process sevenZipProcess = new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = sevenZip,
                        Arguments = sevenZipArguments,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };

                if (Settings.CurrentSettings.Arguments.Elevate == true) {
                    sevenZipProcess.StartInfo.Verb = "runas";
                }

                sevenZipProcess.Start();
                sevenZipProcess.WaitForExit();

            } catch (FileNotFoundException e) {
                MessageBox.Show($"{e.Message}\r\n\r\n7z.exe was expected at: {sevenZip}", "7-Zip not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();

            } catch (Exception e) {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();

            } finally {
                DoBackupBackgroundWorker.ReportProgress(100);

                try { File.Delete(_TempFile); } catch { }

                if (File.Exists(workingName)) {
                    long fileSize = new FileInfo(workingName).Length;
                    string fileSizeFormatted = BytesToString(fileSize);

                    if (Settings.CurrentSettings.Arguments.DoAutocopy) {
                        try {
                            string tempWorkingName = new FileInfo(workingName).Name;
                            string tempWorkingNames = "";
                            foreach (string copyLocation in Settings.CurrentSettings.Backup.CopyTo) {
                                string copyTo = Path.Combine(copyLocation, tempWorkingName);
                                tempWorkingNames += copyTo + "\r\n";

                                File.Copy(workingName, copyTo, false);
                            }

                            MessageBox.Show($"Zip file saved to: {workingName}\r\n(size {fileSizeFormatted})\r\n\r\nZip file was also copied to:\r\n\r\n{tempWorkingNames}", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Exit();
                        } catch (Exception e) {
                            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }

                    } else {
                        MessageBox.Show($"Zip file saved to: {workingName}\r\n(size {fileSizeFormatted})", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();

                    }

                } else {
                    MessageBox.Show("The zip file was not found after saving!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

            }
        }

        private string BytesToString(long byteCount) {
            // From https://stackoverflow.com/a/4975942
            string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0) { return $"0 {suffix[0]} "; }

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString()} {suffix[place]}";
        }

    }

}
