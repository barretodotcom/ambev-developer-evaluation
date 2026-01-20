using AutoMapper;
using ApplicationOrderBy = Ambev.DeveloperEvaluation.Application.Read.Common.OrderBy;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Pagination;

/// <summary>
/// Profile for mapping between Application and API order by fields
/// </summary>
public class OrderByProfile : Profile
{
    public OrderByProfile()
    {
        CreateMap<OrderBy, ApplicationOrderBy>();
    }
}