using System.Linq;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Entities;

namespace chess.engine.Extensions
{
    public static class BoardLocationExtensions
    {
        public static string ToChessCoord(this BoardLocation loc)
        {
            return $"{(char)('A' + loc.X - 1)}{loc.Y}".ToLower();
        }
    }

    public static class BoardMoveExtensions
    {
        public static string ToChessCoords(this BoardMove move)
        {
            return $"{move.From.ToChessCoord()}{move.To.ToChessCoord()}";
        }

        public static string ToChessMove(this BoardMove move)
        {
            string promote = string.Empty;
            if (move.MoveType == (int) DefaultActions.UpdatePiece)
            {
                var d = (ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData) move.ExtraData;
                promote = $"+{d.PieceName.ToString().First()}";
            }
            return $"{move.From.ToChessCoord()}{move.To.ToChessCoord()}{promote}";
        }
    }
}