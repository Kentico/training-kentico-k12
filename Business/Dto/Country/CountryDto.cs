using System;

namespace Business.Dto.Country
{
    /// <summary>
    /// A lightweight representation of Kentico's <see cref="CMS.Globalization.CountryInfo"/> objects.
    /// </summary>
    public class CountryDto : IDto
    {
        public Guid CountryGuid { get; set; }

        public string CountryCodeName { get; set; }

        public string CountryDisplayName { get; set; }
    }
}
