using Elastic.Clients.Elasticsearch;
using MessengerAPI.Domain.Users;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessengerAPI.Infrastructure.Users;

public class ElasticsearchIndexInitializationService : IHostedService
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ElasticsearchIndexInitializationService> _logger;

    public ElasticsearchIndexInitializationService(
        ElasticsearchClient client,
        ILogger<ElasticsearchIndexInitializationService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var indexExists = await _client.Indices.ExistsAsync("users", cancellationToken);

            if (!indexExists.Exists)
            {
                var response = await _client.Indices.CreateAsync("users", c => c
                    .Settings(s => s
                        .Analysis(a => a
                            .TokenFilters(tf => tf
                                .EdgeNGram("edge_ngram_filter", en => en
                                    .MinGram(2)
                                    .MaxGram(30)))
                            .Analyzers(an => an
                                .Custom("username_analyzer", ca => ca
                                    .Tokenizer("standard")
                                    .Filter(new List<string> { "lowercase", "asciifolding", "edge_ngram_filter" }))
                                .Custom("search_analyzer", sa => sa
                                    .Tokenizer("standard")
                                    .Filter(new List<string> { "lowercase", "asciifolding" })))))
                    .Mappings(m => m
                        .Properties<UserIndexModel>(p => p
                            .LongNumber(n => n.Id)
                            .Text(t => t.Username, txt => txt
                                .Analyzer("username_analyzer")
                                .SearchAnalyzer("search_analyzer"))
                            .Keyword(t => t.GlobalName)
                            .Keyword(t => t.Image)
                        )
                    ), cancellationToken);

                if (response.IsSuccess())
                {
                    _logger.LogInformation("Elasticsearch index 'users' created");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Elasticsearch index");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
