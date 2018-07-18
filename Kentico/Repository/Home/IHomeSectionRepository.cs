using Kentico.Dto.Sections;

namespace Kentico.Repository.Home
{
    public interface IHomeSectionRepository : IRepository
    {
        HomeSectionDto GetHomeSection();
    }
}
