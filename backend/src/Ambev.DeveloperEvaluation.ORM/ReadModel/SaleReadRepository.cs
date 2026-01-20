using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Application.Read.Common;
using Ambev.DeveloperEvaluation.Application.Read.Enums;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.ReadModel;

public sealed class SaleReadRepository : ISaleReadRepository
{
    private readonly DefaultContext _dbContext;

    public SaleReadRepository(DefaultContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static readonly Dictionary<string, Expression<Func<GetAllSalesReadModel, object>>> AllowedOrderFields =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["saleDate"] = s => s.SaleDate,
        };


    public async Task<(IReadOnlyList<GetAllSalesReadModel>, int)> GetAllSalesAsync(int page, int pageSize,
        OrderBy? orderBy,
        CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext.Sales
            .AsNoTracking();

        var totalItems = await baseQuery.CountAsync(cancellationToken);

        var query =  baseQuery
            .Include(s => s.Items)
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new GetAllSalesReadModel
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                CustomerId = s.CustomerId,
                CustomerName = s.CustomerName,
                ItemsQuantity = s.Items.Count,
                SaleDate = s.SaleDate,
                Status = s.Status.GetDescription()
            });

        query = ApplyOrderBy(query, orderBy);
        
        var data = await query.ToListAsync(cancellationToken);

        return (data, totalItems);
    }

    private IQueryable<GetAllSalesReadModel> ApplyOrderBy(IQueryable<GetAllSalesReadModel> query, OrderBy? orderBy)
    {
        if (orderBy == null)
            return query;
        
        
        if (!AllowedOrderFields.TryGetValue(orderBy.Field, out var orderExpression))
        {
            orderExpression = AllowedOrderFields["date"];
        }

        return orderBy.Direction == OrderDirection.Asc ? query.OrderBy(orderExpression) : query.OrderByDescending(orderExpression);
            
    }

    public async Task<GetSaleReadModel> GetSaleAsync(Guid saleId, CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext.Sales
            .AsNoTracking();

        var sale = await baseQuery
            .Include(s => s.Items)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new GetSaleReadModel
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                CustomerId = s.CustomerId,
                CustomerName = s.CustomerName,
                SaleDate = s.SaleDate,
                Status = s.Status.GetDescription(),
                Items = s.Items.Select(si => new GetSaleItemReadModel()
                {
                    Id = si.Id,
                    Status = si.Status.GetDescription(),
                    ProductId = si.ProductId,
                    ProductName = si.ProductName,
                    Quantity = si.Quantity,
                    UnitPrice = si.UnitPrice.Value,
                    DiscountPercentage = si.DiscountPercentage.Value,
                }).ToList(),
            })
            .FirstAsync(l => l.Id == saleId, cancellationToken);

        return sale;
    }
}