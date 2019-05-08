using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Pieces;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging.Abstractions;

namespace chess.engine.Board
{
    public class BoardConvert
    {
        public static string Serialise(ChessGame chessGameBoard)
        {
            var sb = new StringBuilder();

            for (int y = 8; y >= 1; y--)
            {
                for (int x = 1; x <= 8; x++)
                {
                    var item = chessGameBoard.Board[x - 1, y - 1];
                    var c = TextRepresentation(item);
                    sb.Append(c);
                }
            }

            sb.Append(chessGameBoard.CurrentPlayer.ToString().First());

            var whiteKing = chessGameBoard.BoardState.GetItem(King.StartPositionFor(Colours.White));
            var blackKing = chessGameBoard.BoardState.GetItem(King.StartPositionFor(Colours.Black));

            sb.Append(whiteKing.Paths.FlattenMoves().Any(p => p.MoveType == MoveType.CastleQueenSide)
                ? "1"
                : "0");
            sb.Append(whiteKing.Paths.FlattenMoves().Any(p => p.MoveType == MoveType.CastleKingSide)
                ? "1"
                : "0");

            sb.Append(blackKing.Paths.FlattenMoves().Any(p => p.MoveType == MoveType.CastleQueenSide)
                ? "1"
                : "0");
            sb.Append(blackKing.Paths.FlattenMoves().Any(p => p.MoveType == MoveType.CastleKingSide)
                ? "1"
                : "0");


            return sb.ToString();
        }

        private static char TextRepresentation(LocatedItem<ChessPieceEntity> item)
        {
            if (item == null) return '.';

            var textRepresentation = PieceNameMapper.ToChar(item.Item.Piece, item.Item.Player);
            // TODO: Add enpassant check
            return textRepresentation;
        }

        public static ChessGame Deserialise(string boardformat69char)
        {
            if (boardformat69char.Length != 69) throw new ArgumentException($"Invalid serialised board, must be 69 characters long (yours was {boardformat69char.Length})", nameof(boardformat69char));

            // TODO: Handling of invalid formats
            int idx = 1;
            var pieces = boardformat69char.Trim().Substring(0, 64);
            var toBePlaced = new List<Action<BoardEngine<ChessPieceEntity>>>();

            foreach (var piece in pieces)
            {
                var locX = (idx-1) % 8 + 1;
                var locY = 8 - (idx-1) / 8;

                if (piece != '.')
                {
                    var chessPieceName = PieceNameMapper.FromChar(piece);
                    var colour = char.IsUpper(piece) ? Colours.White : Colours.Black;
                    var newPiece = ChessPieceEntityFactory.Create(chessPieceName, colour);

                    toBePlaced.Add((engine) => engine.AddPiece(newPiece, BoardLocation.At(locX, locY)));
                }
                idx++;
            }


            var whoseTurn = boardformat69char[64] == 'W'
                ? Colours.White
                : Colours.Black;

            var setup = new DeserialisedBoardSetup(toBePlaced);
            return new ChessGame(
                NullLogger<ChessGame>.Instance,
                HelperFactory.ChessBoardEngineProviderNoLoggers,
                setup,
                whoseTurn
            );
        }

        public class DeserialisedBoardSetup : IBoardSetup<ChessPieceEntity>
        {
            private readonly IEnumerable<Action<BoardEngine<ChessPieceEntity>>> _toBePlaced;

            public DeserialisedBoardSetup(IEnumerable<Action<BoardEngine<ChessPieceEntity>>> toBePlaced)
            {
                _toBePlaced = toBePlaced;
            }


            public void SetupPieces(BoardEngine<ChessPieceEntity> engine)
            {
                foreach (var action in _toBePlaced)
                {
                    action(engine);
                }
            }
        }
    }
}