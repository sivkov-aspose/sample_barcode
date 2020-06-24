using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace sample_barcode
{
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
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (string.IsNullOrEmpty(Configuration.GetValue<string>("Settings:AsposeCloudAppKey"))
                || string.IsNullOrEmpty(Configuration.GetValue<string>("Settings:AsposeCloudAppSid"))
                || string.IsNullOrEmpty(Configuration.GetValue<string>("Settings:GroupdocsCloudAppSid"))
                || string.IsNullOrEmpty(Configuration.GetValue<string>("Settings:GroupdocsCloudAppKey"))
            ) throw new Exception("Apose Cloud AppSid/key or Groupdocs Cloud AppSid/key pair not defined");
            
            logger.LogInformation($"Using {Configuration.GetValue<string>("Settings:AsposeCloudAppSid")} for barcode, {Configuration.GetValue<string>("Settings:GroupdocsCloudAppSid")} for conversion");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
