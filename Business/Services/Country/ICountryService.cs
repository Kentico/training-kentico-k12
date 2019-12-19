using System.Collections.Generic;

using Business.Dto.Country;

namespace Business.Services.Country
{
    /// <summary>
    /// Abstraction from Kentico's <see cref="CMS.Globalization.CountryInfoProvider"/>.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Gets country objects.
        /// </summary>
        /// <returns>A sequence of lightweight country objects.</returns>
        IEnumerable<CountryDto> GetCountries();
    }
}
