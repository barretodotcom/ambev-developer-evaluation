using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly IMapper _mapper;

    public GetSaleHandler(ISaleReadRepository saleReadRepository, IMapper mapper)
    {
        _mapper = mapper;
        _saleReadRepository = saleReadRepository;
    }
    
    public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var readModel = await _saleReadRepository.GetSaleAsync(request.Id, cancellationToken);

        return _mapper.Map<GetSaleResult>(readModel);
    }
}