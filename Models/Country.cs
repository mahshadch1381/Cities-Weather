namespace First_Project.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int population { get; set; }  
        public ICollection<City>?Cities { get; set; }   
           

    }
}
