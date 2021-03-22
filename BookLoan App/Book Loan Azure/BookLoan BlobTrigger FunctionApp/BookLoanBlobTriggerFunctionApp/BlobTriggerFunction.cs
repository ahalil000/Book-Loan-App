using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Collections.Generic;

namespace BookLoanBlobTriggerFunctionApp
{
    public static class BlobTriggerFunction
    {
        static char[] separator = { '|' };

        [FunctionName("BlobTriggerFunction")]
        public static void Run([BlobTrigger("bookloantriggerblobs/{name}", 
            Connection = "AZURE_STORAGE_CONNECTION_STRING_DEV")]Stream myBlob, 
            string name, TraceWriter log)
        {
            //bookloantriggerblobs / data /{ name}
            using (StreamReader sr = new StreamReader(myBlob))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    log.Info($"File line read: " + line);
                    string[] splitdata = line.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                    // TODO - code to process data into Azure table.
                    // ...
                }
            }

            log.Info($"C# Blob trigger function (BookLoanBlobTriggerFunctionApp) Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
