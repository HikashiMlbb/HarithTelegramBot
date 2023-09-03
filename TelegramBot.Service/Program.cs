using Telegram.Bot.Polling;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Infrastructure.BotSettings;
using TelegramBot.Service;
using Microsoft.Extensions.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddOptions<BotOptions>()
            .BindConfiguration(BotOptions.PathToSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IBot, Bot>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IStoppingToken, StoppingToken>();
        services.AddSingleton<ICommandExecutor, CommandExecutor>();
    })
    .Build();

host.Run();