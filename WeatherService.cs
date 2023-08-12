namespace First_Project
{
    using First_Project.relation;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public interface IWeatherService
    {
        Task<double> GetTemperatureAsync(string city);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            _apiKey = "3bd430ec23ca470e35dc0b05c1f50b47";
        }

        public async Task<double> GetTemperatureAsync(string city)
        {
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&APPID={_apiKey}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(json);
                    if (weatherData != null)
                    {
                        double temperatureCelsius = weatherData.main.temp - 273.15;

                        return temperatureCelsius;
                    }
                    else
                        return 0.0;
                }
                else
                {
                    throw new HttpRequestException($"Failed to fetch weather data. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Error: {e.Message}");
            }
        }
    }

}
