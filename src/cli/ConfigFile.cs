using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Converters;

static class ConfigFile
{
	public const string FileName = "config.json";
	
	private static readonly JsonSerializerOptions _defaultJsonSerializationOptions = new() 
	{
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		Converters = {
			new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
			new DateMonthConverter(),
			new TableCoordinatesConverter(),
		}
	};
	
	public static JsonSerializerOptions DefaultJsonSerializationOptions => _defaultJsonSerializationOptions;
	
	
	public static Options Read(bool mustCreateIfMissing = true)
	{
		Options config;
		if (File.Exists(FileName)) {
			try {
				string configJson = File.ReadAllText(FileName);
				config = JsonSerializer.Deserialize<Options>(configJson, DefaultJsonSerializationOptions)
					?? throw new Exception("Could not deserialize config file.");
			} catch (Exception e) {
				Console.WriteLine($"Error: could not read config file '{FileName}'.");
				Console.WriteLine(e.Message);
				Console.WriteLine("Switch using default config...");
				config = new Options();
			}
		} else if (mustCreateIfMissing) {
			Console.WriteLine($"Config file '{FileName}' not found. Creating new one.");
			Create();
			return Read(false);
		} else {
			Console.WriteLine($"Config file '{FileName}' not found. Using internal default config.");
			config = new Options();
		}
		return config;
	}
	
	public static void Create()
	{
		Options options = new();
		string json = options.Serialize();
		File.WriteAllText(FileName, json);
	}
}