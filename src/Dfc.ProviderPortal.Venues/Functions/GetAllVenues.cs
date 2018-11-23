
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
    public static class GetAllVenues
    {
        [FunctionName("GetAllVenues")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed GetAllVenues request");
            Task<IEnumerable<Venue>> task = new VenueStorage().GetAll(log);
            task.Wait();

            // Return results
            log.Info($"GetAllVenues returning results");
            //return req.CreateResponse<string>(HttpStatusCode.OK, JsonConvert.SerializeObject(task.Result));
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(task.Result), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
