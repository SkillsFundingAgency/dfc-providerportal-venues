
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;


namespace Dfc.ProviderPortal.Venues
{
    public static class GetAllVenues
    {
        [FunctionName("GetAllVenues")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                log.LogInformation("GetAllVenues starting");
                Task<IEnumerable<Venue>> task = new VenueStorage().GetAllAsync(new LogHelper(log));

                // Return results
                log.LogInformation($"GetAllVenues returning results");
                response = req.CreateResponse(HttpStatusCode.OK);
                task.Wait();
                response.Content = new StringContent(JsonConvert.SerializeObject(task.Result), Encoding.UTF8, "application/json");

            } catch (Exception ex) {
                throw ex;
            }
            return response;
        }
    }
}
