using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public sealed class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new DomainException("The sale does not exist.");
        
        if (sale.Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is cancelled and cannot be updated.");
            
        sale.Update(command.SaleNumber, command.CustomerId, command.CustomerName);

        foreach (var updateSaleItemCommand in command.Items)
        {
            UpdateSaleItem(sale, updateSaleItemCommand);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<UpdateSaleResult>(sale);

        return result;
    }

    private void UpdateSaleItem(Sale sale, UpdateSaleItemCommand updateSaleItemCommand)
    {
        switch (updateSaleItemCommand.Operation)
        {
            case UpdateSaleItemOperation.Create:
                sale.AddItem(updateSaleItemCommand.ProductId, updateSaleItemCommand.ProductName,
                    updateSaleItemCommand.Quantity, Money.Create(updateSaleItemCommand.UnitPrice));
                break;
            case UpdateSaleItemOperation.Update:
                sale.UpdateItem(updateSaleItemCommand.Id!.Value, updateSaleItemCommand.ProductId,
                    updateSaleItemCommand.ProductName, updateSaleItemCommand.Quantity,
                    Money.Create(updateSaleItemCommand.UnitPrice));
                break;
            case UpdateSaleItemOperation.Cancel:
                sale.CancelItem(updateSaleItemCommand.Id!.Value);
                break;
        }
    }
}