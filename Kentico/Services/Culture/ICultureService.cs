using System.Collections.Generic;
using Kentico.Dto.Culture;
using Kentico.Repository;

namespace Kentico.Services.Culture
{
    public interface ICultureService : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
