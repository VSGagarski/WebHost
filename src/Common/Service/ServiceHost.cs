using Common.Commands;
using Common.Events;
using Common.RabbitMq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Hosting;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace Common.Service
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost webHost;

        public ServiceHost(IWebHost webHost)
        {
            this.webHost = webHost;
        }

        public async Task RunAsync()
            => await webHost.RunAsync();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace ?? throw new InvalidOperationException();
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseStartup<TStartup>();
            return new HostBuilder(webHostBuilder.Build());
        }
    }

    public abstract class BuilderBase
    {
        public abstract ServiceHost Build();
    }

    public class HostBuilder : BuilderBase
    {
        private readonly IWebHost webHost;
        private IBusClient busClient;

        public HostBuilder(IWebHost webHost)
        {
            this.webHost = webHost;
        }

        public BusBuilder UseRabbitMq()
        {
            busClient = (IBusClient)webHost.Services.GetService(typeof(IBusClient));
            return new BusBuilder(webHost, busClient);
        }

        public override ServiceHost Build()
        {
            return new ServiceHost(webHost);
        }
    }

    public class BusBuilder : BuilderBase
    {
        private readonly IBusClient busClient;
        private readonly IWebHost webHost;

        public BusBuilder(IWebHost webHost, IBusClient busClient)
        {
            this.webHost = webHost;
            this.busClient = busClient;
        }

        public override ServiceHost Build()
        {
            return new ServiceHost(webHost);
        }

        public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
        {
            using (var serviceScope = webHost.Services.CreateScope())
            {
                var service = serviceScope.ServiceProvider;

                try
                {
                    var handler = service.GetRequiredService<ICommandHandler<TCommand>>();
                    busClient.WithCommandHandlerAsync(handler);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return this;
        }

        public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
        {
            using (var serviceScope = webHost.Services.CreateScope())
            {
                var service = serviceScope.ServiceProvider;

                try
                {
                    var handler = service.GetRequiredService<IEventHandler<TEvent>>();
                    busClient.WithEventHandlerAsync(handler);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return this;
        }
    }



}
