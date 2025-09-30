using System;
using System.Threading.Tasks;

namespace HangFireProj.Jobs
{
    /// <summary>
    /// Base class for HangFire jobs providing common functionality
    /// </summary>
    public abstract class BaseJob : IJobService
    {
        /// <summary>
        /// Execute the job with logging and error handling
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Task representing the job execution</returns>
        public async Task ExecuteAsync(string jobId, object parameters = null)
        {
            try
            {
                LogJobStart(jobId, parameters);
                
                // BEP - Execute the actual job logic
                await ExecuteJobAsync(jobId, parameters);
                
                LogJobSuccess(jobId);
            }
            catch (Exception ex)
            {
                LogJobError(jobId, ex);
                throw; // Re-throw to let HangFire handle retry logic
            }
        }

        /// <summary>
        /// Override this method to implement the actual job logic
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Task representing the job execution</returns>
        protected abstract Task ExecuteJobAsync(string jobId, object parameters = null);

        /// <summary>
        /// Job name for identification and logging
        /// </summary>
        public abstract string JobName { get; }

        /// <summary>
        /// Job description for documentation and logging
        /// </summary>
        public abstract string JobDescription { get; }

        /// <summary>
        /// Log job start
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="parameters">Job parameters</param>
        protected virtual void LogJobStart(string jobId, object parameters)
        {
            // BEP - Log job start with timestamp and parameters
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Starting job '{JobName}' (ID: {jobId})");
            if (parameters != null)
            {
                Console.WriteLine($"Parameters: {parameters}");
            }
        }

        /// <summary>
        /// Log job success
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        protected virtual void LogJobSuccess(string jobId)
        {
            // BEP - Log successful job completion
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Job '{JobName}' (ID: {jobId}) completed successfully");
        }

        /// <summary>
        /// Log job error
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="exception">Exception that occurred</param>
        protected virtual void LogJobError(string jobId, Exception exception)
        {
            // BEP - Log job error with full exception details
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Job '{JobName}' (ID: {jobId}) failed: {exception.Message}");
            Console.WriteLine($"Stack trace: {exception.StackTrace}");
        }
    }

    /// <summary>
    /// Base class for synchronous HangFire jobs
    /// </summary>
    public abstract class BaseSyncJob : ISyncJobService
    {
        /// <summary>
        /// Execute the job with logging and error handling
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        public void Execute(string jobId, object parameters = null)
        {
            try
            {
                LogJobStart(jobId, parameters);
                
                // BEP - Execute the actual job logic
                ExecuteJob(jobId, parameters);
                
                LogJobSuccess(jobId);
            }
            catch (Exception ex)
            {
                LogJobError(jobId, ex);
                throw; // Re-throw to let HangFire handle retry logic
            }
        }

        /// <summary>
        /// Override this method to implement the actual job logic
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        protected abstract void ExecuteJob(string jobId, object parameters = null);

        /// <summary>
        /// Job name for identification and logging
        /// </summary>
        public abstract string JobName { get; }

        /// <summary>
        /// Job description for documentation and logging
        /// </summary>
        public abstract string JobDescription { get; }

        /// <summary>
        /// Log job start
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="parameters">Job parameters</param>
        protected virtual void LogJobStart(string jobId, object parameters)
        {
            // BEP - Log job start with timestamp and parameters
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Starting job '{JobName}' (ID: {jobId})");
            if (parameters != null)
            {
                Console.WriteLine($"Parameters: {parameters}");
            }
        }

        /// <summary>
        /// Log job success
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        protected virtual void LogJobSuccess(string jobId)
        {
            // BEP - Log successful job completion
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Job '{JobName}' (ID: {jobId}) completed successfully");
        }

        /// <summary>
        /// Log job error
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="exception">Exception that occurred</param>
        protected virtual void LogJobError(string jobId, Exception exception)
        {
            // BEP - Log job error with full exception details
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Job '{JobName}' (ID: {jobId}) failed: {exception.Message}");
            Console.WriteLine($"Stack trace: {exception.StackTrace}");
        }
    }
}
