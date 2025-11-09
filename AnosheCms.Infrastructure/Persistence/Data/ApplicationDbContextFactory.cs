// File: AnosheCms.Infrastructure/Persistence/Data/ApplicationDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AnosheCms.Infrastructure.Persistence.Data
{
    // این کلاس به ابزارهای CLI (مانند Add-Migration) می‌گوید که چگونه DbContext را بسازند
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // ۱. پیدا کردن مسیر پروژه Api (جایی که appsettings.json قرار دارد)
            // (از پوشه Infrastructure به پوشه Api می‌رویم)
            string apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Api");

            // ۲. ساختن Configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            // ۳. خواندن Connection String
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidDataException("Connection string 'DefaultConnection' not found in Api/appsettings.json");
            }

            // ۴. ساختن Options
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // ۵. ساختن DbContext
            // (ما 'null' را برای ICurrentUserService ارسال می‌کنیم،
            // زیرا ابزارهای Migration به آن نیازی ندارند)
            return new ApplicationDbContext(optionsBuilder.Options, null);
        }
    }
}