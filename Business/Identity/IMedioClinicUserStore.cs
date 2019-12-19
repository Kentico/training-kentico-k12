using Microsoft.AspNet.Identity;

using Business.Identity.Models;

namespace Business.Identity
{
    /// <summary>
    /// Wrapper around all ASP.NET Identity store interfaces supported in the app.
    /// </summary>
    public interface IMedioClinicUserStore :
        IUserPasswordStore<MedioClinicUser, int>,
        IUserLockoutStore<MedioClinicUser, int>,
        IUserTwoFactorStore<MedioClinicUser, int>,
        IUserRoleStore<MedioClinicUser, int>,
        IUserEmailStore<MedioClinicUser, int>,
        IUserLoginStore<MedioClinicUser, int>,
        IUserSecurityStampStore<MedioClinicUser, int>
    {
    }
}
