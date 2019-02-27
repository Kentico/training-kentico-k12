using System.Collections.Generic;
using Business.Dto.Map;

namespace Business.Repository.Map
{
    public interface IMapRepository 
    {
        IEnumerable<MapLocationDto> GetOfficeLocations();
    }
}
