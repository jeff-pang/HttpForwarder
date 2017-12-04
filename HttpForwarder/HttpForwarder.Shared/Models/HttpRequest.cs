using System;
using System.Collections.Generic;
using System.Text;

namespace HttpForwarder.Shared.Models
{
    public class HttpRequest
    {
        public class HeaderValues : List<string> { }

        public string RequestId { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public IDictionary<string, HeaderValues> Headers { get; set; } = new Dictionary<string, HeaderValues>();
        public IDictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();
        public string ContentType { get; set; }
        public string Body { get; set; }
    }
}
