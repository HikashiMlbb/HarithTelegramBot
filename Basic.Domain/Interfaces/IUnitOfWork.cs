namespace Basic.Domain.Interfaces;

public interface IUnitOfWork
{
    public IBotMembersRepository Members { get; init; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken);
}