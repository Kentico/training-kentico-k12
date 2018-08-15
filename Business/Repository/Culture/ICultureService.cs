using System.Collections.Generic;
using Business.Dto.Culture;

namespace Business.Repository.Culture
{
    public interface ICultureService : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
