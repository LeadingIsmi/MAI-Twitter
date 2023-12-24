using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver; // Необходимые библиотеки для работы с MongoDB
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Twitter.Models;

namespace Twitter
{
    // Интерфейс сервиса
    public interface IPostService
    {
        void CreatePost(string content);
        IEnumerable<Post> GetAllPosts();
        Post GetPostById(string postId);
        void UpdatePost(string postId, string updatedContent);
        void DeletePost(string postId);
        void DeleteComment(string commentId);
        void DeleteAllPosts();
    }

    // Реализация интерфейса
    public class PostService : IPostService
    {
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        public PostService(MongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Twitter_DB");
            _postCollection = database.GetCollection<Post>("Posts");
            _userCollection = database.GetCollection<User>("User");
        }

        // Создание новых постов (Create):
        public void CreatePost(string content)
        {
            // Логика создания поста в базе данных MongoDB
            var post = new Post { Content = content, CreatedAt = DateTime.UtcNow };
            _postCollection.InsertOne(post);
        }

        // Чтение данных пользователей или постов (Read):
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = _postCollection.Find(_ => true).ToList();
            return posts;
        }

        public Post GetPostById(string postId)
        {
            var post = _postCollection.Find(p => p.PostId == postId).FirstOrDefault();
            return post;
        }

        // Обновление информации о профилях (Update):
        public void UpdatePost(string postId, string updatedContent)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.PostId, postId);
            var update = Builders<Post>.Update.Set(p => p.Content, updatedContent);
            _postCollection.UpdateOne(filter, update);
        }

        // Удаление постов или комментариев (Delete):
        public void DeletePost(string postId)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.PostId, postId);
            _postCollection.DeleteOne(filter);
        }

        // Пример для комментария
        public void DeleteComment(string commentId)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.CommentId, commentId);
            _commentCollection.DeleteOne(filter);
        }

        public void DeleteAllPosts()
        {
            _postCollection.DeleteMany(_ => true);
            //_postCollection.DeleteMany(Builders<Post>.Filter.Empty);
        }
    }
}
