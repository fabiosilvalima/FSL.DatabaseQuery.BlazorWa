using FSL.DatabaseQuery.BlazorWa.Service;
using FSL.DatabaseQuery.Core.Models;
using FSL.DatabaseQuery.Core.Repository;
using FSL.DatabaseQuery.Core.Service;
using FSL.Framework.Core.ApiClient.Provider;
using FSL.Framework.Core.ApiClient.Service;
using FSL.Framework.Core.Authentication.Service;
using FSL.Framework.Core.Factory.Service;
using FSL.Framework.Core.Service;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FSL.DatabaseQuery.BlazorWa
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddTransient<IApiClientProvider, HttpClientApiClientProvider>();
            services.AddSingleton<ILoggedUserService, BlazorLoggedUserService>();
            services.AddSingleton<IFactoryService, DefaultFactoryService>();
            services.AddSingleton<IApiClientService, DatabaseApiClientService>();
            services.AddSingleton(new MyConfiguration()
            {
                ApiUrl = "http://localhost/fsl-database-api/api/"
            });
            services.AddSingleton<IDatabaseQueryRepository, ApiDatabaseQueryRepository>();
        }

        public void Configure(
            IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
