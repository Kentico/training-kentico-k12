using System.Threading.Tasks;
using System.Web.Routing;

using MedioClinic.Models.Account;
using MedioClinic.Models;

namespace MedioClinic.Utils
{
    /// <summary>
    /// Manager of user account operations.
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="uploadModel">View model taken from the user.</param>
        /// <param name="emailConfirmed">Signals if email confirmation is required.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>An operation result.</returns>
        Task<IdentityManagerResult<RegisterResultState>> RegisterAsync(RegisterViewModel uploadModel, bool emailConfirmed, RequestContext requestContext);

        /// <summary>
        /// Confirms the user account creation via email.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="token">Confirmation token.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>An operation result.</returns>
        Task<IdentityManagerResult<ConfirmUserResultState>> ConfirmUserAsync(int userId, string token, RequestContext requestContext);

        /// <summary>
        /// Signs the user in.
        /// </summary>
        /// <param name="uploadModel">Credentials view model taken from the user.</param>
        /// <returns>An operation result.</returns>
        Task<IdentityManagerResult<SignInResultState>> SignInAsync(SignInViewModel uploadModel);

        /// <summary>
        /// Signs the user out.
        /// </summary>
        /// <returns>An operation result.</returns>
        IdentityManagerResult<SignOutResultState> SignOut();

        /// <summary>
        /// Sends a unique URL with a reset token to an email address.
        /// </summary>
        /// <param name="uploadModel">Email address taken from the user.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>An operation result.</returns>
        Task<IdentityManagerResult<ForgotPasswordResultState>> ForgotPasswordAsync(EmailViewModel uploadModel, RequestContext requestContext);

        /// <summary>
        /// Verifies the token sent via <see cref="ForgotPasswordAsync(EmailViewModel, RequestContext)"/>.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="token">Verification token.</param>
        /// <returns>A view model to reset the password.</returns>
        Task<IdentityManagerResult<ResetPasswordResultState, ResetPasswordViewModel>> VerifyResetPasswordTokenAsync(int userId, string token);

        /// <summary>
        /// Resets the user password.
        /// </summary>
        /// <param name="uploadModel">New passwords taken from the user.</param>
        /// <returns>An operation result.</returns>
        Task<IdentityManagerResult<ResetPasswordResultState>> ResetPasswordAsync(ResetPasswordViewModel uploadModel);
    }
}