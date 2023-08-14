namespace First_Project
{
    namespace First_Project
    {
        using Newtonsoft.Json.Linq;
        using System;
        using System.Net.Http;
        using System.Text.Json;
        using System.Threading.Tasks;

        public interface IAreaService
        {
            Task<double> GetAreaAsync(string city);
        }

        public class AreaService : IAreaService
        {
            private readonly HttpClient _httpClient;


            public AreaService(HttpClient httpClient)
            {
                _httpClient = httpClient;
                _httpClient.BaseAddress = new Uri("http://api.geonames.org/");
            }

            public async Task<double> GetAreaAsync(string city)
            {
                try
                {
                    string username = "mahshadch1381";
                    string url = $"searchJSON?q={Uri.EscapeDataString(city)}&username={username}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);

                    double? area = (double?)data?.geonames[0]?.areaInSqKm;

                    if (area.HasValue)
                    {
                        return area.Value;
                    }
                    else
                    {
                        return -1; // Or another appropriate value to indicate data not available
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return -1;
                }

            }

        }
    }
}
