using System.Threading.Tasks;
using System.Web.Routing;

using MedioClinic.Models;
using MedioClinic.Models.Profile;

namespace MedioClinic.Utils
{
    /// <summary>
    /// Manager of user profile operations.
    /// </summary>
    public interface IProfileManager
    {
        /// <summary>
        /// Gets a user profile data, together with a result state.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>An operation result state, user profile view model, and a page title.</returns>
        Task<IdentityManagerResult<GetProfileResultState, (IUserViewModel UserViewModel, string PageTitle)>>
            GetProfileAsync(string userName, RequestContext requestContext);

        /// <summary>
        /// Updates user profile data.
        /// </summary>
        /// <param name="uploadModel">Data to update in the store.</param>
        /// <param name="requestContext">Request context.</param>
        /// <returns>An operation result state, user profile view model, and a page title.</returns>
        Task<IdentityManagerResult<PostProfileResultState, (IUserViewModel UserViewModel, string PageTitle)>>
            PostProfileAsync(IUserViewModel uploadModel, RequestContext requestContext);
    }
}
