using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OperationsController(IConfiguration config) {
            _config = config;
        }

        [HttpOptions("reloadconfig")]
        public IActionResult ReloadConfig() {
            try {
                var root = (IConfigurationRoot) _config;
                root.Reload();
                return Ok();
            } catch (Exception e) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
