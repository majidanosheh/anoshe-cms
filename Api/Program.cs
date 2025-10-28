// File: AnosheCms.Api/Program.cs

// --- شروع Using Directives ---
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
// --- پایان Using Directives ---

namespace AnosheCms.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // به .NET می‌گوید که از کلاس Startup ما استفاده کند
                    webBuilder.UseStartup<Startup>();
                });
    }
}