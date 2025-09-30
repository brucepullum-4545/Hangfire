using System;

namespace HangFireProj.Configuration
{
    /// <summary>
    /// Configuration options for HangFire setup
    /// </summary>
    public class HangFireOptions
    {
        /// <summary>
        /// SQL Server connection string for HangFire storage
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database schema name for HangFire tables (default: "HangFire")
        /// </summary>
        public string SchemaName { get; set; } = "HangFire";

        /// <summary>
        /// Number of worker threads (default: Environment.ProcessorCount * 5)
        /// </summary>
        public int WorkerCount { get; set; } = Environment.ProcessorCount * 5;

        /// <summary>
        /// Queue names to process (default: "default")
        /// </summary>
        public string[] Queues { get; set; } = { "default" };

        /// <summary>
        /// Dashboard path (default: "/hangfire")
        /// </summary>
        public string DashboardPath { get; set; } = "/hangfire";

        /// <summary>
        /// Enable HangFire dashboard (default: true)
        /// </summary>
        public bool EnableDashboard { get; set; } = true;

        /// <summary>
        /// Dashboard authorization - set to true to allow all users (default: false)
        /// Note: In production, implement proper authorization
        /// </summary>
        public bool AllowAnonymousDashboard { get; set; } = false;

        /// <summary>
        /// Enable automatic retry for failed jobs (default: true)
        /// </summary>
        public bool EnableAutomaticRetry { get; set; } = true;

        /// <summary>
        /// Maximum retry attempts for failed jobs (default: 3)
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Job expiration timeout in days (default: 7)
        /// </summary>
        public int JobExpirationDays { get; set; } = 7;

        /// <summary>
        /// Enable server heartbeat (default: true)
        /// </summary>
        public bool EnableServerHeartbeat { get; set; } = true;

        /// <summary>
        /// Server heartbeat interval in seconds (default: 30)
        /// </summary>
        public int ServerHeartbeatInterval { get; set; } = 30;
    }
}
