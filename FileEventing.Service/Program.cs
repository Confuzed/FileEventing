using FileEventing.Contract;
using FileEventing.Service.Events.FileModifiedEvent;
using FileEventing.Service.Events.FileUpsertRequest;
using FileEventing.Shared.Configuration;
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
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileChangedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileCreatedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileDeletedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileRenamedEvent>>();

            mt.AddConsumer<UpsertFileRecordConsumer>();

            mt.UsingRabbitMq((ctx, mq) =>
            {
                mq.Host(serviceHost, host =>
                {
                    host.Username(serviceUser);
                    host.Password(servicePassword);
                });
                
                mq.ReceiveEndpoint("file-events", e =>
                {
                    e.ConfigureConsumer<StoreModifiedFileEventConsumer<IFileChangedEvent>>(ctx);
                    e.ConfigureConsumer<StoreModifiedFileEventConsumer<IFileCreatedEvent>>(ctx);
                    e.ConfigureConsumer<StoreModifiedFileEventConsumer<IFileDeletedEvent>>(ctx);
                    e.ConfigureConsumer<StoreModifiedFileEventConsumer<IFileRenamedEvent>>(ctx);
                    e.ConfigureConsumer<UpsertFileRecordConsumer>(ctx);
                });
            });
            
            mt.AddRequestClient<IUpsertFileRequest>();
        });

        services.AddMassTransitHostedService();
    })
    .RunConsoleAsync();