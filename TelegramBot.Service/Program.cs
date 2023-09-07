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
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(@"D:\Roman\Database\harith_chatbot\logs.txt")
            .CreateLogger();

        Log.Logger = logger;
        
        services.AddSingleton<ILogger>(logger);
        
        services.AddHostedService<Worker>();
        
        services.AddInfrastructure(hostCtx.Configuration);
        
        services.AddApplication();
    })
    .Build();

host.Run();