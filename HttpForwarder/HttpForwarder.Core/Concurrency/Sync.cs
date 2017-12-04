using HttpForwarder.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HttpForwarder.Core.Concurrency
{
    public partial class Sync
    {
        public const int DEFAULT_TIMEOUT_MILLISECONDS = 10000;

        private ManualResetEvent _mse = new ManualResetEvent(false);

        public string Uid { get; private set; }
        public string RequestId { get; private set; }
        public ResponseEnvelop Response { get; private set; }
        public int TimeoutMilliseconds { get; private set; }

        public Sync(string uid, int timeoutMilliseconds = DEFAULT_TIMEOUT_MILLISECONDS)
        {
            TimeoutMilliseconds = timeoutMilliseconds;
            RequestId = Guid.NewGuid().ToString("N");
            Uid = uid;
            AddSync(uid,this);
        }

        public bool Complete(ResponseEnvelop response)
        {
            Response = response;
            return _mse.Set();
        }

        public ResponseEnvelop GetResponse()
        {
            if(_mse.WaitOne(TimeoutMilliseconds))
            {
                return Response;
            }
            else
            {
                return new ResponseEnvelop
                {
                    StatusCode = System.Net.HttpStatusCode.GatewayTimeout,
                    HasTimedOut = true
                };
            }
        }
    }
}