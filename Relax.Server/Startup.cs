using Kalantyr.Auth.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Relax.Server.Config;
using Relax.Server.Services;

namespace Relax.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceConfig>(_configuration.GetSection("Service"));
            
            services.Configure<AuthConfig>(_configuration.GetSection("Auth"));
            services.AddSingleton<IAppAuthClient>(sp => new AuthClient(
                sp.GetService<IHttpClientFactory>(),
                sp.GetService<IOptions<AuthConfig>>().Value.AppKey));

            services.AddScoped<CharactersService>();
            services.AddScoped<IHealthCheck, CharactersService>();
            
            services.AddHttpClient<AuthClient>((sp, client) =>
            {
                client.BaseAddress = new Uri(sp.GetService<IOptions<AuthConfig>>().Value.ServiceUrl);
            });

            #region Swagger

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            #endregion

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI();

            #endregion

            app.UseRouting();
            app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
