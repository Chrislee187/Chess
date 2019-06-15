using System.Threading.Tasks;

namespace chess.webapi.client.csharp
{
    public interface IChessGameApiClient
    {
        Task<ChessWebApiResult> ChessGameAsync();
        Task<ChessWebApiResult> ChessGameAsync(System.Threading.CancellationToken cancellationToken);
    }
}