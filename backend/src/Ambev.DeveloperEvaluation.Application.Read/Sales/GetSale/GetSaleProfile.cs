using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<GetSaleReadModel, GetSaleResult>();
        CreateMap<GetSaleItemReadModel, GetSaleItemResult>();
    }
}