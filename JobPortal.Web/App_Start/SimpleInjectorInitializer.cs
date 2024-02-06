[assembly: WebActivator.PostApplicationStartMethod(typeof(JobPortal.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace JobPortal.Web.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
    using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
    using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
    using System;
    using Hangfire;    
    
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
                        
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));   
            
        }
     
        private static void InitializeContainer(Container container)
        {           
            container.Register<IUserService, UserService>(Lifestyle.Transient);
            container.Register<INetworkService, NetworkService>(Lifestyle.Transient);
            container.Register<IJobService, JobService>(Lifestyle.Transient);
            container.Register<IManageService, ManageService>(Lifestyle.Transient);
            container.Register<ITrackService, TrackService>(Lifestyle.Transient);
            container.Register<IHelperService, HelperService>(Lifestyle.Transient);     
            container.Register<JobActivator, SimpleInjectorJobActivator>(Lifestyle.Scoped);
            
        }
    }
}