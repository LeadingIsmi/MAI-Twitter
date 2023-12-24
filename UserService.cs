using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver; // Необходимые библиотеки для работы с MongoDB
using Twitter.Models;
using Hazelcast.DistributedObjects;
using Hazelcast.Core;

namespace Twitter
{
    // Интерфейс сервиса
    public interface IUserService
    {
        void CreateUser(string userId, string name);
        IEnumerable<User> GetAllUsers();
        Task<User> GetUserById(string userId);
        void UpdateUserProfile(string userId, string updatedName);
        void DeleteUser(string postId);
        void DeleteComment(string commentId);
        void DeleteAllUsers();
    }

    // Реализация интерфейса
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        private readonly IHMap<string, User> _userCache;

        public UserService(MongoClient mongoClient, IHMap<string, User> userCache)
        {
            var database = mongoClient.GetDatabase("Twitter_DB");
            _userCollection = database.GetCollection<User>("User");

            _userCache = userCache; ;
        }

        public void CreateUser(string userId, string name)
        {
            // Логика создания поста в базе данных MongoDB
            var user = new User { UserId = userId, UserName = name };
            _userCollection.InsertOne(user);

            // Сохраняем пользователя в кэше
            _userCache.PutAsync(userId, user);
        }

        // Чтение данных пользователей (Read):
        public IEnumerable<User> GetAllUsers()
        {
            var user = _userCollection.Find(_ => true).ToList();
            return user;
        }

        // Пример для пользователя
        public async Task<User> GetUserById(string userId)
        {
            // Проверяем наличие пользователя в кэше
            var user = await _userCache.GetAsync(userId);
            if (user != null)
            {
                return user;
            }

            // Если пользователь не найден в кэше, получаем его из базы данных и добавляем в кэш
            user = await _userCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                await _userCache.PutAsync(userId, user);
            }

            return user;
        }

        // Пример для пользователя
        public void UpdateUserProfile(string userId, string updatedName)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            var update = Builders<User>.Update
                .Set(u => u.UserName, updatedName);
            _userCollection.UpdateOne(filter, update);

            // Обновляем данные в кэше
            var user = GetUserById(userId).GetAwaiter().GetResult();
            if (user != null)
            {
                user.UserName = updatedName;
                _userCache.PutAsync(userId, user);
            }
        }

        // Удаление (Delete):
        public void DeleteUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            _userCollection.DeleteOne(filter);

            // Удаляем пользователя из кэша
            _userCache.RemoveAsync(userId);
        }

        // Пример для комментария
        public void DeleteComment(string commentId)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.CommentId, commentId);
            _commentCollection.DeleteOne(filter);
        }

        public void DeleteAllUsers()
        {
            _userCollection.DeleteMany(_ => true);

            // Очищаем кэш
            _userCache.ClearAsync();
        }
    }
}
