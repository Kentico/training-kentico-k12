using Business.Dto.Home;

namespace Business.Repository.Home
{
    public interface IHomeSectionRepository : IRepository
    {
        HomeSectionDto GetHomeSection();
    }
}
