using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

    public static T? GetAttribute<T>(this ICommonCommand command) where T : Attribute
    {
        return command.GetType().GetCustomAttribute<T>();
    }

    public static bool IsCommand(this Message message)
    {
        return message.Entities is { } entities &&
               entities.Any(entity => entity is { Type: MessageEntityType.BotCommand, Offset: 0 });
    }

    #region Privates

    private static void AddUpdateHandlers(this IServiceCollection services)
    {
        var types = typeof(DependencyInjection).Assembly.GetTypes();
        var typeofInterface = typeof(IHandler);

        foreach (var type in types.Where(t =>
                     t is { IsClass: true, IsPublic: true } && t.IsAssignableTo(typeofInterface)))
            services.AddSingleton(typeofInterface, type);
    }

    private static void AddCommonCommands(IServiceCollection services)
    {
        var types = typeof(DependencyInjection).Assembly.GetTypes();
        var typeofInterface = typeof(ICommonCommand);

        Func<Type, bool> predicate = t => t is { IsClass: true, IsPublic: true } && t.IsAssignableTo(typeofInterface);

        foreach (var type in types.Where(predicate)) services.AddSingleton(typeofInterface, type);
    }

    #endregion
}