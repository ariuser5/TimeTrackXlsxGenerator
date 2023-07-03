using System.Text.Json;
using System.Text.Json.Serialization;

namespace Timesheet.Converters;

public class TableCoordinatesConverter : JsonConverter<(int row, int col)>
{
	public const string Format = "{0}:{1}";
	
	public override (int row, int col) Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		string? valueString = reader.GetString();
		
		if (valueString is null)
			throw new JsonException("Date string is null.");
		
		string[] parts = valueString.Split(':', StringSplitOptions.TrimEntries);
		if (parts.Length == 2 &&
			int.TryParse(parts[0], out int row) &&
			int.TryParse(parts[1], out int col)
		) {
			return (row, col);
		} else {
			throw new JsonException($"Unable to parse table coordinates: {valueString}");
		}
	}

	public override void Write(
		Utf8JsonWriter writer,
		(int row, int col) value,
		JsonSerializerOptions options)
	{
		string formattedValue = string.Format(Format, value.row, value.col);
		writer.WriteStringValue(formattedValue);
	}
}