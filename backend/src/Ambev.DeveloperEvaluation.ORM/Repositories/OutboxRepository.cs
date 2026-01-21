using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly DefaultContext _dbContext;

    public OutboxRepository(DefaultContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<OutboxEntity>> GetUnprocessedAsync(
        int batchSize,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<OutboxEntity>()
            .FromSqlRaw("""
                            SELECT *
                            FROM outbox
                            WHERE processed_on IS NULL
                            ORDER BY occurred_on
                            FOR UPDATE SKIP LOCKED
                            LIMIT {0}
                        """,
                batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(OutboxEntity entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<OutboxEntity>().AddAsync(entity, cancellationToken);
    }
}