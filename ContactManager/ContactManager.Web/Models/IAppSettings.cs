using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Web.Models
{
    public interface IAppSettings
    {
        string ContactUrl { get; set; }
        string CompanyUrl { get; set; }
        void Initialize();
    }
}
