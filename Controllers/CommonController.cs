using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AbsolCase.Controllers
{
    public class CommonController : Controller
    {

        private readonly IConfiguration configuration;

        public CommonController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetConfigurationValue(string sectionName, string paramName)
        {
            var parameterValue = configuration[$"{sectionName}:{paramName}"];
            return Json(new { parameter = parameterValue });
        }
    }
}
