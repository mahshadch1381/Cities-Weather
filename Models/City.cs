using System.Runtime.InteropServices;

namespace First_Project.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long population { get; set; }
        public Country? country { get; set; }   
        public int country_i{ get; set; }
        public string country_name { get; set; }    
        public double tempData { get; set; }
        public DateTime modifiedtime { get; set;} 
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompeleteName { get; set; }

        // Data Source=ACADEMY11\SQLEXPRESS;Initial Catalog=Country;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
    }
}
