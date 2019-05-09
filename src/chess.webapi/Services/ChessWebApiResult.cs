using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global

namespace chess.webapi.Services
{
    public class ChessWebApiResult
    {
        public string Message { get; }
        public string Board { get; set; }
        public string BoardText { get; set; }
        public string[] AvailableMoves { get; }

        [JsonIgnore]
        public ChessGame Game { get; }

        [JsonIgnore]
        public IEnumerable<BoardMove> Moves { get; }

        public ChessWebApiResult(ChessGame game, string msg = "")
        {
            Game = game;
            Board = ChessGameConvert.Serialise(game);
            BoardText = new ChessBoardBuilder().FromChessGame(game).ToString();
            var items = game.BoardState.GetAllItems().ToList();
            Moves = items.SelectMany(i => i.Paths.FlattenMoves());
            AvailableMoves = ToMoveList(items.ToArray());
            Message = msg;
        }

        public ChessWebApiResult(ChessGame game, params LocatedItem<ChessPieceEntity>[] items) :this(game)
        {
            Moves = items.SelectMany(i => i.Paths.FlattenMoves());
            AvailableMoves = ToMoveList(items);
        }

        public string[] ToMoveList(params LocatedItem<ChessPieceEntity>[] locatedItems)
        {
            return locatedItems.SelectMany(i => i.Paths.FlattenMoves()
                .Select(m => $"{m.ToChessCoords()}")).ToArray();
        }
    }
}