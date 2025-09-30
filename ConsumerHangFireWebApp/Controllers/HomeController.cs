using HangFireProj.Jobs;
using ConsumerHangFireWebApp.Jobs;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConsumerHangFireWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Test the Customer Welcome Job
        /// </summary>
        [HttpPost]
        public JsonResult TestWelcomeJob()
        {
            try
            {
                // BEP - Create sample parameters for the welcome job
                var welcomeParams = new CustomerWelcomeParameters
                {
                    CustomerId = new Random().Next(1000, 9999),
                    CustomerName = "John Doe",
                    EmailAddress = "john.doe@example.com",
                    SendAccountSetupInfo = true,
                    SubscribeToNewsletter = true,
                    SignupDate = DateTime.UtcNow
                };

                // BEP - Enqueue the job using the HangFireProj library
                var jobId = JobService.EnqueueJob<CustomerWelcomeJob>(
                    $"welcome-{welcomeParams.CustomerId}", 
                    welcomeParams);

                return Json(new 
                { 
                    success = true, 
                    message = $"Customer Welcome Job queued successfully! Job ID: {jobId}",
                    jobId = jobId,
                    customerName = welcomeParams.CustomerName,
                    customerId = welcomeParams.CustomerId
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Error queuing job: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Test the Order Processing Job
        /// </summary>
        [HttpPost]
        public JsonResult TestOrderJob()
        {
            try
            {
                // BEP - Create sample parameters for the order processing job
                var orderId = new Random().Next(10000, 99999);
                var orderParams = new OrderProcessingParameters
                {
                    OrderId = orderId,
                    CustomerId = new Random().Next(1000, 9999),
                    CustomerName = "Jane Smith",
                    CustomerEmail = "jane.smith@example.com",
                    OrderTotal = 159.99m,
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = "123 Main St, Anytown, USA 12345",
                    PaymentMethod = "Credit Card",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ProductId = 1, ProductName = "Laptop Computer", Quantity = 1, UnitPrice = 99.99m },
                        new OrderItem { ProductId = 2, ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 29.99m }
                    }
                };

                // BEP - Enqueue the job using the HangFireProj library
                var jobId = JobService.EnqueueJob<OrderProcessingJob>(
                    $"order-{orderParams.OrderId}", 
                    orderParams);

                return Json(new 
                { 
                    success = true, 
                    message = $"Order Processing Job queued successfully! Job ID: {jobId}",
                    jobId = jobId,
                    orderId = orderParams.OrderId,
                    orderTotal = orderParams.OrderTotal,
                    customerName = orderParams.CustomerName
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Error queuing job: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Test scheduled job - Customer Welcome Job delayed by 30 seconds
        /// </summary>
        [HttpPost]
        public JsonResult TestScheduledJob()
        {
            try
            {
                // BEP - Create sample parameters for scheduled job
                var welcomeParams = new CustomerWelcomeParameters
                {
                    CustomerId = new Random().Next(1000, 9999),
                    CustomerName = "Bob Johnson",
                    EmailAddress = "bob.johnson@example.com",
                    SendAccountSetupInfo = true,
                    SubscribeToNewsletter = false,
                    SignupDate = DateTime.UtcNow
                };

                // BEP - Schedule the job to run in 30 seconds
                var jobId = JobService.ScheduleJob<CustomerWelcomeJob>(
                    $"scheduled-welcome-{welcomeParams.CustomerId}", 
                    TimeSpan.FromSeconds(30),
                    welcomeParams);

                return Json(new 
                { 
                    success = true, 
                    message = $"Scheduled Welcome Job queued successfully! Will run in 30 seconds. Job ID: {jobId}",
                    jobId = jobId,
                    customerName = welcomeParams.CustomerName,
                    customerId = welcomeParams.CustomerId,
                    scheduledFor = DateTime.UtcNow.AddSeconds(30).ToString("yyyy-MM-dd HH:mm:ss") + " UTC"
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Error scheduling job: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Test recurring job - set up a daily report job
        /// </summary>
        [HttpPost]
        public JsonResult TestRecurringJob()
        {
            try
            {
                // BEP - Create parameters for recurring job
                var welcomeParams = new CustomerWelcomeParameters
                {
                    CustomerId = 9999,
                    CustomerName = "Daily Test Customer",
                    EmailAddress = "daily.test@example.com",
                    SendAccountSetupInfo = false,
                    SubscribeToNewsletter = true,
                    SignupDate = DateTime.UtcNow
                };

                // BEP - Create recurring job that runs every minute for demo purposes
                // In production, you'd use something like "0 9 * * *" for daily at 9 AM
                var jobId = JobService.CreateRecurringJob<CustomerWelcomeJob>(
                    "daily-welcome-test", 
                    "*/1 * * * *",  // Every minute for demo
                    welcomeParams);

                return Json(new 
                { 
                    success = true, 
                    message = $"Recurring job created successfully! Will run every minute. Job ID: {jobId}",
                    jobId = jobId,
                    cronExpression = "*/1 * * * *",
                    description = "Runs every minute for demo purposes"
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Error creating recurring job: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Delete the recurring job
        /// </summary>
        [HttpPost]
        public JsonResult DeleteRecurringJob()
        {
            try
            {
                // BEP - Delete the recurring job
                JobService.DeleteRecurringJob("daily-welcome-test");

                return Json(new 
                { 
                    success = true, 
                    message = "Recurring job deleted successfully!" 
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Error deleting recurring job: {ex.Message}" 
                });
            }
        }
    }
}
