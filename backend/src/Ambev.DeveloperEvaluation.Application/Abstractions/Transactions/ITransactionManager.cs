namespace Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;

public interface ITransactionManager
{
    Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken);
}