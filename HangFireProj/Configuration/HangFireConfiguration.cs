using Hangfire;
using Hangfire.SqlServer;
using HangFireProj.Configuration;
using System;
using System.Configuration;
using System.Web;

namespace HangFireProj.Configuration
{
    /// <summary>
    /// Main configuration class for HangFire setup
    /// </summary>
    public static class HangFireConfiguration
    {
        /// <summary>
        /// Configure HangFire with default settings
        /// </summary>
        /// <param name="connectionString">SQL Server connection string</param>
        /// <returns>Configured HangFire options</returns>
        public static HangFireOptions ConfigureDefault(string connectionString)
        {
            return new HangFireOptions
            {
                ConnectionString = connectionString,
                SchemaName = "hangfire",
                WorkerCount = Environment.ProcessorCount * 5,
                Queues = new[] { "default" },
                DashboardPath = "/hangfire",
                EnableDashboard = true,
                AllowAnonymousDashboard = false,
                EnableAutomaticRetry = true,
                MaxRetryAttempts = 3,
                JobExpirationDays = 7,
                EnableServerHeartbeat = true,
                ServerHeartbeatInterval = 30
            };
        }

        /// <summary>
        /// Configure HangFire from web.config connection strings
        /// </summary>
        /// <param name="connectionStringName">Name of connection string in web.config</param>
        /// <returns>Configured HangFire options</returns>
        public static HangFireOptions ConfigureFromWebConfig(string connectionStringName = "DefaultConnection")
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{connectionStringName}' not found in web.config");
            }

            return ConfigureDefault(connectionString);
        }

        /// <summary>
        /// Initialize HangFire with the provided options
        /// </summary>
        /// <param name="options">HangFire configuration options</param>
        public static void Initialize(HangFireOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new ArgumentException("Connection string is required", nameof(options));

            // Configure HangFire
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(options.ConnectionString, new SqlServerStorageOptions
                {
                    SchemaName = options.SchemaName,
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 25000,
                    TransactionTimeout = TimeSpan.FromMinutes(1)
                });

            // Dashboard is automatically available at /hangfire when Hangfire is configured

            // Configure automatic retry
            if (options.EnableAutomaticRetry)
            {
                GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute
                {
                    Attempts = options.MaxRetryAttempts
                });
            }

            // Start HangFire server
            var serverOptions = new BackgroundJobServerOptions
            {
                WorkerCount = options.WorkerCount,
                Queues = options.Queues,
                ServerTimeout = TimeSpan.FromMinutes(4),
                SchedulePollingInterval = TimeSpan.FromSeconds(15),
                HeartbeatInterval = TimeSpan.FromSeconds(options.ServerHeartbeatInterval),
                ServerCheckInterval = TimeSpan.FromMinutes(1),
                CancellationCheckInterval = TimeSpan.FromSeconds(5)
            };

            // Start the server
            var server = new BackgroundJobServer(serverOptions);
            
            // Store server reference for cleanup if needed
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Application["HangFireServer"] = server;
            }
        }

        /// <summary>
        /// Stop HangFire server
        /// </summary>
        public static void Stop()
        {
            if (HttpContext.Current?.Application["HangFireServer"] is BackgroundJobServer server)
            {
                server.Dispose();
                HttpContext.Current.Application.Remove("HangFireServer");
            }
        }
    }
}
