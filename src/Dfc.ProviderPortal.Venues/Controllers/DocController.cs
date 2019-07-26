using Dfc.ProviderPortal.Venues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dfc.ProviderPortal.Venues.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class DocController : ControllerBase
    {
        [Route("AddVenue")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Venue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddVenue(Venue venue)
        {
            return Ok();
        }

        [Route("GetAllVenues")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Venue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllVenues()
        {
            return Ok();
        }

        [Route("GetVenueById")]
        [HttpGet]
        [HttpPost] 
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenueById(Guid id)
        {
            return Ok();
        }

        [Route("GetVenueByLocationId")]
        [HttpGet]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenueByLocationId(int LocationId)
        {
            return Ok();
        }

        [Route("GetVenueByVenueId")]
        [HttpGet]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenueByVenueId(int venueId)
        {
            return Ok();
        }

        [Route("GetVenuesByPRN")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Venue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenuesByPRN(int prn)
        {
            return Ok();
        }

        [Route("GetVenuesByPRNAndName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Venue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenuesByPRNAndName(int prn, string name)
        {
            return Ok();
        }

        [Route("GetVenuesByUKPRNAndPostCode")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Venue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVenuesByUKPRNAndPostCode(int UKPRN, string PostCode)
        {
            return Ok();
        }

        [Route("UpdateVenueById")]
        [HttpPost]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateVenueById(Venue venue)
        {
            return Ok();
        }
    }
}