using IdGen;
using Microsoft.Extensions.Logging;

namespace MessengerAPI.Core;

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
        var workerId = Math.Abs(hostname.GetHashCode() % 1024);

        logger.LogInformation("Worker ID of this instance: {workerId}", workerId);

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
