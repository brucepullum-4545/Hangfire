using HangFireProj.Jobs;
using System;
using System.Threading.Tasks;

namespace ConsumerHangFireWebApp.Jobs
{
    /// <summary>
    /// Example job for sending welcome emails to new customers
    /// This demonstrates how to create a custom job in a consumer application
    /// </summary>
    public class CustomerWelcomeJob : BaseJob
    {
        public override string JobName => "CustomerWelcomeJob";
        public override string JobDescription => "Sends welcome email to new customers with account setup information";

        protected override async Task ExecuteJobAsync(string jobId, object parameters = null)
        {
            // BEP - Cast parameters to our specific type
            var welcomeParams = parameters as CustomerWelcomeParameters;
            if (welcomeParams == null)
            {
                throw new ArgumentException("CustomerWelcomeParameters are required", nameof(parameters));
            }

            // BEP - Simulate welcome email processing
            Console.WriteLine($"Processing welcome email for customer: {welcomeParams.CustomerName}");
            Console.WriteLine($"Email address: {welcomeParams.EmailAddress}");
            Console.WriteLine($"Customer ID: {welcomeParams.CustomerId}");

            // Simulate email service delay
            await Task.Delay(3000);

            // BEP - In a real application, you would:
            // 1. Load customer data from database
            // 2. Generate personalized email content
            // 3. Send email using your email service (SendGrid, etc.)
            // 4. Update customer record with email sent timestamp
            // 5. Log the activity

            Console.WriteLine($"Welcome email sent successfully to {welcomeParams.CustomerName}!");

            // Simulate additional processing
            if (welcomeParams.SendAccountSetupInfo)
            {
                await Task.Delay(1000);
                Console.WriteLine("Account setup information email sent!");
            }

            if (welcomeParams.SubscribeToNewsletter)
            {
                await Task.Delay(500);
                Console.WriteLine("Customer subscribed to newsletter!");
            }
        }
    }

    /// <summary>
    /// Parameters for the customer welcome job
    /// </summary>
    public class CustomerWelcomeParameters
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public bool SendAccountSetupInfo { get; set; } = true;
        public bool SubscribeToNewsletter { get; set; } = false;
        public DateTime SignupDate { get; set; } = DateTime.UtcNow;
    }
}
