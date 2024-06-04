using LuckyWallet.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LuckyWallet.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("LuckyWalletDb"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        SeedData(dbContext);
    }

    private static void SeedData(DatabaseContext dbContext)
    {

    }
}
