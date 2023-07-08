
using System.Text.Json;

namespace Timesheet.HolydaysApiClient;

public class Downloader
{
    /// <summary>
    /// Endpoint for the API. The year is replaced with the year of the date parameter.
    /// </summary>
    public const string ApiEndpoint = "https://zilelibere.webventure.ro/api/[yyyy]";
    
    public static Task Download(string year, string outputLocation)
    {
        string url = ApiEndpoint.Replace("[yyyy]", year);
        Console.WriteLine($"Downloading data from {url}...");
        
        try {
            using (HttpClient client = new()) {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                
                // Parse JSON response
                List<Holyday> holydays = JsonSerializer.Deserialize<List<Holyday>>(responseBody);
                Console.WriteLine($"Found {holydays.Count} holydays.");
                
                // Generate output file name
                string outputFileName = $"Holydays_{year}.json";
                string outputFile = Path.Combine(outputLocation, outputFileName);
                
                // Save to file
                string json = JsonSerializer.Serialize(holydays);
                File.WriteAllText(outputFile, json);
                Console.WriteLine($"Holydays file generated: {outputFile}");
            }
        } catch (Exception ex) {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
}