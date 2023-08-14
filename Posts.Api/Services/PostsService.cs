using System.Net;
using Microsoft.Extensions.Options;
using Posts.Api.Config;
using Posts.Api.Models;

namespace Posts.Api.Services;

public class PostsService : IPostsService
{
    private readonly PostsApiOptions _apiConfig;
    private readonly HttpClient _httpClient;

    public PostsService(HttpClient httpClient, IOptions<PostsApiOptions> apiConfig)
    {
        _httpClient = httpClient;
        _apiConfig = apiConfig.Value;
    }

    public async Task<IEnumerable<Post>> GetAllPosts()
    {
        var response = await _httpClient.GetAsync(_apiConfig.Endpoint);

        if (response.StatusCode == HttpStatusCode.NotFound) return new List<Post>();

        var responseContent = response.Content;
        var allPosts = await responseContent.ReadFromJsonAsync<List<Post>>();
        return allPosts != null && allPosts.Any() ? allPosts : Enumerable.Empty<Post>();
    }
}