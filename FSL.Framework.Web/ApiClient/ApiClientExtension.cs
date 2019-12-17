using FSL.Framework.Core.ApiClient.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace FSL.Framework.Web.Extensions
{
    public static class ApiClientExtension
    {
        public static IServiceCollection AddApiClient(
            this IServiceCollection services)
        {
            services.AddTransient<IApiClientProvider, HttpClientApiClientProvider>();

            return services;
        }
    }
}
