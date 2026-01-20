using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Common;

/// <summary>
/// Profile for mapping between value objects and their values.
/// </summary>
public class ValueObjectsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for Mapping operation
    /// </summary>
    
    public ValueObjectsProfile()
    {
        CreateMap<Money, decimal>().ConvertUsing(src => src.Value);
        CreateMap<Percentage, decimal>().ConvertUsing(src => src.Value);

    }
}