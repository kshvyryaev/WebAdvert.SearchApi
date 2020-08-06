using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebAdvert.SearchApi.Extensions;
using WebAdvert.SearchApi.Services;

namespace WebAdvert.SearchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElasticSearch(Configuration);
            services.AddTransient<ISearchService, SearchService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            loggerFactory.AddAWSProvider(
                Configuration.GetAWSLoggingConfigSection(),
                formatter: (logLevel, message, exception) => $"[{DateTime.Now} {logLevel} {message} {exception?.Message} {exception?.StackTrace}]");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
