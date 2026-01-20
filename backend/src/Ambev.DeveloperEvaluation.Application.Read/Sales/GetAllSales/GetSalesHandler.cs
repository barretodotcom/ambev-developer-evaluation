using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;

public sealed class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, GetAllSalesPagedResult>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly IMapper _mapper;

    public GetAllSalesHandler(ISaleReadRepository saleReadRepository, IMapper mapper)
    {
        _saleReadRepository = saleReadRepository;
        _mapper = mapper;
    }

    public async Task<GetAllSalesPagedResult> Handle(GetAllSalesQuery query, CancellationToken cancellationToken)
    {
        var (items, totalItems) =
            await _saleReadRepository.GetAllSalesAsync(query.Page, query.PageSize, query.OrderBy, cancellationToken);

        var resultItems = _mapper.Map<List<GetAllSalesResult>>(items);

        return new GetAllSalesPagedResult()
        {
            Data = resultItems,
            PageSize = query.PageSize,
            CurrentPage = query.Page,
            TotalItems = totalItems,
        };
    }
}