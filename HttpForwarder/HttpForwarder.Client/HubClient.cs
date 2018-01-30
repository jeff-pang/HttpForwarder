using HttpForwarder.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HttpForwarder.Client
{
    public class HubClient
    {
        private Config _config;
        public HubClient(Config config)
        {
            _config = config;
        }
        public async Task<int> ExecuteAsync()
        {
            async Task<HubConnection> ConnectAsync(string url)
            {
                // Keep trying to until we can start
                while (true)
                {
                    var conn = new HubConnectionBuilder()
                                    .WithUrl(url)
                                    .WithConsoleLogger(LogLevel.Trace)
                                    .Build();

                    
                    try
                    {

                        await conn.StartAsync();

                        return conn;
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(1000);
                    }
                }
            }

            string baseUrl = $"{_config.ServerUrl}/{_config.ChannelName}";
            Console.WriteLine("Connecting to {0}", baseUrl);
            HubConnection connection = await ConnectAsync(baseUrl);
            try
            {
                connection.On<FwdRequest>("Send", process);
                await connection.InvokeAsync("Join", _config.Uid);

                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
            finally
            {
                await connection.DisposeAsync();
            }

            return 0;
        }

        private void process(FwdRequest request)
        {
            var endpointClient = new RestClient(_config.EndpointUrl);
            Method method = (Method)Enum.Parse(typeof(Method), request.Method);
            var endptRequest = new RestRequest("/"+request.Path, method);

            if(!string.IsNullOrEmpty(request.ContentType))
            {
                endptRequest.AddParameter("Content-Type", request.ContentType, ParameterType.HttpHeader);
                if (!string.IsNullOrEmpty(request.Body))
                {
                    endptRequest.AddParameter(request.ContentType, request.Body, ParameterType.RequestBody);
                }
            }

            foreach (var header in request.Headers)
            {
                endptRequest.AddHeader(header.Key, string.Join(',', header.Value));
            }
            foreach (var cookie in request.Cookies)
            {
                endptRequest.AddCookie(cookie.Key, cookie.Value);
            }
            MemoryStream ms = new MemoryStream();
            endptRequest.ResponseWriter = (responseStream) => responseStream.CopyTo(ms);
            IRestResponse response = endpointClient.Execute(endptRequest);

            var serverclient = new RestClient(_config.ServerUrl);
            var serverRequest = new RestRequest("/client/{uid}/{requestId}", Method.POST);
            serverRequest.AddUrlSegment("uid", _config.Uid);
            serverRequest.AddUrlSegment("requestId", request.RequestId);
            byte[] content = ms.ToArray();
            serverRequest.AddParameter(response.ContentType,content, ParameterType.RequestBody);
            serverRequest.AddHeader("statusCode", response.StatusCode.ToString());
            
            serverclient.Execute(serverRequest);
        }
    }
}