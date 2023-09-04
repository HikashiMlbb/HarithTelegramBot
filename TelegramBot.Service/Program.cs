using Telegram.Bot.Polling;
using TelegramBot.Application;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Service;
using TelegramBot.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostCtx, services) =>
    {
        services.AddHostedService<Worker>();
        
        services.AddInfrastructure(hostCtx.Configuration);
        
        services.AddApplication();
    })
    .Build();

host.Run();