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

                    // �������� ������
                    postService.CreatePost("����� ���� �� ����� ��������");
                    postService.CreatePost("������ ������� ������");

                    // ������ ���� ������ � ����� ���������� � �������
                    var posts = postService.GetAllPosts();
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
                    }

                    // ��������� ID ������� ����� � ���������� ��� �����������
                    var firstPostId = posts.FirstOrDefault()?.PostId;
                    if (!string.IsNullOrEmpty(firstPostId))
                    {
                        postService.UpdatePost(firstPostId, "����� ����� ��� ������� �����");
                    }

                    /*
                    // ��������� ���� ������
                    var allposts = postService.GetAllPosts();

                    // ����� ����� �� ������
                    var postWithSpecificText = allposts.FirstOrDefault(p => p.Content.Contains("������� ���� �� ����� ��������"));

                    if (postWithSpecificText != null)
                    {
                        Console.WriteLine($"ID: {postWithSpecificText.PostId}, Content: {postWithSpecificText.Content}, CreatedAt: {postWithSpecificText.CreatedAt}");
                    }
                    else
                    {
                        Console.WriteLine("���� � ��������� ������� �� ������.");
                    }
                    // ��������� ����� �� ID
                    var postIdToFind = "6586e625ae48f6b6433cca5d";
                    var postById = postService.GetPostById(postIdToFind);

                    if (postById != null)
                    {
                        Console.WriteLine("������ ����: ");
                        Console.WriteLine($"ID: {postById.PostId}, Content: {postById.Content}, CreatedAt: {postById.CreatedAt}");
                    }
                    else
                    {
                        Console.WriteLine("���� � ��������� ID �� ������.");
                    }
                    */

                    // ��������� ���� ID � ��� ��������
                    var postIdToDelete = "6586e625ae48f6b6433cca5d";
                    var postToDelete = postService.GetPostById(postIdToDelete);
                    
                    if (postToDelete != null)
                    {
                        Console.WriteLine("�������� �����: ");
                        Console.WriteLine($"ID: {postToDelete.PostId}, Content: {postToDelete.Content}, CreatedAt: {postToDelete.CreatedAt}");
                        postService.DeletePost(postIdToDelete);
                    }
                    else
                    {
                        Console.WriteLine("���� � ��������� ID �� ������.");
                    }

                    // �������� ���� ������
                    Console.WriteLine("�������� ���� ������.");
                    postService.DeleteAllPosts();
                    var allPostsAfterDelete = postService.GetAllPosts();
                    foreach (var post in allPostsAfterDelete)
                    {
                        Console.WriteLine($"ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("������ ��� ���������� �������� � �������: " + ex.Message);
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
