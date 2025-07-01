using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using TodoDDD.Infrastructure.Data;

namespace TodoDDD.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // appsettings.json'u manuel yükle
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var dbProvider = config["DatabaseProvider"]?.ToLower();
            if (dbProvider == "postgres")
            {
                optionsBuilder.UseNpgsql(config.GetConnectionString("PostgresConnection"));
            }
            else if (dbProvider == "sqlserver")
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("SqlServerConnection"));
            }
            else
            {
                throw new Exception("Geçersiz veritabanı sağlayıcısı.");
            }

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
