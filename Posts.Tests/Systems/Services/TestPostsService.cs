using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Posts.Api.Config;
using Posts.Api.Services;
using Posts.Tests.Fixtures;
using Posts.Tests.Helpers;

namespace Posts.Tests.Systems.Services;

public class TestPostsService
{
    [Fact]
    public async Task GetAllPosts_WhenCalled_InvokesHttpGetRequest()
    {
        //Arrange
        var expectedResponse = PostsFixture.GetTestPosts();
        var handlerMock = MockHttpMessageHandler<Post>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "https://example.com";

        var sut = new PostsService(httpClient, GetConfig(endpoint));

        //Act
        await sut.GetAllPosts();

        //Assert
        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetAllPosts_WhenHits404_ReturnsEmptyListOfPosts()
    {
        //Arrange
        var handlerMock = MockHttpMessageHandler<Post>.SetupReturn404();
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "https://example.com";

        var sut = new PostsService(httpClient, GetConfig(endpoint));

        //Act
        var result = await sut.GetAllPosts();

        //Assert
        result.ToList().Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllPosts_WhenCalled_ReturnsListOfPostsOfExpectedSize()
    {
        //Arrange
        var expectedResponse = PostsFixture.GetTestPosts();
        var handlerMock = MockHttpMessageHandler<Post>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "https://example.com/posts";

        var sut = new PostsService(httpClient, GetConfig(endpoint));

        //Act
        var result = await sut.GetAllPosts();

        //Assert
        result.ToList().Count.Should().Be(expectedResponse.Count);
    }

    [Fact]
    public async Task GetAllPosts_WhenCalled_InvokesConfiguredExternalUrl()
    {
        //Arrange
        var expectedResponse = PostsFixture.GetTestPosts();
        var endpoint = "http://example.com/posts";
        var handlerMock = MockHttpMessageHandler<Post>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var sut = new PostsService(httpClient, GetConfig(endpoint));

        //Act
        var result = await sut.GetAllPosts();

        var uri = new Uri(endpoint);
        //Assert
        handlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => req.Method == HttpMethod.Get
                           && req.RequestUri == uri),
                ItExpr.IsAny<CancellationToken>());
    }

    private IOptions<PostsApiOptions> GetConfig(string endpoint)
    {
        return Options.Create(new PostsApiOptions
        {
            Endpoint = endpoint
        });
    }
}