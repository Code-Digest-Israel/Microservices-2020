using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Output_Service
{
    public class Program
    {
        private const int _port = 5003;
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                if(int.TryParse(Environment.GetEnvironmentVariable("DAPR_HTTP_PORT"), out var daprPort))
                {                    
                    Console.WriteLine($"Found dapr-port from env variable: '{daprPort}'");
                }

                webBuilder.UseUrls($"http://localhost:{_port}");
            });
        }
    }
}
