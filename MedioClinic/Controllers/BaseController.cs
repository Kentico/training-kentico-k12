using System;
using System.Web.Mvc;

using Business.DependencyInjection;
using MedioClinic.Models;

namespace MedioClinic.Controllers
{
    public class BaseController : Controller
    {
        protected IBusinessDependencies Dependencies { get; }

        public string ErrorTitle => Localize("General.Error");

        protected BaseController(IBusinessDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        protected PageViewModel GetPageViewModel(
            string title,
            string message = null,
            bool displayAsRaw = false,
            MessageType messageType = MessageType.Info) =>
            PageViewModel.GetPageViewModel(title, Dependencies, message, displayAsRaw, messageType);

        protected PageViewModel<TViewModel> GetPageViewModel<TViewModel>(
            TViewModel data,
            string title,
            string message = null,
            bool displayAsRaw = false,
            MessageType messageType = MessageType.Info)
            where TViewModel : IViewModel =>
            PageViewModel<TViewModel>.GetPageViewModel(data, title, Dependencies, message, displayAsRaw, messageType);

        protected string Localize(string resourceKey) =>
            Dependencies.LocalizationService.Localize(resourceKey);

        protected string ConcatenateContactAdmin(string messageKey) =>
            Localize(messageKey)
                + " "
                + Localize("ContactAdministrator");

        protected ActionResult InvalidInput<TUploadViewModel>(
            PageViewModel<TUploadViewModel> uploadModel)
            where TUploadViewModel : IViewModel
        {
            var viewModel = GetPageViewModel(
                uploadModel.Data,
                Localize("BasicForm.InvalidInput"),
                Localize("Controllers.Base.InvalidInput.Message"),
                false,
                MessageType.Error);

            return View(viewModel);
        }

        protected void AddErrors<TResultState>(IdentityManagerResult<TResultState> result)
            where TResultState : Enum
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}