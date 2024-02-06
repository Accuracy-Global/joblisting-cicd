using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Hangfire;
using System;
using SimpleInjector;
using JobPortal.Web.Controllers;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
using System.Net;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

[assembly: OwinStartup(typeof(JobPortal.Web.Startup))]
namespace JobPortal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var idProvider = new MyConnectionFactory();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            app.MapSignalR();

            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            //app.UseHangfireDashboard();
            app.UseHangfireDashboard("/app/dashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.UseHangfireServer();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //RecurringJob.AddOrUpdate(() => SMSSender.Send(), Cron.Weekly(DayOfWeek.Monday, 11, 00), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //RecurringJob.AddOrUpdate(() => JobAggregator.IndeedFirst(), Cron.Daily(9), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //RecurringJob.AddOrUpdate(() => JobAggregator.IndeedNext(), Cron.Daily(21), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //RecurringJob.AddOrUpdate(() => JobNotifier.FreshJobsFirst(), Cron.Daily(10), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //RecurringJob.AddOrUpdate(() => JobNotifier.FreshJobsNext(), Cron.Daily(22), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //RecurringJob.AddOrUpdate(() => JobFeeder.Generate(@"C:\inetpub\joblisting\jobsfeed.xml"), Cron.Daily(00, 15), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        }
    }
}