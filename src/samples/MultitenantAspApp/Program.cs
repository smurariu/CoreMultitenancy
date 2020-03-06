using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MultitenantAspApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .AddCommandLine(args)
                            .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                            .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .Build();
            
            host.Run();
        }
    }
}
