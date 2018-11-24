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
    using Leads.Database.Ef;

    using Microsoft.EntityFrameworkCore;

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

