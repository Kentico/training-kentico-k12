using System;
using System.Web.Mvc;

using EnumsNET;

using Business.Identity;
using Business.Identity.Extensions;
using Business.Identity.Models;
using MedioClinic.Models.Profile;

namespace MedioClinic.Utils
{
    public class UserViewModelBinder : IModelBinder
    {
        public IMedioClinicUserManager<MedioClinicUser, int> UserManager { get; set; }

        public DefaultModelBinder DefaultModelBinder => new DefaultModelBinder();

        public UserViewModelBinder(IMedioClinicUserManager<MedioClinicUser, int> userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Looks up roles of the authenticated user and selects a user view model type accordingly.
        /// </summary>
        /// <param name="controllerContext">Controller context.</param>
        /// <param name="bindingContext">Binding context.</param>
        /// <returns>Specific user view model, based on the highest role.</returns>
        /// <exception cref="Exception">Thrown when no user identity is found in the HTTP context.</exception>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(IUserViewModel))
            {
                throw new Exception($"The model is not of type {nameof(IUserViewModel)}.");
            }
            else
            {
                var user = UserManager.FindByNameAsync(controllerContext.HttpContext.User?.Identity?.Name).Result;

                if (user == null)
                {
                    throw new Exception($"The {nameof(IUserViewModel)} model cannot be bound because the user could not be retrieved.");
                }
                else
                {
                    var userRoles = UserManager.GetRolesAsync(user.Id).Result.ToMedioClinicRoles();

                    // The roles should be evaluated from the highest to the lowest.
                    if (FlagEnums.HasAnyFlags(Roles.Doctor, userRoles))
                    {
                        bindingContext.ModelMetadata.Model = new DoctorViewModel();
                    }
                    else if (FlagEnums.HasAnyFlags(Roles.Patient, userRoles))
                    {
                        bindingContext.ModelMetadata.Model = new PatientViewModel();
                    }

                    return DefaultModelBinder.BindModel(controllerContext, bindingContext);
                }
            }
        }
    }
}