
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
    public class AddVenueTests
    {
        //private Venue _venue = null;

        private const string URI_PATH = "http://localhost:7071/api/";
        //private const long EXPECTED_COUNT = 15970;

        private const string VENUE_TO_ADD = "{" +
                                            "  \"UKPRN\": \"123456789\"," +
                                            "  \"PROVIDER_ID\": \"1020304\"," +
                                            "  \"VENUE_ID\": \"2030405\"," +
                                            "  \"VENUE_NAME\": \"My fab venue2\"," +
                                            "  \"PROV_VENUE_ID\": \"3040506\"," +
                                            "  \"ADDRESS_1\": \"123 High Street\"," +
                                            "  \"ADDRESS_2\": \"Learnsville\"," +
                                            "  \"TOWN\": \"Birmingham\"," +
                                            "  \"COUNTY\": \"West Midlands\"," +
                                            "  \"POSTCODE\": \"B1 2JP\"," +
                                            "  \"PHONE\": \"021 121 1212\"," +
                                            "  \"EMAIL\": \"a@b.com\"," +
                                            "  \"WEBSITE\": \"http://www.something.co.uk\"," +
                                            "  \"Status\": 1," +
                                            "  \"Latitude\": 2," +
                                            "  \"Longitude\": 3," +
                                            "  \"LocationId\": 4," +
                                            "  \"TribalLocationId\": 5," +
                                            "  \"DateUpdated\": \"2018-12-07T09:42:39.4909052+00:00\"," +
                                            "  \"UpdatedBy\": \"Ian\"" +
                                            "}";
        private const string ADDED_VENUE = "{{ \"id\": \"{0}\" }}";

        public AddVenueTests() {
            TestHelper.AddEnvironmentVariables();
        }


        //[Fact]
        //public void RunTests()
        //{
        //    _AddVenue_Run();
        //    _GetVenueById_NewlyAdded();
        //    _GetAllVenues_CountAfterAdding();
        //    Assert.True(_venue != null);
        //}



        //[Fact]
        //private void _AddVenue_Run()
        //{
        //    System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "AddVenue"), VENUE_TO_ADD);
        //    Task<HttpResponseMessage> task = AddVenue.Run(rm, new LogHelper((ILogger)null));
        //    _venue = TestHelper.GetAFReturnedObject<Venue>(task);

        //    Assert.True(_venue != null);
        //}

        //[Fact]
        //private void _GetVenueById_NewlyAdded()
        //{
        //    System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetVenueById"),
        //                                                                     string.Format(ADDED_VENUE, _venue.id.ToString()));
        //    Task<HttpResponseMessage> task = GetVenueById.Run(rm, new LogHelper((ILogger)null));
        //    _venue = TestHelper.GetAFReturnedObject<Venue>(task);

        //    Assert.True(_venue != null);
        //}

        //[Fact]
        //public void _GetAllVenues_CountAfterAdding()
        //{
        //    System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "GetAllVenues"), "");
        //    Task<HttpResponseMessage> task = GetAllVenues.Run(rm, new LogHelper((ILogger)null));
        //    IEnumerable<Venue> venues = TestHelper.GetAFReturnedObjects<Venue>(task);

        //    Assert.True(venues.LongCount() == EXPECTED_COUNT);
        //}
    }
}
