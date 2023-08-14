using First_Project.Data;
using First_Project.DTO;

using First_Project.Models;
using First_Project.Services;
using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace First_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IInformationService _informationService;
        private readonly DataContext _Context;
        private readonly ICityPopulationService _icitypopulation;
        private readonly IHumidityService _huservice;

        public CityController(DataContext context, IWeatherService weatherService, IInformationService informationService ,
            ICityPopulationService cityPopulation , IHumidityService humidityservice)   
        {
            _Context = context;
            _weatherService = weatherService;
            _informationService = informationService;
            _icitypopulation = cityPopulation;
            _huservice = humidityservice; 
        }
        [HttpGet("GetAllCities")]
        public async Task<ActionResult<List<City>>> GetAllCities()
        {
            var cities = await _Context.cities
            .Select(c => new CityDto
            {
                Id = c.Id,
                Name = c.Name,
                population = c.population,
                country = c.country_name,
                country_i = c.country_i,
                modifiedtime = c.modifiedtime,
                tempData = c.tempData,
                Longitude = c.Longitude,
                Latitude = c.Latitude,
                CompeleteName = c.CompeleteName,
                Humidity = c.Humidity
            })
              .ToListAsync();
            return Ok(cities);
        }
       

        [HttpGet("GetTempOfCity")]
        public async Task<ActionResult<City>> Get(string cityname)
        {
          try
            {
                var b = await _Context.cities.FirstOrDefaultAsync(c => c.Name == cityname);
                if (b == null)
                {
                    City city = new City();
                    city.Name = cityname;
                    city.modifiedtime = DateTime.Now;
                    try
                    {
                        double temperature = await _weatherService.GetTemperatureAsync(city.Name);
                        string formattedNumber = temperature.ToString("0.00");
                        double temp = Double.Parse(formattedNumber);
                        city.tempData = temp;
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                        string cn = await _informationService.GetCitydetailedInformationAsync(cityname);
                        string lg = await _informationService.GetCityLongitudeAsync(cityname);
                        string lt = await _informationService.GetCityLatitudeAsync(cityname);
                        city.CompeleteName = cn;
                        city.Longitude = lg;
                        city.Latitude = lt;
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                        long pop = await _icitypopulation.GetCityPopulationAsync(cityname);
                        string countNm = await _icitypopulation.GetCitycountryNameAsync(cityname);
                        city.population = pop;
                        city.country_name = countNm;

                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                        string str = await _huservice.GetCityHumidityAsync(cityname);
                        city.Humidity = str;    
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    var g = await _Context.Countries.FirstOrDefaultAsync(c => c.Name == city.country_name);
                    if (g == null)
                    {
                        Country country = new Country();
                        country.Name = city.country_name;
                        country.Cities = new List<City>();
                        country.population = 10120;
                        city.country=country;
                        _Context.Countries.Add(country);
                        await _Context.SaveChangesAsync();
                        g = await _Context.Countries.FirstOrDefaultAsync(c => c.Name == city.country_name);
                    }
                    city.country = g;
                    city.country_i = g.Id;
  
                    _Context.cities.Add(city);
                    
                    if (g.Cities != null)
                    {
                        g.Cities.Add(city);
                    }
                    else
                    {
                        g.Cities = new List<City>();
                        g.Cities.Add(city);
                    }
                    await _Context.SaveChangesAsync();
                    string modifyTime = $"Last updated Time is {city.modifiedtime}";
                    string apiResponse = $"Temperature in {city.Name} : {city.tempData}°C";
                    string moreinfo = $"Latitude  : {city.Latitude},\nLongitude : {city.Longitude} ,\nCompeleteName : {city.CompeleteName},";
                    string popp = $"countryname  : {city.country_name},\npopulation : {city.population}";
                    string hum = $"Humidity : {city.Humidity}";
                    return Ok(apiResponse + ".\n" + modifyTime+".\n" + moreinfo + "\n" + popp + "\n" + hum);  
                }
                DateTime submissionTime = b.modifiedtime;
                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - submissionTime;
                if (timeDifference.TotalMinutes >= 30)
                {
                    double temperature = await _weatherService.GetTemperatureAsync(b.Name);
                    b.modifiedtime = DateTime.Now;
                    string formattedNumber = temperature.ToString("0.00");
                    double temp = Double.Parse(formattedNumber);
                    string str = await _huservice.GetCityHumidityAsync(cityname);
                    b.Humidity = str;
                    b.tempData = temp;
                    await _Context.SaveChangesAsync();
                    string apiResponse = $"Temperature in {b.Name} : {formattedNumber}°C";
                    string modifyTime = $"Last updated Time is {b.modifiedtime}";
                    string moreinfo = $"Latitude : {b.Latitude},\nLongitude : {b.Longitude} ,\nCompeleteName : {b.CompeleteName},";
                    string popp = $"countryname : {b.country_name},\npopulation : {b.population}";
                    string update = "there it is updated:";
                    string hum =$"Humidity : {b.Humidity}";
                    return Ok(update + "\n" + apiResponse + ".\n" + modifyTime + ".\n" + moreinfo + "\n" + popp + "\n" + hum);
                }
                else
                {
                    string modifyTime = $"Last updated Time is {b.modifiedtime}";
                    string apiResponse = $"Temperature in {b.Name} : {b.tempData}°C";
                    string moreinfo = $"Latitude  : {b.Latitude},\nLongitude : {b.Longitude} ,\nCompeleteName : {b.CompeleteName},";
                    string popp = $"countryname  : {b.country_name},\npopulation : {b.population}";
                    string hum = $"Humidity : {b.Humidity}";
                    return Ok(apiResponse + ".\n" + modifyTime + ".\n" + moreinfo + "\n" + popp + "\n" + hum);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    var innerException = ex.InnerException;
                }
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");  
            }  
            
        }
        /*
        [HttpDelete("DeleteCity")]
         public async Task<ActionResult<List<City>>> Delete(int id)
         {
             var city = await _Context.cities
         .Include(c => c.country) 
         .FirstOrDefaultAsync(c => c.Id == id);
             try
             {
                 string na = city.Name;

                 if (city.country != null)
                 {
                     city.country.Cities.Remove(city);
                 }
                 _Context.cities.Remove(city);
                 await _Context.SaveChangesAsync();
                 return NoContent();
             }
             catch(Exception e)
             {
                 return BadRequest("we do not have this city");
             }
         } */
    }
}
