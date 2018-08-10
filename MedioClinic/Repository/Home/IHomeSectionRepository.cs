using MedioClinic.Dto.Home;

namespace MedioClinic.Repository.Home
{
    public interface IHomeSectionRepository : IRepository
    {
        HomeSectionDto GetHomeSection();
    }
}
