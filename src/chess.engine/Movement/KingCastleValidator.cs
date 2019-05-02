using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class KingCastleValidator : IMoveValidator
    {

        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var kingLoc = move.From;

            var king = boardState.GetEntityOrNull(kingLoc);
            var kingIsValid = king.EntityType == ChessPieceName.King; // && !king.MoveHistory.Any()

            var rookLoc = move.ChessMoveType == ChessMoveType.CastleKingSide
                ? BoardLocation.At($"H{move.From.Rank}")
                : BoardLocation.At($"A{move.From.Rank}");
            var rook = boardState.GetEntityOrNull(rookLoc);
            var rookIsValid = rook.EntityType == ChessPieceName.Rook
                              && rook.Player == king.Player; // && !rook.MoveHistory.Any()

            var pathBetween = CalcPathBetweenKingAndCastle(move, king);

            var destinationIsEmptyValidator = new DestinationIsEmptyValidator();
            var pathIsEmpty = pathBetween.All(loc 
                => destinationIsEmptyValidator.ValidateMove(
                    new ChessMove(move.From, loc, ChessMoveType.MoveOnly), 
                    boardState));

            var destinationNotUnderAttackValidator = new DestinationNotUnderAttackValidator();
            var pathNotUnderAttack = pathBetween.All(loc 
                => destinationNotUnderAttackValidator.ValidateMove(
                    new ChessMove(move.From, loc, ChessMoveType.MoveOnly),
                    boardState));
            
            return kingIsValid && rookIsValid && pathIsEmpty && pathNotUnderAttack;
        }

        private static List<BoardLocation> CalcPathBetweenKingAndCastle(ChessMove move, ChessPieceEntity king)
        {
            var pathBetween = new List<BoardLocation>();
            if (move.From.File < move.To.File)
            {
                pathBetween.Add(move.From.MoveRight(king.Player));
                pathBetween.Add(move.From.MoveRight(king.Player, 2));
            }
            else
            {
                pathBetween.Add(move.From.MoveLeft(king.Player));
                pathBetween.Add(move.From.MoveLeft(king.Player, 2));
                pathBetween.Add(move.From.MoveLeft(king.Player, 3));
            }

            return pathBetween;
        }


        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }
    }
}