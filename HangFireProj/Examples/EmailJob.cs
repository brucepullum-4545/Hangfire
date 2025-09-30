using HangFireProj.Jobs;
using System;
using System.Threading.Tasks;

namespace HangFireProj.Examples
{
    /// <summary>
    /// Example job for sending emails
    /// </summary>
    public class EmailJob : BaseJob
    {
        public override string JobName => "EmailJob";
        public override string JobDescription => "Sends email notifications to users";

        protected override async Task ExecuteJobAsync(string jobId, object parameters = null)
        {
            // BEP - Example email job implementation
            var emailParams = parameters as EmailParameters;
            if (emailParams == null)
            {
                throw new ArgumentException("Email parameters are required", nameof(parameters));
            }

            // Simulate email sending
            await Task.Delay(2000); // Simulate network delay

            // BEP - Log email sending details
            Console.WriteLine($"Sending email to: {emailParams.To}");
            Console.WriteLine($"Subject: {emailParams.Subject}");
            Console.WriteLine($"Body: {emailParams.Body}");

            // In a real implementation, you would use an email service here
            // await _emailService.SendAsync(emailParams.To, emailParams.Subject, emailParams.Body);
        }
    }

    /// <summary>
    /// Parameters for email job
    /// </summary>
    public class EmailParameters
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; } = "noreply@example.com";
    }
}
