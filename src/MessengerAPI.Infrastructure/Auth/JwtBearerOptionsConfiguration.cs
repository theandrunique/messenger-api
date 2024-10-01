using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

internal class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private JwtSettings _jwtSettings { get; }
    private readonly IKeyManagementService _keyService;

    public JwtBearerOptionsConfiguration(
        IOptions<JwtSettings> jwtSettings,
        IKeyManagementService keyManagementService)
    {
        _jwtSettings = jwtSettings.Value;
        _keyService = keyManagementService;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
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
            OnTokenValidated = context =>
            {
                try
                {
                    var userIdentity = new UserIdentity(context.Principal);
                    context.Principal.AddIdentity(userIdentity);
                    return Task.CompletedTask;
                }
                catch
                {
                    context.Fail("Invalid payload");
                    return Task.CompletedTask;
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
        };
    }

    private IEnumerable<SecurityKey> GetPublicKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
    {
        var key = _keyService.GetKeyById(kid);
        var securityKey = new RsaSecurityKey(key);
        yield return securityKey;
    }
}
