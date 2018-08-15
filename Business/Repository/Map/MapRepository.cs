using System.Collections.Generic;
using System.Linq;
using Business.Dto.Map;
using Business.Services.Query;

namespace Business.Repository.Map
{
    public class MapRepository : BaseRepository, IMapRepository
    {
        public MapRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public IEnumerable<MapLocationDto> GetOfficeLocations()
        {
            return DocumentQueryService.GetDocuments<CMS.DocumentEngine.Types.Training.MapLocation>()
                .AddColumns("Longitude", "Latitude", "Tooltip")
                .OrderByAscending("NodeOrder")
                .ToList()
                .Select(m => new MapLocationDto()
                {
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    Tooltip = m.Tooltip
                });
        }
    }
}
