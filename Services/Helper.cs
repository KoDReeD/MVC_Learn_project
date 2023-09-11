using Microsoft.EntityFrameworkCore;
using WebStore.Data;

namespace WebStore.Services;

public static class Helper
{
    public static ApplicationDbContext GetContext()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var connectionString = config.GetConnectionString("PostgresConnection");
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        
        return new ApplicationDbContext(options);
    }
}