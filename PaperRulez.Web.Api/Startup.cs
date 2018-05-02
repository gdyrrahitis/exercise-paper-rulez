using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PaperRulez.Web.Api.Startup))]

namespace PaperRulez.Web.Api
{
    using System;
    using System.Web.Http;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    public class Startup: IDisposable
    {
        private IWindsorContainer _container;

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            _container = new WindsorContainer().Install(FromAssembly.This());
            CastleWindsorConfig.Register(config, _container);
            WebApiConfig.Register(config);

            app.UseWebApi(config);
            app.MapSignalR();

            RouteConfig.RegisterRoutes(RouteTable.Routes);//MVC Routing
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _container.Resolve<IRabbitMqManager>().ListenForFileRemovedEvent();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
