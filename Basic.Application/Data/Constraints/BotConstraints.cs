using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Basic.Application.Data.Constraints;

public static class BotConstraints
{
    public static readonly ReceiverOptions ReceiverOptions = new()
    {
        AllowedUpdates = new[] { UpdateType.Message, UpdateType.ChatMember, UpdateType.Poll, UpdateType.CallbackQuery },
        Limit = 1,
        ThrowPendingUpdates = true
    };
}