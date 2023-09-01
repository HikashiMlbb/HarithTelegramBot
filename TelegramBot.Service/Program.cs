using Telegram.Bot.Polling;
using TelegramBot.Application.Services;
using TelegramBot.Domain.Interfaces;
using TelegramBot.Service;
using TelegramBot.Domain.POCOs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.AddTOptions(hostCtx.Configuration);
        services.AddHostedService<Worker>();

        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IBot, Bot>();
        services.AddSingleton<IStoppingToken, StoppingToken>();
    })
    .Build();

host.Run();