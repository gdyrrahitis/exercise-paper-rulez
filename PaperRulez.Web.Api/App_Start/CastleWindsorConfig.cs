namespace PaperRulez.Web.Api
{
    using System;
    using System.IO;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Loader.Service;
    using Loader.Service.BlobStorages;
    using Loader.Service.Loaders;
    using PaperRulez.Models;
    using RabbitMQ.Client;

    public static class CastleWindsorConfig
    {
        public static void Register(HttpConfiguration configuration, IWindsorContainer container)
        {
            configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Classes.FromThisAssembly()
                .Pick().If(t => t.Name.EndsWith("Controller"))
                .Configure(configurer => configurer.Named(configurer.Implementation.Name))
                .LifestylePerWebRequest());


            container.Register(Component.For<IRabbitMqManager>().ImplementedBy<RabbitMqManager>().LifestyleSingleton());
            container.Register(Component.For<ILoaderFactory>().ImplementedBy<LoaderFactory>().LifestyleSingleton());
            container.Register(Component.For<ILoader>().ImplementedBy<TextFileLoader>().LifestyleTransient());
            container.Register(Component.For<IBlobStorage>().ImplementedBy<FileSystemBlobStorage>()
                .DependsOn(Dependency.OnValue("root", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyClients")))
                .LifestyleTransient());

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqConstants.RabbitMqUri)
            };
            container.Register(Component.For<IConnectionFactory>().Instance(connectionFactory).LifestyleSingleton());
        }
    }
}