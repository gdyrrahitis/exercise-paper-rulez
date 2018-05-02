using System;

namespace PaperRulez.Store.Service
{
    using System.IO;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using Repositories;
    using Syrx;
    using Syrx.Commanders.Databases;
    using Syrx.Connectors.Databases;
    using Syrx.Connectors.Databases.SqlServer;
    using Syrx.Readers.Databases;
    using Syrx.Settings.Databases;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Store Service";
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component.For<IRabbitMqManager>().ImplementedBy<RabbitMqManager>().LifestyleSingleton());
            container.Register(Component.For<ILookupStore>().ImplementedBy<SqlServerLookupStore>());

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqConstants.RabbitMqUri)
            };
            container.Register(Component.For<IConnectionFactory>().Instance(connectionFactory));
            var raw = File.ReadAllText("Store.Configuration.Settings.json");
            var settings = JsonConvert.DeserializeObject<DatabaseCommanderSettings>(raw);
            container.Register(Component.For<IDatabaseCommanderSettings>().Instance(settings).LifestyleSingleton());
            container.Register(Component.For<IDatabaseCommandReader>().ImplementedBy<DatabaseCommandReader>());
            container.Register(Component.For<IDatabaseConnector>().ImplementedBy<SqlServerDatabaseConnector>());
            container.Register(Component.For(typeof(ICommander<>)).ImplementedBy(typeof(DatabaseCommander<>)));
            using (var manager = container.Resolve<IRabbitMqManager>())
            {
                manager.ListenForProcessingSucceededEvent();
                Console.ReadLine();
            }
        }
    }
}
