
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
    /// <summary>
    /// Controller classs for Venues API
    /// Provider actions moved from VenuesController to allow Swagger to resolve routes properly
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private ILogger _log = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public ProvidersController(ILogger<ProvidersController> logger)
        {
            _log = logger;
        }

        /// <summary>
        /// All venues by PRN, for example:
        /// GET api/providers/10000409/venues
        /// </summary>
        /// <param name="PRN">UKPRN value</param>
        /// <returns>List of matching venues</returns>
        //[Route("{PRN}/venues")]
        [HttpGet("{PRN}/venues", Name = "VenuesGetByPRN")]
        public ActionResult<IEnumerable<Venue>> GetByPRN(int PRN)
        {
            return new ActionResult<IEnumerable<Venue>>(new VenueStorage().GetByPRN(PRN, _log));
        }

        /// <summary>
        /// All venues by PRN with an exact (case sensitive) match on name, for example:
        /// GET api/providers/10000409/venues/DENTON COMMUNITY COLLEGE
        /// </summary>
        /// <param name="PRN">UKPRN value</param>
        /// <param name="Name">Venue name</param>
        /// <returns>List of matching venues</returns>
        //[Route("{PRN}/venues/{Name}")]
        [HttpGet("{PRN}/venues/{Name}", Name = "VenuesGetByPRNAndName")]
        public ActionResult<IEnumerable<Venue>> GetByPRNAndName(int PRN, string Name)
        {
            IEnumerable<Venue> results = new VenueStorage().GetByPRN(PRN, _log);
            return new ActionResult<IEnumerable<Venue>>(results.Where(p => p.VENUE_NAME == Name));
        }
    }
}
