using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WcfAndAutofac
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Create your container
            var builder = new ContainerBuilder();

            // Register your service implementations.
            builder.RegisterType<Settings>().As<ISettings>();
            builder.RegisterType<Service1>();
            builder.RegisterType<RestService1>();

            // Register your PerInstanceContextJitModuleContainer
            builder.RegisterType<PerInstanceContextJitModuleContainer>().As<IPerInstanceContextJitModuleContainer>();
            var container = builder.Build();

            // Assign the container
            AutofacHostFactory.Container = container;
        }
    }
}