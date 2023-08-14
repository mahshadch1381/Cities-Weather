using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IInformationService
{
    Task<string> GetCityLatitudeAsync(string city);
    Task<string> GetCityLongitudeAsync(string city);
    Task<string> GetCitydetailedInformationAsync(string city);
    
}

public class InformationService : IInformationService
{
    private readonly HttpClient _httpClient;

    public InformationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetCitydetailedInformationAsync(string city)
    {
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
        // Set a generic user agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGeocodingApp/1.0");
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        JArray data = JArray.Parse(content);
        JObject cityInfo = (JObject)data[0];
        //string name = cityInfo["display_name"];
        Console.WriteLine("Latitude: " + cityInfo["lat"]);
        Console.WriteLine("Longitude: " + cityInfo["lon"]);
    
        return "" + cityInfo["display_name"] ;
      
    }
    public async Task<string> GetCityLongitudeAsync(string city)
    {
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
        // Set a generic user agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGeocodingApp/1.0");
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        JArray data = JArray.Parse(content);
        JObject cityInfo = (JObject)data[0];
        //string name = cityInfo["display_name"];
  
        return "" + cityInfo["lon"];
    }
    public async Task<string> GetCityLatitudeAsync(string city)
    {
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
        // Set a generic user agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGeocodingApp/1.0");
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        JArray data = JArray.Parse(content);
        JObject cityInfo = (JObject)data[0];
        //string name = cityInfo["display_name"];
  
        return "" + cityInfo["lat"];
    }
}