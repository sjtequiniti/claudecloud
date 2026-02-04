using CPCRoots;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SupervalFunction;

public class GetRoot
{
    private readonly ILogger<GetRoot> _logger;

    public GetRoot(ILogger<GetRoot> logger)
    {
        _logger = logger;
    }

    [Function("GetRoot")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("GetRoot function processing request.");

        string? type = req.Query["type"].FirstOrDefault();
        string? numberStr = req.Query["number"].FirstOrDefault();

        if (string.IsNullOrEmpty(type))
        {
            return new BadRequestObjectResult("Missing 'type' parameter. Use 'square' or 'cube'.");
        }

        if (type != "square" && type != "cube")
        {
            return new BadRequestObjectResult($"Invalid type '{type}'. Use 'square' or 'cube'.");
        }

        if (string.IsNullOrEmpty(numberStr))
        {
            return new BadRequestObjectResult("Missing 'number' parameter.");
        }

        if (!double.TryParse(numberStr, out double number))
        {
            return new BadRequestObjectResult($"Invalid number '{numberStr}'.");
        }

        if (number < 0)
        {
            return new BadRequestObjectResult("Number must be non-negative.");
        }

        using var roots = new RootsClass();
        var (result, hasError, errMsg) = type == "square"
            ? roots.SquareRootCalculation(number)
            : roots.CubeRootCalculation(number);

        if (hasError)
        {
            return new BadRequestObjectResult($"APL error: {errMsg}");
        }

        return new OkObjectResult(result.ToString());
    }
}
