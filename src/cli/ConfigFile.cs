using Timesheet.Serialization;

public static class ConfigFile
{
	public const string FileName = "config.json";
	
	public static Config DefaultConfig { get; } = new()
	{
		OutputLocation = Options.Defaults.OutputLocation,
		UserName = Options.Defaults.UserName,
		StartCell = Options.Defaults.StartCell,
		RowsSpace = Options.Defaults.RowsSpace,
		WorkHours = Options.Defaults.WorkHours,
		DescriptionPlaceholder = Options.Defaults.DescriptionPlaceholder,
		DateFormat = Options.Defaults.DateFormat,
	};
	
	
	public static Config? Read(bool mustCreateIfMissing = true)
	{
		Config? config = null;
		if (File.Exists(FileName)) {
			try {
				string configJson = File.ReadAllText(FileName);
				config = Serializer.Deserialize<Config>(configJson)
					?? throw new Exception("Could not deserialize config file.");
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		} else if (mustCreateIfMissing) {
			Console.WriteLine($"Config file '{FileName}' not found. Creating new one.");
			CreateDefault();
			return Read(false);
		}
		return config;
	}
	
	public static void CreateDefault()
	{
		string json = Serializer.Serialize(DefaultConfig);
		File.WriteAllText(FileName, json);
	}
}