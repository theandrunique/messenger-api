using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Infrastructure.Auth;

internal class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthOptions _options;
    private readonly IKeyManagementService _keyService;
    private readonly ILogger<JwtBearerOptionsConfiguration> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public JwtBearerOptionsConfiguration(
        ILogger<JwtBearerOptionsConfiguration> logger,
        IOptions<ApplicationOptions> options,
        IServiceScopeFactory scopeFactory,
        IKeyManagementService keyManagementService)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value.AuthOptions;
        _keyService = keyManagementService;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        options.MapInboundClaims = false;
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _options.Audience,
            ValidIssuer = _options.Issuer,
            IssuerSigningKeyResolver = GetPublicKeys
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var userIdentity = new UserIdentity(context.Principal);

                        var _revokedTokenStore = scope.ServiceProvider.GetRequiredService<IRevokedTokenService>();

                        if (await _revokedTokenStore.IsTokenRevokedAsync(userIdentity.TokenId))
                        {
                            _logger.LogInformation("Revoked token attempted: {TokenId}", userIdentity.TokenId);
                            context.Fail("Token revoked");
                            return;
                        }

                        context.Principal?.AddIdentity(userIdentity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error during parsing token payload {Claims}",
                            context.Principal?.Claims.Select(c => new { c.Type, c.Value }));

                        context.Fail("Invalid token payload");
                    }
                }
            },

            OnAuthenticationFailed = context =>
            {
                _logger.LogInformation("Authentication failed: {Message}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    }

    private IEnumerable<SecurityKey> GetPublicKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
    {
        if (_keyService.TryGetKeyById(kid, out var key))
        {
            yield return new RsaSecurityKey(key);
        }
        else
        {
            _logger.LogInformation($"Key with id {kid} not found");
            yield break;
        }
    }
}
