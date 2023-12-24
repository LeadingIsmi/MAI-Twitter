using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using Twitter.Models;

namespace Twitter
{
    public interface IElasticsearchService
    {
        Task<bool> IndexPostAsync(Post post);
        Task<IEnumerable<Post>> SearchPostsAsync(string query);
        Task<IEnumerable<Post>> FilterPostsAsync(DateTime startDate, DateTime endDate);
    }

    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        public async Task<bool> IndexPostAsync(Post post)
        {
            var response = await _elasticClient.IndexDocumentAsync(post);
            return response.IsValid;
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Post>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Content)
                        .Query(query)
                    )
                )
            );

            return searchResponse.Documents;
        }

        public async Task<IEnumerable<Post>> FilterPostsAsync(DateTime startDate, DateTime endDate)
        {
            var searchResponse = await _elasticClient.SearchAsync<Post>(s => s
                .Query(q => q
                    .DateRange(r => r
                        .Field(f => f.CreatedAt)
                        .GreaterThanOrEquals(startDate)
                        .LessThanOrEquals(endDate)
                    )
                )
            );

            return searchResponse.Documents;
        }
    }   
}
