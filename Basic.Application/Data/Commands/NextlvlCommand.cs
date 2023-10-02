using Basic.Domain.Interfaces;
using Basic.Domain.ValueObjects;
using Partitions.Shared;
using Telegram.Bot.Types;

namespace Basic.Application.Data.Commands;

[Command("nextlvl")]
public class NextlvlCommand : ICommonCommand
{
    private readonly IUnitOfWork _uow;

    public NextlvlCommand(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var usr = await _uow.Members.FindUserByAccountAsync(new Account(message.From.Id, message.Chat.Id));
        usr!.Level += 1;
        await _uow.CompleteAsync(cancellationToken);
    }
}