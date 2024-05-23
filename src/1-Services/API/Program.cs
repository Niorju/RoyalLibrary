using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
namespace WebAPI.NETCore
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        async static Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

            var host = CreateHostBuilder(args);
            await host.Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)                    
                    .ConfigureWebHostDefaults(webBuilder =>
                    {

                        webBuilder.UseStartup<Startup>();
                    });


    }
}
