namespace First_Project.Services
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public interface ICityPopulationService
    {
        Task<long> GetCityPopulationAsync(string city);
        Task<string> GetCitycountryNameAsync(string city);
        Task<(long population, string countryName)> GetCityPopulationAndCountryAsync(string city);
    }
    public class CityPopulationService : ICityPopulationService
    {
        private readonly HttpClient _httpClient;

        public CityPopulationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.geonames.org/");
        }

        public async Task<long> GetCityPopulationAsync(string city)
        {
            try
            {
                string username = "mahshadch1381";
                string url = $"searchJSON?q={Uri.EscapeDataString(city)}&username={username}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);
                    long population = (long)data?.geonames[0]?.population;

                    return population;
                }

                return 0;
            }
            catch (Exception ex)
            {

                return 1;
            }
        }

        public async Task<string> GetCitycountryNameAsync(string city)
        {
            try
            {
                string username = "mahshadch1381";
                string url = $"searchJSON?q={Uri.EscapeDataString(city)}&username={username}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);

                    string countryName = (string)data?.geonames[0]?.countryName;

                    return countryName;
                }

                return "l";
            }
            catch (Exception ex)
            {

                return "lp";
            }
        }
        public async Task<(long population, string countryName)> GetCityPopulationAndCountryAsync(string city)
        {
            var populationTask = GetCityPopulationAsync(city);
            var countryNameTask = GetCitycountryNameAsync(city);

            await Task.WhenAll(populationTask, countryNameTask);

            return (populationTask.Result, countryNameTask.Result);
        }
    }
}
