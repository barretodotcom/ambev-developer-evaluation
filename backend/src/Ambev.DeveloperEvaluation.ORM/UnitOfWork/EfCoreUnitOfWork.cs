using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.UnitOfWork;

public class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;

    public EfCoreUnitOfWork(DefaultContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
}