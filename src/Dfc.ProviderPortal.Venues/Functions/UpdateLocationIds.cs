
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;


namespace Dfc.ProviderPortal.Venues
{
    public static class UpdateLocationIds
    {
        [FunctionName("UpdateLocationIds")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed UpdateLocationIds request");

            // Store venue location ids in Cosmos DB collection
            Task<IEnumerable<Venue>> task = new VenueStorage().UpdateLocationIdsAsync(new LogHelper(log));
            task.Wait();

            // Return results
            log.LogInformation($"UpdateLocationIds returning results");
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(task.Result), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
