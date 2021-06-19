using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Comments { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public DateTime LastDateContacted { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }

        public Contact() { }

        // Other model related methods
    }
}
