using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using Business.Attributes;
using Business.DependencyInjection;
using Business.Identity.Models;
using MedioClinic.Config;
using MedioClinic.Models;
using MedioClinic.Models.Profile;
using MedioClinic.Utils;

namespace MedioClinic.Controllers
{
    // In production, use [RequireHttps].
    [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class ProfileController : BaseController
    {
        public IProfileManager ProfileManager { get; }

        public ProfileController(
            IProfileManager profileManager,
            IBusinessDependencies dependencies) : base(dependencies)
        {
            ProfileManager = profileManager ?? throw new ArgumentNullException(nameof(profileManager));
        }

        // GET: Profile
        [MedioClinicAuthorize(Roles = Roles.Doctor | Roles.Patient, SiteName = AppConfig.Sitename)]
        public async Task<ActionResult> Index() =>
            await GetProfileAsync();

        // POST: Profile
        [MedioClinicAuthorize(Roles = Roles.Doctor | Roles.Patient, SiteName = AppConfig.Sitename)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PageViewModel<IUserViewModel> uploadModel)
        {
            var message = ConcatenateContactAdmin("Error.Message");

            if (ModelState.IsValid)
            {
                var profileResult = await ProfileManager.PostProfileAsync(uploadModel.Data, Request.RequestContext);

                switch (profileResult.ResultState)
                {
                    case PostProfileResultState.UserNotFound:
                        message = ConcatenateContactAdmin("Controllers.Profile.Index.UserNotFound.Message");
                        break;
                    case PostProfileResultState.UserNotMapped:
                    case PostProfileResultState.UserNotUpdated:
                        message = ConcatenateContactAdmin("Controllers.Profile.Index.UserNotUpdated.Message");
                        break;
                    case PostProfileResultState.UserUpdated:
                        message = Localize("Controllers.Profile.Index.UserUpdated.Message");
                        break;
                }

                var model = GetPageViewModel(profileResult.Data.UserViewModel, profileResult.Data.PageTitle ?? ErrorTitle, message);

                return View(model);
            }

            return await GetProfileAsync();
        }

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <returns>Either a user profile page, or a not-found page.</returns>
        protected async Task<ActionResult> GetProfileAsync()
        {
            var userName = HttpContext.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                var profileResult = await ProfileManager.GetProfileAsync(userName, Request.RequestContext);

                if (profileResult.Success)
                {
                    var model = GetPageViewModel(profileResult.Data.UserViewModel, profileResult.Data.PageTitle);

                    return View(model);
                }
            }

            return UserNotFound();
        }

        /// <summary>
        /// Displays a not-found page.
        /// </summary>
        /// <returns>A not-found page.</returns>
        protected ActionResult UserNotFound()
        {
            var message = Dependencies.LocalizationService.Localize("General.UserNotFound");

            return View("UserMessage", GetPageViewModel(ErrorTitle, message, messageType: MessageType.Error));
        }
    }
}