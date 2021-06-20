using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManager.Web.Models
{
    // a duplicate of RequestResult class from the API. Should have created a common dll that defines these common objects
    // also, RequestResult should be named "Response"?
    public class RequestResult<T> where T : class
    {
        public bool Result { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}