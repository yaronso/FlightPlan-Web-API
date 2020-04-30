using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        // GET api/Flights? relative_to =< DATE_TIME >

        // GET api/Flights?relative_to=<DATE_TIME> &sync_all

        // DELETE api/Flight/7

    }
}