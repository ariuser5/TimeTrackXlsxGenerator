// Snapshots the current time instance
DateTime now = DateTime.Now;

// Read configuration from json file
bool mustCreateConfigFileIfMissing = !HasNoConfigFlag(args);
Config config = ConfigFile.Read(mustCreateConfigFileIfMissing);

// Generate output file name
string outputFileName = $"TimeTrack_{now:yyyy_MM}.xlsx";
string outputFile = Path.Combine(config.OutputLocation, outputFileName);
TimesheetDocument.Create(config, now, outputFile);

static bool HasNoConfigFlag(string[] args)
{
	for (int i = 0; i < args.Length; i++)
	{
		if (args[i] == "--no-config")
			return true;
	}
	return false;
}