using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace chess.blazor
{
    public interface IChessGameApiClient
    {
        Task<string> ChessGameAsync();
        Task<string> ChessGameAsync(System.Threading.CancellationToken cancellationToken);

        #region NotImplemented

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> MoveAsync(string board, string move);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> MoveAsync(string board, string move, CancellationToken cancellationToken);

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMovesAsync(string board);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMovesAsync(string board, CancellationToken cancellationToken);

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMoves2Async(string board);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMoves2Async(string board, CancellationToken cancellationToken);

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetWhiteMovesAsync(string board);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetWhiteMovesAsync(string board, CancellationToken cancellationToken);

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetBlackMovesAsync(string board);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetBlackMovesAsync(string board, CancellationToken cancellationToken);

        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMoves3Async(string board, string location);

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        Task<string> GetMoves3Async(string board, string location, CancellationToken cancellationToken);

        #endregion
    }


    // TODO: Move to a seperate client component as part of the chess.api, make a seperate nuget package to reference here
    public abstract class ApiClientBase
    {
        protected string _baseUrl;
        protected HttpClient _httpClient;
        private string _prefix = "api";

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }
        public ApiClientBase(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        protected async Task<string> GetStringAsync(string apiEndPoint) => await _httpClient.GetStringAsync(BuildUri(apiEndPoint));

        private string BuildUri(string apiEndPoint)
        {
            var builder = new UriBuilder(_baseUrl);
            builder.Path = $"/{_prefix}/{apiEndPoint}";
            return builder.Uri.AbsoluteUri;
        }
    }

    public class ChessGameApiClient : ApiClientBase, IChessGameApiClient {
        public ChessGameApiClient(HttpClient httpClient, string baseUrl) : base(httpClient, baseUrl)
        {}

        public Task<string> ChessGameAsync() => ChessGameAsync(CancellationToken.None);

        public async Task<string> ChessGameAsync(CancellationToken cancellationToken) => await GetStringAsync("chessgame");

        #region NotImplemented
        public Task<string> MoveAsync(string board, string move)
        {
            throw new NotImplementedException();
        }

        public Task<string> MoveAsync(string board, string move, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMovesAsync(string board)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMovesAsync(string board, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMoves2Async(string board)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMoves2Async(string board, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetWhiteMovesAsync(string board)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetWhiteMovesAsync(string board, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlackMovesAsync(string board)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlackMovesAsync(string board, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMoves3Async(string board, string location)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMoves3Async(string board, string location, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
