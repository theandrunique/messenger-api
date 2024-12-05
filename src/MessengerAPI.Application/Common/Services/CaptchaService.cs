using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MessengerAPI.Application.Common.Services;

public class CaptchaService
{
    private readonly IHCaptchaApi _client;
    private readonly HCaptchaOptions _options;

    public CaptchaService(IHCaptchaApi client, IOptions<HCaptchaOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<bool> Verify(string token)
    {
        var response = await _client.Verify(_options.Secret, token);
        Console.WriteLine(JsonConvert.SerializeObject(response));
        return response.Success;
    }
}
