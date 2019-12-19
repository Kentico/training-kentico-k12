using System.Web.Mvc;

using Business.Identity;
using Business.Identity.Models;
using MedioClinic.Models.Profile;
using MedioClinic.Utils;

namespace MedioClinic
{
    public class ModelBindingConfig
    {
        /// <summary>
        /// Registers a custom model binder for <see cref="IUserViewModel"/> upload view models.
        /// </summary>
        public static void RegisterModelBinders() =>
            ModelBinders.Binders.Add(typeof(IUserViewModel),
                new UserViewModelBinder(DependencyResolver.Current.GetService<IMedioClinicUserManager<MedioClinicUser, int>>()));
    }
}