using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using Business.Models;

namespace Business.Extensions
{
    public static class UrlExtensions
    {

        /// <summary>
        /// Custom extension method for retrieving image urls based on their path
        /// </summary>
        /// <param name="helper">Html helper</param>
        /// <param name="path">Path to file</param>
        /// <param name="size">Size constraints</param>
        /// <returns></returns>
        public static string KenticoImageUrl(this UrlHelper helper, string path, IImageSizeConstraint size = null)
        {
            return helper.Kentico().ImageUrl(path, size?.GetSizeConstraint() ?? SizeConstraint.Empty);
        }

    }
}
