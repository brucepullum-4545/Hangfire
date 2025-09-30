# HangFireProj - Complete HangFire Library for .NET Framework 4.8

## What is HangFire?

**HangFire** is a background job processing library for .NET applications. It allows you to run code in the background without blocking your main application, making your web apps faster and more reliable.

### Why Use HangFire?

**Traditional Problem**: When you need to send emails, process files, or generate reports, doing it directly in your web application causes:
- Slow response times (users wait while tasks complete)
- Timeouts on long-running tasks
- Resource blocking on your web server
- Lost tasks if the web request fails

**HangFire Solution**:
- **Background Processing**: Tasks run separately from web requests
- **Reliability**: Jobs are stored in a database and retry on failure
- **Scalability**: Multiple servers can process jobs
- **Monitoring**: Web dashboard to see job status and performance
- **Scheduling**: Run jobs immediately, delayed, or on a schedule

### Real-World Examples

- **Email Notifications**: Welcome emails, password resets, order confirmations
- **File Processing**: Convert images, process CSV files, generate PDFs
- **Data Cleanup**: Archive old records, clean temporary files
- **Report Generation**: Create daily/weekly/monthly reports
- **API Calls**: Sync data with external services

## How HangFire Works

```
[Your App] → [Creates Job] → [Database Storage] → [HangFire Server] → [Executes Job]
     ↓              ↓              ↓                    ↓
  Web Request    Job Queued    Job Stored         Job Processed
```

### Job Types
1. **Fire-and-Forget Jobs**: Run immediately when created
2. **Delayed Jobs**: Run after a specific time delay
3. **Recurring Jobs**: Run on a schedule (like CRON)
4. **Continuations**: Run after another job completes

## Project Overview

This library provides everything you need to get started with HangFire:

```
HangFireProj/
├── Configuration/          # Setup and configuration classes
├── Jobs/                   # Job management classes
├── Extensions/             # Easy integration methods
├── Dashboard/              # Web dashboard configuration
└── Examples/               # Ready-to-use example jobs
```

### Key Features
- **Easy Integration**: Simple extension methods for quick setup
- **Production Ready**: Error handling, logging, security, monitoring
- **Comprehensive Examples**: Email, file processing, report generation
- **Flexible Configuration**: Extensive customization options

## Setup Instructions

### Prerequisites
- **Visual Studio 2019 or later** (required)
- SQL Server (Express, Standard, or Enterprise)
- .NET Framework 4.8

### Step 1: Add to Your Project

1. **Open Visual Studio**
2. **File → Open → Project/Solution**
3. **Select `HangFireProj.sln`**
4. **Right-click solution → Restore NuGet Packages**
5. **Right-click project → Build**
6. **Add as project reference** to your main project

### Step 2: Install Required Packages

In your main project, install these NuGet packages:

**Using Package Manager Console:**
```powershell
Install-Package Hangfire.Core -Version 1.7.32
Install-Package Hangfire.SqlServer -Version 1.7.32
Install-Package Hangfire.AspNetCore -Version 1.7.32
```

### Step 3: Database Setup

1. **Create database**:
   ```sql
   CREATE DATABASE HangFireDB;
   ```

2. **Add connection string to web.config**:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Server=.;Database=HangFireDB;Integrated Security=true;" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

### Step 4: Initialize HangFire

In your `Global.asax.cs`:
```csharp
using HangFireProj.Extensions;

protected void Application_Start(object sender, EventArgs e)
{
    // BEP - Initialize HangFire with default settings
    var options = "DefaultConnection".InitializeHangFireFromConfig();
    options.ConfigureDashboard();
}

protected void Application_End(object sender, EventArgs e)
{
    // BEP - Clean up resources
    HangFireExtensions.StopHangFire();
}
```

### Step 5: Enable Dashboard

Add to your `web.config`:
```xml
<system.webServer>
  <handlers>
    <add name="HangfireDashboard" path="hangfire" type="Hangfire.Dashboard.DashboardHandler" verb="*" />
  </handlers>
</system.webServer>

<!-- BEP - Assembly binding redirects to resolve version conflicts -->
<runtime>
  <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
    <dependentAssembly>
      <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
      <bindingRedirect oldVersion="0.0.0.0-4.0.5.0" newVersion="4.0.5.0" />
    </dependentAssembly>
    <dependentAssembly>
      <assemblyIdentity name="System.Memory" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
      <bindingRedirect oldVersion="0.0.0.0-4.0.5.0" newVersion="4.0.5.0" />
    </dependentAssembly>
    <dependentAssembly>
      <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
      <bindingRedirect oldVersion="0.0.0.0-4.0.5.0" newVersion="4.0.5.0" />
    </dependentAssembly>
    <dependentAssembly>
      <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
      <bindingRedirect oldVersion="0.0.0.0-12.0.0.0" newVersion="12.0.0.0" />
    </dependentAssembly>
  </assemblyBinding>
</runtime>
```

Access dashboard at: `http://yoursite.com/hangfire`

## Usage Examples

### Creating Your First Job

```csharp
using HangFireProj.Jobs;
using System.Threading.Tasks;

public class WelcomeEmailJob : BaseJob
{
    public override string JobName => "WelcomeEmailJob";
    public override string JobDescription => "Sends welcome email to new users";

    protected override async Task ExecuteJobAsync(string jobId, object parameters = null)
    {
        // BEP - Get email parameters
        var emailData = parameters as EmailData;
        
        // BEP - Send email (replace with real email service)
        Console.WriteLine($"Sending welcome email to: {emailData.EmailAddress}");
        await Task.Delay(2000); // Simulate work
        
        // In real implementation:
        // await _emailService.SendAsync(emailData.EmailAddress, "Welcome!", emailBody);
    }
}

public class EmailData
{
    public string EmailAddress { get; set; }
    public string UserName { get; set; }
}
```

### Running Jobs

```csharp
using HangFireProj.Jobs;

// BEP - Immediate job
var jobId = JobService.EnqueueJob<WelcomeEmailJob>("email-001", emailData);

// BEP - Delayed job (run after 30 minutes)
var delayedJobId = JobService.ScheduleJob<WelcomeEmailJob>("email-002", TimeSpan.FromMinutes(30), emailData);

// BEP - Recurring job (run daily at 2 AM)
JobService.CreateRecurringJob<DailyReportJob>("daily-report", "0 2 * * *", reportData);
```

## Configuration Options

```csharp
var options = new HangFireOptions
{
    ConnectionString = "your-connection-string",
    SchemaName = "HangFire",                    // Database schema
    WorkerCount = 10,                          // Number of background workers
    Queues = new[] { "default", "priority" },  // Queue names
    DashboardPath = "/hangfire",               // Dashboard URL
    EnableDashboard = true,                    // Enable dashboard
    AllowAnonymousDashboard = false,           // Security setting
    EnableAutomaticRetry = true,               // Retry failed jobs
    MaxRetryAttempts = 3,                      // How many retries
    JobExpirationDays = 7,                     // How long to keep job data
    EnableServerHeartbeat = true,              // Health monitoring
    ServerHeartbeatInterval = 30               // Heartbeat frequency
};
```

## CRON Expression Reference

```
┌───────────── minute (0 - 59)
│ ┌───────────── hour (0 - 23)
│ │ ┌───────────── day of month (1 - 31)
│ │ │ ┌───────────── month (1 - 12)
│ │ │ │ ┌───────────── day of week (0 - 6)
│ │ │ │ │
* * * * *
```

**Common Examples:**
- `"0 2 * * *"` - Daily at 2:00 AM
- `"0 */6 * * *"` - Every 6 hours
- `"0 0 * * 1"` - Every Monday at midnight
- `"0 0 1 * *"` - First day of every month
- `"0 9-17 * * 1-5"` - Every hour from 9 AM to 5 PM, Monday to Friday

## Best Practices

### Job Design
1. **Keep Jobs Small**: Break large tasks into smaller jobs
2. **Handle Exceptions**: Always wrap risky code in try-catch
3. **Use Parameters**: Pass data through parameters, not closures
4. **Idempotent**: Jobs should be safe to run multiple times
5. **Logging**: Always log important information

### Performance
1. **Worker Count**: Start with CPU count × 2, adjust based on load
2. **Queue Management**: Use different queues for different priorities
3. **Database**: Use dedicated database for HangFire
4. **Monitoring**: Watch dashboard for performance issues

### Security
1. **Dashboard Access**: Always implement proper authorization
2. **Connection Strings**: Use secure connection strings
3. **Job Data**: Don't store sensitive data in job parameters
4. **Network**: Secure database access

## Dependencies

- **Hangfire.Core** (1.7.32) - Core HangFire functionality
- **Hangfire.SqlServer** (1.7.32) - SQL Server storage
- **Hangfire.AspNetCore** (1.7.32) - ASP.NET integration
- **Newtonsoft.Json** (12.0.3) - JSON serialization
- **System.Data.SqlClient** (4.8.5) - SQL Server connectivity

## Getting Help

1. **Check the Dashboard**: First place to look for job status
2. **Console Logs**: Check application console for error messages
3. **HangFire Documentation**: [https://docs.hangfire.io/](https://docs.hangfire.io/)
4. **Stack Overflow**: Search for "HangFire" tag

