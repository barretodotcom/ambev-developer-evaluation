using Ambev.DeveloperEvaluation.Application.Abstractions.Commands;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public sealed class CancelSaleHandler : ICommandHandler<CancelSaleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of CreateUserHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work responsible for committing transactional changes.</param>
    /// <param name="saleRepository">The sale repository</param>
    public CancelSaleHandler(IUnitOfWork unitOfWork, ISaleRepository saleRepository)
    {
        _unitOfWork = unitOfWork;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the CancelSaleCommand command
    /// </summary>
    /// <param name="command">The CancelSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <remarks>
    /// This operation changes the sale status to <see cref="SaleStatus.Cancelled"/>
    /// and may trigger domain events.
    /// </remarks>
    public async Task Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale is null)
            throw new DomainException("The sale does not exist.");
        
        sale.Cancel();
        foreach (var saleItem in sale.Items)
        {
            saleItem.Cancel();
        }        
        
        _saleRepository.Update(sale);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}