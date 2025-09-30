using HangFireProj.Extensions;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ConsumerHangFireWebApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // BEP - HangFire is now configured in Startup.cs (OWIN)
            System.Diagnostics.Debug.WriteLine("Application started - HangFire will be configured via OWIN");
        }

        protected void Application_End()
        {
            // BEP - OWIN will handle HangFire cleanup automatically
            System.Diagnostics.Debug.WriteLine("Application ending - OWIN will handle HangFire cleanup");
        }
    }
}
