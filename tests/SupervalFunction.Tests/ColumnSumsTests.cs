using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace SupervalFunction.Tests;

public class ColumnSumsTests
{
    private readonly Mock<ILogger<ColumnSums>> _mockLogger;

    public ColumnSumsTests()
    {
        _mockLogger = new Mock<ILogger<ColumnSums>>();
    }

    private static HttpRequest CreateMockPostRequest(string body)
    {
        var mockRequest = new Mock<HttpRequest>();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        mockRequest.Setup(r => r.Body).Returns(stream);
        mockRequest.Setup(r => r.ContentLength).Returns(stream.Length);
        return mockRequest.Object;
    }

    private static HttpRequest CreateMockPostRequestWithEmptyBody()
    {
        var mockRequest = new Mock<HttpRequest>();
        var stream = new MemoryStream();
        mockRequest.Setup(r => r.Body).Returns(stream);
        mockRequest.Setup(r => r.ContentLength).Returns(0);
        return mockRequest.Object;
    }

    [Fact]
    public async Task SimpleMatrix_ReturnsCorrectColumnSums()
    {
        // 3 rows x 4 columns, sums: [6, 15, 24, 33]
        var csv = "1,2,3,4\n5,6,7,8\n0,7,14,21\n";
        // col sums: 1+5+0=6, 2+6+7=15, 3+7+14=24, 4+8+21=33

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var sums = JsonSerializer.Deserialize<double[]>((string)result.Value!);
        Assert.NotNull(sums);
        Assert.Equal(4, sums.Length);
        Assert.Equal(6.0, sums[0]);
        Assert.Equal(15.0, sums[1]);
        Assert.Equal(24.0, sums[2]);
        Assert.Equal(33.0, sums[3]);
    }

    [Fact]
    public async Task TwoRows_ReturnsSums()
    {
        // CPC requires minimum 2 rows
        var csv = "10.5,20.3,30.7\n1.5,2.7,3.3\n";
        // col sums: 12, 23, 34

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var sums = JsonSerializer.Deserialize<double[]>((string)result.Value!);
        Assert.NotNull(sums);
        Assert.Equal(3, sums.Length);
        Assert.Equal(12.0, sums[0], 2);
        Assert.Equal(23.0, sums[1], 2);
        Assert.Equal(34.0, sums[2], 2);
    }

    [Fact]
    public async Task DecimalValues_SumsCorrectly()
    {
        // Values similar to numbers.csv format
        var csv = "0.00,0.00,0.00,11558.64\n0.00,0.00,0.00,812.64\n";
        // col sums: 0, 0, 0, 12371.28

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var sums = JsonSerializer.Deserialize<double[]>((string)result.Value!);
        Assert.NotNull(sums);
        Assert.Equal(4, sums.Length);
        Assert.Equal(0.0, sums[0], 2);
        Assert.Equal(0.0, sums[1], 2);
        Assert.Equal(0.0, sums[2], 2);
        Assert.Equal(12371.28, sums[3], 2);
    }

    [Fact]
    public async Task EmptyBody_ReturnsBadRequest()
    {
        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequestWithEmptyBody();

        var result = await function.Run(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ResponseIsJsonArray()
    {
        var csv = "1,2\n3,4\n";

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var json = (string)result.Value!;
        // Must be valid JSON
        var doc = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }

    [Fact]
    public async Task AllZeros_ReturnsZeros()
    {
        var csv = "0,0,0\n0,0,0\n0,0,0\n";

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var sums = JsonSerializer.Deserialize<double[]>((string)result.Value!);
        Assert.NotNull(sums);
        Assert.Equal(3, sums.Length);
        Assert.All(sums, s => Assert.Equal(0.0, s));
    }

    [Fact]
    public async Task SampleNumbersCsv_ReturnsCorrectSums()
    {
        // Use the actual sample CSV from cpc/addnumbers/v1/numbers.csv
        // 34 rows x 8 columns. Column sums computed independently:
        //   col0:  10684.80
        //   col1:      0.00
        //   col2:  31965.12
        //   col3:      0.00
        //   col4: 107935.68
        //   col5: 192302.40
        //   col6:  58791.60
        //   col7: 105673.36
        var csv = File.ReadAllText(FindSampleCsv());

        var function = new ColumnSums(_mockLogger.Object);
        var request = CreateMockPostRequest(csv);

        var result = await function.Run(request) as OkObjectResult;

        Assert.NotNull(result);
        var sums = JsonSerializer.Deserialize<double[]>((string)result.Value!);
        Assert.NotNull(sums);
        Assert.Equal(8, sums.Length);
        Assert.Equal(10684.80, sums[0], 2);
        Assert.Equal(0.00, sums[1], 2);
        Assert.Equal(31965.12, sums[2], 2);
        Assert.Equal(0.00, sums[3], 2);
        Assert.Equal(107935.68, sums[4], 2);
        Assert.Equal(192302.40, sums[5], 2);
        Assert.Equal(58791.60, sums[6], 2);
        Assert.Equal(105673.36, sums[7], 2);
    }

    private static string FindSampleCsv()
    {
        // Walk up from test binary to repo root, then into cpc/addnumbers/v1/
        var dir = AppContext.BaseDirectory;
        while (dir != null && !File.Exists(Path.Combine(dir, "README.md")))
        {
            dir = Directory.GetParent(dir)?.FullName;
        }
        if (dir == null)
            throw new InvalidOperationException("Could not find repo root");
        return Path.Combine(dir, "cpc", "addnumbers", "v1", "numbers.csv");
    }
}
