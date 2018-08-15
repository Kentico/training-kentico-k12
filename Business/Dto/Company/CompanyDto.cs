namespace Business.Dto.Company
{
    public class CompanyDto : IDto
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }

        public string ShortAddress => $"{Street}, {City}, {Country}";
    }
}
