
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
    public static class GetVenuesByPRN
    {
        private class PostData {
            public string PRN { get; set; }
        }

        [FunctionName("GetVenuesByPRN")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            // Get passed argument (from query if present, if from JSON posted in body if not)
            log.LogInformation($"GetVenuesByPRN starting");
            string PRN = req.RequestUri.ParseQueryString()["prn"]?.ToString()
                            ?? (await req.Content.ReadAsAsync<PostData>())?.PRN;
            if (PRN == null)
                throw new FunctionException("Missing PRN argument", "GetVenuesByPRN", null);
            if (!int.TryParse(PRN, out int parsed))
                throw new FunctionException("Invalid PRN argument", "GetVenuesByPRN", null);

            log.LogInformation("C# HTTP trigger function processing GetVenuesByPRN request");
            IEnumerable<Venue> results = new VenueStorage().GetByPRN(parsed, log);

            // Return results
            log.LogInformation($"GetVenuesByPRN returning results");
            //return req.CreateResponse<string>(HttpStatusCode.OK, JsonConvert.SerializeObject(task.Result));
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(results), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
