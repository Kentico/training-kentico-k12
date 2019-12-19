using System;
using System.Collections.Generic;

using Business.DependencyInjection;
using Business.Identity;
using Business.Identity.Models;
using MedioClinic.Models;

namespace MedioClinic.Utils
{
    /// <summary>
    /// Base class for identity-related managers.
    /// </summary>
    public abstract class BaseIdentityManager
    {
        public IMedioClinicUserManager<MedioClinicUser, int> UserManager { get; }

        public IBusinessDependencies Dependencies { get; }

        public BaseIdentityManager(
            IMedioClinicUserManager<MedioClinicUser, int> userManager,
            IBusinessDependencies dependencies)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        /// <summary>
        /// Logs exceptions and gets a result object.
        /// </summary>
        /// <typeparam name="TResultState">Result states of the client code.</typeparam>
        /// <param name="methodName">Method name to log.</param>
        /// <param name="exception">An exception to log.</param>
        /// <param name="result">An operation result.</param>
        protected void HandleException<TResultState>(string methodName, Exception exception, ref IdentityManagerResult<TResultState> result)
            where TResultState : Enum
        {
            Dependencies.ErrorHelperService.LogException(this.GetType().Name, methodName, exception);
            result.Success = false;
            result.Errors.Add(exception.Message);
        }
    }
}