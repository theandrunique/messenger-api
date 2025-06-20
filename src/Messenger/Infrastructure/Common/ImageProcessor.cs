using Messenger.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Messenger.Infrastructure.Common;

public class ImageProcessor : IImageProcessor
{
    public bool IsValidImageSignature(IFormFile file)
    {
        try
        {
            using var stream = new BinaryReader(file.OpenReadStream());
            var headerBytes = stream.ReadBytes(12);
            return Image.DetectFormat(headerBytes) != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<MemoryStream> ProcessImageAsWebp(IFormFile file, int width, int height)
    {
        await using var stream = file.OpenReadStream();

        using var image = await Image.LoadAsync(stream);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(width, height),
            Mode = ResizeMode.Crop
        }));

        var output = new MemoryStream();
        await image.SaveAsWebpAsync(output);
        output.Position = 0;
        return output;
    }
}
