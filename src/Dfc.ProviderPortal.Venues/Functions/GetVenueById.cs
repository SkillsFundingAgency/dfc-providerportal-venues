
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
    public static class GetVenueById
    {
        private class PostData {
            public string id { get; set; }
        }

        [FunctionName("GetVenueById")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            // Get passed argument (from query if present, if from JSON posted in body if not)
            log.LogInformation($"GetVenueById starting");
            string Id = req.RequestUri.ParseQueryString()["id"]?.ToString()
                            ?? (await req.Content.ReadAsAsync<PostData>())?.id;
            if (Id == null)
                throw new FunctionException("Missing id argument", "GetVenueById", null);
            if (!Guid.TryParse(Id, out Guid parsed))
                throw new FunctionException("Invalid id argument", "GetVenueById", null);

            //log.Info("C# HTTP trigger function processed GetVenueById request");
            Venue v = new VenueStorage().GetById(parsed, log);

            // Return results
            log.LogInformation($"GetVenueById returning { v.VENUE_NAME }");
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(v), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
