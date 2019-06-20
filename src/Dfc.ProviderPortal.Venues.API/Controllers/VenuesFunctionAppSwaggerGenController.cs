
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Dfc.ProviderPortal.Venues.Models;
using Microsoft.Azure.Documents;
using System.IO;


namespace Dfc.ProviderPortal.Venues.API.Controllers
{
    /// <summary>
    /// Controller containing one stub method per Azure function
    /// Allows Swashbuckle to generate the swagger doc for the Function App
    /// </summary>

    [Route("api")]
    [ApiController]
    public class VenuesFunctionAppSwaggerGenController : ControllerBase
    {
        /// <summary />
        [HttpPost("AddVenue", Name = "AddVenue")]
        public void AddVenue([FromBody] Venue venue)
        {
            return;
        }

        /// <summary />
        [HttpGet("GetAllVenues", Name = "GetAllVenues")]
        public ActionResult<IEnumerable<Venue>> GetAllVenues()
        {
            return new ActionResult<IEnumerable<Venue>>(new Venue[] {
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "a" },
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "b" }
            });
        }

        /// <summary />
        [HttpGet("GetVenueById", Name = "GetVenueById")]
        public ActionResult<Venue> GetVenueById(Guid id)
        {
            return new ActionResult<Venue>(
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenueById", ADDRESS_2 = id.ToString() }
            );
        }

        /// <summary />
        [HttpGet("GetVenueByVenueId", Name = "GetVenueByVenueId")]
        public ActionResult<Venue> GetVenueByVenueId(int VenueId)
        {
            return new ActionResult<Venue>(
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenueByVenueId", ADDRESS_2 = VenueId.ToString() }
            );
        }

        /// <summary />
        [HttpGet("GetVenuesByPRN", Name = "GetVenuesByPRN")]
        public ActionResult<IEnumerable<Venue>> GetVenuesByPRN(int PRN)
        {
            return new ActionResult<IEnumerable<Venue>>(new Venue[] {
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByPRN", ADDRESS_2 = PRN.ToString() },
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByPRN", ADDRESS_2 = PRN.ToString() }
            });
        }

        /// <summary />
        [HttpGet("GetVenuesByPRNAndName", Name = "GetVenuesByPRNAndName")]
        public ActionResult<IEnumerable<Venue>> GetVenuesByPRNAndName(int PRN, string Name)
        {
            return new ActionResult<IEnumerable<Venue>>(new Venue[] {
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByPRNAndName", ADDRESS_2 = PRN.ToString(), COUNTY = Name },
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByPRNAndName", ADDRESS_2 = PRN.ToString(), COUNTY = Name }
            });
        }

        /// <summary />
        [HttpGet("GetVenuesByUKPRNAndPostCode", Name = "GetVenuesByUKPRNAndPostCode")]
        public ActionResult<IEnumerable<Venue>> GetVenuesByUKPRNAndPostCode(int UKPRN, string PostCode)
        {
            return new ActionResult<IEnumerable<Venue>>(new Venue[] {
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByUKPRNAndPostCode", ADDRESS_2 = UKPRN.ToString(), COUNTY = PostCode },
                new Venue() { id = Guid.NewGuid(), ADDRESS_1 = "GetVenuesByUKPRNAndPostCode", ADDRESS_2 = UKPRN.ToString(), COUNTY = PostCode }
            });
        }

        /// <summary />
        [HttpPost("UpdateVenueById", Name = "UpdateVenueById")]
        public ActionResult<Document> UpdateVenueById(Venue venue)
        {
            return new ActionResult<Document>(
                new Document()
            );
        }
    }
}