# Drag and Drop Backup
Drag and drop files into the window to compress them into a zip file using 7-Zip (not included)

### Why?
This is how I backup small projects and whatnot that I work on. This used to be a PowerShell script but wanted more flexibility than right-clicking a file.

Most of it can be configured with a settings file written in JSON.

### How?

Whatever you drag and drop into the program will be zipped using 7-Zip and saved to the directory where you dragged everything from.

If you have the flag ```--autocopy``` set then when it is finished creating the zip file it will also be copied to the paths you set in the settings file.

### Settings file?

The default one will be created when you run the program for the first time. There is no settings ui.

The date and time formatting at [Github](https://github.com/dotnet/docs/blob/master/docs/standard/base-types/custom-date-and-time-format-strings.md) or [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

##### Example:

```json
{
  "General": {
    "WindowLocation": {
      "X": 8,
      "Y": 520
    }
  },
  "Backup": {
    "SevenZip": "C:\\Program Files\\7-Zip\\7z.exe",
    "NameFormat": "%F Backup %D %T.zip",
    "DateFormat": "M-d-yyyy",
    "TimeFormat": "h.mmtt",
    "CopyTo": [
      "C:\\Backups",
      "D:\\Backups"
    ]
  }
}
```
