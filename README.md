# Drag and Drop Backup [![Build status](https://ci.appveyor.com/api/projects/status/5syhi6rhmrfbbkqb?svg=true)](https://ci.appveyor.com/project/Xathz/drag-and-drop-backup)

Drag and drop files into the window to compress them into a zip file using 7-Zip (not included)

### Why?
This is how I backup small projects and whatnot that I work on. This used to be a PowerShell script but wanted more flexibility than right-clicking a file.

Most of it can be configured with a settings file written in JSON.

### How?

Whatever you drag and drop into the program will be zipped using 7-Zip and saved to the directory where you dragged everything from.

##### Command line switches

|Switch|What it does|
|---|---|
|```--autocopy```|When the zip is created it will also be copied to the paths you set in the settings file.|
|```--elevate```|Elevates the 7-Zip process when launched. Will run as an admin to read/write the zip.|

### Settings file?

The default one will be created when you run the program for the first time. There is no settings ui.

##### NameFormat

|Variable|Meaning|Required|
|---|---|---|
|%F|File name (auto generated)|Yes|
|%D|Date|No|
|%T|Time|No|

##### DateFormat / TimeFormat

Date and time formatting documentation at [Github](https://github.com/dotnet/docs/blob/master/docs/standard/base-types/custom-date-and-time-format-strings.md) or [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

##### SevenZipMethods

This is for additional/custom [method arguments](https://sevenzip.osdn.jp/chm/cmdline/switches/method.htm). It allows you to set the compression level, enable/disable multi-threading, and [many more](https://sevenzip.osdn.jp/chm/cmdline/switches/method.htm) options for 7-Zip.

If you change the compression type it is recommended you change the `.zip` portion of **NameFormat** to match correctly.

##### Example

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
    "SevenZipMethods": "-mx=7",
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
### Known issues

You cannot drag and drop between windows with different elevations. [Explanation](https://blogs.msdn.microsoft.com/patricka/2010/01/28/q-why-doesnt-drag-and-drop-work-when-my-application-is-running-elevated-a-mandatory-integrity-control-and-uipi).
