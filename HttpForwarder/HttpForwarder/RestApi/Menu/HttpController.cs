using HttpForwarder.Core.Concurrency;
using HttpForwarder.Shared.Models;
using HttpForwarder.Core.WebSocks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HttpForwarder.RestApi
{
    public class HttpController : Controller
    {
        MessagingService _messagingService;
        public HttpController(MessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        [Route("{uid}/{*path}")]
        public ContentResult Index(string uid, string path)
        {
            var req = HttpContext.Request;
            Sync sync = new Sync(uid);
            RequestEnvelop request = new RequestEnvelop
            {
                Uid = uid,
                RequestId = sync.RequestId,
                Request = new HttpRequest
                {
                    RequestId = sync.RequestId,
                    Path = path,
                    Method = req.Method,
                    ContentType = req.ContentType,
                    Body = req.Body.AsString()
                }
            };

            foreach(var kvp in req.Cookies)
            {
                request.Request.Cookies[kvp.Key] = kvp.Value;
            }

            foreach(var kvp in req.Headers)
            {
                if(!request.Request.Headers.ContainsKey(kvp.Key))
                {
                    request.Request.Headers[kvp.Key] = new HttpRequest.HeaderValues();
                }

                foreach(string item in kvp.Value)
                {
                    request.Request.Headers[kvp.Key].Add(item);
                }
            }

            _messagingService.SendMessage(uid, request.Request);

            ResponseEnvelop response = sync.GetResponse();
            var result = new ContentResult();
            if (!response.HasTimedOut)
            {
                result.Content = response.ResponseStream.AsString();
                result.ContentType = response.ContentType;
            }

            result.StatusCode = (int)response.StatusCode;
            return result;
        }
        
        [HttpPost("client/{uid}/{requestId}")]
        public StatusCodeResult ClientResponse(string uid, string requestId)
        {
            var sync = Sync.GetSync(uid, requestId);
            if (sync != null)
            {
                string code = Request.Headers["statusCode"].ToString();
                HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), code);
                ResponseEnvelop responseEnv = new ResponseEnvelop
                {
                    ResponseStream = Request.Body,
                    ContentType = Request.ContentType,
                    StatusCode = statusCode
                };

                sync.Complete(responseEnv);

                return new StatusCodeResult((int)HttpStatusCode.OK);
            }
            else
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
        }
    }
}