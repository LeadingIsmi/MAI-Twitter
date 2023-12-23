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
                    postService.CreatePost("Пример поста 1");

                    // Чтение всех постов и вывод информации в консоль
                    var posts = postService.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при выполнении операции удаления постов: " + ex.Message);
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
