
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
    public static class SyncVenues
    {
        [FunctionName("SyncVenues")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed SyncVenues request");
            IEnumerable<Venue> venues = new VenueStorage().Sync(log, out int count);
            //task.Wait();

            // Store venues in Cosmos DB collection
            log.Info($"Inserting {count} providers to CosmosDB providers collection");
            Task<bool> task = new VenueStorage().InsertDocs(venues, log);
            task.Wait();
            //return req.CreateResponse<string>(HttpStatusCode.OK, JsonConvert.SerializeObject(output));

            // Return results
            log.Info($"SyncVenues returning results");
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(venues), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
