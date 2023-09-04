using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;
using TelegramBot.Application.Commands.Common;
using TelegramBot.Application.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;

namespace TelegramBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registering common commands
        services.AddSingleton<ICommonCommand, AddMemberCommand>();
        services.AddSingleton<ICommonCommand, ReadMemberCommand>();
        
        // Adding services
        services.AddSingleton<IBot, Bot>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IStoppingToken, StoppingToken>();
        services.AddSingleton<ICommandExecutor, CommandExecutor>();

        return services;
    }
}