namespace First_Project
{
 using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

    public interface IHumidityService
    { 
  
    Task<string> GetCityHumidityAsync(string city);
           
    }
    public class HumidityService : IHumidityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public HumidityService(HttpClient httpClient)
        { 
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
            _apiKey = "3bd430ec23ca470e35dc0b05c1f50b47";
        }

         public async Task<string> GetCityHumidityAsync(string city)
         {
            try
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);
                    int Humidity = (int)data?.main?.humidity;
                    string result = "" + Humidity;

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }
        }

            
        }
    

}
