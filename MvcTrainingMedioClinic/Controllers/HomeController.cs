using System.Web.Mvc;
using Business.DependencyInjection;
using Business.Repository.Company;
using Business.Repository.Home;
using MvcTrainingMedioClinic.Models.Home;

namespace MvcTrainingMedioClinic.Controllers
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