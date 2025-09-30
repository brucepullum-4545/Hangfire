using HangFireProj.Jobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerHangFireWebApp.Jobs
{
    /// <summary>
    /// Example job for processing customer orders
    /// This demonstrates a more complex business process job
    /// </summary>
    public class OrderProcessingJob : BaseJob
    {
        public override string JobName => "OrderProcessingJob";
        public override string JobDescription => "Processes customer orders, updates inventory, and sends confirmation emails";

        protected override async Task ExecuteJobAsync(string jobId, object parameters = null)
        {
            // BEP - Cast parameters to our specific type
            var orderParams = parameters as OrderProcessingParameters;
            if (orderParams == null)
            {
                throw new ArgumentException("OrderProcessingParameters are required", nameof(parameters));
            }

            Console.WriteLine($"Processing order #{orderParams.OrderId} for customer {orderParams.CustomerName}");
            Console.WriteLine($"Order total: ${orderParams.OrderTotal:F2}");
            Console.WriteLine($"Items count: {orderParams.Items?.Count ?? 0}");

            // BEP - Step 1: Validate order
            await ValidateOrder(orderParams);

            // BEP - Step 2: Process payment
            await ProcessPayment(orderParams);

            // BEP - Step 3: Update inventory
            await UpdateInventory(orderParams);

            // BEP - Step 4: Send confirmation email
            await SendOrderConfirmation(orderParams);

            // BEP - Step 5: Schedule follow-up tasks
            await ScheduleFollowUpTasks(orderParams);

            Console.WriteLine($"Order #{orderParams.OrderId} processed successfully!");
        }

        private async Task ValidateOrder(OrderProcessingParameters orderParams)
        {
            Console.WriteLine("Validating order...");
            await Task.Delay(1000); // Simulate validation time

            // BEP - In real implementation:
            // - Check customer account status
            // - Validate shipping address
            // - Check product availability
            // - Validate pricing

            Console.WriteLine("Order validation completed");
        }

        private async Task ProcessPayment(OrderProcessingParameters orderParams)
        {
            Console.WriteLine($"Processing payment of ${orderParams.OrderTotal:F2}...");
            await Task.Delay(2000); // Simulate payment processing

            // BEP - In real implementation:
            // - Call payment gateway API
            // - Handle payment failures
            // - Store payment transaction details
            // - Send payment receipts

            Console.WriteLine("Payment processed successfully");
        }

        private async Task UpdateInventory(OrderProcessingParameters orderParams)
        {
            Console.WriteLine("Updating inventory...");
            
            if (orderParams.Items != null)
            {
                foreach (var item in orderParams.Items)
                {
                    Console.WriteLine($"Reducing inventory for {item.ProductName} by {item.Quantity}");
                    await Task.Delay(200); // Simulate database update per item
                }
            }

            // BEP - In real implementation:
            // - Update product quantities in database
            // - Check for low stock alerts
            // - Update product availability status
            // - Log inventory changes

            Console.WriteLine("Inventory updated successfully");
        }

        private async Task SendOrderConfirmation(OrderProcessingParameters orderParams)
        {
            Console.WriteLine("Sending order confirmation email...");
            await Task.Delay(1500); // Simulate email sending

            // BEP - In real implementation:
            // - Generate order confirmation email
            // - Include order details and tracking info
            // - Send via email service
            // - Log email activity

            Console.WriteLine($"Order confirmation sent to {orderParams.CustomerEmail}");
        }

        private async Task ScheduleFollowUpTasks(OrderProcessingParameters orderParams)
        {
            Console.WriteLine("Scheduling follow-up tasks...");
            await Task.Delay(500);

            // BEP - In real implementation, you might schedule:
            // - Shipping notification job
            // - Customer feedback request job
            // - Inventory reorder job if stock is low
            // - Customer loyalty points update job

            Console.WriteLine("Follow-up tasks scheduled");
        }
    }

    /// <summary>
    /// Parameters for the order processing job
    /// </summary>
    public class OrderProcessingParameters
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
    }

    /// <summary>
    /// Order item details
    /// </summary>
    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
