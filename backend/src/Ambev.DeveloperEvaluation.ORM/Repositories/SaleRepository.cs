using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(DefaultContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<List<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var query = DbSet.Where(l => l.CustomerId == customerId);

        return await query.ToListAsync(cancellationToken);
    }
     
}