# TimeTrackXlsxGenerator
"TimeTrackXlsxGenerator" - A C# tool for generating custom Xlsx files to track work hours and activities.

## Command
```
timesheet[.exe] [<date>] [(-o | --output-locationo) <directory>] [(-n | --name) <user-name>]
                [(-c | --start-cell) <table-coordinate>] [(-s | --space) <row-to-table-distance>]
                [(-h | --workhours) <work-hours>] [(-p | --description-placeholder) <desc-placehodler>]
                [(-f | --date-format) <date-format>] [(-w | --worksheet) <worksheet-name>]
                [--allow-missing-config]
```

## Options

`[<date>]`: This optional argument allows you to specify a specific year and month for the timesheet. If provided, the timesheet will be generated for that specific date. If not provided, the timesheet will be generated for the current month.
 - Allowed formats:
   - MM.yyyy
   - yyyy.MM
   - MM/yyyy
   - yyyy/MM
   - MM-yyyy
   - yyyy-MM
   - MM_yyyy
   - yyyy_MM

`[(-o | --output-location)] <directory>]`: This option allows you to specify the output location or directory where the generated timesheet file will be saved. You can provide the directory path after this option. Default is application's working folder.

`[(-n | --name) <user-name>]`: Use this option to specify the name of the user for whom the timesheet is being generated. You can provide the user's name after this option. The name will be included in the generated timesheet file. Default is **"User"**.

`[(-c | --start-cell) <table-coordinate>]`: This option lets you specify the starting cell coordinate for the table in the generated timesheet. You can provide the coordinate value after this option. It determines where the table will begin in the Excel file. Default is **"4:2"**.

`[(-s | --space) <row-to-table-distance>]`: Use this option to specify the number of rows that should be left empty between the header and table content in the generated timesheet. You can provide the number of rows after this option. It helps in organizing and structuring the timesheet. Default is **7**.

`[(-h | --workhours) <work-hours>]`: This option allows you to specify the number of worked hours per day. You can provide the work hours value after this option. It will be used to calculate the total hours worked for each day in the timesheet. Default is **8**.

`[(-p | --description-placeholder) <desc-placeholder>]`: Use this option to set a placeholder text for the description column in the timesheet. You can provide the placeholder text after this option. It will be displayed in the description column of each day in the timesheet. Default is **"\<Desk\>"**.

`[(-f | --date-format) <date-format>]`: This option lets you specify the format in which dates should be displayed in the timesheet. You can provide the desired date format after this option. It allows customization of the date representation. Default is **"dd.MM.yyyy"**.
 - `<date-format>` can be any format supported by C#.

`[(-w | --worksheet) <worksheet-name>]`: Use this option to specify the name of the worksheet where the timesheet will be generated. You can provide the desired worksheet name after this option. It helps in organizing multiple sheets within the Excel file. Default is **unspecified**.

`[--allow-missing-config]`: This option will suppress config file generation if missing. By default, if the config.json file is missing, the program will automatically generate one with the default values. Default is **false**.

Note: Square brackets indicate optional arguments, and parentheses indicate alternative options for the same argument.

