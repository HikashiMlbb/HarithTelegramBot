using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Public.Stat;

[Command("stat")]
public record StatCommand : ITextCommand;