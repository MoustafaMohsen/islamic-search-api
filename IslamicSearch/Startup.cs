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
using Microsoft.EntityFrameworkCore;
using IslamicSearch.Data;
using System.IO;
using IslamicSearch.Helpers;
using IslamicSearch.Services;

namespace IslamicSearch
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            env.EnvironmentName = "Development";
            // path to sensetive appsettings jsons, keep out .git folder
            string PrivateAppsettiingsPath = "";
            var pathtest = Path.Combine(env.ContentRootPath, "AppSettings");


            // if in Development
            if (env.IsDevelopment())
            {
                PrivateAppsettiingsPath = Path.Combine(env.ContentRootPath, "..", "..", "Data", "AppSettings");
            }
            // if in Production
            if (env.IsProduction())
            {
                PrivateAppsettiingsPath = Path.Combine(env.ContentRootPath, "AppSettings");
            }
            // if in Staging
            if (env.IsStaging())
            {
                PrivateAppsettiingsPath = Path.Combine(env.ContentRootPath, "AppSettings");
            }
            // add settings files
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{PrivateAppsettiingsPath}/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{PrivateAppsettiingsPath}/adminsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Sqlite
            services.AddDbContext<AppDbContext>(option=>option.UseSqlite( Configuration.GetConnectionString("SqliteConnection") ) );

            // add configuration
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // add BlockService
            services.AddScoped<IBlockService, BlockService>();


            services.AddHttpClient();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
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
        }
    }
}
