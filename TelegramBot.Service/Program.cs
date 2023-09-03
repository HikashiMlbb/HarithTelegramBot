using Telegram.Bot.Polling;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Service;
using TelegramBot.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.AddHostedService<Worker>();
        
        services.AddInfrastructure(hostCtx.Configuration);

        services.AddSingleton<IBot, Bot>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IStoppingToken, StoppingToken>();
        services.AddSingleton<ICommandExecutor, CommandExecutor>();
    })
    .Build();

host.Run();