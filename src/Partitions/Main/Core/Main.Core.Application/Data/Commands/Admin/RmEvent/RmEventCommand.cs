using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Admin.RmEvent;

[Command("rmevnt")]
public record RmEventCommand : ITextCommand;