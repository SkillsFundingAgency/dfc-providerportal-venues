
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
    public class VenuesTests
    {
        private IEnumerable<Venue> _venues = null;
        private Venue _venue = null;

        private const string URI_PATH = "http://localhost:7071/api/";
        private const long EXPECTED_COUNT = 15969;

        private const string VENUE_TO_ADD = "{" +
                                            "  \"UKPRN\": \"123456789\"," +
                                            "  \"VENUE_NAME\": \"My fab venue2\"," +
                                            "  \"ADDRESS_1\": \"123 High Street\"," +
                                            "  \"ADDRESS_2\": \"Learnsville\"," +
                                            "  \"TOWN\": \"Birmingham\"," +
                                            "  \"COUNTY\": \"West Midlands\"," +
                                            "  \"POSTCODE\": \"B12 3YZ\"" +
                                            "}";
        private const string ADDED_VENUE = "{ \"id\": \"{0}\" }";

        private const string VENUE_BY_ID = "{ \"id\": \"e3f1acbc-9eb2-4c38-81ec-fb2feb270035\" }";
        private const string VENUE_BY_PRN = "{ \"PRN\": 123456789 }";
        private const string VENUE_BY_PRN_AND_NAME = "{" +
                                                     "  \"PRN\": 123456789," +
                                                     "  \"Name\": \"My fab venue\" }";

        public VenuesTests() {
            TestHelper.AddEnvironmentVariables();
        }


        [Fact]
        public void Playlist_AddVenue()
        {
            AddVenue_Run();
            GetVenueById_NewlyAdded();
            Assert.True(_venue != null);
        }




        [Fact]
        public void GetAllVenues_ReturnsResults()
        {
            //Task<HttpResponseMessage> task = GetAllVenues.Run(new System.Net.Http.HttpRequestMessage(
            //                                                        HttpMethod.Post, "http://localhost:7071/api/GetAllVenues"),
            //                                                   new LogHelper((ILogger)null));

            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetAllVenues"), "");
            Task<HttpResponseMessage> task = GetAllVenues.Run(rm, new LogHelper((ILogger)null));

            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);
            Assert.True(_venues.Any());
            //Assert.All<Venue>(venues, v => Guid.TryParse(v.id.ToString(), out Guid g));
        }

        [Fact]
        public void GetVenuesByPRN_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_PRN);
            Task<HttpResponseMessage> task = GetVenuesByPRN.Run(rm, new LogHelper((ILogger)null));
            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);

            Assert.True(_venues.Any());
        }

        [Fact]
        public void GetVenuesByPRNAndName_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_PRN_AND_NAME);
            Task<HttpResponseMessage> task = GetVenuesByPRNAndName.Run(rm, new LogHelper((ILogger)null));
            _venues = TestHelper.GetAFReturnedObjects<Venue>(task);

            Assert.True(_venues.Any());
        }

        [Fact]
        public void GetAllVenues_ExpectedCount()
        {
            Assert.True(_venues.LongCount() == EXPECTED_COUNT);
        }

        [Fact]
        public void AddVenue_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "AddVenue"), VENUE_TO_ADD);
            Task<HttpResponseMessage> task = AddVenue.Run(rm, new LogHelper((ILogger)null));
            _venue = TestHelper.GetAFReturnedObject<Venue>(task);

            Assert.True(_venue != null);
        }

        [Fact]
        public void GetVenueById_NewlyAdded()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             string.Format(ADDED_VENUE, _venue.id.ToString()));
            Task<HttpResponseMessage> task = AddVenue.Run(rm, new LogHelper((ILogger)null));
            _venue = TestHelper.GetAFReturnedObject<Venue>(task);

            Assert.True(_venue != null);
        }

        [Fact]
        public void GetAllVenues_CountAfterAdding()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetAllVenues"), "");
            Task<HttpResponseMessage> task = GetAllVenues.Run(rm, new LogHelper((ILogger)null));
            IEnumerable<Venue> venues = TestHelper.GetAFReturnedObjects<Venue>(task);

            Assert.True(venues.LongCount() == EXPECTED_COUNT + 1);
        }

        [Fact]
        public void GetVenueById_Run()
        {
            System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
                                                                             VENUE_BY_ID);
            Task<HttpResponseMessage> task = GetVenueById.Run(rm, new LogHelper((ILogger)null));
            Venue venue = TestHelper.GetAFReturnedObject<Venue>(task);

            Assert.True(venue != null);
        }
    }
}
