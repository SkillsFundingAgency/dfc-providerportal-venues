
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
    public class GetVenuesTests
    {
        private IEnumerable<Venue> _venues = null;

        private const string URI_PATH = "http://localhost:7071/api/";
        private const long EXPECTED_COUNT = 15979;

        private const string VENUE_BY_ID = "{ \"id\": \"e3f1acbc-9eb2-4c38-81ec-fb2feb270035\" }";
        private const string VENUE_BY_PRN = "{ \"PRN\": 123456789 }";
        private const string VENUE_BY_PRN_AND_NAME = "{" +
                                                     "  \"PRN\": 123456789," +
                                                     "  \"Name\": \"My fab venue\" }";

        public GetVenuesTests() {
            TestHelper.AddEnvironmentVariables();
        }


        [Fact]
        public void RunTests()
        {
            _GetAllVenues_ReturnsResults();
            _GetAllVenues_ExpectedCount();
            _GetVenueById_Run();
            _GetVenuesByPRN_Run();
            _GetVenuesByPRNAndName_Run();
            Assert.True(true);
        }




        [Fact]
        public void _GetAllVenues_ReturnsResults()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetAllVenues"), "");
            Task<HttpResponseMessage> task = GetAllVenues.Run(rm, new LogHelper((ILogger)null));

            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);
            Assert.True(_venues.Any());
            //Assert.All<Venue>(venues, v => Guid.TryParse(v.id.ToString(), out Guid g));
        }

        [Fact]
        public void _GetVenuesByPRN_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_PRN);
            Task<HttpResponseMessage> task = GetVenuesByPRN.Run(rm, new LogHelper((ILogger)null));
            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);

            Assert.True(_venues.Any());
        }

        [Fact]
        public void _GetVenuesByPRNAndName_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_PRN_AND_NAME);
            Task<HttpResponseMessage> task = GetVenuesByPRNAndName.Run(rm, new LogHelper((ILogger)null));
            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);

            Assert.True(_venues.Any());
        }

        [Fact]
        public void _GetAllVenues_ExpectedCount()
        {
            Assert.True(_venues.LongCount() == EXPECTED_COUNT);
        }

        [Fact]
        public void _GetVenueById_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_ID);
            Task<HttpResponseMessage> task = GetVenueById.Run(rm, new LogHelper((ILogger)null));
            Venue venue = TestHelper.GetAFReturnedObject<Venue>(task);

            Assert.True(venue != null);
        }
    }
}
