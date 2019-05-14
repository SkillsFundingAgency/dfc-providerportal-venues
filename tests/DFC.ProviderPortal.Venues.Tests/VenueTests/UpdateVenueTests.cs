
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Dfc.ProviderPortal.Venues;
using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;
using Xunit;


namespace Dfc.ProviderPortal.Venues.Tests
{
    public class UpdateVenueTests
    {
        private Venue _venue = null;

        private const string URI_PATH = "http://localhost:7071/api/";

        private const string VENUE_TO_UPDATE = "{" +
                                               "  \"id\": \"e4017138-9575-438e-9e27-47d1219a7e84\"," +
                                               "  \"UKPRN\": \"987654321\"," +
                                               "  \"VENUE_NAME\": \"My fab venue7\"," +
                                               "  \"ADDRESS_1\": \"124 Low Street\"," +
                                               "  \"ADDRESS_2\": \"Learnstown\"," +
                                               "  \"TOWN\": \"Manchester\"," +
                                               "  \"COUNTY\": \"Lancashire\"," +
                                               "  \"POSTCODE\": \"M23 4XY\"," +
                                               "  \"PHONE\": \"021 121 1212\"," +
                                               "  \"EMAIL\": \"a@b.com\"," +
                                               "  \"WEBSITE\": \"http://www.something.co.uk\"," +
                                               "  \"Status\": 1," +
                                               "  \"DateUpdated\": \"2018-12-25T12:34:56.4909052+00:00\"," +
                                               "  \"UpdatedBy\": \"Someone\"" +
                                               "}";
        private const string ADDED_VENUE = "{{ \"id\": \"{0}\" }}";

        public UpdateVenueTests() {
            TestHelper.AddEnvironmentVariables();
        }


        //[Fact]
        //public void RunTests()
        //{
        //    _UpdateVenue_Run();
        //    Assert.True(_venue != null);
        //}



        //[Fact]
        //private void _UpdateVenue_Run()
        //{
        //    System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "UpdateVenueById"), VENUE_TO_UPDATE);
        //    Task<HttpResponseMessage> task = UpdateVenueById.Run(rm, new LogHelper((ILogger)null));
        //    _venue = TestHelper.GetAFReturnedObject<Venue>(task);

        //    Assert.True(_venue != null);
        //}
    }
}
