using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using Kentico.Membership;

namespace Business.Identity.Proxies
{
    // Hotfix-independent variant (begin)
    /*
    /// <summary>
    /// Wrapper around the <see cref="UserStore"/> implementation for DI purposes.
    /// </summary>
    public interface IKenticoUserStore : IDisposable
    {
        Task AddLoginAsync(User user, UserLoginInfo login);
        Task AddToRoleAsync(User user, string roleName);
        Task CreateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> FindAsync(UserLoginInfo login);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(int userId);
        Task<User> FindByNameAsync(string userName);
        Task<int> GetAccessFailedCountAsync(User user);
        Task<string> GetEmailAsync(User user);
        Task<bool> GetEmailConfirmedAsync(User user);
        Task<bool> GetLockoutEnabledAsync(User user);
        Task<DateTimeOffset> GetLockoutEndDateAsync(User user);
        Task<IList<UserLoginInfo>> GetLoginsAsync(User user);
        Task<string> GetPasswordHashAsync(User user);
        Task<IList<string>> GetRolesAsync(User user);
        Task<string> GetSecurityStampAsync(User user);
        Task<bool> GetTwoFactorEnabledAsync(User user);
        Task<bool> HasPasswordAsync(User user);
        Task<int> IncrementAccessFailedCountAsync(User user);
        Task<bool> IsInRoleAsync(User user, string roleName);
        Task RemoveFromRoleAsync(User user, string roleName);
        Task RemoveLoginAsync(User user, UserLoginInfo login);
        Task ResetAccessFailedCountAsync(User user);
        Task SetEmailAsync(User user, string email);
        Task SetEmailConfirmedAsync(User user, bool confirmed);
        Task SetLockoutEnabledAsync(User user, bool enabled);
        Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd);
        Task SetPasswordHashAsync(User user, string passwordHash);
        Task SetSecurityStampAsync(User user, string stamp);
        Task SetTwoFactorEnabledAsync(User user, bool enabled);
        Task UpdateAsync(User user);

        /// <summary>
        /// Explicit method signature to satisfy requirements of the Dispose pattern 
        /// (https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose).
        /// </summary>
        /// <param name="disposing">Flag to signal an explicit method call.</param>
        void Dispose(bool disposing);
    }
    */
    // Hotfix-independent variant (end)
}
