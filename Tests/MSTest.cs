using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using Twitter.Models;

namespace Twitter.Tests
{
    [TestClass]
    public class ElasticsearchServiceTests
    {
        private IElasticsearchService _elasticsearchService;

        public ElasticsearchServiceTests(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [TestInitialize]
        public void Initialize(IConfiguration configuration)
        {
            // Создание экземпляра сервиса перед каждым тестом
            // Здесь происходит инициализация сервиса с необходимыми параметрами

            var elasticsearchConfig = Configuration.GetSection("Elasticsearch").Get<ElasticsearchOptions>();

            var settings = new ConnectionSettings(new Uri(elasticsearchConfig.Url))
                .DefaultIndex(elasticsearchConfig.DefaultIndex);

            var elasticClient = new ElasticClient(settings);

            // Инициализация сервиса с использованием созданного клиента
            _elasticsearchService = new ElasticsearchService(elasticClient);
        }

        [TestMethod]
        public async Task IndexPostAsync_ShouldIndexPostSuccessfully()
        {
            // Создание поста для индексации
            var post = new Post
            {
                PostId = "1",
                Content = "Test post content",
                CreatedAt = DateTime.Now // Установите дату создания поста
                // Другие поля поста
            };

            // Индексация поста
            var result = await _elasticsearchService.IndexPostAsync(post);

            // Проверка успешности индексации
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SearchPostsByContentAsync_ShouldReturnMatchingPosts()
        {
            var post2 = new Post
            {
                PostId = "2",
                Content = "Test post 2 content",
                CreatedAt = DateTime.Now // Установите дату создания поста
                // Другие поля поста
            };

            // Выполнение поиска постов по тексту
            var query = "search query";
            var result = await _elasticsearchService.SearchPostsAsync(query);

            // Проверка наличия найденных постов
            Assert.IsNotNull(result);
            // Проверки на соответствие ожидаемым результатам
        }

        // Другие тесты для методов поиска по тексту и поиска по периоду
    }
}
