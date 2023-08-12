using System.Runtime.InteropServices;

namespace First_Project.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int population { get; set; }
        public Country? country { get; set; }   
        public int country_i{ get; set; }
        public double tempData { get; set; }
        public DateTime modifiedtime { get; set;}      
        // Data Source=ACADEMY11\SQLEXPRESS;Initial Catalog=Country;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
    }
}
