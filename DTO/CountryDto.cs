namespace First_Project.DTO
{
    public class CountryDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int population { get; set; }
        public ICollection<String>? Cities { get; set; }
    }
    
}
