using TelegramBot.Application.Data.Shared;

namespace TelegramBot.Application.Data.Commands.Admin.Events;

[Command("events")]
public record EventsCommand : ITextCommand;