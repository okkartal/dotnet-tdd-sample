using Posts.Api.Models;

namespace Posts.Api.Services;

public interface IPostsService
{
    Task<IEnumerable<Post>> GetAllPosts();
}