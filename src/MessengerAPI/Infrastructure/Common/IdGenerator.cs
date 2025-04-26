using System.Security.Cryptography;
using System.Text;
using IdGen;
using MessengerAPI.Core;
using Microsoft.Extensions.Logging;

namespace MessengerAPI.Infrastructure.Common;

internal class IdGenerator : IIdGenerator
{
    private readonly IdGen.IdGenerator _generator;

    public IdGenerator(ILogger<IdGenerator> logger)
    {
        var hostname = Environment.GetEnvironmentVariable("HOSTNAME");
        if (string.IsNullOrEmpty(hostname))
        {
            throw new Exception("HOSTNAME environment variable is not set");
        }

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(hostname));
        var workerId = BitConverter.ToUInt16(hash, 0) % 1024;

        logger.LogInformation("Worker ID: {workerId}", workerId);

        var epoch = new DateTime(2005, 5, 20);

        var structure = new IdStructure(41, 10, 12);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));

        _generator = new IdGen.IdGenerator(workerId, options);
    }

    public long CreateId()
    {
        return _generator.CreateId();
    }
}
