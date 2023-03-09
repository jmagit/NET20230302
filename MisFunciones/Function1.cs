using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace MisFunciones {
    public class Function1 {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> log) {
            _logger = log;
        }

        [FunctionName("ejemplo-http")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
            //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "clientes")]
            //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "clientes/{id}")]
            //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "clientes")]
            //[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "clientes/{id}")]
            //[HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "clientes/{id}")]
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            if(name == null) {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = data?.name;
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            if(string.IsNullOrEmpty(name))
                return new BadRequestObjectResult(new { status = 400, tittle = "Falta el name" });
            return new OkObjectResult(responseMessage);
        }
    }
}

