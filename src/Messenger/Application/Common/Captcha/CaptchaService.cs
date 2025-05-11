using Messenger.Application.Common.Interfaces;
using Messenger.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Common.Captcha;

public class CaptchaService
{
    private readonly IHCaptchaApi _client;
    private readonly HCaptchaOptions _options;

    public CaptchaService(IHCaptchaApi client, IOptions<ApplicationOptions> options)
    {
        _client = client;
        _options = options.Value.HCaptchaOptions;
    }

    public async Task<bool> Verify(string token)
    {
        var response = await _client.Verify(_options.Secret, token);
        return response.Success;
    }
}
