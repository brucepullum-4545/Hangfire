using HangFireProj.Configuration;
using HangFireProj.Dashboard;
using System;
using System.Web;

namespace HangFireProj.Extensions
{
    /// <summary>
    /// Extension methods for easy HangFire integration
    /// </summary>
    public static class HangFireExtensions
    {
        /// <summary>
        /// Initialize HangFire with default configuration
        /// </summary>
        /// <param name="connectionString">SQL Server connection string</param>
        /// <returns>Configured HangFire options</returns>
        public static HangFireOptions InitializeHangFire(this string connectionString)
        {
            // BEP - Initialize HangFire with default configuration
            var options = HangFireConfiguration.ConfigureDefault(connectionString);
            HangFireConfiguration.Initialize(options);
            return options;
        }

        /// <summary>
        /// Initialize HangFire from web.config connection string
        /// </summary>
        /// <param name="connectionStringName">Name of connection string in web.config</param>
        /// <returns>Configured HangFire options</returns>
        public static HangFireOptions InitializeHangFireFromConfig(this string connectionStringName)
        {
            // BEP - Initialize HangFire from web.config connection string
            var options = HangFireConfiguration.ConfigureFromWebConfig(connectionStringName);
            HangFireConfiguration.Initialize(options);
            return options;
        }

        /// <summary>
        /// Configure HangFire dashboard in Global.asax Application_Start
        /// </summary>
        /// <param name="options">HangFire configuration options</param>
        public static void ConfigureDashboard(this HangFireOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (!options.EnableDashboard)
                return;

            // BEP - Configure HangFire dashboard with authorization
            HangFireDashboardConfig.Configure(options);
        }

        /// <summary>
        /// Configure HangFire dashboard with custom authorization
        /// </summary>
        /// <param name="options">HangFire configuration options</param>
        /// <param name="authorizationCallback">Custom authorization callback</param>
        public static void ConfigureDashboard(this HangFireOptions options, Func<HttpContext, bool> authorizationCallback)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (!options.EnableDashboard)
                return;

            // BEP - Configure HangFire dashboard with custom authorization
            HangFireDashboardConfig.Configure(options, authorizationCallback);
        }

        /// <summary>
        /// Stop HangFire server (call in Application_End)
        /// </summary>
        public static void StopHangFire()
        {
            // BEP - Stop HangFire server and cleanup resources
            HangFireConfiguration.Stop();
        }
    }
}
