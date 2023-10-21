using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Admin.MkEvent;

[Command("mkevnt")]
public record MkEventCommand : ITextCommand;