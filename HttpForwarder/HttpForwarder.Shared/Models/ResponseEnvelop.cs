using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HttpForwarder.Shared.Models
{
    public class ResponseEnvelop
    {
        public bool HasTimedOut { get; set; }
        public string ContentType { get; set; }
        public Stream ResponseStream { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}