using Dfc.ProviderPortal.Venues;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
[assembly: FunctionsStartup(typeof(Startup))]

namespace Dfc.ProviderPortal.Venues
{
    public class Startup : FunctionsStartup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services
        //        .AddMvcCore()
        //        .SetCompatibilityVersion(CompatibilityVersion.Latest)
        //        .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
        //        .AddApiExplorer();

        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course Directory Venues API", Version = "v1" });
        //    });
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    app.UseSwagger();

        //    app.UseSwaggerUI(c =>
        //    {
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Directory Venues API");
        //    });

        //    app.UseMvc();
        //}

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
        }
    }
}