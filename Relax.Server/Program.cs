using Kalantyr.Auth.Client;
using Microsoft.Extensions.Options;
using Relax.Server;
using Relax.Server.Config;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builderContext, services) =>
    {
        //        services.Configure<ServiceConfig>(builderContext.Configuration.GetSection("Service"));
        services.Configure<AuthConfig>(builderContext.Configuration.GetSection("Auth"));

        services.AddSingleton<CharactersRegistry>();
        services.AddSingleton<IAppAuthClient>(sp => new AuthClient(
            sp.GetService<IHttpClientFactory>(),
            sp.GetService<IOptions<AuthConfig>>().Value.AppKey));
        services.AddHostedService<Worker>();

        services.AddHttpClient<AuthClient>((sp, client) =>
        {
            client.BaseAddress = new Uri(sp.GetService<IOptions<AuthConfig>>().Value.ServiceUrl);
        });
    })
    .Build();

await host.RunAsync();
