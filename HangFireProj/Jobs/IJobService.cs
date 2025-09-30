using System;
using System.Threading.Tasks;

namespace HangFireProj.Jobs
{
    /// <summary>
    /// Interface for job services that can be executed by HangFire
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Execute the job
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        /// <returns>Task representing the job execution</returns>
        Task ExecuteAsync(string jobId, object parameters = null);

        /// <summary>
        /// Job name for identification and logging
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// Job description for documentation and logging
        /// </summary>
        string JobDescription { get; }
    }

    /// <summary>
    /// Interface for job services that can be executed synchronously
    /// </summary>
    public interface ISyncJobService
    {
        /// <summary>
        /// Execute the job synchronously
        /// </summary>
        /// <param name="jobId">Unique identifier for this job execution</param>
        /// <param name="parameters">Optional parameters for the job</param>
        void Execute(string jobId, object parameters = null);

        /// <summary>
        /// Job name for identification and logging
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// Job description for documentation and logging
        /// </summary>
        string JobDescription { get; }
    }
}
