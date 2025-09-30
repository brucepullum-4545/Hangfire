using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;

[assembly: OwinStartup(typeof(ConsumerHangFireWebApp.Startup))]

namespace ConsumerHangFireWebApp
{
    /// <summary>
    /// OWIN startup class for Hangfire configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure OWIN pipeline and Hangfire
        /// </summary>
        /// <param name="app">OWIN application builder</param>
        public void Configuration(IAppBuilder app)
        {
            // Get connection string from web.config
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection connection string not found in web.config");
            }

            // Configure Hangfire
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    SchemaName = "hangfire",
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 25000,
                    TransactionTimeout = TimeSpan.FromMinutes(1)
                });

            // Configure Hangfire dashboard
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "HangFire Dashboard"
            });

            // Configure Hangfire server
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 5,
                Queues = new[] { "default" },
                ServerTimeout = TimeSpan.FromMinutes(4),
                SchedulePollingInterval = TimeSpan.FromSeconds(15),
                HeartbeatInterval = TimeSpan.FromSeconds(30),
                ServerCheckInterval = TimeSpan.FromMinutes(1),
                CancellationCheckInterval = TimeSpan.FromSeconds(5)
            });
        }
    }
}
