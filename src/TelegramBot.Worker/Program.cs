using Main.Core;
using Serilog;
using Serilog.Events;
using TelegramBot.Application;
using TelegramBot.Persistence;
using TelegramBot.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        using var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(hostCtx.Configuration)
            .CreateLogger();

        Log.Logger = logger;

        services.AddBasicPartition(hostCtx.Configuration);
        
        services.AddApplication();
        services.AddPersistence();
        
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
Log.CloseAndFlush();