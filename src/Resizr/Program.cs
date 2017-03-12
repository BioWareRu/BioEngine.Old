using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;

namespace BioEngine.Resizr
{
    [UsedImplicitly]
    public class Program
    {
        [UsedImplicitly]
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}