using FileEventing.Service.Configuration;
using FileEventing.Service.Data;
using FileEventing.Service.Events.FileChanged;
using FileEventing.Service.Events.FileCreated;
using FileEventing.Service.Events.FileDeleted;
using FileEventing.Service.Events.FileRenamed;
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

        services
            .AddOptions<InfluxDbOptions>()
            .BindConfiguration(InfluxDbOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<FileChangedEventConsumer>();
            mt.AddConsumer<FileCreatedEventConsumer>();
            mt.AddConsumer<FileDeletedEventConsumer>();
            mt.AddConsumer<FileRenamedEventConsumer>();

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
                    e.ConfigureConsumer<FileChangedEventConsumer>(ctx);
                    e.ConfigureConsumer<FileCreatedEventConsumer>(ctx);
                    e.ConfigureConsumer<FileDeletedEventConsumer>(ctx);
                    e.ConfigureConsumer<FileRenamedEventConsumer>(ctx);
                });
            });
        });

        services.AddSingleton<IFileEventWriter, InfluxDbFileEventWriter>();
    });