using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using Business.Identity;
using Business.Identity.Models;

namespace MedioClinic
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(urlHelper.Action("Signin", "Account")),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<MedioClinicUserManager, MedioClinicUser, int>(
                        // Sets the interval after which the validity of the user's security stamp is checked
                        validateInterval: TimeSpan.FromMinutes(10),
                        regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        getUserIdCallback: ((claimsIdentity) => int.Parse(claimsIdentity.GetUserId()))),
                    // Redirect to logon page with return url
                    OnApplyRedirect = context => context.Response.Redirect(urlHelper.Action("SignIn", "Account") + new Uri(context.RedirectUri).Query)
                },
                ExpireTimeSpan = TimeSpan.FromDays(14),
                SlidingExpiration = true
            });
        }
    }
}