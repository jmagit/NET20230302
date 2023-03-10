using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MisFunciones {
    public static class FlujoDeTrabajo {
        #region Secuencial y Paralelo
        #region Funciones actividad
        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log) {
            var rand = new Random();
            var sleep = rand.Next(1000, 2000);
            log.LogInformation("Saying hello to {name} ({sleep}).", name, sleep);
            Thread.Sleep(sleep);
            return $"Hola {name}! ({sleep})";
        }

        [FunctionName(nameof(SayGoodbye))]
        public static string SayGoodbye([ActivityTrigger] IDurableActivityContext context, ILogger log) {
            string name = context.GetInput<string>();
            log.LogInformation("Saying goodbye to {name}.", name);
            return $"Adiós {name}! ({DateTime.Now})";
        }
        #endregion
        #region Funciones cliente
        [FunctionName("Encadenamiento_HttpStart")]
        public static async Task<HttpResponseMessage> EncadenamientoHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Encadenamiento", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("Paralelismo_HttpStart")]
        public static async Task<HttpResponseMessage> ParalelismoHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Paralelismo", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        #endregion
        #region Funciones orquestador
        [FunctionName("Encadenamiento")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            DateTime startTime = context.CurrentUtcDateTime;
            var outputs = new List<string>();
            var cityes = new[] { "Tokyo", "Seattle", "London" };

            foreach(var city in cityes)
                outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), city));

            //for(var i = 0; i < cityes.Length; i++) {
            //    await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(10)), CancellationToken.None);
            //    outputs.Add(await context.CallActivityAsync<string>(nameof(SayGoodbye), cityes[i]));
            //}

            //outputs.AddRange(await context.CallSubOrchestratorAsync<List<string>>("Paralelismo", null));

            outputs.Add($"Duration: {context.CurrentUtcDateTime.Subtract(startTime)}");
            return outputs;
        }

        [FunctionName("Paralelismo")]
        public static async Task<List<string>> RunParalelismo(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            DateTime startTime = context.CurrentUtcDateTime;
            var outputs = new List<string>();
            var cityes = new[] { "Tokyo", "Seattle", "London" };

            var parallelTasks = new List<Task<string>>();
            foreach(var city in cityes)
                parallelTasks.Add(context.CallActivityAsync<string>(nameof(SayGoodbye), city));
            await Task.WhenAll(parallelTasks);
            outputs = parallelTasks.Select(t => t.Result).ToList();
            outputs.Add($"Duration: {context.CurrentUtcDateTime.Subtract(startTime)}");
            return outputs;
        }
        #endregion
        #endregion

        #region Expiracion
        [FunctionName(nameof(DuracionVariable))]
        public static string DuracionVariable([ActivityTrigger] int duracion, ILogger log) {
            duracion *= 1000;
            log.LogInformation("Espera {name}s.", duracion);
            Thread.Sleep(duracion);
            return $"Finalizado en {duracion}! ({DateTime.Now})";
        }

        [FunctionName("Expiracion")]
        public static async Task<string> RunExpiracion(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {

            int duracion = 20;
            TimeSpan timeout = TimeSpan.FromSeconds(10);
            DateTime deadline = context.CurrentUtcDateTime.Add(timeout);

            using(var cts = new CancellationTokenSource()) {
                Task activityTask = context.CallActivityAsync<string>(nameof(DuracionVariable), duracion);
                Task timeoutTask = context.CreateTimer(deadline, cts.Token);

                Task winner = await Task.WhenAny(activityTask, timeoutTask);
                if(winner == activityTask) { // cancel timeout
                    cts.Cancel();
                } 

                return winner == activityTask ? "Completado" : (activityTask.IsCanceled ? "Expirado" : "Cancelado");
            }
        }

        [FunctionName("Expiracion_HttpStart")]
        public static async Task<HttpResponseMessage> ExpiracionHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Expiracion", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        #endregion

        #region Monitor
        [FunctionName("Periodic_Loop")]
        public static async Task RunPeriodic(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            Console.WriteLine(await context.CallActivityAsync<string>(nameof(DuracionVariable), 0));
            DateTime nextIteration = context.CurrentUtcDateTime.AddSeconds(5);
            await context.CreateTimer(nextIteration, CancellationToken.None);
            context.ContinueAsNew(null);
        }

        [FunctionName("Periodic_HttpStart")]
        public static async Task<HttpResponseMessage> OrchestrationTrigger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient client) {
            string instanceId = "StaticId";
            await client.StartNewAsync("Periodic_Loop", instanceId);
            return client.CreateCheckStatusResponse(req, instanceId);
        }
        #endregion

        #region Interacción humana
        [FunctionName("Approval")]
        public static async Task<string> RunApproval(
            [OrchestrationTrigger] IDurableOrchestrationContext context) {
            DateTime deadline = context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(90));
            using(var cts = new CancellationTokenSource()) {
                Task<bool> approvalTask = context.WaitForExternalEvent<bool>("Approval");
                Task timeoutTask = context.CreateTimer(deadline, cts.Token);
                Task winner = await Task.WhenAny(approvalTask, timeoutTask);
                if(winner == approvalTask) { // cancel timeout
                    cts.Cancel();
                }
                return winner == approvalTask ? (approvalTask.Result ? "Aprobado" : "Rechazado") : "Cancelado";
            }
        }

        [FunctionName("Approval_HttpStart")]
        public static async Task<HttpResponseMessage> ApprovalHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Approval", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        [FunctionName("Approval_OK")]
        public static async Task<IActionResult> ApprovalOK(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log) {
            string instanceId = req.Query["id"];
            await client.RaiseEventAsync(instanceId, "Approval", true);

            log.LogInformation("Approval granted with ID = '{instanceId}'.", instanceId);

            return new OkObjectResult("Approval granted");
        }
        [FunctionName("Approval_KO")]
        public static async Task<IActionResult> ApprovalKO(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log) {
            string instanceId = req.Query["id"];
            await client.RaiseEventAsync(instanceId, "Approval", false);

            log.LogInformation("Approval denied with ID = '{instanceId}'.", instanceId);

            return new OkObjectResult("Approval denied");
        }
        #endregion

        #region Entidad con estado
        [FunctionName("Counter")]
        public static void Counter([EntityTrigger] IDurableEntityContext ctx) {
            switch(ctx.OperationName.ToLowerInvariant()) {
                case "add":
                    ctx.SetState(ctx.GetState<int>() + ctx.GetInput<int>());
                    break;
                case "reset":
                    ctx.SetState(0);
                    break;
                case "get":
                    ctx.Return(ctx.GetState<int>());
                    break;
            }
        }

        [FunctionName("Counter_Orchestration")]
        public static async Task<int> RunCounterOrchestration([OrchestrationTrigger] IDurableOrchestrationContext context) {
            var entityId = new EntityId(nameof(Counter), "steps");
            int currentValue = await context.CallEntityAsync<int>(entityId, "Get");
            if(currentValue >= 10) {
                context.SignalEntity(entityId, "Reset");
            } else {
                context.SignalEntity(entityId, "Add", 1);
            }
            return await context.CallEntityAsync<int>(entityId, "Get");
        }

        [FunctionName("Counter_HttpStart")]
        public static async Task<HttpResponseMessage> CounterHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Counter_Orchestration", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("Counter_Query")]
        public static async Task<IActionResult> RunCounterQuery(
        //public static async Task<HttpResponseMessage> RunCounterQuery(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestMessage req,
        [DurableClient] IDurableEntityClient client) {
            var entityId = new EntityId(nameof(Counter), "steps");
            var stateResponse = await client.ReadEntityStateAsync<int>(entityId);
            return new OkObjectResult(stateResponse.EntityState);
            //return await Task.Run(() => req.CreateResponse(HttpStatusCode.OK, stateResponse.EntityState));
        }
        #endregion
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Contador {
        [JsonProperty("value")]
        public int CurrentValue { get; set; }

        public void Add(int amount) => this.CurrentValue += amount;

        public void Reset() => this.CurrentValue = 0;

        public int Get() => this.CurrentValue;

        [FunctionName(nameof(Contador))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<Contador>();
    }
}