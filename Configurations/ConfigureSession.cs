using AbsolCase.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace AbsolCase.Configurations
{
    public class ConfigureSession : ActionFilterAttribute
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public ConfigureSession(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var baseURL = CommonMethods.GetWebBaseURL(_env, _config);
            string sessionData = filterContext.HttpContext.Session.GetString("userData");
            if (string.IsNullOrEmpty(sessionData))
            {
                Controller controller = filterContext.Controller as Controller;
                controller.TempData["sessionExpired"] = "Your session expired, Please login again";
                filterContext.Result = new RedirectResult(baseURL + "Security/Account/Login");
            }
        }
    }
}
