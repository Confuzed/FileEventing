using FileEventing.Contract;
using FileEventing.Service;
using FileEventing.Service.Events.FileModifiedEvent;
using FileEventing.Service.Events.FileUpsertRequest;
using FileEventing.Shared.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

await CreateHostBuilder(args).RunConsoleAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        // ReSharper disable once StringLiteralTypo
        config.AddEnvironmentVariables("FileEventing_");
    })
    .ConfigureAppConfiguration((builder, config) =>
    {
        var environment = builder.HostingEnvironment.EnvironmentName
            ?? "Development";

        config
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables(ConfigurationNames.EnvironmentConfigurationPrefix)
            .AddCommandLine(args);
    })
    .ConfigureServices((configContext, services) =>
    {
        services
            .AddOptions<BusOptions>()
            .Bind(configContext.Configuration
                .GetSection(BusOptions.ConfigurationSectionName))
            .ValidateDataAnnotations();

        var fileDbConnectionString = configContext.Configuration.GetConnectionString("Files");
        if (string.IsNullOrWhiteSpace(fileDbConnectionString))
            throw new InvalidOperationException("File data connection string missing from configuration");

        services.AddSqlServer<FileDataContext>(fileDbConnectionString);

        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileChangedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileCreatedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileDeletedEvent>>();
            mt.AddConsumer<StoreModifiedFileEventConsumer<IFileRenamedEvent>>();

            mt.AddConsumer<UpsertFileRecordConsumer>();

            mt.UsingRabbitMq((ctx, mq) =>
            {
                var mqBusOptions = ctx.GetRequiredService<IOptions<BusOptions>>();
                
                mq.Host(mqBusOptions.Value.Host, host =>
                {
                    host.Username(mqBusOptions.Value.User);
                    host.Password(mqBusOptions.Value.Password);
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
    });