using System.Collections.Generic;
using MedioClinic.Dto.Map;

namespace MedioClinic.Repository.Map
{
    public interface IMapRepository : IRepository
    {
        IEnumerable<MapLocationDto> GetOfficeLocations();
    }
}
