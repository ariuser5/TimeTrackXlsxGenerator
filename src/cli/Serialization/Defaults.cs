using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Converters;

namespace Timesheet.Serialization;
public class Defaults
{
    private static readonly JsonSerializerOptions _serializationOptions = new() 
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
	
	public static JsonSerializerOptions JsonSerializationOptions => _serializationOptions;
}