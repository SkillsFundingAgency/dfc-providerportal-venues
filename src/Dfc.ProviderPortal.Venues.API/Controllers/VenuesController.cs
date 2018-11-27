
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Dfc.ProviderPortal.Venues.Models;
using Dfc.ProviderPortal.Venues.Storage;


namespace Dfc.ProviderPortal.Venues.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private ILogger _log = null;

        public VenuesController(ILogger<VenuesController> logger) {
            _log = logger;
        }

        // GET api/venues
        [HttpGet(Name="VenuesGetAll")]
        public ActionResult<IEnumerable<Venue>> Get()
        {
            Task<IEnumerable<Venue>> task = new VenueStorage().GetAllAsync(_log);
            task.Wait();
            return new ActionResult<IEnumerable<Venue>>(task.Result);
        }

        // GET api/providers/10000409/venues
        [Route("/api/providers/{PRN}/venues")]
        [HttpGet(Name = "VenuesGetByPRN")]
        public ActionResult<IEnumerable<Venue>> GetByPRN(int PRN)
        {
            return new ActionResult<IEnumerable<Venue>>(new VenueStorage().GetByPRN(PRN, _log));
        }

        // GET api/providers/10000409/venues/DENTON COMMUNITY COLLEGE
        [Route("/api/providers/{PRN}/venues/{Name}")]
        [HttpGet(Name = "VenuesGetByPRNAndName")]
        public ActionResult<IEnumerable<Venue>> GetByPRNAndName(int PRN, string Name)
        {
            IEnumerable<Venue> results = new VenueStorage().GetByPRN(PRN, _log);
            return new ActionResult<IEnumerable<Venue>>(results.Where(p => p.VENUE_NAME == Name));
        }

        // GET api/venues/620a83bf-902f-4257-9a37-2c9a95276ad3
        [HttpGet("{id}", Name="VenueGetById")]
        public ActionResult<Venue> Get(Guid id)
        {
            Venue venue = new VenueStorage().GetById(id, _log);
            return new ActionResult<Venue>(venue) ?? NotFound();
        }

        // POST api/venues
        [HttpPost]
        public void Post([FromBody] Venue venue)
        {
            Task task = new VenueStorage().InsertDocAsync(venue, _log);
            task.Wait();
        }

        //// POST api/venues
        //[HttpPost]
        //public IActionResult Create(Venue venue)
        //{
        //    Venue v = new Venue();
        //    return CreatedAtRoute("VenueGetById", new { v.id }, v);
        //}

        //// PUT api/venues/5
        //[HttpPut("{id}")]
        //public void Put(Guid id, [FromBody] Venue venue)
        //{
        //}

        //// DELETE api/venues/5
        //[HttpDelete("{id}")]
        //public void Delete(Guid id)
        //{
        //}
    }
}
