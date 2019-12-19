using System.Collections.Generic;
using System.Linq;

using CMS.Globalization;

using Business.Dto.Country;

namespace Business.Services.Country
{
    public class CountryService : BaseService, ICountryService
    {
        public IEnumerable<CountryDto> GetCountries() =>
            CountryInfoProvider.GetCountries()
                .Columns("CountryGUID", "CountryName", "CountryDisplayName")
                .TypedResult
                .Items
                .Select(country => new CountryDto
                    {
                        CountryGuid = country.CountryGUID,
                        CountryCodeName = country.CountryName,
                        CountryDisplayName = country.CountryDisplayName
                    });
    }
}
