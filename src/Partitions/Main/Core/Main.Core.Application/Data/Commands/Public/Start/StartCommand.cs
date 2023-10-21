using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Public.Start;

[Command("start")]
public record StartCommand : ITextCommand;