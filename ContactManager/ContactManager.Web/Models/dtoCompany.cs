using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManager.Web.Models
{
    public class dtoCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}