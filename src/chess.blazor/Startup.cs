using System.Net.Http;
using chess.webapi.client.csharp;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using PgnReader;

namespace chess.blazor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPgnSerialisationService, PgnSerialisationService>();

            // TODO: Need to hook up IConfiguration and pull host out to config
            services.AddTransient<IChessGameApiClient>(provider 
                => new ChessGameApiClient(provider.GetService<HttpClient>(), "https://chess-web-api.azurewebsites.net"));
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
