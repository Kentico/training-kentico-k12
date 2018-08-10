using MedioClinic.Dto.Sections;

namespace MedioClinic.Repository.Home
{
    public interface IHomeSectionRepository : IRepository
    {
        HomeSectionDto GetHomeSection();
    }
}
