using ContactManager.Web.Proxy;
using ContactManager.Web.Models;
using Serilog;
//using Microsoft.Extensions.Logging;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace ContactManager.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IApiService, ApiService>();
            container.RegisterType<IAppSettings, AppSettings>();

            /*
            container.RegisterInstance<ILoggerFactory>(loggerFactory);
            container.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));
            container.RegisterType<ILogger>(new ContainerControlledLifetimeManager(), new InjectionFactory((ctr, type, name) =>
            {
                ILogger log = new Serilog.LoggerConfiguration()
                    //.WriteTo.Console() //Your serilog config here
                    .CreateLogger();

                return log;
            }));
            */
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}