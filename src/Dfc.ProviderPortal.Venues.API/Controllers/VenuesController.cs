
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
        //private Microsoft.Azure.WebJobs.Host.TraceWriter _log = null;
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

        // GET api/venues/5
        [HttpGet("{id}", Name="VenueGetById")]
        public ActionResult<Venue> Get(Guid id)
        {
            Venue v = new VenueStorage().GetById(id, _log);
            return new ActionResult<Venue>(v) ?? NotFound();
        }

        //// POST api/venues
        //[HttpPost]
        //public void Post([FromBody] Venue venue)
        //{
        //}

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
