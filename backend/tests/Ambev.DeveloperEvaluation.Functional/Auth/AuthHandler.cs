using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Functional.Auth;
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ClaimsPrincipal _user;

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ClaimsPrincipal? user = null
    ) : base(options, logger, encoder, clock)
    {
        _user = user ?? new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "SalesManager")
            }, "Test")
        );
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var ticket = new AuthenticationTicket(_user, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public class OptionsMonitorStub : IOptionsMonitor<AuthenticationSchemeOptions>
{
    public AuthenticationSchemeOptions CurrentValue => new();
    public AuthenticationSchemeOptions Get(string name) => new();
    public IDisposable OnChange(Action<AuthenticationSchemeOptions, string> listener) => null!;
}
