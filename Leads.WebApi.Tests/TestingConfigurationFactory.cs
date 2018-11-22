using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Leads.Database.File;
using Leads.Database.Static;
using Leads.DbAdapter;
using Leads.Services;
namespace Leads.WebApi.Tests
{
    public class TestingConfigurationFactory<TStartup>
    : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                services.AddScoped<ILeadsDb, LeadsFileDb>(
                    provider => new LeadsFileDb("LeadsFiles"));
                services.AddScoped<ISubAreasDb, SubAreasStaticDatabase>();
                services.AddScoped<LeadsService>();
                services.AddScoped<SubAreasService>();

                //// Create a new service provider.
                //var serviceProvider = new ServiceCollection()
                //    .AddEntityFrameworkInMemoryDatabase()
                //    .BuildServiceProvider();

                //// Add a database context (ApplicationDbContext) using an in-memory
                //// database for testing.
                //services.AddDbContext<CatalogContext>(options =>
                //{
                //    options.UseInMemoryDatabase("InMemoryDbForTesting");
                //    options.UseInternalServiceProvider(serviceProvider);
                //});

                //services.AddDbContext<AppIdentityDbContext>(options =>
                //{
                //    options.UseInMemoryDatabase("Identity");
                //    options.UseInternalServiceProvider(serviceProvider);
                //});

                //// Build the service provider.
                //var sp = services.BuildServiceProvider();

                //// Create a scope to obtain a reference to the database
                //// context (ApplicationDbContext).
                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var db = scopedServices.GetRequiredService<CatalogContext>();
                //    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

                //    var logger = scopedServices
                //        .GetRequiredService<ILogger<CustomWebRazorPagesApplicationFactory<TStartup>>>();

                //    // Ensure the database is created.
                //    db.Database.EnsureCreated();

                //    try
                //    {
                //        // Seed the database with test data.
                //        CatalogContextSeed.SeedAsync(db, loggerFactory).Wait();
                //    }
                //    catch (Exception ex)
                //    {
                //        logger.LogError(ex, $"An error occurred seeding the " +
                //            "database with test messages. Error: {ex.Message}");
                //    }
                //}
            });
        }
    }
}

