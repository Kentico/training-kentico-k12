using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Business.Identity
{
    /// <summary>
    /// Wrapper around the <see cref="MedioClinicSignInManager"/> and its <see cref="Microsoft.AspNet.Identity.Owin.SignInManager{TUser, TKey}"/> base class for DI purposes.
    /// </summary>
    /// <typeparam name="TUser">The type of the user object.</typeparam>
    /// <typeparam name="TKey">The unique key.</typeparam>
    public interface IMedioClinicSignInManager<TUser, TKey> : IDisposable
        where TUser : class, IUser<TKey>
        where TKey: IEquatable<TKey>
    {
        /// <summary>
        /// AuthenticationType that will be used by sign in, defaults to DefaultAuthenticationTypes.ApplicationCookie
        /// </summary>
        string AuthenticationType { get; set; }

        /// <summary>
        /// Used to operate on users
        /// </summary>
        IMedioClinicUserManager<TUser, TKey> UserManager { get; set; }

        /// <summary>
        /// Used to sign in identities
        /// </summary>
        IAuthenticationManager AuthenticationManager { get; set; }

        /// <summary>
        /// Convert a string id to the proper TKey using Convert.ChangeType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TKey ConvertIdFromString(string id);

        /// <summary>
        /// Convert a TKey userId to a string, by default this just calls ToString()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string ConvertIdToString(TKey id);

        /// <summary>
        /// Called to generate the ClaimsIdentity for the user, override to add additional claims before SignIn
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> CreateUserIdentityAsync(TUser user);

        /// <summary>
        /// Sign the user in using an associated external login
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent);

        /// <summary>
        /// Get the user id that has been verified already or null.
        /// </summary>
        /// <returns></returns>
        Task<TKey> GetVerifiedUserIdAsync();

        /// <summary>
        /// Has the user been verified (ie either via password or external login)
        /// </summary>
        /// <returns></returns>
        Task<bool> HasBeenVerifiedAsync();

        /// <summary>
        /// Sign in the user in using the user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="shouldLockout"></param>
        /// <returns></returns>
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        /// <summary>
        /// Send a two factor code to a user
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<bool> SendTwoFactorCodeAsync(string provider);

        /// <summary>
        /// Creates a user identity and then signs the identity using the AuthenticationManager
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        Task SignInAsync(TUser user, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Two factor verification step
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="code"></param>
        /// <param name="isPersistent"></param>
        /// <param name="rememberBrowser"></param>
        /// <returns></returns>
        Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);
    }
}