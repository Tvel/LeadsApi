using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Leads.WebApi
{
    using Leads.Database.Ef;
    using Leads.Database.File;
    using Leads.Database.Static;
    using Leads.DbAdapter;
    using Leads.Services;

    using Microsoft.EntityFrameworkCore;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            switch (this.Configuration["DbType"])
            {
                case "SqlServer":
                    services.AddDbContext<LeadsContext>(
                        options => options.UseSqlServer(this.Configuration.GetConnectionString("LeadsSqlServer")));

                    services.AddScoped<ILeadsDb, LeadsEfDb>();
                    services.AddScoped<ISubAreasDb, SubAreasEfDb>();
                    break;
                case "SQLite":
                    services.AddDbContext<LeadsContext>(
                        options => { options.UseSqlite(this.Configuration.GetConnectionString("LeadsSQLite")); });

                    services.AddScoped<ILeadsDb, LeadsEfDb>();
                    services.AddScoped<ISubAreasDb, SubAreasEfDb>();
                    break;
                case "FileAndStatic":
                    services.AddScoped<ILeadsDb, LeadsFileDb>(x => new LeadsFileDb(this.Configuration.GetConnectionString("FileDirectory")));
                    services.AddScoped<ISubAreasDb, SubAreasStaticDatabase>();
                    break;
                default:
                    throw new NotImplementedException();
            }

            services.AddScoped<LeadsService>();
            services.AddScoped<SubAreasService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            switch (this.Configuration["DbType"])
            {
                case "SqlServer":
                case "SQLite":
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        var context = serviceScope.ServiceProvider.GetService<LeadsContext>();

                        if (context.Database.IsInMemory())
                        {
                            context.Database.EnsureCreated();
                        }
                        else
                        {
                            context.Database.Migrate();
                        }
                    }
                    break;
                case "FileAndStatic":
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
