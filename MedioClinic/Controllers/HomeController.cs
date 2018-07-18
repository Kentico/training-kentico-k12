using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.CompanyService;
using MedioClinic.Models.Home;

namespace MedioClinic.Controllers
{
    public class HomeController : BaseController
    {
        private ICompanyServiceRepository CompanyServiceRepository { get; }
        public HomeController(IBusinessDependencies dependencies, ICompanyServiceRepository companyServiceRepository) : base (dependencies)
        {
            CompanyServiceRepository = companyServiceRepository;
        }

        public ActionResult Index()
        {
            var model = GetPageViewModel(new HomeViewModel()
            {
                CompanyServices = CompanyServiceRepository.GetCompanyServices()
            }, "Home");

            return View(model);
        }
    }
}