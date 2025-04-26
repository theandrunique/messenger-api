using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Common.Interfaces;

public interface IImageProcessor
{
    bool IsValidImageSignature(IFormFile file);
    Task<MemoryStream> ProcessImageAsWebp(IFormFile file, int width, int height);
}
