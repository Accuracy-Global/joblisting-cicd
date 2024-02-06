using Hangfire;
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Services.Contracts;
#pragma warning restore CS0234 // The type or namespace name 'Services' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using SimpleInjector.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Injector = SimpleInjector;

namespace JobPortal.Web
{
    public class SimpleInjectorJobActivator : JobActivator
    {
        private readonly Injector.Container _container;
        private readonly Injector.Lifestyle _lifestyle;

        public SimpleInjectorJobActivator(Injector.Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;         
        }
    

        public override object ActivateJob(Type jobType)
        {
            return _container.GetInstance(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            if (_lifestyle == null || _lifestyle != Injector.Lifestyle.Scoped)
            {
                return new SimpleInjectorScope(_container, new Injector.Scope(_container));
            }
            return new SimpleInjectorScope(_container, Injector.Lifestyle.Scoped.GetCurrentScope(_container));
        }
    }

    internal class SimpleInjectorScope : JobActivatorScope
    {
        private readonly Injector.Container _container;
        private readonly Injector.Scope _scope;

        public SimpleInjectorScope(Injector.Container container, Injector.Scope scope)
        {
            _container = container;
            _scope = scope;
        }

        public override object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public override void DisposeScope()
        {
            if (_scope != null)
            {
                _scope.Dispose();
            }
        }
    }
}