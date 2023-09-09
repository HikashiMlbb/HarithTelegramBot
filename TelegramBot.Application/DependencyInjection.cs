using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;
using TelegramBot.Application.Data.Commands.Common.AttributesAndInterfaces;
using TelegramBot.Application.Data.Interfaces;
using TelegramBot.Application.Services;

namespace TelegramBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddCommonCommands(services);
        AddUpdateHandlers(services);
        
        // Adding services
        services.AddSingleton<IBot, Bot>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IStoppingToken, StoppingToken>();
        services.AddSingleton<ICommandExecutor, CommandExecutor>();

        return services;
    }

    private static void AddUpdateHandlers(IServiceCollection services)
    {
        Type[] types = typeof(DependencyInjection).Assembly.GetTypes();
        Type typeofInterface = typeof(IHandler);

        foreach (Type type in types.Where(t => t is { IsClass: true, IsPublic: true } && t.IsAssignableTo(typeofInterface)))
        {
            services.AddSingleton(typeofInterface, type);
        }
    }

    private static IServiceCollection AddCommonCommands(IServiceCollection services)
    {
        Type[] types = typeof(DependencyInjection).Assembly.GetTypes();
        Type typeofInterface = typeof(ICommonCommand);

        foreach (Type type in types.Where(t => t is { IsClass: true, IsPublic: true } && t.IsAssignableTo(typeofInterface)))
        {
            services.AddSingleton(typeofInterface, type);
        }

        return services;
    }
}