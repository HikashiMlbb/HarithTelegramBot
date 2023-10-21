using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Admin.Events;

[Command("events")]
public record EventsCommand : ITextCommand;