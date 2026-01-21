using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Read;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.AutoMapper;

/// <summary>
/// Provides integration tests to validate AutoMapper configuration across the application.
/// </summary>
/// <remarks>
/// This class ensures that all AutoMapper profiles defined in the application layer
/// and read layer assemblies are correctly configured. It helps prevent mapping
/// errors at runtime by asserting that the mapping configuration is valid.
/// </remarks>
public class AutoMapperConfigurationTests
{
    /// <summary>
    /// Given all AutoMapper profiles in the application and read layer assemblies
    /// When the AutoMapper configuration is built
    /// Then the configuration is valid and no mapping errors exist.
    /// </summary>
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