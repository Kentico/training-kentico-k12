using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using CMS.Helpers;
using CMS.Membership;
using Kentico.Membership;

using Business.Services.Context;
using Business.Identity.Extensions;
using Business.Identity.Helpers;
using Business.Identity.Models;
using Business.Identity.Proxies;

namespace Business.Identity
{
    /// <summary>
    /// App-level implementation of the ASP.NET Identity interfaces wrapped in <see cref="IMedioClinicUserStore"/>.
    /// </summary>

    // Hotfix-independent variant (begin)
    /* 
    public class MedioClinicUserStore : IMedioClinicUserStore
    */
    // Hotfix-independent variant (end)

    // HF 12.0.34+ variant (begin)
    public class MedioClinicUserStore : KenticoUserStore<MedioClinicUser>, IMedioClinicUserStore
    // HF 12.0.34+ variant (end)
    {
        // Hotfix-independent variant (begin)
        /*
        private ISiteContextService SiteContextService { get; }

        private IKenticoUserStore KenticoUserStore { get; }
        */
        // Hotfix-independent variant (end)

        public MedioClinicUserStore(string siteName) : base(siteName)
        {
        }

        // Hotfix-independent variant (begin)
        /*
        /// <summary>
        /// Creates a new instance of the class with required dependencies.
        /// </summary>
        /// <param name="siteContextService">Service that mainly provides the current site name and ID.</param>
        /// <param name="kenticoUserStore">The <see cref="Kentico.Membership.UserStore"/> wrapped in <see cref="IKenticoUserStore"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="siteContextService"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="kenticoUserStore"/> is <see langword="null"/>.</exception>
        public MedioClinicUserStore(ISiteContextService siteContextService, IKenticoUserStore kenticoUserStore)
        {
            SiteContextService = siteContextService ?? throw new ArgumentNullException(nameof(siteContextService));
            KenticoUserStore = kenticoUserStore ?? throw new ArgumentNullException(nameof(kenticoUserStore));
        }
        
        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user">A user to create.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is <see langword="null"/>.</exception>
        public Task CreateAsync(MedioClinicUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userInfo = user.ToUserInfo();

            userInfo.UserGUID = user.GUID;
            userInfo.PasswordFormat = UserInfoProvider.NewPasswordFormat;
            userInfo.UserPasswordLastChanged = DateTime.Now;
            userInfo.IsExternal = user.IsExternal;

            UserInfoProvider.SetUserInfo(userInfo);
            UserInfoProvider.AddUserToSite(userInfo.UserName, SiteContextService.SiteName);

            user.Id = userInfo.UserID;

            return Task.FromResult(0);
        }

        public Task DeleteAsync(MedioClinicUser user) =>
            KenticoUserStore.DeleteAsync(user);

        public Task<MedioClinicUser> FindByIdAsync(int userId) =>
            Task.FromResult(UserInfoProvider
                .GetUserInfo(userId)
                .ToMedioClinicUser());

        public Task<MedioClinicUser> FindByNameAsync(string userName) =>
            Task.FromResult(UserInfoProvider
                .GetUserInfo(userName)
                .ToMedioClinicUser());

        public Task<MedioClinicUser> FindByEmailAsync(string email) =>
            Task.FromResult(UserInfoProvider
                .GetUsers()
                .WhereEquals("Email", email)
                .TopN(1)
                .FirstOrDefault()
                .ToMedioClinicUser());

        /// <summary>
        /// Gets a user by an external login.
        /// </summary>
        /// <param name="login">The external login.</param>
        /// <returns>The user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="login"/> is <see langword="null"/>.</exception>
        public Task<MedioClinicUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var loginInfo = ExternalLoginInfoProvider.GetExternalLogins()
                                                .WhereEquals("LoginProvider", login.LoginProvider)
                                                .WhereEquals("IdentityKey", login.ProviderKey)
                                                .TopN(1)
                                                .FirstOrDefault();

            return loginInfo != null ? FindByIdAsync(loginInfo.UserID) : Task.FromResult<MedioClinicUser>(null);
        }

        /// <summary>
        /// Updates the user in Kentico.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is <see langword="null"/>.</exception>
        /// <exception cref="Exception">Thrown when there's no <see cref="UserInfo"/> for the <paramref name="user"/>.</exception>
        public Task UpdateAsync(MedioClinicUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userInfo = UserInfoProvider.GetUserInfo(user.Id);

            if (userInfo == null)
            {
                throw new Exception(ResHelper.GetString("General.UserNotFound"));
            }

            UserHelper.UpdateUserInfo(ref userInfo, user);
            UserInfoProvider.SetUserInfo(userInfo);

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(MedioClinicUser user) =>
            KenticoUserStore.GetPasswordHashAsync(user);

        public Task<bool> HasPasswordAsync(MedioClinicUser user) =>
            KenticoUserStore.HasPasswordAsync(user);

        public Task SetPasswordHashAsync(MedioClinicUser user, string passwordHash) =>
            KenticoUserStore.SetPasswordHashAsync(user, passwordHash);

        public Task<int> GetAccessFailedCountAsync(MedioClinicUser user) =>
            KenticoUserStore.GetAccessFailedCountAsync(user);

        public Task<bool> GetLockoutEnabledAsync(MedioClinicUser user) =>
            KenticoUserStore.GetLockoutEnabledAsync(user);

        public Task<DateTimeOffset> GetLockoutEndDateAsync(MedioClinicUser user) =>
            KenticoUserStore.GetLockoutEndDateAsync(user);

        public Task<int> IncrementAccessFailedCountAsync(MedioClinicUser user) =>
            KenticoUserStore.IncrementAccessFailedCountAsync(user);

        public Task ResetAccessFailedCountAsync(MedioClinicUser user) =>
            KenticoUserStore.ResetAccessFailedCountAsync(user);

        public Task SetLockoutEnabledAsync(MedioClinicUser user, bool enabled) =>
            KenticoUserStore.SetLockoutEnabledAsync(user, enabled);

        public Task SetLockoutEndDateAsync(MedioClinicUser user, DateTimeOffset lockoutEnd) =>
            KenticoUserStore.SetLockoutEndDateAsync(user, lockoutEnd);

        public Task<bool> GetTwoFactorEnabledAsync(MedioClinicUser user) =>
            KenticoUserStore.GetTwoFactorEnabledAsync(user);

        public Task SetTwoFactorEnabledAsync(MedioClinicUser user, bool enabled) =>
            KenticoUserStore.SetTwoFactorEnabledAsync(user, enabled);

        public Task<IList<string>> GetRolesAsync(MedioClinicUser user) =>
            KenticoUserStore.GetRolesAsync(user);

        public Task<bool> IsInRoleAsync(MedioClinicUser user, string roleName) =>
            KenticoUserStore.IsInRoleAsync(user, roleName);

        public Task RemoveFromRoleAsync(MedioClinicUser user, string roleName) =>
            KenticoUserStore.RemoveFromRoleAsync(user, roleName);

        public Task AddToRoleAsync(MedioClinicUser user, string roleName) =>
            KenticoUserStore.AddToRoleAsync(user, roleName);

        public Task SetEmailAsync(MedioClinicUser user, string email) =>
            KenticoUserStore.SetEmailAsync(user, email);

        public Task<string> GetEmailAsync(MedioClinicUser user) =>
            KenticoUserStore.GetEmailAsync(user);

        public Task<bool> GetEmailConfirmedAsync(MedioClinicUser user) =>
            KenticoUserStore.GetEmailConfirmedAsync(user);

        public Task SetEmailConfirmedAsync(MedioClinicUser user, bool confirmed) =>
            KenticoUserStore.SetEmailConfirmedAsync(user, confirmed);

        public Task AddLoginAsync(MedioClinicUser user, UserLoginInfo login) =>
            KenticoUserStore.AddLoginAsync(user, login);

        public Task RemoveLoginAsync(MedioClinicUser user, UserLoginInfo login) =>
            KenticoUserStore.RemoveLoginAsync(user, login);

        public Task<IList<UserLoginInfo>> GetLoginsAsync(MedioClinicUser user) =>
            KenticoUserStore.GetLoginsAsync(user);

        public Task SetSecurityStampAsync(MedioClinicUser user, string stamp) =>
            KenticoUserStore.SetSecurityStampAsync(user, stamp);

        public Task<string> GetSecurityStampAsync(MedioClinicUser user) =>
            KenticoUserStore.GetSecurityStampAsync(user);

        /// <summary>
        /// Implementation of the Dispose pattern (https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose).
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implementation of the Dispose pattern (https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose).
        /// </summary>
        /// <param name="disposing">Flag that signals an explicit method call.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Cleanup of a managed component.
                KenticoUserStore.Dispose(true);
            }
        }
        */
        // Hotfix-independent variant (end)
    }
}
