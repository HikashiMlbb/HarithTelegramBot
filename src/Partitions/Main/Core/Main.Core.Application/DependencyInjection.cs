using Main.Core.Application.Data.Handlers;
using Main.Core.Application.Services;
using Main.Core.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application.Shared;

namespace Main.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddServices(services);
        services.AddSingleton<IMessageHandler, MessageHandler>();
        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IRewardService, RewardService>();
        services.AddSingleton<IMemberService, MemberService>();
    }
}