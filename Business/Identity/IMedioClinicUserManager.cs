using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Business.Identity
{
    /// <summary>
    /// Wrapper around the <see cref="MedioClinicUserStore"/> and its <see cref="Kentico.Membership.UserStore"/> main component for DI purposes.
    /// </summary>
    /// <typeparam name="TUser">The type of the user object.</typeparam>
    /// <typeparam name="TKey">The unique key.</typeparam>
    public interface IMedioClinicUserManager<TUser, TKey> : IDisposable
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Used to hash/verify passwords
        /// </summary>
        IPasswordHasher PasswordHasher { get; set; }

        /// <summary>
        ///     Used to validate users before changes are saved
        /// </summary>
        IIdentityValidator<TUser> UserValidator { get; set; }

        /// <summary>
        ///     Used to validate passwords before persisting changes
        /// </summary>
        IIdentityValidator<string> PasswordValidator { get; set; }

        /// <summary>
        ///     Used to create claims identities from users
        /// </summary>
        IClaimsIdentityFactory<TUser, TKey> ClaimsIdentityFactory { get; set; }

        /// <summary>
        ///     Used to send email
        /// </summary>
        IIdentityMessageService EmailService { get; set; }

        /// <summary>
        ///     Used to send a sms message
        /// </summary>
        IIdentityMessageService SmsService { get; set; }

        /// <summary>
        ///     Used for generating reset password and confirmation tokens
        /// </summary>
        IUserTokenProvider<TUser, TKey> UserTokenProvider { get; set; }

        /// <summary>
        ///     If true, will enable user lockout when users are created
        /// </summary>
        bool UserLockoutEnabledByDefault { get; set; }

        /// <summary>
        ///     Number of access attempts allowed before a user is locked out (if lockout is enabled);
        /// </summary>
        int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        /// <summary>
        ///     Default amount of time that a user is locked out for after MaxFailedAccessAttemptsBeforeLockout is reached
        /// </summary>
        TimeSpan DefaultAccountLockoutTimeSpan { get; set; }

        /// <summary>
        ///     Returns true if the store is an IUserTwoFactorStore
        /// </summary>
        bool SupportsUserTwoFactor { get; }

        /// <summary>
        ///     Returns true if the store is an IUserPasswordStore
        /// </summary>
        bool SupportsUserPassword { get; }

        /// <summary>
        ///     Returns true if the store is an IUserSecurityStore
        /// </summary>
        bool SupportsUserSecurityStamp { get; }

        /// <summary>
        ///     Returns true if the store is an IUserRoleStore
        /// </summary>
        bool SupportsUserRole { get; }

        /// <summary>
        ///     Returns true if the store is an IUserLoginStore
        /// </summary>
        bool SupportsUserLogin { get; }

        /// <summary>
        ///     Returns true if the store is an IUserEmailStore
        /// </summary>
        bool SupportsUserEmail { get; }

        /// <summary>
        ///     Returns true if the store is an IUserPhoneNumberStore
        /// </summary>
        bool SupportsUserPhoneNumber { get; }

        /// <summary>
        ///     Returns true if the store is an IUserClaimStore
        /// </summary>
        bool SupportsUserClaim { get; }

        /// <summary>
        ///     Returns true if the store is an IUserLockoutStore
        /// </summary>
        bool SupportsUserLockout { get; }

        /// <summary>
        ///     Returns true if the store is an IQueryableUserStore
        /// </summary>
        bool SupportsQueryableUsers { get; }

        /// <summary>
        ///     Returns an IQueryable of users if the store is an IQueryableUserStore
        /// </summary>
        IQueryable<TUser> Users { get; }

        /// <summary>
        /// Maps the registered two-factor authentication providers for users by their id
        /// </summary>
        IDictionary<string, IUserTokenProvider<TUser, TKey>> TwoFactorProviders { get; }

        /// <summary>
        ///     Creates a ClaimsIdentity representing the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType);

        /// <summary>
        ///     Create a user with no password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateAsync(TUser user);

        /// <summary>
        ///     Update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> UpdateAsync(TUser user);

        /// <summary>
        ///     Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteAsync(TUser user);

        /// <summary>
        ///     Find a user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TUser> FindByIdAsync(TKey userId);

        /// <summary>
        ///     Find a user by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<TUser> FindByNameAsync(string userName);

        /// <summary>
        ///     Create a user with the given password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateAsync(TUser user, string password);

        /// <summary>
        ///     Return a user with the specified username and password or null if there is no match.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<TUser> FindAsync(string userName, string password);

        /// <summary>
        ///     Returns true if the password is valid for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(TUser user, string password);

        /// <summary>
        ///     Returns true if the user has a password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasPasswordAsync(TKey userId);

        /// <summary>
        ///     Add a user password only if one does not already exist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> AddPasswordAsync(TKey userId, string password);

        /// <summary>
        ///     Change a user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(TKey userId, string currentPassword,
            string newPassword);

        /// <summary>
        ///     Remove a user's password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> RemovePasswordAsync(TKey userId);

        /// <summary>
        ///     Returns the current security stamp for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetSecurityStampAsync(TKey userId);

        /// <summary>
        ///     Generate a new security stamp for a user, used for SignOutEverywhere functionality
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> UpdateSecurityStampAsync(TKey userId);

        /// <summary>
        ///     Generate a password reset token for the user using the UserTokenProvider
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetTokenAsync(TKey userId);

        /// <summary>
        ///     Reset a user's password using a reset password token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetPasswordAsync(TKey userId, string token, string newPassword);

        /// <summary>
        ///     Returns the user associated with this login
        /// </summary>
        /// <returns></returns>
        Task<TUser> FindAsync(UserLoginInfo login);

        /// <summary>
        ///     Remove a user login
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveLoginAsync(TKey userId, UserLoginInfo login);

        /// <summary>
        ///     Associate a login with a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<IdentityResult> AddLoginAsync(TKey userId, UserLoginInfo login);

        /// <summary>
        ///     Gets the logins for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<UserLoginInfo>> GetLoginsAsync(TKey userId);

        /// <summary>
        ///     Add a user claim
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<IdentityResult> AddClaimAsync(TKey userId, Claim claim);

        /// <summary>
        ///     Remove a user claim
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveClaimAsync(TKey userId, Claim claim);

        /// <summary>
        ///     Get a users's claims
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<Claim>> GetClaimsAsync(TKey userId);

        /// <summary>
        ///     Add a user to a role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> AddToRoleAsync(TKey userId, string role);

        /// <summary>
        /// Method to add user to multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        Task<IdentityResult> AddToRolesAsync(TKey userId, params string[] roles);

        /// <summary>
        /// Remove user from multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        Task<IdentityResult> RemoveFromRolesAsync(TKey userId, params string[] roles);

        /// <summary>
        ///     Remove a user from a role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveFromRoleAsync(TKey userId, string role);

        /// <summary>
        ///     Returns the roles for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(TKey userId);

        /// <summary>
        ///     Returns true if the user is in the specified role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> IsInRoleAsync(TKey userId, string role);

        /// <summary>
        ///     Get a user's email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetEmailAsync(TKey userId);

        /// <summary>
        ///     Set a user's email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IdentityResult> SetEmailAsync(TKey userId, string email);

        /// <summary>
        ///     Find a user by his email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        ///     Get the email confirmation token for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateEmailConfirmationTokenAsync(TKey userId);

        /// <summary>
        ///     Confirm the user's email with confirmation token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IdentityResult> ConfirmEmailAsync(TKey userId, string token);

        /// <summary>
        ///     Returns true if the user's email has been confirmed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsEmailConfirmedAsync(TKey userId);

        /// <summary>
        ///     Get a user's phoneNumber
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetPhoneNumberAsync(TKey userId);

        /// <summary>
        ///     Set a user's phoneNumber
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<IdentityResult> SetPhoneNumberAsync(TKey userId, string phoneNumber);

        /// <summary>
        ///     Set a user's phoneNumber with the verification token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePhoneNumberAsync(TKey userId, string phoneNumber, string token);

        /// <summary>
        ///     Returns true if the user's phone number has been confirmed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsPhoneNumberConfirmedAsync(TKey userId);

        /// <summary>
        ///     Generate a code that the user can use to change their phone number to a specific number
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<string> GenerateChangePhoneNumberTokenAsync(TKey userId, string phoneNumber);

        /// <summary>
        ///     Verify the code is valid for a specific user and for a specific phone number
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<bool> VerifyChangePhoneNumberTokenAsync(TKey userId, string token, string phoneNumber);

        /// <summary>
        ///     Verify a user token with the specified purpose
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="purpose"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> VerifyUserTokenAsync(TKey userId, string purpose, string token);

        /// <summary>
        ///     Get a user token for a specific purpose
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateUserTokenAsync(string purpose, TKey userId);

        /// <summary>
        ///     Register a two factor authentication provider with the TwoFactorProviders mapping
        /// </summary>
        /// <param name="twoFactorProvider"></param>
        /// <param name="provider"></param>
        void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<TUser, TKey> provider);

        /// <summary>
        ///     Returns a list of valid two factor providers for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> GetValidTwoFactorProvidersAsync(TKey userId);

        /// <summary>
        ///     Verify a two factor token with the specified provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="twoFactorProvider"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> VerifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider, string token);

        /// <summary>
        ///     Get a token for a specific two factor provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="twoFactorProvider"></param>
        /// <returns></returns>
        Task<string> GenerateTwoFactorTokenAsync(TKey userId, string twoFactorProvider);

        /// <summary>
        ///     Notify a user with a token using a specific two-factor authentication provider's Notify method
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="twoFactorProvider"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IdentityResult> NotifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider,
            string token);

        /// <summary>
        ///     Get whether two factor authentication is enabled for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> GetTwoFactorEnabledAsync(TKey userId);

        /// <summary>
        ///     Set whether a user has two factor authentication enabled
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task<IdentityResult> SetTwoFactorEnabledAsync(TKey userId, bool enabled);

        // SMS/Email methods

        /// <summary>
        ///     Send an email to the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task SendEmailAsync(TKey userId, string subject, string body);

        /// <summary>
        ///     Send a user a sms message
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendSmsAsync(TKey userId, string message);

        /// <summary>
        ///     Returns true if the user is locked out
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsLockedOutAsync(TKey userId);

        /// <summary>
        ///     Sets whether lockout is enabled for this user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task<IdentityResult> SetLockoutEnabledAsync(TKey userId, bool enabled);

        /// <summary>
        ///     Returns whether lockout is enabled for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> GetLockoutEnabledAsync(TKey userId);

        /// <summary>
        ///     Returns when the user is no longer locked out, dates in the past are considered as not being locked out
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DateTimeOffset> GetLockoutEndDateAsync(TKey userId);

        /// <summary>
        ///     Sets the when a user lockout ends
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        Task<IdentityResult> SetLockoutEndDateAsync(TKey userId, DateTimeOffset lockoutEnd);

        /// <summary>
        /// Increments the access failed count for the user and if the failed access account is greater than or equal
        /// to the MaxFailedAccessAttempsBeforeLockout, the user will be locked out for the next DefaultAccountLockoutTimeSpan
        /// and the AccessFailedCount will be reset to 0. This is used for locking out the user account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> AccessFailedAsync(TKey userId);

        /// <summary>
        ///     Resets the access failed count for the user to 0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetAccessFailedCountAsync(TKey userId);

        /// <summary>
        ///     Returns the number of failed access attempts for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetAccessFailedCountAsync(TKey userId);
    }
}