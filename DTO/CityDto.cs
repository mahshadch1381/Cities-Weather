namespace First_Project.DTO
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long population { get; set; }
        public string? country { get; set; }
        public int country_i { get; set; }
        public double tempData { get; set; }
        public DateTime modifiedtime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompeleteName { get; set; }
    }
}
