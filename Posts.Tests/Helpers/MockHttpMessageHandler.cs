using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Moq.Protected;

namespace Posts.Tests.Helpers;

internal static class MockHttpMessageHandler<T>
{
    internal static Mock<HttpMessageHandler> SetupBasicGetResourceList(List<T> expectedResponse)
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
        };

        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(mockResponse);

        return handlerMock;
    }

    internal static Mock<HttpMessageHandler> SetupReturn404()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonSerializer.Serialize(""))
        };

        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(mockResponse);

        return handlerMock;
    }

    internal static Mock<HttpMessageHandler> SetupBasicGetResourceList(List<Post> expectedResponse, string endpoint)
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
        };

        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var handlerMock = new Mock<HttpMessageHandler>();

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(endpoint),
            Method = HttpMethod.Get
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                httpRequestMessage,
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(mockResponse);

        return handlerMock;
    }
}