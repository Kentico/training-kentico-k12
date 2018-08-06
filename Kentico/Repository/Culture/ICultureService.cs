using System.Collections.Generic;
using Kentico.Dto.Culture;

namespace Kentico.Repository.Culture
{
    public interface ICultureService : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
