using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MisFunciones {
    public class Coreografia {
        [FunctionName("Queue-Function")]
        [return: Table("Registro")]
        public Mensaje RunQueue([QueueTrigger("my-queue")] string myQueueItem, ILogger log) {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            return new Mensaje() { Texto = myQueueItem };
        }

        [FunctionName("Blob-Function")]
        [return: Table("Registro")]
        public Fichero RunBlob([BlobTrigger("my-contenedor/{name}")] Stream myBlob,
            string name,
            [Queue("my-queue")] out string myQueueItem,
            ILogger log) {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            myQueueItem = $"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes";
            return new Fichero() { Name = name, Size = myBlob.Length };
        }

        [FunctionName("Genera-Fichero")]
        [OpenApiOperation(operationId: "RunGeneraFichero")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> RunGeneraFichero(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "genera-fichero/{name}")] HttpRequest req,
            string name,
            [Blob("my-contenedor/{name}-{rand-guid}.txt", FileAccess.Write)] Stream fich,
            ILogger _logger) {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            fich.Write(Encoding.ASCII.GetBytes(responseMessage));
            return new OkObjectResult(responseMessage);
        }
    }
}
