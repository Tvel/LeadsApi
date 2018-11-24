namespace Leads.WebApi.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Leads.DbAdapter;
    using Leads.Services;
    using Leads.Database.Ef;

    public class TestingConfigurationFactory<TStartup>
    : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<LeadsContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                services.AddScoped<ILeadsDb, LeadsEfDb>();
                services.AddScoped<ISubAreasDb, SubAreasEfDb>();
                services.AddScoped<LeadsService>();
                services.AddScoped<SubAreasService>();

                var sp = services.BuildServiceProvider();

                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var db = scopedServices.GetRequiredService<LeadsContext>();
                //    db.Database.EnsureCreated();
                //}
            });
        }
    }
}

