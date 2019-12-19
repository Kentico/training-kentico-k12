using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

using MedioClinic.Models;

namespace MedioClinic.Extensions
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
        public static string KenticoImageUrl(this UrlHelper helper, string path, IImageSizeConstraint size = null) =>
            helper.Kentico().ImageUrl(path, size?.GetSizeConstraint() ?? SizeConstraint.Empty);

        /// <summary>
        /// Builds an absolute URL out of context information.
        /// </summary>
        /// <param name="helper">HTML helper.</param>
        /// <param name="request">HTTP request.</param>
        /// <param name="action">Controller action.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns></returns>
        public static string AbsoluteUrl(this UrlHelper helper, HttpRequestBase request, string action, string controller = null, object routeValues = null)
        {
            var scheme = request?.Url?.Scheme;
            var domain = request?.Url?.Host;

            var relativePath = string.IsNullOrEmpty(controller)
                ? helper.Action(action, routeValues)
                : helper.Action(action, controller, routeValues);

            return $"{scheme}://{domain}{relativePath}";
        }

        /// <summary>
        /// Makes a string contain only characters allowed in URLs.
        /// </summary>
        /// <param name="input">String to transform.</param>
        /// <returns>String transformed to be URL-compliant.</returns>
        public static string ToUrlCompliantString(this string input)
        {
            var allowedCharacters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=";
            var stringBuilder = new StringBuilder();

            foreach (var character in input)
            {
                var charToAdd = allowedCharacters.Contains(character) ? character : '_';
                stringBuilder.Append(charToAdd);
            }

            return stringBuilder.ToString();
        }
    }
}
