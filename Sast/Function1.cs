using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SAST
{
    public static class ComputerVisionFunction
    {
        [FunctionName("ComputerVisionFunction")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP Trigger function processed a request.");

            string url = req.Query["url"];

            if (string.IsNullOrWhiteSpace(url))
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                url = data?.url ?? "";
            }


            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials("AX1234B52Z669"));
            client.Endpoint = "https://thiago-test-environment.azurewebsites.net";

            var results = await client.AnalyzeImageAsync(url, new List<VisualFeatureTypes>() {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description, VisualFeatureTypes.Tags
            });

            return new OkObjectResult(JsonConvert.SerializeObject(results));
        }
    }
}