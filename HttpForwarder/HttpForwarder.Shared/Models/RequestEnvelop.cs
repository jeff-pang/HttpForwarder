using System;
using System.Collections.Generic;
using System.Text;

namespace HttpForwarder.Shared.Models
{
    public class RequestEnvelop
    {
        public string RequestId { get; set; }
        public string Uid { get; set; }
        public HttpRequest Request { get; set; }
    }
}
