using System.Collections.Generic;
using Business.Dto.Culture;

namespace Business.Services.Culture
{
    public interface ICultureService
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
