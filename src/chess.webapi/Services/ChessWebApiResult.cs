using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.SAN;
using chess.webapi.client.csharp;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global

namespace chess.webapi.Services
{
    public class ChessWebApiResult : client.csharp.ChessWebApiResult
    {
        [JsonIgnore]
        public ChessGame Game { get; }

        [JsonIgnore]
        public IEnumerable<BoardMove> Moves { get; }

        public ChessWebApiResult(
            ChessGame game, 
            Colours toMove, 
            string message, 
            params LocatedItem<ChessPieceEntity>[] items
            ) 
        {
            Game = game;
            Board = ChessGameConvert.Serialise(game);
            BoardText = new ChessBoardBuilder().FromChessGame(game).ToTextBoard();
            Moves = items.SelectMany(i => i.Paths.FlattenMoves());
            AvailableMoves = ToMoveList(items);
            WhoseTurn = toMove.ToString();
            Message = message;
        }

        public Move[] ToMoveList(params LocatedItem<ChessPieceEntity>[] locatedItems)
        {
            return locatedItems
                .SelectMany(i => i.Paths.FlattenMoves())
                .Select(m => new Move
                {
                    Coord = $"{m.ToChessCoords()}",
                    SAN = StandardAlgebraicNotation.ParseFromGameMove(Game.BoardState, m, true).ToNotation()
                }).ToArray();
        }
    }
}