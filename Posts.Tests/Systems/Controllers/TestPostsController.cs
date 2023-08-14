using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Posts.Api.Controllers;
using Posts.Api.Services;
using Posts.Tests.Fixtures;

namespace Posts.Tests.Systems.Controllers;

public class TestPostsController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockPostsService = new Mock<IPostsService>();
        mockPostsService
            .Setup(service => service.GetAllPosts())
            .ReturnsAsync(PostsFixture.GetTestPosts());

        var sut = new PostsController(mockPostsService.Object);

        //Act
        var result = (OkObjectResult)await sut.Get();

        //Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesPostsServiceExactlyOnce()
    {
        var mockPostsService = new Mock<IPostsService>();
        mockPostsService
            .Setup(service => service.GetAllPosts())
            .ReturnsAsync(new List<Post>());
        //Arrange
        var sut = new PostsController(mockPostsService.Object);

        //Act
        var result = await sut.Get();

        //Assert
        mockPostsService.Verify(service => service.GetAllPosts(), Times.Once);
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfPosts()
    {
        //Arrange
        var mockPostsService = new Mock<IPostsService>();

        mockPostsService
            .Setup(service => service.GetAllPosts())
            .ReturnsAsync(PostsFixture.GetTestPosts());

        var sut = new PostsController(mockPostsService.Object);

        //Act
        var result = await sut.Get();

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<Post>>();
    }

    [Fact]
    public async Task Get_OnNoPostsFound_Returns404()
    {
        //Arrange
        var mockPostsService = new Mock<IPostsService>();

        mockPostsService
            .Setup(service => service.GetAllPosts())
            .ReturnsAsync(new List<Post>());

        var sut = new PostsController(mockPostsService.Object);

        //Act
        var result = await sut.Get();

        //Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}