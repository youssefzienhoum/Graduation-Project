using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Client.AbstructServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Refit.HttpClientFactory;

namespace Report.Client.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRefitClient<IAiVisionClient>()
          .ConfigureHttpClient(client =>
          {
              client.BaseAddress = new Uri(
                  builder.Configuration["Services:AI"]);
          });
          

            return services;
        }
    }
}
