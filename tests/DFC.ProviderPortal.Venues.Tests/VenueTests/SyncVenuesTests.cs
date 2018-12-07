
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
    public class SyncVenuesTests
    {
        private IEnumerable<Venue> _venues = null;

        private const string URI_PATH = "http://localhost:7071/api/";


        public SyncVenuesTests()
        {
            TestHelper.AddEnvironmentVariables();
        }

        
        //[Fact]
        //private void SyncVenues_Run()
        //{
        //    _venues = new List<Venue>();
        //    System.Net.Http.HttpRequestMessage rm = TestHelper.CreateRequest(new Uri(URI_PATH + "SyncVenues"), "");
        //    Task<HttpResponseMessage> task = SyncVenues.Run(rm, new LogHelper((ILogger)null));
        //    _venues = TestHelper.GetAFReturnedObject<IEnumerable<Venue>>(task);

        //    Assert.True(_venues?.Any());
        //}
    }
}
