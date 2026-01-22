using Ambev.DeveloperEvaluation.Domain.Dtos.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Services.Sales;

public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerById(Guid id, CancellationToken cancellationToken);
}