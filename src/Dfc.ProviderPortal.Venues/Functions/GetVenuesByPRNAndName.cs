
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
    public static class GetVenuesByPRNAndName
    {
        //private class PostData {
        //    public string PRN { get; set; }
        //    public string ProviderName { get; set; }
        //}

        [FunctionName("GetVenuesByPRNAndName")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            IEnumerable<Venue> results = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenuesByPRNAndName starting");
                dynamic args = await (dynamic)req.Content.ReadAsAsync<object>();
                string PRN = req.RequestUri.ParseQueryString()["prn"]?.ToString() ?? args?.PRN;
                string name = req.RequestUri.ParseQueryString()["name"]?.ToString() ?? args?.Name;

                if (PRN == null)
                    //throw new FunctionException("Missing PRN argument", "GetVenuesByPRNAndName", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing PRN argument"));
                else if (name == null)
                    //throw new FunctionException("Missing ProviderName argument", "GetVenuesByPRNAndName", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing Name argument"));
                else if (!int.TryParse(PRN, out int parsed))
                    //throw new FunctionException("Invalid PRN argument", "GetVenuesByPRNAndName", null);
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid PRN argument"));
                else {
                    // Get data
                    results = new VenueStorage().GetByPRN(parsed, log)
                                              // JIRA DFC-5942 - Make search case insensitive
                                              //.Where(p => p.VENUE_NAME == name);
                                                .Where(p => p.VENUE_NAME.ToLower() == name.ToLower());


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
