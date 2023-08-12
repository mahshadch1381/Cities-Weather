using First_Project.Models;
using Microsoft.EntityFrameworkCore;
namespace First_Project.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        {

        }
        public DbSet<City> cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        //public DbSet<WeatherData> weatherDatas { get; set; }
        //public DbSet<TempData> tempDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=ACADEMY11\\SQLEXPRESS;Initial Catalog=Country;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public void InsertWithIdentityOn(Country entity)
        {
           
            var newEntity = new Country
            {
                    
                    Name = entity.Name,
                    population = entity.population,
                    Cities = entity.Cities,
                // Other properties...
            };

            Countries.Add(newEntity);
            SaveChanges();
        }
        public void InsertWithIdentityOn_city(City entity)
        {
            
            var newEntity = new City
            {
                
                Name = entity.Name,
                population = entity.population,
                country = entity.country,
                tempData = entity.tempData,
                // Other properties...
            };

            cities.Add(newEntity);
            SaveChanges();
        }
    }
}
