using System.Collections.Generic;

using Business.Dto.Airport;

namespace Business.Repository.Airport
{
    public interface IAirportRepository : IRepository
    {
        IEnumerable<AirportDto> GetAirportDtos(string searchPhrase = null);
    }
}
