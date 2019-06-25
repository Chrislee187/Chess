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
            // "https://chess-web-api.azurewebsites.net"
            //"https://localhost:5001"
            services.AddTransient<IChessGameApiClient>(provider 
                => new ChessGameApiClient(provider.GetService<HttpClient>(), "https://localhost:5001"));
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
