using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Movement;
using Newtonsoft.Json;

namespace chess.webapi.Services
{
    public class ChessGameResult
    {
        [JsonIgnore]
        public ChessGame Game { get; }

        [JsonIgnore]
        public IEnumerable<BoardMove> Moves { get; }

        public string BoardSerialised { get; set; }
        public string Board { get; set; }

        public string[] AvailableMoves { get; }
        public ChessGameResult(ChessGame game, string msg = "")
        {
            Game = game;
            BoardSerialised = ChessGameConvert.Serialise(game);
            Board = new EasyBoardBuilder().FromChessGame(game).ToString();
            var items = game.BoardState.GetAllItems().ToList();
            Moves = items.SelectMany(i => i.Paths.FlattenMoves());
            AvailableMoves = ToMoveList(items.ToArray());
        }

        public ChessGameResult(ChessGame game, params LocatedItem<ChessPieceEntity>[] items) :this(game)
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