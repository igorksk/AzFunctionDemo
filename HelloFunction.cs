using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzFunctionDemo
{
    public static class HelloFunction
    {
        [FunctionName("HelloFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
                HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string? name = req.Query["name"];

            if (string.IsNullOrEmpty(name))
            {
                using var reader = new StreamReader(req.Body);
                string requestBody = await reader.ReadToEndAsync();
                dynamic? data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(requestBody);
                name = data?.name;
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
