using System;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MisFunciones
{
    public class BlobFunction
    {
        //[FunctionName("BlobFunction")]
        //public static void Run(
        //    [BlobTrigger("my-contenedor/{name}")] Stream myBlob,
        //    [Blob("my-contenedor-copia/{name}.txt", FileAccess.Write)] Stream stream,
        //    string name, ILogger log) {
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        //    var fich = new StreamWriter(stream);
        //    fich.Write((new StreamReader(myBlob)).ReadToEnd().ToUpper());
        //    fich.Close();
        //}
        [FunctionName("Blob-Function1")]
        [return: Blob("my-contenedor-copia/{name}.txt", FileAccess.Write)]
        public static Byte[] Run(
            [BlobTrigger("my-contenedor/{name}")] Stream myBlob,
            string name, ILogger log) {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            Console.WriteLine($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            return Encoding.UTF8.GetBytes((new StreamReader(myBlob)).ReadToEnd().ToUpper());
            //return new MemoryStream(Encoding.UTF8.GetBytes((new StreamReader(myBlob)).ReadToEnd().ToUpper()));
        }
    }
}
