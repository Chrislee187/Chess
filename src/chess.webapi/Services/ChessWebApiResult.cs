using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine;
using chess.engine.Algebraic;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global

namespace chess.webapi.Services
{
    public class ChessWebApiResult
    {
        public string Message { get; }
        public string Board { get; set; }
        public string BoardText { get; set; }
        public Move[] AvailableMoves { get; }
        public string WhoseTurn { get; }
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

        public ChessWebApiResult(
            ChessGame game, 
            Colours toMove, 
            string message, 
            params LocatedItem<ChessPieceEntity>[] items
            ) :this(game)
        {
            Moves = items.SelectMany(i => i.Paths.FlattenMoves());
            AvailableMoves = ToMoveList(items);
            WhoseTurn = toMove.ToString();
            Message = message;
        }

        public Move[] ToMoveList(params LocatedItem<ChessPieceEntity>[] locatedItems)
        {
            return locatedItems.SelectMany(i => i.Paths.FlattenMoves()
                .Select(m => new Move($"{m.ToChessCoords()}", StandardAlgebraicNotation.ParseFromGameMove(Game.BoardState, m).ToNotation() ))).ToArray();
        }

        public class Move
        {
            public string SAN { get; }
            public string Coord { get; }

            public Move(string coord, string san = "")
            {
                Coord = coord;
                SAN = san;
            }
        }
    }
}