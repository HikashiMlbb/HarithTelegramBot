using Serilog;
using TelegramBot.Application;
using TelegramBot.Infrastructure;
using TelegramBot.Persistence;
using TelegramBot.Service;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        using var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(hostCtx.Configuration)
            .CreateLogger();

        Log.Logger = logger;

        services.AddLogging(c => c.ClearProviders());

        services.AddHostedService<Worker>();

        services.AddApplication();
        services.AddInfrastructure(hostCtx.Configuration, "PostgreSQL");
        services.AddPersistence();
    })
    .Build();

host.Run();
Log.CloseAndFlush();