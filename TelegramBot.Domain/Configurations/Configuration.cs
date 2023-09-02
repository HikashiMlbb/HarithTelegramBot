using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Domain.Configurations;

public static class Configuration
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection(BotOptions.Position);
        var botOptions = section.Get<BotOptions>();

        if (botOptions == null)
        {
            throw new NullReferenceException(
                $"TelegramBot.Domain.POCOs.Configuration.AddTOptions: {BotOptions.Position} which required to run this bot is null.");
        }

        services.AddOptions<BotOptions>().Configure(op => section.Bind(op));
        
        return services;
    }
}