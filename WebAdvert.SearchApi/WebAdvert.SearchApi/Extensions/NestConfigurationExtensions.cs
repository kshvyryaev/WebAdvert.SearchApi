using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using WebAdvert.SearchApi.Models;

namespace WebAdvert.SearchApi.Extensions
{
    public static class NestConfigurationExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchUrl = configuration.GetSection("Es").GetValue<string>("Url");

            var connectionSettings = new ConnectionSettings(new Uri(elasticSearchUrl))
                .DefaultIndex("adverts")
                .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));

            var client = new ElasticClient(connectionSettings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
