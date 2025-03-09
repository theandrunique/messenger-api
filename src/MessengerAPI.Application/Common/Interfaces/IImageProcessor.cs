using Microsoft.AspNetCore.Http;

namespace MessengerAPI.Application.Common.Interfaces;

public interface IImageProcessor
{
    bool IsValidImageSignature(IFormFile file);
    Task<MemoryStream> ProcessImage(IFormFile file);
}
