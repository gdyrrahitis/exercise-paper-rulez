namespace PaperRulez.Finalizer.Service
{
    using System;
    using System.IO;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Loader.Service;
    using Loader.Service.BlobStorages;
    using Loader.Service.Loaders;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Finalizer Service";
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component.For<IRabbitMqManager>().ImplementedBy<RabbitMqManager>().LifestyleSingleton());
            container.Register(Component.For<ILoaderFactory>().ImplementedBy<LoaderFactory>().LifestyleSingleton());
            container.Register(Component.For<ILoader>().ImplementedBy<TextFileLoader>().LifestyleTransient());
            container.Register(Component.For<IBlobStorage>().ImplementedBy<FileSystemBlobStorage>()
                .DependsOn(Dependency.OnValue("root", Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyClients")))
                .LifestyleTransient());

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqConstants.RabbitMqUri)
            };
            container.Register(Component.For<IConnectionFactory>().Instance(connectionFactory));
            using (var manager = container.Resolve<IRabbitMqManager>())
            {
                manager.ListenForProcessEndEvent();
                Console.ReadLine();
            }
        }
    }
}
