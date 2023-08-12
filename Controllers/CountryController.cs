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
    public class CountryController : ControllerBase
    {
        private readonly DataContext _Context;
        public CountryController(DataContext context)
        {
            _Context = context;
        }
        [HttpGet("GetAllCountries")]
        public async Task<ActionResult<List<CountryDto>>> Get()
        {
           
              var countryDto = await _Context.Countries
              .Select(c => new CountryDto
              {
                  Id = c.Id,
                  Name = c.Name,
                  population = c.population,
                  Cities = c.Cities.Select(city => city.Name).ToList()
              })
               .ToListAsync();

            if (countryDto != null)
            {
                return Ok(countryDto);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetCountry")]
        public async Task<ActionResult<CountryDto>> Get(int id)
        {
            var countryDto = await _Context.Countries
          .Where(c => c.Id == id)
          .Select(c => new CountryDto
          {
              Id = c.Id,
              Name = c.Name,
              population = c.population,
              Cities = c.Cities.Select(city => city.Name).ToList()
          })
          .FirstOrDefaultAsync();

            if (countryDto != null)
            {
                return Ok(countryDto);
            }
            else
            {
                return NotFound();
            }
        }
         [HttpPost("PostCountry")]
          public async Task<ActionResult<List<Country>>> Post(Country country)
            {
            var makingCountry = await _Context.Countries.FirstOrDefaultAsync(c => c.Name == country.Name);
            if (makingCountry != null)
            {
                return BadRequest("We have this country already");
            }
            if (country.Cities != null && country.Cities.Any())
                {
                    foreach (var city in country.Cities)
                    {
                    city.country = country;
                    }
                } 
                _Context.Countries.Add(country);
                await _Context.SaveChangesAsync();
                var countries = await _Context.Countries.Include(c => c.Cities).ToListAsync();
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };
                string jsonString = JsonSerializer.Serialize(countries, options);
                return Ok(jsonString);
            }
       
        [HttpDelete("DeleteCountry")]
        public async Task<ActionResult<List<Country>>> Delete(int id)
        {
            var country = await _Context.Countries
         .Include(c => c.Cities) 
         .FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound("we do not have this country");
            }
            _Context.cities.RemoveRange(country.Cities);
            _Context.Countries.Remove(country);
            await _Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
