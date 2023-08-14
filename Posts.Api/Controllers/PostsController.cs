using Microsoft.AspNetCore.Mvc;
using Posts.Api.Services;

namespace Posts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostsService _postsService;

    public PostsController(IPostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpGet(Name = "GetPosts")]
    public async Task<IActionResult> Get()
    {
        var posts = await _postsService.GetAllPosts();

        if (posts.Any())
            return Ok(posts);
        return NotFound();
    }
}