using System.Web.Mvc;
using MedioClinic.DI;
using MedioClinic.Repository.Company;
using MedioClinic.Repository.Home;
using Web.Models.Home;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private ICompanyServiceRepository CompanyServiceRepository { get; }
        private IHomeSectionRepository HomeSectionRepository { get; }

        public HomeController(
            IBusinessDependencies dependencies, 
            ICompanyServiceRepository companyServiceRepository,
            IHomeSectionRepository homeSectionRepository) : base (dependencies)
        {
            CompanyServiceRepository = companyServiceRepository;
            HomeSectionRepository = homeSectionRepository;
        }

        public ActionResult Index()
        {
            var homeSection = HomeSectionRepository.GetHomeSection();

            if (homeSection == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new HomeViewModel()
            {
                CompanyServices = CompanyServiceRepository.GetCompanyServices(),
                HomeSection = homeSection
            }, homeSection.Title);

            return View(model);
        }
    }
}