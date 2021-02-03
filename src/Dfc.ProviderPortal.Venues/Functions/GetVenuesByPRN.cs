
using System;
using System.Net;
using System.Net.Http;
using System.Linq;
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
    public static class GetVenuesByPRN
    {
        //private class PostData {
        //    public string PRN { get; set; }
        //}

        [FunctionName("GetVenuesByPRN")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            IEnumerable<Venue> results = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenuesByPRN starting");

                // NOTE: This is a dirty fix as a workaround to make sure this function works with both "POST" and "GET" properly.
                // This is needed to fix a bug with this when using "GET" to keep Dev CD app working whilst API-M changes are going in.
                // TODO: Refactor this eventually.
                var PRN = string.Empty;

                try
                {
                    dynamic args = await (dynamic)req.Content.ReadAsAsync<object>();
                    PRN = req.RequestUri.ParseQueryString()["prn"]?.ToString() ?? args?.PRN;
                }
                catch (Exception e)
                {
                    PRN = req.RequestUri.ParseQueryString()["prn"]?.ToString();
                }

                if (PRN == null)
                    //throw new FunctionException("Missing PRN argument", "GetVenuesByPRN", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing PRN argument"));
                else if (!int.TryParse(PRN, out int parsed))
                    //throw new FunctionException("Invalid PRN argument", "GetVenuesByPRN", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid PRN argument"));
                else {
                    // Get data
                    results = new VenueStorage().GetByPRN(parsed, log);
                    log.LogInformation($"GetVenuesByPRN returning { results.LongCount() } venues");

                    // Return results
                    response = req.CreateResponse(results.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent);
                    response.Content = new StringContent(JsonConvert.SerializeObject(results), Encoding.UTF8, "application/json");
                }
            } catch (Exception ex) {
                throw ex;
            }
            return response;
        }
    }
}