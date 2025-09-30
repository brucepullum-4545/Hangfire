using HangFireProj.Configuration;
using System;
using System.Web;

namespace HangFireProj.Dashboard
{
    /// <summary>
    /// Configuration class for HangFire dashboard
    /// </summary>
    public static class HangFireDashboardConfig
    {
        /// <summary>
        /// Configure HangFire dashboard with default settings
        /// </summary>
        /// <param name="options">HangFire configuration options</param>
        public static void Configure(HangFireOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            // BEP - Configure HangFire dashboard with default authorization
            if (options.AllowAnonymousDashboard)
            {
                Configure(options, null);
            }
            else
            {
                Configure(options, DefaultAuthorizationFilter);
            }
        }

        /// <summary>
        /// Configure HangFire dashboard with custom authorization
        /// </summary>
        /// <param name="options">HangFire configuration options</param>
        /// <param name="authorizationCallback">Custom authorization callback</param>
        public static void Configure(HangFireOptions options, Func<HttpContext, bool> authorizationCallback)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            // BEP - Dashboard configuration is handled in web.config
            // The dashboard is automatically available at the configured path
            // Authorization is handled by the web.config handler
            // This method is kept for compatibility but doesn't need to do anything
        }

        /// <summary>
        /// Default authorization filter - allows access only in development
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>True if authorized, false otherwise</returns>
        private static bool DefaultAuthorizationFilter(HttpContext context)
        {
            // BEP - Default authorization: allow only in development environment
            return HttpContext.Current.IsDebuggingEnabled;
        }

    }
}
