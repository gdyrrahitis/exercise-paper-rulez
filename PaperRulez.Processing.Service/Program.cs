namespace PaperRulez.Processing.Service
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Consumers;
    using Models;
    using Processors;
    using RabbitMQ.Client;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Processing Service";
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component.For<IRabbitMqManager>().ImplementedBy<RabbitMqManager>().LifestyleSingleton());
            container.Register(Component.For<IHandler>().ImplementedBy<Handler>());
            container.Register(Component.For<IProcessorFactory>().ImplementedBy<ProcessorFactory>());
            container.Register(Component.For<IProcessor>().ImplementedBy<LookupProcessor>());

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqConstants.RabbitMqUri)
            };
            container.Register(Component.For<IConnectionFactory>().Instance(connectionFactory));
            using (var manager = container.Resolve<IRabbitMqManager>())
            {
                manager.ListenForFileLoadRequestCommand();
                Console.ReadLine();
            }
        }
    }
}
