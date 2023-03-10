using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MisFunciones {
    public static class DurableFunction {
        [FunctionName("DurableFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            var outputs = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();

            //// Replace "hello" with the name of your Durable Activity Function.
            //outputs.Add(await context.CallActivityAsync<string>("Saluda", "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>("Saluda", "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>("Saluda", "London"));
            // Replace "hello" with the name of your Durable Activity Function.
            //var f1 = await context.CallActivityAsync<string>("Saluda", "Tokyo");
            //outputs.Add(await context.CallActivityAsync<string>("Saluda", f1 + " Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>("Saluda", "London"));

            tasks.Add(context.CallActivityAsync<string>("Saluda", "Tokyo"));
            tasks.Add(context.CallActivityAsync<string>("Saluda", "Seattle"));
            tasks.Add(context.CallActivityAsync<string>("Saluda", "London"));

            await Task.WhenAll(tasks);
            outputs.AddRange(tasks.Select(t => t.Result));
            outputs.AddRange(await context.CallSubOrchestratorAsync<List<string>>("CarreraFunction", null));

            //await Task.WhenAny(tasks);
            //outputs.AddRange(tasks.Where(t => t.IsCompleted).Select(t => t.Result));
            return outputs;
        }

        [FunctionName("CarreraFunction")]
        public static async Task<List<string>> RunCarrera(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            var outputs = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();


            tasks.Add(context.CallActivityAsync<string>("Despide", ("Madrid", 1)));
            tasks.Add(context.CallActivityAsync<string>("Saluda", "Tokyo"));
            tasks.Add(context.CallActivityAsync<string>("Saluda", "Seattle"));
            tasks.Add(context.CallActivityAsync<string>("Saluda", "London"));

            await Task.WhenAny(tasks);
            outputs.AddRange(tasks.Where(t => t.IsCompleted).Select(t => t.Result));
            return outputs;
        }

        [FunctionName("Saluda")]
        public static string SayHello([ActivityTrigger] string name, ILogger log) {
            Random rnd = new Random();
            log.LogInformation("Saying hello to {name}.", name);
            Thread.Sleep(rnd.Next(1,10) * 1000);
            return $"Hello {name}! ({DateTime.Now})";
        }
        [FunctionName("Despide")]
        public static string SayGoodbye([ActivityTrigger] (string name, int duration) tupla, ILogger log) {
            Random rnd = new Random();
            log.LogInformation("Saying hello to {name}.", tupla.name);
            Thread.Sleep(tupla.duration * 1000);
            return $"Adios {tupla.name}! ({DateTime.Now})";
        }

        [FunctionName("DurableFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableFunction", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}