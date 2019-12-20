using System;
using System.Web;
using System.Web.Mvc;

using CMS.Membership;

using EnumsNET;
using Business.Identity.Extensions;
using Business.Identity.Models;

namespace Business.Attributes
{
    /// <summary>
    /// Authorizes users based on their strongly-typed <see cref="Identity.Models.Roles"/> roles.
    /// </summary>
    public class MedioClinicAuthorizeAttribute : AuthorizeAttribute
    {
        public new Roles Roles { get; set; }

        public string SiteName { get; set; }

        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
            {
                throw new ArgumentNullException(nameof(authorizationContext));
            }

            var user = HttpContext.Current.User;

            if (user != null)
            {
                var userRoles = UserInfoProvider.GetRolesForUser(user.Identity?.Name, SiteName).ToMedioClinicRoles();

                if (user.Identity?.IsAuthenticated == false || !FlagEnums.HasAnyFlags(Roles, userRoles))
                {
                    // Call a framework method designed for such cases.
                    HandleUnauthorizedRequest(authorizationContext);
                } 
            }
        }
    }
}
