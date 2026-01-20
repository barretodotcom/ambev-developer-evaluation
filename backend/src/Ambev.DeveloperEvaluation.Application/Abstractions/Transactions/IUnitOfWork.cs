namespace Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}