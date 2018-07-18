using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.CompanyService;
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
            var model = GetPageViewModel(new HomeViewModel()
            {
                CompanyServices = CompanyServiceRepository.GetCompanyServices(),
                HomeSection = HomeSectionRepository.GetHomeSection()
            }, "Home");

            return View(model);
        }
    }
}