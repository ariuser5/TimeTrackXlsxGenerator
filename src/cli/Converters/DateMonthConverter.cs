using System.Text.Json;
using System.Text.Json.Serialization;

namespace Timesheet.Converters;

class DateMonthConverter : JsonConverter<DateMonth>
{
	public override DateMonth Read(
		ref Utf8JsonReader reader, 
		Type typeToConvert, 
		JsonSerializerOptions options)
	{
		string? dateString = reader.GetString();
		
		if (dateString is null)
			throw new JsonException("Date string is null.");
		
		if (DateMonth.TryParse(dateString, out DateMonth date)) {
			return date;
		} else {
			throw new JsonException($"Unable to parse date string: {dateString}");
		}
	}

	public override void Write(
		Utf8JsonWriter writer, 
		DateMonth value, 
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}