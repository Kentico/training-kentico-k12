using System.Collections.Generic;

using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;

using Business.Dto.Airport;

namespace Business.Repository.Airport
{
    public class AirportRepository : IAirportRepository
    {
        protected const string CustomTableClassName = "customtable.Airports";
        protected const string IataCodeColumn = "AirportIataCode";
        protected const string NameColumn = "AirportName";

        public DataClassInfo AirportsTable => DataClassInfoProvider.GetDataClassInfo(CustomTableClassName);

        public IEnumerable<AirportDto> GetAirportDtos(string searchPhrase = null)
        {
            if (AirportsTable != null)
            {
                var query = CustomTableItemProvider.GetItems(CustomTableClassName)
                    .OrderBy("AirportName");

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    query.WhereContains(NameColumn, searchPhrase);
                }

                foreach (var airport in query)
                {
                    var iataCode = ValidationHelper.GetString(airport.GetValue(IataCodeColumn), "");
                    var name = ValidationHelper.GetString(airport.GetValue(NameColumn), "");

                    yield return new AirportDto
                    {
                        AirportIataCode = iataCode,
                        AirportName = name
                    };
                } 
            }
        }
    }
}
