using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Twitter.Models;


namespace Twitter.Controllers
{
    [ApiController]
    [Route("MY_API_KEY/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // Создание нового поста
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostRequest request)
        {
            _postService.CreatePost(request.Content);
            return Ok("Пост успешно создан");
        }

        // Получение всех постов
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            foreach (var post in posts)
            {
                // Вывод информации о постах в консоль
                Console.WriteLine($"Post ID: {post.PostId}, Content: {post.Content}, CreatedAt: {post.CreatedAt}");
            }
            return Ok(posts);
        }

        // Получение поста по ID
        [HttpGet("{postId}")]
        public IActionResult GetPostById(string postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // Обновление поста по ID
        [HttpPut("{postId}")]
        public IActionResult UpdatePost(string postId, [FromBody] UpdatePostRequest request)
        {
            _postService.UpdatePost(postId, request.Content);
            return Ok("Пост успешно обновлен");
        }

        // Удаление поста по ID
        [HttpDelete("{postId}")]
        public IActionResult DeletePost(string postId)
        {
            _postService.DeletePost(postId);
            return Ok("Пост успешно удален");
        }


        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllPosts()
        {
            try
            {
                //await _postService.DeleteAllPosts();
                return Ok("All posts deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting all posts: {ex.Message}");
            }
        }
    }
}

