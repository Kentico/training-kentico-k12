using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Routing;

using EnumsNET;

using Business.DependencyInjection;
using Business.Identity;
using Business.Identity.Extensions;
using Business.Identity.Models;
using Business.Repository.Avatar;
using Business.Services.Errors;
using Business.Services.Model;
using MedioClinic.Config;
using MedioClinic.Extensions;
using MedioClinic.Models;
using MedioClinic.Models.Account;
using MedioClinic.Models.Profile;

namespace MedioClinic.Utils
{
    public class ProfileManager : BaseIdentityManager, IProfileManager
    {
        public IAvatarRepository AvatarRepository { get; }

        public IFileManager FileManager { get; }

        public IUserModelService UserModelService { get; }

        public IErrorHelperService ErrorHelperService { get; }

        public IMedioClinicUserStore UserStore { get; }

        public ProfileManager(
            IAvatarRepository avatarRepository,
            IFileManager fileManager,
            IUserModelService userModelService,
            IErrorHelperService errorHelperService,
            IMedioClinicUserManager<MedioClinicUser, int> userManager,
            IMedioClinicUserStore userStore,
            IBusinessDependencies dependencies)
                : base(userManager, dependencies)
        {
            AvatarRepository = avatarRepository ?? throw new ArgumentNullException(nameof(avatarRepository));
            FileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            UserModelService = userModelService ?? throw new ArgumentNullException(nameof(userModelService));
            ErrorHelperService = errorHelperService ?? throw new ArgumentNullException(nameof(errorHelperService));
            UserStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

        public async Task<IdentityManagerResult<GetProfileResultState, (IUserViewModel UserViewModel, string PageTitle)>>
            GetProfileAsync(string userName, RequestContext requestContext)
        {
            var profileResult = new IdentityManagerResult<GetProfileResultState, (IUserViewModel, string)>();
            MedioClinicUser user = null;

            try
            {
                user = await UserManager.FindByNameAsync(userName);
            }
            catch (Exception ex)
            {
                var pr = profileResult as IdentityManagerResult<GetProfileResultState>;
                HandleException(nameof(GetProfileAsync), ex, ref pr);
                profileResult.ResultState = GetProfileResultState.UserNotFound;

                return profileResult;
            }

            (var model, var title) = GetViewModelByUserRoles(user, requestContext);

            if (model != null)
            {
                profileResult.Success = true;
                profileResult.ResultState = GetProfileResultState.UserFound;
                profileResult.Data = (model, title);
            }

            return profileResult;
        }

        public async Task<IdentityManagerResult<PostProfileResultState, (IUserViewModel UserViewModel, string PageTitle)>>
            PostProfileAsync(IUserViewModel uploadModel, RequestContext requestContext)
        {
            var profileResult = new IdentityManagerResult<PostProfileResultState, (IUserViewModel, string)>();
            var userTitle = Dependencies.LocalizationService.Localize("General.User");
            var userDoesntExistTitle = Dependencies.LocalizationService.Localize("Adm.User.NotExist");
            profileResult.Data = (uploadModel, userTitle);
            MedioClinicUser user = null;

            try
            {
                user = await UserManager.FindByIdAsync(uploadModel.CommonUserViewModel.Id);
            }
            catch (Exception ex)
            {
                HandlePostProfileException(ref profileResult, ex, PostProfileResultState.UserNotFound);
                profileResult.Data = (uploadModel, userDoesntExistTitle);

                return profileResult;
            }

            var commonUserModelCustomMappings = new Dictionary<(string propertyName, Type propertyType), object>
            {
                { (nameof(MedioClinicUser.Email), typeof(string)), uploadModel.CommonUserViewModel.EmailViewModel.Email },
            };

            try
            {
                // Map the common user properties.
                user = UserModelService.MapToMedioClinicUser(uploadModel.CommonUserViewModel, user, commonUserModelCustomMappings);

                // Map all other potential properties of specific models (patient, doctor, etc.)
                user = UserModelService.MapToMedioClinicUser(uploadModel, user);
            }
            catch (Exception ex)
            {
                HandlePostProfileException(ref profileResult, ex, PostProfileResultState.UserNotMapped);

                return profileResult;
            }

            try
            {
                // We need to use the user store directly due to the design of Microsoft.AspNet.Identity.Core.UserManager.UpdateAsync().
                await UserStore.UpdateAsync(user);

                var avatarFile = uploadModel.CommonUserViewModel.AvatarFile;

                if (avatarFile != null)
                {
                    var avatarBinary = FileManager.GetPostedFileBinary(avatarFile);
                    AvatarRepository.UploadUserAvatar(user, avatarBinary);
                }
            }
            catch (Exception ex)
            {
                HandlePostProfileException(ref profileResult, ex, PostProfileResultState.UserNotUpdated);

                return profileResult;
            }

            (var model, var title) = GetViewModelByUserRoles(user, requestContext, true);

            if (model != null)
            {
                profileResult.Success = true;
                profileResult.ResultState = PostProfileResultState.UserUpdated;
                profileResult.Data = (model, title);
            }

            return profileResult;
        }

        /// <summary>
        /// Computes the user view model, based on roles.
        /// </summary>
        /// <param name="user">User to compute the view model by.</param>
        /// <param name="requestContext">Request context.</param>
        /// <param name="forceAvatarFileOverwrite">Flag that signals the need to update the app-local physical avatar file.</param>
        /// <returns>The view model and a page title.</returns>
        protected (IUserViewModel UserViewModel, string PageTitle) GetViewModelByUserRoles(
            MedioClinicUser user,
            RequestContext requestContext,
            bool forceAvatarFileOverwrite = false)
        {
            if (user != null)
            {
                var roles = user.Roles.ToMedioClinicRoles();
                string avatarPhysicalPath = EnsureAvatarPhysicalPath(user, requestContext, forceAvatarFileOverwrite);

                var avatarRelativePath = avatarPhysicalPath != null
                        ? FileManager.GetServerRelativePath(requestContext.HttpContext.Request, avatarPhysicalPath)
                        : string.Empty;

                var commonUserModelCustomMappings = new Dictionary<(string propertyName, Type propertyType), object>
                {
                    { (nameof(CommonUserViewModel.EmailViewModel), typeof(EmailViewModel)), new EmailViewModel { Email = user.Email } },
                    { (nameof(CommonUserViewModel.AvatarContentPath), typeof(string)), avatarRelativePath }
                };

                object mappedParentModel = null;

                try
                {
                    // Map the common user properties.
                    var mappedCommonUserModel = UserModelService.MapToCustomModel(user, typeof(CommonUserViewModel), commonUserModelCustomMappings);

                    Type userViewModelType = FlagEnums.HasAnyFlags(roles, Roles.Doctor) ? typeof(DoctorViewModel) : typeof(PatientViewModel);

                    var parentModelCustomMappings = new Dictionary<(string propertyName, Type propertyType), object>
                    {
                        { (nameof(CommonUserViewModel), typeof(CommonUserViewModel)), mappedCommonUserModel }
                    };

                    // Map all other potential properties of specific models (patient, doctor, etc.)
                    mappedParentModel = UserModelService.MapToCustomModel(user, userViewModelType, parentModelCustomMappings);
                }
                catch (Exception ex)
                {
                    ErrorHelperService.LogException(nameof(ProfileManager), nameof(GetViewModelByUserRoles), ex);

                    return (null, null);
                }

                return ((IUserViewModel)mappedParentModel, GetRoleTitle(roles));
            }

            return (null, null);
        }

        /// <summary>
        /// Checks for existence of the avatar physical file and creates it if it doesn't exist.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="requestContext">Request context.</param>
        /// <param name="forceOverwrite">Flag to overwrite the obsolete local file.</param>
        /// <returns>Physical path to the local avatar file.</returns>
        protected string EnsureAvatarPhysicalPath(MedioClinicUser user, RequestContext requestContext, bool forceOverwrite = false)
        {
            (var avatarFileName, var avatarBinary) = AvatarRepository.GetUserAvatar(user);

            avatarFileName = avatarFileName ?? AppConfig.DefaultAvatarFileName;
            string avatarPhysicalPath = GetAvatarContentPath(avatarFileName, requestContext);

            if (!avatarFileName.Equals(AppConfig.DefaultAvatarFileName, StringComparison.OrdinalIgnoreCase))
            {
                FileManager.EnsureFile(avatarPhysicalPath, avatarBinary, forceOverwrite);
            }

            return avatarPhysicalPath;
        }

        /// <summary>
        /// Gets the avatar physical path.
        /// </summary>
        /// <param name="avatarFileName">Avatar file name.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>A complete filesystem path to the avatar file.</returns>
        protected string GetAvatarContentPath(string avatarFileName, RequestContext requestContext)
        {
            var physicalDirectoryPath = requestContext.HttpContext.Server.MapPath($"{AppConfig.ContentDirectory}/{AppConfig.AvatarDirectory}");
            FileManager.EnsureDirectory(physicalDirectoryPath);
            var fileName = avatarFileName.ToUrlCompliantString();

            return $"{physicalDirectoryPath}\\{fileName}";
        }

        /// <summary>
        /// Gets a role title.
        /// </summary>
        /// <param name="roles">Role of the user.</param>
        /// <returns>A friendly name of the role.</returns>
        protected string GetRoleTitle(Roles roles) =>
            FlagEnums.HasAnyFlags(roles, Roles.Doctor)
                ? Dependencies.LocalizationService.Localize("ProfileManager.GetRoleTitle.Doctor")
                : Dependencies.LocalizationService.Localize("ProfileManager.GetRoleTitle.Patient");

        /// <summary>
        /// Handles exceptions raised in <see cref="ProfileManager.PostProfileAsync(IUserViewModel, RequestContext)"/>
        /// </summary>
        /// <param name="profileResult">The profile manager result.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="resultState">The result state.</param>
        protected void HandlePostProfileException(ref IdentityManagerResult<PostProfileResultState, (IUserViewModel, string)> profileResult, Exception ex, PostProfileResultState resultState)
        {
            var pr = profileResult as IdentityManagerResult<PostProfileResultState>;
            HandleException(nameof(PostProfileAsync), ex, ref pr);
            profileResult.ResultState = resultState;
        }
    }
}