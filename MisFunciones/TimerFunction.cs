using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MisFunciones
{
    public class TimerFunction
    {


        [FunctionName("TimerFunction")]
        [return: Queue("myqueue-items")]
        public static string RunTimerTrigger([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, ILogger log) {
            if(myTimer.IsPastDue) {
                log.LogInformation("Timer is running late!");
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            return $"C# Timer trigger function executed at: {DateTime.Now}";
        }

        [FunctionName("QueueTrigger")]
        public static void QueueTrigger([QueueTrigger("myqueue-items")] string myQueueItem, ILogger log) {
            log.LogInformation($"C# function processed: {myQueueItem}");
            Console.WriteLine(myQueueItem);
        }

    }
}
