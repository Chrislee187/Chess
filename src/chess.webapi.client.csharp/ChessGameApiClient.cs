using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace chess.webapi.client.csharp
{
    public class ChessGameApiClient : ApiClientBase, IChessGameApiClient
    {
        public ChessGameApiClient(HttpClient httpClient, string baseUrl) : base(httpClient, baseUrl)
        { }

        public Task<ChessWebApiResult> ChessGameAsync() => ChessGameAsync(CancellationToken.None);

        public async Task<ChessWebApiResult> ChessGameAsync(CancellationToken cancellationToken)
        {
            // TODO: Find out why availableMoves is not deserialised when using GetJsonAsync method from the new .NET Json libraries
            return JsonConvert.DeserializeObject<ChessWebApiResult>(await GetStringAsync("chessgame"));
        }
    }

    public class ChessWebApiResult
    {
        public string Message { get; set; }
        public string Board { get; set; }
        public string BoardText { get; set; }
        public Move[] AvailableMoves { get; set; }
        public string WhoseTurn { get; set; }
        
    }

    public class Move
    {
        public string SAN { get; set; }
        public string Coord { get; set; }
    }
}