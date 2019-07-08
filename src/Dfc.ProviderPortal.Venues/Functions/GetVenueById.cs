
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
        //private class PostData {
        //    public string id { get; set; }
        //}

        [FunctionName("GetVenueById")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            Venue v = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenueById starting");
                string Id = req.RequestUri.ParseQueryString()["id"]?.ToString()
                                ?? (await (dynamic)req.Content.ReadAsAsync<object>())?.id;
                if (Id == null)
                    //throw new FunctionException("Missing id argument", "GetVenueById", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing id argument"));
                else if (!Guid.TryParse(Id, out Guid parsed))
                    //throw new FunctionException("Invalid id argument", "GetVenueById", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid id argument"));
                else {
                    // Get data
                    v = new VenueStorage().GetById(parsed, log);
                    log.LogInformation($"GetVenueById returning { v?.VENUE_NAME ?? "no results"}");

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
