using System.Text.Json;
using System.Text.Json.Serialization;

namespace Timesheet.Converters;

class TableCoordinatesConverter : JsonConverter<(int row, int col)>
{
	public override (int row, int col) Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		string? valueString = reader.GetString();
		
		if (valueString is null)
			throw new JsonException("Date string is null.");
		
		string[] parts = valueString.Split(':', StringSplitOptions.TrimEntries);
		if (int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int col)) {
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
		writer.WriteStringValue($"{value.row}:{value.col}");
	}
}