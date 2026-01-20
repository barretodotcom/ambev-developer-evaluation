using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;

namespace Ambev.DeveloperEvaluation.ORM.Transactions;

public sealed class EfCoreTransactionManager : ITransactionManager
{
    private readonly DefaultContext _context;

    public EfCoreTransactionManager(DefaultContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            await action(ct);
            await _context.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
