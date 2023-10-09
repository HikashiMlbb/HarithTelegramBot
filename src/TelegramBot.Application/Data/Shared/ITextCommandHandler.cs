using Telegram.Bot.Types;

namespace TelegramBot.Application.Data.Shared;

public interface ITextCommandHandler<T> where T : ITextCommand
{
    public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
}