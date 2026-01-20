using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Profile for mapping between Application and API CancelSale requests/responses
/// </summary>
public class CancelSaleRequestProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale feature
    /// </summary>
    public CancelSaleRequestProfile()
    {
        CreateMap<CancelSaleRequest, CancelSaleCommand>();
    }
}