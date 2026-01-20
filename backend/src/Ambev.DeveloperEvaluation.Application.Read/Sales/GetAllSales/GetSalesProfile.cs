using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;

/// <summary>
/// Profile for mapping between Application and Domain CreateSale request/response
/// </summary>
public class GetAllSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale operation
    /// </summary>
    public GetAllSalesProfile()
    {
        CreateMap<GetAllSalesReadModel, GetAllSalesResult>();
    }
}