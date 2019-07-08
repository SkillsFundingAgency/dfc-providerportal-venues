using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dfc.ProviderPortal.Venues.Functions
{
    public class GetVenuesByUKPRNAndPostCode
    {
        [FunctionName("GetVenuesByUKPRNAndPostCode")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
                                                  ILogger log)
        {
            IEnumerable<Venue> results = null;
            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.InternalServerError);

            try
            {
                // Get passed argument (from query if present, if from JSON posted in body if not)
                log.LogInformation($"GetVenuesByUKPRNAndPostCode starting");
                string UKPRN = req.RequestUri.ParseQueryString()["UKPRN"]?.ToString() ?? (await (dynamic)req.Content.ReadAsAsync<object>())?.UKPRN;
                string PostCode = req.RequestUri.ParseQueryString()["PostCode"]?.ToString() ?? (await (dynamic)req.Content.ReadAsAsync<object>())?.PostCode;

                if (UKPRN == null)
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing UKPRN argument"));
                else if (PostCode == null)
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Missing PostCode argument"));
                else if (!int.TryParse(UKPRN, out int parsedUKPRN))
                    response = req.CreateResponse(HttpStatusCode.BadRequest, ResponseHelper.ErrorMessage("Invalid UKPRN argument"));
                else
                {
                    // Get data
                    results = new VenueStorage().GetByPRN(parsedUKPRN, log)
                                                .Where(p => p.POSTCODE.ToLower() == PostCode.ToLower());

                    // Return results
                    response = req.CreateResponse(results.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent);
                    response.Content = new StringContent(JsonConvert.SerializeObject(results), Encoding.UTF8, "application/json");
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
