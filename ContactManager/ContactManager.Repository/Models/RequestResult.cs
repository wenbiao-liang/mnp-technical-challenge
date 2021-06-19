using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Models
{
    public class RequestResult<T> where T: class
    {
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
