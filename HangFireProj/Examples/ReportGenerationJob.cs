using HangFireProj.Jobs;
using System;
using System.Threading.Tasks;

namespace HangFireProj.Examples
{
    /// <summary>
    /// Example synchronous job for report generation
    /// </summary>
    public class ReportGenerationJob : BaseSyncJob
    {
        public override string JobName => "ReportGenerationJob";
        public override string JobDescription => "Generates reports and saves them to file system";

        protected override void ExecuteJob(string jobId, object parameters = null)
        {
            // BEP - Example report generation job implementation
            var reportParams = parameters as ReportParameters;
            if (reportParams == null)
            {
                throw new ArgumentException("Report parameters are required", nameof(parameters));
            }

            // BEP - Simulate report generation
            Console.WriteLine($"Generating report: {reportParams.ReportName}");
            Console.WriteLine($"Report type: {reportParams.ReportType}");
            Console.WriteLine($"Date range: {reportParams.StartDate:yyyy-MM-dd} to {reportParams.EndDate:yyyy-MM-dd}");

            // Simulate report generation time
            var processingTime = Math.Min(reportParams.RecordCount / 100, 5000); // Max 5 seconds
            System.Threading.Thread.Sleep(processingTime);

            // BEP - Simulate file saving
            var fileName = $"{reportParams.ReportName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{reportParams.OutputFormat}";
            Console.WriteLine($"Report saved as: {fileName}");
            Console.WriteLine($"Report contains {reportParams.RecordCount} records");

            // In a real implementation, you would:
            // 1. Query the database for report data
            // 2. Generate the report (PDF, Excel, etc.)
            // 3. Save the file to the specified location
            // 4. Send notification if required
        }
    }

    /// <summary>
    /// Parameters for report generation job
    /// </summary>
    public class ReportParameters
    {
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecordCount { get; set; }
        public string OutputFormat { get; set; } = "pdf";
        public string OutputPath { get; set; } = @"C:\Reports";
    }
}
