using System.Text.Json;
using System.Text.Json.Serialization;

static class ConfigFile
{
	public const string FileName = "config.json";
	
	private static readonly JsonSerializerOptions _defaultJsonSerializationOptions = new() 
	{
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};
	
	public static JsonSerializerOptions DefaultJsonSerializationOptions => _defaultJsonSerializationOptions;
	
	public static Config Read(bool mustCreateIfMissing = true)
	{
		Config config;
		if (File.Exists(FileName)) {
			try {
				string configJson = File.ReadAllText(FileName);
				config = JsonSerializer.Deserialize<Config>(configJson, DefaultJsonSerializationOptions)
					?? throw new Exception("Could not deserialize config file.");
			} catch (Exception e) {
				Console.WriteLine($"Error: could not read config file '{FileName}'.");
				Console.WriteLine(e.Message);
				config = new Config();
			}
		} else if (mustCreateIfMissing) {
			Console.WriteLine($"Config file '{FileName}' not found. Creating new one.");
			Create();
			return Read(false);
		} else {
			Console.WriteLine($"Config file '{FileName}' not found. Using internal config defaults.");
			config = new Config();
		}
		
		string json = Serialize(config);
		Console.WriteLine($"Configuration:\n{json}");
		
		return config;
	}
	
	public static void Create()
	{
		Config config = new();
		string json = Serialize(config);
		File.WriteAllText(FileName, json);
	}
	
	private static string Serialize(Config config)
	{
		return JsonSerializer.Serialize(config, DefaultJsonSerializationOptions);
	}
}