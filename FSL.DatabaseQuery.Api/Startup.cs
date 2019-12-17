using FSL.DatabaseQuery.Api.Repository;
using FSL.DatabaseQuery.Api.Service;
using FSL.DatabaseQuery.Core.Repository;
using FSL.Framework.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FSL.DatabaseQuery.Api
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            services
                .AddApiFslFramework(Configuration)
                .Config(opt =>
                {
                    opt.AddDefaultConfiguration();
                    opt.AddJwtAuthentication();
                    opt.AddAuthorizationService<ApiAuthorizationService>();
                    opt.AddSingleton<IDatabaseQueryRepository, DatabaseQuerySqlRepository>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApiFslFramework();
        }
    }
}
