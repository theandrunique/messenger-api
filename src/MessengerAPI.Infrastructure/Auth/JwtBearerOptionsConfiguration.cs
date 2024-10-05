using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

internal class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;
    private readonly IKeyManagementService _keyService;
    private readonly ILogger<JwtBearerOptionsConfiguration> _logger;

    public JwtBearerOptionsConfiguration(
        ILogger<JwtBearerOptionsConfiguration> logger,
        IOptions<JwtSettings> jwtSettings,
        IKeyManagementService keyManagementService)
    {
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
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
            ValidAudience = _jwtSettings.Audience,
            ValidIssuer = _jwtSettings.Issuer,
            IssuerSigningKeyResolver = GetPublicKeys
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                try
                {
                    var userIdentity = new UserIdentity(context.Principal);
                    //if (!await _revokedTokenStore.IsTokenValidAsync(userIdentity.TokenId))
                    //{
                        //context.Fail("Token revoked");
                        //return;
                    //}

                    context.Principal.AddIdentity(userIdentity);
                }
                catch
                {
                    context.Fail("Invalid payload");
                }
            },

            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["accessToken"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken))
                    context.Token = accessToken;

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                _logger.LogWarning("Authentication failed: {Message}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    }

    private IEnumerable<SecurityKey> GetPublicKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
    {
        if (!_keyService.TryGetKeyById(kid, out var key))
        {
            yield return null;
        }
        var securityKey = new RsaSecurityKey(key);
        yield return securityKey;
    }
}
