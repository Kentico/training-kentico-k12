using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Business.DependencyInjection;
using Business.Identity;
using Business.Identity.Models;
using Business.Repository.Avatar;
using MedioClinic.Config;
using MedioClinic.Extensions;
using MedioClinic.Models.Account;
using MedioClinic.Models;

namespace MedioClinic.Utils
{
    public class AccountManager : BaseIdentityManager, IAccountManager
    {
        public IMedioClinicSignInManager<MedioClinicUser, int> SignInManager { get; }

        public IAuthenticationManager AuthenticationManager { get; }

        public IAvatarRepository AvatarRepository { get; set; }


        public AccountManager(
            IMedioClinicUserManager<MedioClinicUser, int> userManager,
            IMedioClinicSignInManager<MedioClinicUser, int> signInManager,
            IAuthenticationManager authenticationManager,
            IAvatarRepository avatarRepository,
            IBusinessDependencies dependencies)
            : base(userManager, dependencies)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            AuthenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
            AvatarRepository = avatarRepository ?? throw new ArgumentNullException(nameof(avatarRepository));
        }

        public async Task<IdentityManagerResult<RegisterResultState>> RegisterAsync(RegisterViewModel uploadModel, bool emailConfirmed, RequestContext requestContext)
        {
            var user = new MedioClinicUser
            {
                UserName = uploadModel.EmailViewModel.Email,
                Email = uploadModel.EmailViewModel.Email,
                FirstName = uploadModel.FirstName,
                LastName = uploadModel.LastName,
                Enabled = !emailConfirmed
            };

            var accountResult = new IdentityManagerResult<RegisterResultState>();
            IdentityResult identityResult = null;

            try
            {
                identityResult = await UserManager.CreateAsync(user, uploadModel.PasswordConfirmationViewModel.Password);
            }
            catch (Exception ex)
            {
                HandleException(nameof(RegisterAsync), ex, ref accountResult);

                return accountResult;
            }

            if (identityResult != null && identityResult.Succeeded)
            {
                // Registration: Confirmed registration (begin)
                if (emailConfirmed)
                {
                    string token = null;

                    try
                    {
                        token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    }
                    catch (Exception ex)
                    {
                        accountResult.ResultState = RegisterResultState.TokenNotCreated;
                        HandleException(nameof(RegisterAsync), ex, ref accountResult);

                        return accountResult;
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        var confirmationUrl = new UrlHelper(requestContext).AbsoluteUrl(
                                        requestContext.HttpContext.Request,
                                        "ConfirmUser",
                                        routeValues: new { userId = user.Id, token });

                        await UserManager.SendEmailAsync(user.Id,
                            Dependencies.LocalizationService.Localize("AccountManager.Register.Email.Confirm.Subject"),
                            Dependencies.LocalizationService.LocalizeFormat("AccountManager.Register.Email.Confirm.Body", confirmationUrl));

                        accountResult.Success = true;
                        accountResult.ResultState = RegisterResultState.EmailSent;
                    }
                }
                // Registration: Confirmed registration (end)

                // Registration: Direct sign in (begin)
                else
                {
                    identityResult = await AddToPatientRoleAsync(user.Id);

                    try
                    {
                        await CreateNewAvatarAsync(user, requestContext.HttpContext.Server);
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        accountResult.ResultState = RegisterResultState.SignedIn;
                        accountResult.Success = true;
                    }
                    catch (Exception ex)
                    {
                        accountResult.ResultState = RegisterResultState.NotSignedIn;
                        HandleException(nameof(RegisterAsync), ex, ref accountResult);

                        return accountResult;
                    }
                }
                // Registration: Direct sign in (end)
            }

            accountResult.Errors.AddNonNullRange(identityResult.Errors);

            return accountResult;
        }

        public async Task<IdentityManagerResult<ConfirmUserResultState>> ConfirmUserAsync(int userId, string token, RequestContext requestContext)
        {
            var accountResult = new IdentityManagerResult<ConfirmUserResultState>();
            IdentityResult identityResult = IdentityResult.Failed();

            try
            {
                identityResult = await UserManager.ConfirmEmailAsync(userId, token);
            }
            catch (Exception ex)
            {
                accountResult.ResultState = ConfirmUserResultState.EmailNotConfirmed;
                HandleException(nameof(ConfirmUserAsync), ex, ref accountResult);

                return accountResult;
            }

            if (identityResult.Succeeded && (await AddToPatientRoleAsync(userId)).Succeeded)
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    await CreateNewAvatarAsync(user, requestContext.HttpContext.Server);
                    accountResult.Success = true;
                    accountResult.ResultState = ConfirmUserResultState.UserConfirmed;
                }
                catch (Exception ex)
                {
                    accountResult.ResultState = ConfirmUserResultState.AvatarNotCreated;
                    HandleException(nameof(ConfirmUserAsync), ex, ref accountResult);

                    return accountResult;
                }
            }

            accountResult.Errors.AddNonNullRange(identityResult.Errors);

            return accountResult;
        }

        public async Task<IdentityManagerResult<SignInResultState>> SignInAsync(SignInViewModel uploadModel)
        {
            var accountResult = new IdentityManagerResult<SignInResultState, SignInViewModel>();
            MedioClinicUser user = null;

            try
            {
                user = await UserManager.FindByNameAsync(uploadModel.EmailViewModel.Email);
            }
            catch (Exception ex)
            {
                var ar = accountResult as IdentityManagerResult<SignInResultState>;
                accountResult.ResultState = SignInResultState.UserNotFound;
                HandleException(nameof(SignInAsync), ex, ref ar);

                return accountResult;
            }

            // Registration: Confirmed registration (begin)
            if (user != null && !await UserManager.IsEmailConfirmedAsync(user.Id))
            {
                accountResult.ResultState = SignInResultState.EmailNotConfirmed;

                return accountResult;
            }
            // Registration: Confirmed registration (end)

            SignInStatus signInStatus = SignInStatus.Failure;

            try
            {
                signInStatus = await SignInManager.PasswordSignInAsync(uploadModel.EmailViewModel.Email, uploadModel.PasswordViewModel.Password, uploadModel.StaySignedIn, false);
            }
            catch (Exception ex)
            {
                var ar = accountResult as IdentityManagerResult<SignInResultState>;
                accountResult.ResultState = SignInResultState.NotSignedIn;
                HandleException(nameof(SignInAsync), ex, ref ar);

                return accountResult;
            }

            if (signInStatus == SignInStatus.Success)
            {
                accountResult.Success = true;
                accountResult.ResultState = SignInResultState.SignedIn;
            }

            return accountResult;
        }

        public IdentityManagerResult<SignOutResultState> SignOut()
        {
            var accountResult = new IdentityManagerResult<SignOutResultState>();

            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                accountResult.Success = true;
                accountResult.ResultState = SignOutResultState.SignedOut;
            }
            catch (Exception ex)
            {
                accountResult.ResultState = SignOutResultState.NotSignedOut;
                HandleException(nameof(SignOut), ex, ref accountResult);
            }

            return accountResult;
        }

        public async Task<IdentityManagerResult<ForgotPasswordResultState>> ForgotPasswordAsync(EmailViewModel uploadModel, RequestContext requestContext)
        {
            var accountResult = new IdentityManagerResult<ForgotPasswordResultState>();
            MedioClinicUser user = null;

            try
            {
                user = await UserManager.FindByEmailAsync(uploadModel.Email);
            }
            catch (Exception ex)
            {
                accountResult.ResultState = ForgotPasswordResultState.UserNotFound;
                HandleException(nameof(ForgotPasswordAsync), ex, ref accountResult);

                return accountResult;
            }

            // Registration: Confirmed registration (begin)
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                accountResult.ResultState = ForgotPasswordResultState.EmailNotConfirmed;

                return accountResult;
            }
            // Registration: Confirmed registration (end)

            string token = null;

            try
            {
                token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            }
            catch (Exception ex)
            {
                accountResult.ResultState = ForgotPasswordResultState.TokenNotCreated;
                HandleException(nameof(ForgotPasswordAsync), ex, ref accountResult);

                return accountResult;
            }

            var resetUrl = new UrlHelper(requestContext).AbsoluteUrl(
                requestContext.HttpContext.Request,
                "ResetPassword",
                "Account",
                new { userId = user.Id, token });

            try
            {
                await UserManager.SendEmailAsync(user.Id, Dependencies.LocalizationService.Localize("PassReset.Title"),
                        Dependencies.LocalizationService.LocalizeFormat("AccountManager.ForgotPassword.Email.Body", resetUrl));
            }
            catch (Exception ex)
            {
                accountResult.ResultState = ForgotPasswordResultState.EmailNotSent;
                HandleException(nameof(ForgotPasswordAsync), ex, ref accountResult);

                return accountResult;
            }

            accountResult.Success = true;
            accountResult.ResultState = ForgotPasswordResultState.EmailSent;

            return accountResult;
        }

        public async Task<IdentityManagerResult<ResetPasswordResultState, ResetPasswordViewModel>> VerifyResetPasswordTokenAsync(int userId, string token)
        {
            var accountResult = new IdentityManagerResult<ResetPasswordResultState, ResetPasswordViewModel>();
            var tokenVerified = false;

            try
            {
                tokenVerified = await UserManager.VerifyUserTokenAsync(userId, "ResetPassword", token);
            }
            catch (Exception ex)
            {
                var ar = accountResult as IdentityManagerResult<ResetPasswordResultState>;
                accountResult.ResultState = ResetPasswordResultState.InvalidToken;
                HandleException(nameof(VerifyResetPasswordTokenAsync), ex, ref ar);

                return accountResult;
            }

            accountResult.Success = true;
            accountResult.ResultState = ResetPasswordResultState.TokenVerified;

            accountResult.Data = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return accountResult;
        }

        public async Task<IdentityManagerResult<ResetPasswordResultState>> ResetPasswordAsync(ResetPasswordViewModel uploadModel)
        {
            var accountResult = new IdentityManagerResult<ResetPasswordResultState>();
            var identityResult = IdentityResult.Failed();

            try
            {
                identityResult = await UserManager.ResetPasswordAsync(
                    uploadModel.UserId,
                    uploadModel.Token,
                    uploadModel.PasswordConfirmationViewModel.Password);
            }
            catch (Exception ex)
            {
                accountResult.ResultState = ResetPasswordResultState.PasswordNotReset;
                HandleException(nameof(ResetPasswordAsync), ex, ref accountResult);

                return accountResult;
            }

            if (identityResult.Succeeded)
            {
                accountResult.Success = true;
                accountResult.ResultState = ResetPasswordResultState.PasswordReset;
            }

            return accountResult;
        }

        /// <summary>
        /// Adds a user to the patient role.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>An identity result.</returns>
        protected async Task<IdentityResult> AddToPatientRoleAsync(int userId)
        {
            var patientRole = Roles.Patient.ToString();

            return await UserManager.AddToRolesAsync(userId, patientRole);
        }

        /// <summary>
        /// Creates a new user avatar.
        /// </summary>
        /// <param name="user">A user.</param>
        /// <param name="server">A server object.</param>
        /// <returns></returns>
        protected async Task CreateNewAvatarAsync(MedioClinicUser user, HttpServerUtilityBase server)
        {
            var path = server.MapPath($"{AppConfig.ContentDirectory}/{AppConfig.AvatarDirectory}/{AppConfig.DefaultAvatarFileName}");
            user.AvatarId = AvatarRepository.CreateUserAvatar(path, $"Custom {user.UserName}");
            await UserManager.UpdateAsync(user);
        }
    }
}