using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ObservabilitySampleApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MouseController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return new OkResult();
        }
    }
}