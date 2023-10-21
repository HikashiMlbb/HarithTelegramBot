using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Services;
using TelegramBot.Application.Services.Interfaces;
using TelegramBot.Application.Shared;
using TelegramBot.Partitions.Shared.Commands;

namespace TelegramBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddTextCommand(services);
        AddUpdateHandlers(services);

        // Adding services
        services.AddSingleton<IBotService, BotService>();
        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<IStoppingToken, CancellationService>();
        services.AddSingleton<ICommandExecuteService, CommandExecuteService>();


        return services;
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

    private static void AddTextCommand(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var typeofTextCommandInterface = typeof(ITextCommand);
        var commands = new Stack<ITextCommand>();

        var results = new Dictionary<ITextCommand, Type?>();
        
        IncludeTextCommands(services, assemblies, typeofTextCommandInterface, commands);
        IncludeTextCommandHandlers(services, commands, assemblies, results);

        var sb = new StringBuilder();
        var areThereFailures = results.Any(x => x.Value == null);
        var notNullCommands = results.Where(x => x.Value is not null).ToArray();
        var nullCommands = results.Where(x => x.Value is null).ToArray();

        sb.AppendLine($"It was successfully added {notNullCommands.Length} / {commands.Count} bot commands:");
        foreach (var notNullCommand in notNullCommands)
        {
            sb.AppendLine($"{notNullCommand.Key.GetType().FullName}: {notNullCommand.Value!.FullName}");
        }

        Log.Information(sb.ToString());
        sb.Clear();
        
        if (!areThereFailures)
        {
            return;
        }

        sb.AppendLine($"There are has NOT been added {nullCommands.Length} bot commands:");
        foreach (var nullCommand in nullCommands)
        {
            sb.AppendLine($"{nullCommand.Key.GetType().FullName}");
        }
        
        Log.Warning(sb.ToString(), nullCommands.Select(x => x.Key).ToArray());
    }

    private static void IncludeTextCommands(IServiceCollection services, IEnumerable<Assembly> assemblies,
        Type typeofTextCommandInterface, Stack<ITextCommand> commands)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAssignableTo(typeofTextCommandInterface))
                {
                    continue;
                }

                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (type.GetCustomAttribute<CommandAttribute>() is null)
                {
                    continue;
                }

                commands.Push(Activator.CreateInstance(type) as ITextCommand ?? throw new InvalidOperationException());
                services.AddSingleton(typeofTextCommandInterface, type);
            }
        }
    }

    private static void IncludeTextCommandHandlers(IServiceCollection services, Stack<ITextCommand> commands, Assembly[] assemblies, Dictionary<ITextCommand, Type?> results) 
    {
        foreach (var command in commands)
        {
            var typeofCommand = command.GetType();
            var typeofInterface = typeof(ITextCommandHandler<>);

            var targetType = typeofInterface.MakeGenericType(typeofCommand);
            
            results.Add(command, null);

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsAssignableTo(targetType))
                    {
                        continue;
                    }

                    if (!type.IsClass || type.IsAbstract)
                    {
                        continue;
                    }

                    services.AddSingleton(targetType, type);
                    results[command] = type;
                }
            }
        }
        
        
    }

    #endregion
}