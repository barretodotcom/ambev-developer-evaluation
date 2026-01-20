using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Read;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.AutoMapper;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void AutoMapper_Configuration_Should_Be_Valid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(ApplicationLayer).Assembly);
            cfg.AddMaps(typeof(ApplicationReadLayer).Assembly);
        });

        config.AssertConfigurationIsValid();
    }
}