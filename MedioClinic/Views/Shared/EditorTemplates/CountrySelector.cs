using System.Web.Mvc;

using Business.Services.Country;

namespace MedioClinic.Views
{
    public class CountrySelector<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Country service objects supplied via DI.
        /// </summary>
        /// See https://autofaccn.readthedocs.io/en/latest/integration/mvc.html#enable-property-injection-for-view-pages
        public ICountryService CountryService { get; set; }

        /// <summary>
        /// Left empty in purpose as countries should be loaded explicitly in the Razor file.
        /// </summary>
        public override void Execute()
        {
        }
    }
}