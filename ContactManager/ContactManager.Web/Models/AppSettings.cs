using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ContactManager.Web.Models
{
    public class AppSettings : IAppSettings
    {
        public string ContactUrl { get; set; }
        public string CompanyUrl { get; set; }
        public virtual void Initialize()
        {
            var webApiUrl = ConfigurationManager.AppSettings["WebApiUrl"].TrimEnd(new[] { '/', '\\' });
            ContactUrl = $"{webApiUrl}/Contact";
            CompanyUrl = $"{webApiUrl}/Company";
        }

        public AppSettings()
        {
            // stupid
            Initialize();
        }
    }
}