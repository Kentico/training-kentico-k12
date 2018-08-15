namespace Business.Dto.Map
{
    public class MapLocationDto : IDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Tooltip { get; set; }
    }
}
