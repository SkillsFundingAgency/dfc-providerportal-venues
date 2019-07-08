
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
    public static class AddVenue
    {
        [FunctionName("AddVenue")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            Venue venue = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"AddVenue starting");
                venue = await req.Content.ReadAsAsync<Venue>(); //(dynamic)<object>

                if (venue.ADDRESS_1 == null)
                    //throw new FunctionException("Missing ADDRESS_1 argument", "GetVenuesByPRNAndName", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing ADDRESS_1 argument"));
                else {
                    // Insert data as new document in collection
                    var results = await new VenueStorage().InsertDocAsync(venue, log);

                    // Return results
                    response = req.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(venue), Encoding.UTF8, "application/json");
                }
            } catch (Exception ex) {
                throw ex;
            }
            return response;
        }
    }
}
