using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;

namespace ConsumerHangFireWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            // BEP - Ignore Hangfire dashboard routes
            routes.IgnoreRoute("hangfire/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
