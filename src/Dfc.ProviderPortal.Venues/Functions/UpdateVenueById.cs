
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;


namespace Dfc.ProviderPortal.Venues
{
    public static class UpdateVenueById
    {
        [FunctionName("UpdateVenueById")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            Venue venue = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument from JSON posted in body if not)
                log.LogInformation($"UpdateVenueById starting");
                venue = await req.Content.ReadAsAsync<Venue>(); //(dynamic)<object>

                // Insert data as new document in collection
                Document result = await new VenueStorage().UpdateDocAsync(venue, log);
                if (result == null)
                    response = req.CreateResponse(HttpStatusCode.BadRequest,
                                                  ResponseHelper.ErrorMessage($"Cannot update document with id {venue?.id}"));
                else
                    response = req.CreateResponse(HttpStatusCode.OK);

                // Return results
                venue = (dynamic)result;
                response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            } catch (Exception ex) {
                throw ex;
            }
            return response;
        }
    }
}
