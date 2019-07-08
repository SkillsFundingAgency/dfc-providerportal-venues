using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dfc.ProviderPortal.Venues.Models;
using System.Net;
using Dfc.ProviderPortal.Venues.Storage;
using Newtonsoft.Json;

namespace Dfc.ProviderPortal.Venues.Functions
{
    public static class GetVenueByVenueId
    {
        [FunctionName("GetVenueByVenueId")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
                                                         ILogger log)
        {
            Venue v = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try
            {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenueByVenueId starting");
                string venueId = req.RequestUri.ParseQueryString()["venueId"]?.ToString()
                                ?? (await (dynamic)req.Content.ReadAsAsync<object>())?.venueId;
                if (string.IsNullOrEmpty(venueId))
                {
                    log.LogInformation($"GetVenueByVenueId has missing venueId argument");
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing venueId argument"));
                }
                else if (!int.TryParse(venueId, out int parsedVenueId))
                {
                    log.LogInformation($"GetVenueByVenueId has invalid venueId argument");
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid venueId argument"));
                }
                else
                {
                    // Get data
                    v = new VenueStorage().GetByVenueId(parsedVenueId, log);
                    log.LogInformation($"GetVenueByVenueId returning { v?.VENUE_NAME ?? "no results"}");

                    // Return results
                    response = req.CreateResponse(v == null ? HttpStatusCode.NoContent : HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(v), Encoding.UTF8, "application/json");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
