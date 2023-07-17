using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace AbsolCase.Utility
{
    public class CommonMethods
    {
        public static string GetBaseURLForFileStorage(IWebHostEnvironment _env, IConfiguration _config)
        {
            try
            {
                string serverBasePath = "";
                serverBasePath = _config.GetValue<string>("App:RemoteServerUrl");
                //if (_env.IsEnvironment("local"))
                //{
                //    serverBasePath = _env.WebRootPath;
                //    serverBasePath = serverBasePath.Replace('\\', '/');
                //}
                return serverBasePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetApiBaseURL(IWebHostEnvironment _env, IConfiguration _config)
        {
            try
            {
                var serverBasePath = _config.GetValue<string>("App:RemoteServerUrl");
                return serverBasePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetWebBaseURL(IWebHostEnvironment _env, IConfiguration _config)
        {
            try
            {
                var serverBasePath = _config.GetValue<string>("App:RemoteWebUrl");
                return serverBasePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
