using HangFireProj.Jobs;
using System;
using System.Threading.Tasks;

namespace HangFireProj.Examples
{
    /// <summary>
    /// Example job for data processing
    /// </summary>
    public class DataProcessingJob : BaseJob
    {
        public override string JobName => "DataProcessingJob";
        public override string JobDescription => "Processes data files and updates database records";

        protected override async Task ExecuteJobAsync(string jobId, object parameters = null)
        {
            // BEP - Example data processing job implementation
            var dataParams = parameters as DataProcessingParameters;
            if (dataParams == null)
            {
                throw new ArgumentException("Data processing parameters are required", nameof(parameters));
            }

            // BEP - Simulate data processing steps
            Console.WriteLine($"Processing data file: {dataParams.FilePath}");
            Console.WriteLine($"Processing type: {dataParams.ProcessingType}");
            Console.WriteLine($"Batch size: {dataParams.BatchSize}");

            // Simulate processing time based on file size
            var processingTime = Math.Min(dataParams.FileSize / 1000, 10000); // Max 10 seconds
            await Task.Delay(processingTime);

            // BEP - Simulate database updates
            Console.WriteLine($"Processed {dataParams.FileSize} bytes of data");
            Console.WriteLine($"Updated {dataParams.FileSize / 100} database records");

            // In a real implementation, you would:
            // 1. Read the file
            // 2. Process the data
            // 3. Update the database
            // 4. Log the results
        }
    }

    /// <summary>
    /// Parameters for data processing job
    /// </summary>
    public class DataProcessingParameters
    {
        public string FilePath { get; set; }
        public string ProcessingType { get; set; }
        public int FileSize { get; set; }
        public int BatchSize { get; set; } = 1000;
    }
}
