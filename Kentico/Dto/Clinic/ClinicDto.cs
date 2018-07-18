namespace Kentico.Dto.Clinic
{
    public class ClinicDto : IDto
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
