using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SupervalFunction;

public class HelloWorld
{
    private readonly ILogger<HelloWorld> _logger;

    public HelloWorld(ILogger<HelloWorld> logger)
    {
        _logger = logger;
    }

    [Function("HelloWorld")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"].FirstOrDefault() ?? "World";

        return new OkObjectResult($"Hello, {name}!");
    }
}
