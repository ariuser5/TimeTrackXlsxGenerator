using System.Text.Json;

namespace Timesheet.Serialization;
public static class Serializer
{
	public static string Serialize<T>(T obj) {
		return JsonSerializer.Serialize(obj, Defaults.JsonSerializationOptions);
	}
	
	public static T? Deserialize<T>(string json) {
		return JsonSerializer.Deserialize<T>(json, Defaults.JsonSerializationOptions);
	}
}