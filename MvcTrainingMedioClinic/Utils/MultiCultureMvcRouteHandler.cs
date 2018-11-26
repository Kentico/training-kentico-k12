using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTrainingMedioClinic.Utils
{
    /// <summary>
    /// Creates an object that implements the <see cref="IHttpHandler"/> interface and passes the request context to it.
    /// Configures the current thread to use the culture specified by the 'culture' URL parameter.
    /// </summary>
    public class MultiCultureMvcRouteHandler : MvcRouteHandler
    {

        /// <summary>
        /// Name of the parameter used to identify the requested culture (see RouteConfig.cs for details)
        /// </summary>
        public const string CultureUrlParam = "culture";

        /// <summary>
        /// Returns the HTTP handler using the specified request context. 
        /// Sets the <see cref="Thread.CurrentCulture"/> and <see cref="Thread.CurrentUICulture"/> of the current thread to the culture specified by the 'culture' URL parameter.
        /// </summary>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            try
            {

                // Gets the requested culture from the route
                var cultureName = requestContext.RouteData.Values[CultureUrlParam].ToString();

                // Creates a new CultureInfo object for the requested culture
                var culture = new CultureInfo(cultureName);

                // Sets the culture for the thread
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
            catch
            {
                // Returns a 404 response if the culture prefix is invalid
                requestContext.HttpContext.Response.StatusCode = 404;
            }

            return base.GetHttpHandler(requestContext);
        }
    }
}