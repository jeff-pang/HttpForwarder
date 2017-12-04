using HttpForwarder.Client.Configurations;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace HttpForwarder.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var config=SystemConfig.GetConfig<Config>("client");
            HubClient client = new HubClient(config);
            client.ExecuteAsync().Wait();
        }
    }
}