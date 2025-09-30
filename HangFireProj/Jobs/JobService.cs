using Hangfire;
using System;
using System.Threading.Tasks;

namespace HangFireProj.Jobs
{
    /// <summary>
    /// Service class for managing HangFire jobs
    /// </summary>
    public class JobService
    {
        /// <summary>
        /// Enqueue a job for immediate execution
        /// </summary>
        /// <typeparam name="T">Type implementing IJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string EnqueueJob<T>(string jobId, object parameters = null) where T : IJobService
        {
            // BEP - Enqueue job using HangFire's BackgroundJob.Enqueue method
            return BackgroundJob.Enqueue<T>(job => job.ExecuteAsync(jobId, parameters));
        }

        /// <summary>
        /// Enqueue a synchronous job for immediate execution
        /// </summary>
        /// <typeparam name="T">Type implementing ISyncJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string EnqueueSyncJob<T>(string jobId, object parameters = null) where T : ISyncJobService
        {
            // BEP - Enqueue synchronous job using HangFire's BackgroundJob.Enqueue method
            return BackgroundJob.Enqueue<T>(job => job.Execute(jobId, parameters));
        }

        /// <summary>
        /// Schedule a job for delayed execution
        /// </summary>
        /// <typeparam name="T">Type implementing IJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="delay">Delay before execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string ScheduleJob<T>(string jobId, TimeSpan delay, object parameters = null) where T : IJobService
        {
            // BEP - Schedule job with delay using HangFire's BackgroundJob.Schedule method
            return BackgroundJob.Schedule<T>(job => job.ExecuteAsync(jobId, parameters), delay);
        }

        /// <summary>
        /// Schedule a synchronous job for delayed execution
        /// </summary>
        /// <typeparam name="T">Type implementing ISyncJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="delay">Delay before execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string ScheduleSyncJob<T>(string jobId, TimeSpan delay, object parameters = null) where T : ISyncJobService
        {
            // BEP - Schedule synchronous job with delay using HangFire's BackgroundJob.Schedule method
            return BackgroundJob.Schedule<T>(job => job.Execute(jobId, parameters), delay);
        }

        /// <summary>
        /// Schedule a job for execution at a specific time
        /// </summary>
        /// <typeparam name="T">Type implementing IJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="enqueueAt">DateTime when the job should be executed</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string ScheduleJobAt<T>(string jobId, DateTime enqueueAt, object parameters = null) where T : IJobService
        {
            // BEP - Schedule job for specific time using HangFire's BackgroundJob.Schedule method
            return BackgroundJob.Schedule<T>(job => job.ExecuteAsync(jobId, parameters), enqueueAt);
        }

        /// <summary>
        /// Schedule a synchronous job for execution at a specific time
        /// </summary>
        /// <typeparam name="T">Type implementing ISyncJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="enqueueAt">DateTime when the job should be executed</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Job ID from HangFire</returns>
        public static string ScheduleSyncJobAt<T>(string jobId, DateTime enqueueAt, object parameters = null) where T : ISyncJobService
        {
            // BEP - Schedule synchronous job for specific time using HangFire's BackgroundJob.Schedule method
            return BackgroundJob.Schedule<T>(job => job.Execute(jobId, parameters), enqueueAt);
        }

        /// <summary>
        /// Create a recurring job using CRON expression
        /// </summary>
        /// <typeparam name="T">Type implementing IJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="cronExpression">CRON expression for scheduling</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Recurring job ID</returns>
        public static string CreateRecurringJob<T>(string jobId, string cronExpression, object parameters = null) where T : IJobService
        {
            // BEP - Create recurring job using HangFire's RecurringJob.AddOrUpdate method
            RecurringJob.AddOrUpdate<T>(jobId, job => job.ExecuteAsync(jobId, parameters), cronExpression);
            return jobId;
        }

        /// <summary>
        /// Create a recurring synchronous job using CRON expression
        /// </summary>
        /// <typeparam name="T">Type implementing ISyncJobService</typeparam>
        /// <param name="jobId">Unique identifier for the job</param>
        /// <param name="cronExpression">CRON expression for scheduling</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Recurring job ID</returns>
        public static string CreateRecurringSyncJob<T>(string jobId, string cronExpression, object parameters = null) where T : ISyncJobService
        {
            // BEP - Create recurring synchronous job using HangFire's RecurringJob.AddOrUpdate method
            RecurringJob.AddOrUpdate<T>(jobId, job => job.Execute(jobId, parameters), cronExpression);
            return jobId;
        }

        /// <summary>
        /// Delete a recurring job
        /// </summary>
        /// <param name="jobId">Job ID to delete</param>
        public static void DeleteRecurringJob(string jobId)
        {
            // BEP - Remove recurring job using HangFire's RecurringJob.RemoveIfExists method
            RecurringJob.RemoveIfExists(jobId);
        }

        /// <summary>
        /// Delete a background job
        /// </summary>
        /// <param name="jobId">Job ID to delete</param>
        /// <returns>True if job was deleted, false if not found</returns>
        public static bool DeleteJob(string jobId)
        {
            // BEP - Delete background job using HangFire's BackgroundJob.Delete method
            return BackgroundJob.Delete(jobId);
        }

        /// <summary>
        /// Trigger a recurring job immediately
        /// </summary>
        /// <param name="jobId">Recurring job ID to trigger</param>
        public static void TriggerRecurringJob(string jobId)
        {
            // BEP - Trigger recurring job immediately using HangFire's RecurringJob.TriggerJob method
            RecurringJob.TriggerJob(jobId);
        }
    }
}
