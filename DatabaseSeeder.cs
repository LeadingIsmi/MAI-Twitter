using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Twitter.Models;

namespace Twitter
{
    public class DatabaseSeeder
    {
        private readonly IMongoCollection<Post> _postCollection;

        public DatabaseSeeder(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Twitter_DB");
            _postCollection = database.GetCollection<Post>("Posts");
        }

        public void RecreateCollection()
        {
            // Удаление существующей коллекции
            _postCollection.Database.DropCollection("Posts");

            // Создание новой коллекции (если нужно)
            _postCollection.Database.CreateCollection("Posts");
        }

        public void SeedData()
        {
            // Очистка коллекции (если нужно)
            //_postCollection.DeleteMany(Builders<Post>.Filter.Empty);

            // Добавление тестовых данных
            var posts = new[]
            {
                new Post { Content = "Первый пост", CreatedAt = DateTime.UtcNow },
                new Post { Content = "Второй пост", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            };

            _postCollection.InsertMany(posts);
        }
    }
}
