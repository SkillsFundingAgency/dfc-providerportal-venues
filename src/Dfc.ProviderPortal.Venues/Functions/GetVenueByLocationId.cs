
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
    public static class GetVenueByLocationId
    {
        //private class PostData {
        //    public string LocationId { get; set; }
        //}

        [FunctionName("GetVenueByLocationId")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            Venue v = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenueByLocationId starting");
                string LocationId = req.RequestUri.ParseQueryString()["LocationId"]?.ToString()
                                            ?? (await (dynamic)req.Content.ReadAsAsync<object>())?.LocationId;
                if (LocationId == null)
                    //throw new FunctionException("Missing LocationId argument", "GetVenueByLocationId", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing LocationId argument"));
                else if (!int.TryParse(LocationId, out int parsed))
                    //throw new FunctionException("Invalid LocationId argument", "GetVenueByLocationId", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid LocationId argument"));
                else {
                    // Get data
                    v = new VenueStorage().GetByLocationId(parsed, log);
                    log.LogInformation($"GetVenueByLocationId returning { v?.VENUE_NAME ?? "no results"}");

                    // Return results
                    response = req.CreateResponse(v == null ? HttpStatusCode.NoContent : HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(v), Encoding.UTF8, "application/json");
                }
            } catch (Exception ex) {
                throw ex;
            }
            return response;
        }
    }
}
