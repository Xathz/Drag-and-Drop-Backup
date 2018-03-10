using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DragAndDropBackup {

    public class Settings {

        /// <summary>
        /// Settings file location.
        /// Automatically set, will be the same name as the executable, .settings will be the file extension.
        /// </summary>
        private readonly string _SettingsLocation = "";

        /// <summary>
        /// Current (active) settings.
        /// </summary>
        public Current CurrentSettings = new Current();

        public Settings() {
            FileInfo thisProcess = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            _SettingsLocation = thisProcess.DirectoryName;
            _SettingsLocation = Path.Combine(_SettingsLocation, Path.GetFileNameWithoutExtension(thisProcess.FullName) + ".settings");

            if (File.Exists(_SettingsLocation)) {
                LoadSettings();
            }
        }

        /// <summary>
        /// Load settings from disk at <see cref="_SettingsLocation" />.
        /// </summary>
        public void LoadSettings() {
            try {
                if (!File.Exists(_SettingsLocation)) { SaveSettings(); }

                using (StreamReader jsonFile = File.OpenText(_SettingsLocation)) {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    CurrentSettings = (Current)jsonSerializer.Deserialize(jsonFile, typeof(Current));
                }
            } catch (FileNotFoundException e) {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Save settings to disk at <see cref="_SettingsLocation" />.
        /// </summary>
        public void SaveSettings() {
            JsonSerializer jsonSerializer = new JsonSerializer() {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented
            };

            using (StreamWriter streamWriter = new StreamWriter(_SettingsLocation))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                jsonSerializer.Serialize(jsonWriter, CurrentSettings, typeof(Current));
            }
        }

        public class Current {

            /// <summary>
            /// General settings.
            /// </summary>
            public General General { get; set; } = new General();

            /// <summary>
            /// Backup settings.
            /// </summary>
            public Backup Backup { get; set; } = new Backup();

            /// <summary>
            /// Set arguments. Does not get saved to disk.
            /// </summary>
            [JsonIgnore]
            public Arguments Arguments { get; set; } = new Arguments();
        }

        public class General {

            /// <summary>
            /// Current window locations [X, Y].
            /// </summary>
            public WindowLocation WindowLocation { get; set; } = new WindowLocation();
        }

        public class Backup {

            /// <summary>
            /// The path to 7-Zip. Default: C:\Program Files\7-Zip\7z.exe
            /// </summary>
            public string SevenZip { get; set; } = @"C:\Program Files\7-Zip\7z.exe";

            /// <summary>
            /// Additional 7-Zip method arguments. Default: -mx=7
            /// </summary>
            /// <remarks>
            /// https://sevenzip.osdn.jp/chm/cmdline/switches/method.htm
            /// </remarks>
            public string SevenZipMethods { get; set; } = "-mx=7";

            /// <summary>
            /// The format of the backup zip. Default: %F Backup %D %T.zip
            /// </summary>
            public string NameFormat { get; set; } = "%F Backup %D %T.zip";

            /// <summary>
            /// Format of the date. Default: M-d-yyyy
            /// </summary>
            /// <remarks>
            /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            /// https://github.com/dotnet/docs/blob/master/docs/standard/base-types/custom-date-and-time-format-strings.md
            /// </remarks>
            public string DateFormat { get; set; } = "M-d-yyyy";

            /// <summary>
            /// Format of the time. Default: h.mmtt
            /// </summary>
            /// <remarks>
            /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            /// https://github.com/dotnet/docs/blob/master/docs/standard/base-types/custom-date-and-time-format-strings.md
            /// </remarks>
            public string TimeFormat { get; set; } = "h.mmtt";

            /// <summary>
            /// Places to copy the zip file to when finished.
            /// </summary>
            public List<string> CopyTo { get; set; } = new List<string>();
        }

        public class WindowLocation {

            /// <summary>
            /// Position X. Default: 8
            /// </summary>
            public int X { get; set; } = 8;

            /// <summary>
            /// Position Y. Default: 520
            /// </summary>
            public int Y { get; set; } = 520;
        }

        public class Arguments {

            /// <summary>
            /// When the zip is created it will also be copied to the paths you set in the settings file.
            /// </summary>
            public bool DoAutocopy { get; set; }  = false;

            /// <summary>
            /// Elevates the 7-Zip process when launched. Will run as an admin to read/write the zip.
            /// </summary>
            public bool Elevate { get; set; } = false;
        }

    }

}
