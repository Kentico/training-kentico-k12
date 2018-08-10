using System.Collections.Generic;
using MedioClinic.Dto.Culture;

namespace MedioClinic.Repository.Culture
{
    public interface ICultureService : IRepository
    {
        IEnumerable<CultureDto> GetSiteCultures();
    }
}
