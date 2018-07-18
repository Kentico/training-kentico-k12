using System.Collections.Generic;
using Kentico.Dto.Culture;

namespace Kentico.Repository.Culture
{
    public interface ICultureRepository : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
