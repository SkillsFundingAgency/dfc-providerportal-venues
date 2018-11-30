
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
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
    public static class GetVenuesByPRNAndProviderName
    {
        private class PostData {
            public string PRN { get; set; }
            public string ProviderName { get; set; }
        }

        [FunctionName("GetVenuesByPRNAndProviderName")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
                                                          ILogger log)
        {
            // Get passed argument (from query if present, if from JSON posted in body if not)
            log.LogInformation($"GetVenuesByPRNAndProviderName starting");
            string PRN = req.RequestUri.ParseQueryString()["prn"]?.ToString()
                            ?? (await req.Content.ReadAsAsync<PostData>())?.PRN;
            string ProviderName = req.RequestUri.ParseQueryString()["ProviderName"]?.ToString()
                            ?? (await req.Content.ReadAsAsync<PostData>())?.ProviderName;
            if (PRN == null)
                throw new FunctionException("Missing PRN argument", "GetVenuesByPRNAndProviderName", null);
            if (ProviderName == null)
                throw new FunctionException("Missing ProviderName argument", "GetVenuesByPRNAndProviderName", null);
            if (!int.TryParse(PRN, out int parsed))
                throw new FunctionException("Invalid PRN argument", "GetVenuesByPRNAndProviderName", null);

            log.LogInformation("C# HTTP trigger function processing GetVenuesByPRNAndProviderName request");
            IEnumerable<Venue> results = new VenueStorage().GetByPRN(parsed, log)
                                                           .Where(p => p.VENUE_NAME == ProviderName);

            // Return results
            log.LogInformation($"GetVenuesByPRNAndProviderName returning results");
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(results), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
