using System.Web.Mvc;

using Business.DependencyInjection;
using MedioClinic.Models;

namespace MedioClinic.Controllers
{
    public class BaseController : Controller
    {
        protected IBusinessDependencies Dependencies { get; }

        protected BaseController(IBusinessDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        protected PageViewModel GetPageViewModel(string title) =>
            PageViewModel.GetPageViewModel(title, Dependencies);

        protected PageViewModel<TViewModel> GetPageViewModel<TViewModel>(TViewModel data, string title)
            where TViewModel : IViewModel =>
            PageViewModel<TViewModel>.GetPageViewModel(data, title, Dependencies);
    }
}