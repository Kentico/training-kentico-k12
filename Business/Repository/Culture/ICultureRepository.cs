using System.Collections.Generic;
using Business.Dto.Culture;

namespace Business.Repository.Culture
{
    public interface ICultureRepository : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
