using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository : IRepository<Sale>
{
    /// <summary>
    /// Retrieves all sales associated with a specific customer
    /// </summary>
    /// <param name="customerId">The ID of the customer whose sales are to be retrieved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of sales for the specified customer. Returns an empty list if none are found.</returns>
    Task<List<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);

}