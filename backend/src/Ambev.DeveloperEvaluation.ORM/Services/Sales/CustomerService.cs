using Ambev.DeveloperEvaluation.Domain.Dtos.Sales;
using Ambev.DeveloperEvaluation.Domain.Services.Sales;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Services.Sales;

public class CustomerService : ICustomerService
{
    private readonly DefaultContext _dbContext;

    public CustomerService(DefaultContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<CustomerDto?> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var customerDto = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)  
            .Select(u => new CustomerDto(u.Id, u.Username))  
            .FirstOrDefaultAsync(cancellationToken);

        return customerDto;
    }
}