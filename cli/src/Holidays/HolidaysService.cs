using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Timesheet.Holidays;

internal class HolidaysService : IHolidaysService
{
    /// <summary>
    /// Endpoint for the API. The year is replaced with the year of the date parameter.
    /// </summary>
    public const string ApiEndpoint = "https://zilelibere.webventure.ro/api/[yyyy]";
    
    private record Holiday(string Name, HolidayDate[] Date);
    private record HolidayDate(DateTime Date, string Weekday);
 
    private readonly Dictionary<DateTime, string> _cachedHolidays;
    
    public HolidaysService()
    {
        List<Holiday> holidays = GetHolidaysFromApi();
        
        _cachedHolidays = new Dictionary<DateTime, string>();
        
        foreach (Holiday holiday in holidays) {
            foreach (HolidayDate holidayDate in holiday.Date) {
                DateTime timeRoundedToDate = holidayDate.Date.Date;   // Don't blame me. That's the api contract.
                
                if (_cachedHolidays.ContainsKey(timeRoundedToDate)) {
                    Console.WriteLine($"Duplicate holiday key entry found: {holiday.Name} on {timeRoundedToDate}");
                    continue;
                }
                
                _cachedHolidays.Add(timeRoundedToDate, holiday.Name);
            }
        }
    }
    
	public string? GetHolidayName(DateTime date)
	{
		DateTime timeRoundedToDate = date.Date; // To ensure only the date part is used for the lookup.
        return _cachedHolidays.ContainsKey(timeRoundedToDate) ? _cachedHolidays[timeRoundedToDate] : null;
	}
    
    private static List<Holiday> GetHolidaysFromApi()
    {
        string url = ApiEndpoint.Replace("[yyyy]", DateTime.Now.Year.ToString());
        Console.WriteLine($"Downloading data from {url}...");
        
        List<Holiday> result = new();
        
        try {
            using (HttpClient client = new()) {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                byte[] responseBodyBytes = response.Content.ReadAsByteArrayAsync().Result;
                string responseBody = Encoding.Default.GetString(responseBodyBytes);
                
                // Parse JSON response
                var options = new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new DateTimeJsonConverter() },
                };
                List<Holiday>? responseHolidays = JsonSerializer.Deserialize<List<Holiday>>(responseBody, options);
                Console.WriteLine($"Found {responseHolidays?.Count ?? 0} national holidays.");
                
                if (responseHolidays is not null) {
                    result = responseHolidays ?? new List<Holiday>();
                }
            }
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
        
        return result;
    }
    
    
    internal class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString() ?? throw new JsonException("Date string is null.");
            return DateTime.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy/MM/dd"));
        }
    }
}