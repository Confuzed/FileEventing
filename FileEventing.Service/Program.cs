using FileEventing.Shared.Configuration;
using FileEventing.Service.Logging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        // ReSharper disable once StringLiteralTypo
        config.AddEnvironmentVariables("FILEEVENTING_");
    })
    .ConfigureServices((configContext, services) =>
    {
        var serviceOptions = configContext.Configuration
            .GetRequiredSection(BusOptions.ConfigurationSectionName)
            .Get<BusOptions>();

        var serviceHost = serviceOptions.Host
                          ?? throw new InvalidOperationException("Configuration missing service host");
        var serviceUser = serviceOptions.User
                          ?? throw new InvalidOperationException("Configuration missing service user");
        var servicePassword = serviceOptions.Password
                              ?? throw new InvalidOperationException("Configuration missing service password");

        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<FileEventLoggingConsumer>();
            
            mt.UsingRabbitMq((ctx, mq) =>
            {
                mq.Host(serviceHost, host =>
                {
                    host.Username(serviceUser);
                    host.Password(servicePassword);
                });
                
                mq.ReceiveEndpoint("file-events", e =>
                {
                    e.ConfigureConsumer<FileEventLoggingConsumer>(ctx);
                });
            });
        });

        services.AddMassTransitHostedService();
    })
    .RunConsoleAsync();