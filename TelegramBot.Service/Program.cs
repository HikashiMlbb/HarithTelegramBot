using Serilog;
using TelegramBot.Application;
using TelegramBot.Infrastructure;
using TelegramBot.Service;
using ILogger = Serilog.ILogger;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        using var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(hostCtx.Configuration)
            .CreateLogger();

        Log.Logger = logger;

        services.AddSingleton<ILogger>(logger);

        services.AddHostedService<Worker>();

        services.AddInfrastructure(hostCtx.Configuration, "SqlServerDefault");
        services.AddApplication();
    })
    .Build();

host.Run();
Log.CloseAndFlush();