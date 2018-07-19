using System.Collections.Generic;
using Kentico.Dto.Map;

namespace Kentico.Repository.Map
{
    public interface IMapRepository : IRepository
    {
        IEnumerable<MapLocationDto> GetOfficeLocations();
    }
}
