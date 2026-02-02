using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace SupervalFunction.Tests;

public class GetRootTests
{
    private readonly Mock<ILogger<GetRoot>> _mockLogger;

    public GetRootTests()
    {
        _mockLogger = new Mock<ILogger<GetRoot>>();
    }

    private static HttpRequest CreateMockRequest(string? type, string? number)
    {
        var mockRequest = new Mock<HttpRequest>();
        var query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { "type", type ?? "" },
            { "number", number ?? "" }
        });
        mockRequest.Setup(r => r.Query).Returns(query);
        return mockRequest.Object;
    }

    [Fact]
    public void SquareRoot_Of16_Returns4()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("square", "16");

        var result = function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("4", result.Value);
    }

    [Fact]
    public void CubeRoot_Of27_Returns3()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("cube", "27");

        var result = function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("3", result.Value);
    }

    [Fact]
    public void SquareRoot_Of2_ReturnsApproximateValue()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("square", "2");

        var result = function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var value = double.Parse((string)result.Value!);
        Assert.True(Math.Abs(value - 1.41421356) < 0.0001);
    }

    [Fact]
    public void InvalidType_ReturnsBadRequest()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("fourth", "16");

        var result = function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void MissingType_ReturnsBadRequest()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest(null, "16");

        var result = function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void InvalidNumber_ReturnsBadRequest()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("square", "abc");

        var result = function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void MissingNumber_ReturnsBadRequest()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("square", null);

        var result = function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void NegativeNumber_ReturnsBadRequest()
    {
        var function = new GetRoot(_mockLogger.Object);
        var request = CreateMockRequest("square", "-4");

        var result = function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
