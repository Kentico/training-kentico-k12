using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedioClinic.Infrastructure
{
    /// <summary>
    /// Creates an object that implements the <see cref="IHttpHandler"/> interface and passes the request context to it.
    /// Configures the current thread to use the culture specified by the 'culture' URL parameter.
    /// </summary>
    public class MultiCultureMvcRouteHandler : MvcRouteHandler
    {

        /// <summary>
        /// Name of the param used to identify culture (see RouteConfig.cs for details)
        /// </summary>
        public const string CultureUrlParam = "culture";

        /// <summary>
        /// Returns the HTTP handler by using the specified HTTP context. 
        /// <see cref="Thread.CurrentCulture"/> and <see cref="Thread.CurrentUICulture"/> of the current thread are set to the culture specified by the 'culture' URL parameter.
        /// </summary>
        /// <param name="requestContext">Request context.</param>
        /// <returns>HTTP handler.</returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            try
            {

                // Get the requested culture from the route
                var cultureName = requestContext.RouteData.Values[CultureUrlParam].ToString();

                // Get culture
                var culture = new CultureInfo(cultureName);

                // Set culture for the thread
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
            catch
            {
                // Return 404 when culture prefix is invalid
                requestContext.HttpContext.Response.StatusCode = 404;
            }

            return base.GetHttpHandler(requestContext);
        }
    }
}