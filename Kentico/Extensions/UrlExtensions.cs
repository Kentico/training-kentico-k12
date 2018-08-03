using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using Kentico.Models;

namespace Kentico.Extensions
{
    public static class UrlExtensions
    {
        public static string KenticoImageUrl(this UrlHelper target, string path, IImageSizeConstraint size = null)
        {
            return target.Kentico().ImageUrl(path, size?.GetSizeConstraint() ?? SizeConstraint.Empty);
        }

    }
}
