using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Twitter.Models;

namespace Twitter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var postService = services.GetRequiredService<IPostService>();

                    // Создание постов
                    postService.CreatePost("Ильяс спит во время сериалов");
                    postService.CreatePost("Исмаил похавал чипсов");

                    // Чтение всех постов и вывод информации в консоль
                    var posts = postService.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
                    }

                    // Получение ID первого поста и обновление его содержимого
                    var firstPostId = posts.FirstOrDefault()?.PostId;
                    if (!string.IsNullOrEmpty(firstPostId))
                    {
                        postService.UpdatePost(firstPostId, "Новый текст для первого поста");
                    }

                    /*
                    // Получение всех постов
                    var allposts = postService.GetAllPosts();

                    // Поиск поста по тексту
                    var postWithSpecificText = allposts.FirstOrDefault(p => p.Content.Contains("Ильясик спит во время сериалов"));

                    if (postWithSpecificText != null)
                    {
                        Console.WriteLine($"ID: {postWithSpecificText.PostId}, Content: {postWithSpecificText.Content}, CreatedAt: {postWithSpecificText.CreatedAt}");
                    }
                    else
                    {
                        Console.WriteLine("Пост с указанным текстом не найден.");
                    }
                    // Получение поста по ID
                    var postIdToFind = "6586e625ae48f6b6433cca5d";
                    var postById = postService.GetPostById(postIdToFind);

                    if (postById != null)
                    {
                        Console.WriteLine("Найден пост: ");
                        Console.WriteLine($"ID: {postById.PostId}, Content: {postById.Content}, CreatedAt: {postById.CreatedAt}");
                    }
                    else
                    {
                        Console.WriteLine("Пост с указанным ID не найден.");
                    }
                    */

                    // Удаляемый пост ID и его удаление
                    var postIdToDelete = "6586e625ae48f6b6433cca5d";
                    var postToDelete = postService.GetPostById(postIdToDelete);
                    
                    if (postToDelete != null)
                    {
                        Console.WriteLine("Удаление поста: ");
                        Console.WriteLine($"ID: {postToDelete.PostId}, Content: {postToDelete.Content}, CreatedAt: {postToDelete.CreatedAt}");
                        postService.DeletePost(postIdToDelete);
                    }
                    else
                    {
                        Console.WriteLine("Пост с указанным ID не найден.");
                    }

                    // Удаление всех постов
                    Console.WriteLine("Удаление всех постов.");
                    postService.DeleteAllPosts();
                    var allPostsAfterDelete = postService.GetAllPosts();
                    foreach (var post in allPostsAfterDelete)
                    {
                        Console.WriteLine($"ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при выполнении операции с постами: " + ex.Message);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
