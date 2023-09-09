using Serilog;
using Serilog.Core;
using TelegramBot.Application;
using TelegramBot.Service;
using TelegramBot.Infrastructure;
using ILogger = Serilog.ILogger;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        using Logger logger = new LoggerConfiguration()
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