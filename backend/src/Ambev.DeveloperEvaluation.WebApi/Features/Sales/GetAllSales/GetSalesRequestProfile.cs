using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;

/// <summary>
/// Profile for mapping between Application Read and API GetAllSales requests/responses
/// </summary>
public class GetAllSalesRequestProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllSales feature
    /// </summary>
    public GetAllSalesRequestProfile()
    {
        CreateMap<GetAllSalesRequest, GetAllSalesQuery>();
        CreateMap<GetAllSalesResult, GetAllSalesResponse>();
    }
}