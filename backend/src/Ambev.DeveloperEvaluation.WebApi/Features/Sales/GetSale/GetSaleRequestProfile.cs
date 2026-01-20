using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Profile for mapping between Application and API CancelSale requests/responses
/// </summary>
public class GetSaleRequestProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale feature
    /// </summary>
    public GetSaleRequestProfile()
    {
        CreateMap<GetSaleRequest, GetSaleQuery>();
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
}