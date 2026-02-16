using AddNumbersinFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SupervalFunction;

public class ColumnSums
{
    private readonly ILogger<ColumnSums> _logger;

    public ColumnSums(ILogger<ColumnSums> logger)
    {
        _logger = logger;
    }

    [Function("ColumnSums")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("ColumnSums function processing request.");

        using var reader = new StreamReader(req.Body);
        var csv = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(csv))
        {
            return new BadRequestObjectResult("Missing CSV payload.");
        }

        // CPC requires CRLF line endings
        csv = csv.Replace("\r\n", "\n").Replace("\n", "\r\n");

        var tempPath = Path.Combine(Path.GetTempPath(), $"columnsums-{Guid.NewGuid()}.csv");
        try
        {
            await File.WriteAllTextAsync(tempPath, csv);

            using var adder = new AddNumbers();
            var cpcResult = adder.AddNumbersinFile(tempPath);

            // CPC returns nested array [[sums...]]; unwrap outer layer
            var result = cpcResult;
            if (result.StartsWith("[[") && result.EndsWith("]]"))
            {
                result = result.Substring(1, result.Length - 2);
            }

            return new OkObjectResult(result);
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }
}
