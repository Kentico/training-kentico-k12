
using System.Web.Mvc;
using CMS.Helpers;

namespace MedioClinic.Extensions
{
    public static class LocalizationExtensions
    {

        /// <summary>
        /// Custom extension method that localizes text based on given key
        /// </summary>
        /// <param name="helper">Html helper</param>
        /// <param name="key">Full key stored in Localization app within Kentico</param>
        /// <returns></returns>
        public static string Localize(this HtmlHelper helper, string key)
        {
            return ResHelper.GetString(key);
        }
    }
}
