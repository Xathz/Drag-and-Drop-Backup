using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Models;

namespace DragAndDropBackup {

    public class Backup : IDisposable {

        private DateTime _StartTime;
        private readonly string _ListFilePath = Path.GetTempFileName();
        private readonly List<string> _FilesToBackup = new List<string>();
        private readonly int _CompressionLevel;
        private readonly string _SevenZipPath;
        private string _ZipFilePath;

        private ExecutionResult _ExecutionResult;
        private CancellationToken _CancellationToken;

        private long _InitialSize = 0, _CompressedSize = 0, _DifferenceSize = 0;

        public delegate void ProgressChange(object sender, ProgressChangeEventArgs e);
        public event ProgressChange OnProgressChange;

        public Backup(IEnumerable<string> files, int compressionLevel = 5, string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe") {
            _FilesToBackup.AddRange(files);
            _CompressionLevel = compressionLevel;
            _SevenZipPath = sevenZipPath;

            File.WriteAllText(_ListFilePath, string.Join(Environment.NewLine, _FilesToBackup), Encoding.UTF8);
        }

        public void Dispose() => File.Delete(_ListFilePath);

        public void Start(CancellationToken cancellationToken) {
            _StartTime = DateTime.Now;
            _CancellationToken = cancellationToken;

            ExecuteAsync();
        }

        private async Task ExecuteAsync() {
            (string fileName, string directoryName) = DetermineNames();
            string dateDate = DateTime.Now.ToString("M-d-yyyy" /* Settings.CurrentSettings.Backup.DateFormat */).ToLower();
            string dateTime = DateTime.Now.ToString("h.mmtt" /* Settings.CurrentSettings.Backup.TimeFormat */).ToLower();

            string formattedName = "%F Backup %D %T.zip"; // Settings.CurrentSettings.Backup.NameFormat;
            formattedName = formattedName.Replace("%F", fileName);
            formattedName = formattedName.Replace("%D", dateDate);
            formattedName = formattedName.Replace("%T", dateTime);

            _ZipFilePath = Path.Combine(directoryName, formattedName);

            //_ExecutionResult = await new Cli(_SevenZipPath)
            //        .EnableStandardErrorValidation(false)
            //        .EnableExitCodeValidation(false)
            //        .SetCancellationToken(_CancellationToken)
            //        .SetStandardOutputCallback(c => HandleStandardOutput(c))
            //        .SetStandardErrorCallback(c => HandleStandardError(c))
            //        .SetArguments($"a \"{_ZipFilePath}\" @\"{_ListFilePath}\" -mx={_CompressionLevel} -sccUTF-8 -bsp1")
            //        .ExecuteAsync();

        }

        private (string fileName, string directoryName) DetermineNames() {
            string firstItem = _FilesToBackup[0];
            FileAttributes firstItemAttributes = File.GetAttributes(firstItem);
            
            // Example Path: C:\TopDirectory\SubDirectory\SomeFile.txt
            if (_FilesToBackup.Count == 1) { // If there is only one dropped file or directory.

                if (firstItemAttributes.HasFlag(FileAttributes.Directory)) { // If it is a directory.

                    // Returns: SubDirectory
                    return (new DirectoryInfo(firstItem).Name, Path.GetDirectoryName(firstItem));

                } else { // If it is a file.

                    // Returns: SomeFile
                    return (Path.GetFileNameWithoutExtension(firstItem), Path.GetDirectoryName(firstItem));

                }

            } else { // If there is more than one dropped file or directory.

                // If the first one in the list is a directory.
                if (firstItemAttributes.HasFlag(FileAttributes.Directory)) {

                    // Returns: TopDirectory
                    return (new DirectoryInfo(Path.GetDirectoryName(firstItem)).Name, Path.GetDirectoryName(firstItem));

                } else { // If the first one in the list is a file.

                    // Returns: SubDirectory
                    return (new DirectoryInfo(Path.GetDirectoryName(firstItem)).Name, Path.GetDirectoryName(firstItem));

                }
            }
        }

        private void ProgressChanged(int progress, int timeRemaining) {
            if (OnProgressChange == null) { return; }

            ProgressChangeEventArgs args = new ProgressChangeEventArgs(0, 0);
            OnProgressChange(this, args);
        }

        private void HandleStandardOutput(string line) {
            if (string.IsNullOrWhiteSpace(line)) { return; }

        }

        private void HandleStandardError(string line) {
            if (string.IsNullOrWhiteSpace(line)) { return; }

        }

    }

}
