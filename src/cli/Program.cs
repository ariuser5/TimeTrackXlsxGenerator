
// Read configuration from command line arguments and config file
Options? options = OptionsReader.Read(args, onError: (ex) => Console.WriteLine(ex.Message));

if (options is null) return;
else {
	string json = Timesheet.Serialization.Serializer.Serialize(options);
	Console.WriteLine($"Timesheet options:\n{json}");
}

// Generate output file name
string outputFileName = $"TimeTrack_{options.Date.ToString("MM.yyyy")}.xlsx";
string outputFile = Path.Combine(options.OutputLocation, outputFileName);
TimesheetDocument.Create(options, outputFile);