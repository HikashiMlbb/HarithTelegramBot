using Serilog;
using Basic.Core;
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

        services.AddBasicCore(hostCtx);
    })
    .Build();

host.Run();
Log.CloseAndFlush();