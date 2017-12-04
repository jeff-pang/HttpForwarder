using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace HttpForwarder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = AppContext.BaseDirectory;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Logger(lc =>
                    lc.Filter.ByExcluding(evt => evt.Level == Serilog.Events.LogEventLevel.Debug)
                    .Enrich.FromLogContext()
                   .WriteTo.LiterateConsole()
                   .WriteTo.RollingFile(Path.Combine(path, "logs", "{Date}.log")))
                .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Debug)
                    .Enrich.FromLogContext()
                   .WriteTo.RollingFile(Path.Combine(path, "logs", "{Date}-details.log")))
                .CreateLogger();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
