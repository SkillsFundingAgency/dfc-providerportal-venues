
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dfc.ProviderPortal.Venues.Models;


namespace Dfc.ProviderPortal.Venues.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        // GET api/venues
        [HttpGet(Name="VenuesGetAll")]
        public ActionResult<IEnumerable<Venue>> Get()
        {
            return new Venue[] { new Venue() { id = Guid.NewGuid() }, new Venue() { id = Guid.NewGuid() } };
        }

        // GET api/venues/5
        [HttpGet("{id}", Name="VenueGetById")]
        public ActionResult<Venue> Get(Guid id)
        {
            return new Venue() { id = Guid.NewGuid() };
        }

        // POST api/venues
        [HttpPost]
        public void Post([FromBody] Venue venue)
        {
        }

        // POST api/venues
        [HttpPost]
        public IActionResult Create(Venue venue)
        {
            Venue v = new Venue();
            return CreatedAtRoute("VenueGetById", new { v.id }, v);
        }

        // PUT api/venues/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Venue venue)
        {
        }

        // DELETE api/venues/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
