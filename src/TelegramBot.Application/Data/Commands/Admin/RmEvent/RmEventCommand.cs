using TelegramBot.Application.Data.Shared;

namespace TelegramBot.Application.Data.Commands.Admin.RmEvent;

[Command("rmevnt")]
public record RmEventCommand : ITextCommand;