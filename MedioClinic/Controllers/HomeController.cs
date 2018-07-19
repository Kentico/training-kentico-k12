using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.Company;
using Kentico.Repository.Home;
using MedioClinic.Models.Home;

namespace MedioClinic.Controllers
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

            var model = GetPageViewModel(new HomeViewModel()
            {
                CompanyServices = CompanyServiceRepository.GetCompanyServices(),
                HomeSection = homeSection
            }, homeSection.Title);

            return View(model);
        }
    }
}