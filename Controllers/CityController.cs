using First_Project.Data;
using First_Project.DTO;
using First_Project.Models;
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
        private readonly DataContext _Context;
        public CityController(DataContext context, IWeatherService weatherService)
        {
            _Context = context;
            _weatherService = weatherService;                         
        }
        [HttpGet]
        public async Task<ActionResult<List<City>>> Get()
        {
            var cities = await _Context.cities
            .Select(c => new CityDto
            {
                Id = c.Id,
                Name = c.Name,
                population = c.population,
                country = c.country.Name,
                country_i = c.country_i,
                modifiedtime = c.modifiedtime,
                tempData = c.tempData
            })
              .ToListAsync();
            return Ok(cities);
        }
        /*[HttpGet("{id}")]
        public async Task<ActionResult<City>> Get(int id)
        {
            try
            {
                var b = await _Context.cities.FindAsync(id);
                DateTime submissionTime = b.modifiedtime;
                DateTime currentTime = DateTime.Now; 
                TimeSpan timeDifference = currentTime - submissionTime;

                if (timeDifference.TotalMinutes >= 30)
                {
                    double temperature = await _weatherService.GetTemperatureAsync(b.Name);
                    b.modifiedtime= DateTime.Now;
                    b.tempData = temperature;
                    await _Context.SaveChangesAsync();
                    string apiResponse = $"Temperature in {b.Name}: {temperature} °C";
                    return Ok(apiResponse);
                }
                else
                {
                    string apiResponse = $"Temperature in {b.Name}: {b.tempData} °C";
                    return Ok(apiResponse);
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
        }*/

        [HttpGet("{cityname}")]
        public async Task<ActionResult<City>> Get(string cityname)
        {
            try
            {
                var b = await _Context.cities.FirstOrDefaultAsync(c => c.Name == cityname);
                if (b == null)
                {
                    return BadRequest("not this city found");
                }
                DateTime submissionTime = b.modifiedtime;
                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - submissionTime;
                if (timeDifference.TotalMinutes >= 30)
                {
                    double temperature = await _weatherService.GetTemperatureAsync(b.Name);
                    b.modifiedtime = DateTime.Now;
                    b.tempData = temperature;
                    await _Context.SaveChangesAsync();
                    string apiResponse = $"Temperature in {b.Name}: {temperature} °C";
                    return Ok("there it is updated :" + apiResponse);
                }
                else
                {
                    string apiResponse = $"Temperature in {b.Name}: {b.tempData} °C";
                    return Ok(apiResponse);
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
        [HttpPost]
        public async Task<ActionResult<List<City>>> Post(InputCity incity)
        {
            var country = await _Context.Countries.FirstOrDefaultAsync(c => c.Name == incity.country_Name);
            if (country == null)
            {
                return NotFound("Associated country not found.");
            }
            City city = new City();
            city.Name = incity.Name;
            city.population = incity.population;
            city.modifiedtime = DateTime.Now;
            city.country = country;
            try
            {
                double temperature = await _weatherService.GetTemperatureAsync(city.Name);
                city.tempData = temperature;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                  
                    var innerException = ex.InnerException;
                }
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
            _Context.cities.Add(city);

            if (country.Cities!=null){
                country.Cities.Add(city);   
            }
            else
            {
                country.Cities = new List<City>();
                country.Cities.Add(city);
            }

            await _Context.SaveChangesAsync();
            var cities = await _Context.cities
                .Include(c => c.country) 
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,  
            };
            string jsonString = JsonSerializer.Serialize(cities, options);
            return Ok(jsonString);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<City>>> Delete(int id)
        {
            var city = await _Context.cities
        .Include(c => c.country) 
        .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            if (city.country != null)
            {
                city.country.Cities.Remove(city);
            }
            _Context.cities.Remove(city);
            await _Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
