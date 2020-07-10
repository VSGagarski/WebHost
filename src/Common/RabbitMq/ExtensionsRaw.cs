using Common.Commands;
using Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Instantiation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMq
{
    public static class ExtensionsRaw
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient busClient,
    ICommandHandler<TCommand> handler)
    where TCommand : ICommand
    => busClient.SubscribeAsync<TCommand>(async msg =>
        await handler.HandleAsync(msg), context =>
            context.UseSubscribeConfiguration(cfg =>
                cfg.FromDeclaredQueue(q =>
                    q.WithName(GetQueueName<TCommand>()))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient busClient, IEventHandler<TEvent> handler)
            where TEvent : IEvent
            => busClient.SubscribeAsync<TEvent>(async msg =>
                await handler.HandleAsync(msg), ctx =>
                ctx.UseSubscribeConfiguration(cfg =>
                    cfg.FromDeclaredQueue(q =>
                        q.WithName(GetQueueName<TEvent>()))));


        private static string GetQueueName<T>()
            => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            configuration.GetSection("RabbitMq").Bind(options);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = options
            });
            services.AddSingleton<IBusClient>(_ => client);
        }
    }
}
