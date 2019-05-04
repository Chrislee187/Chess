using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.Movement.SimpleValidators;

namespace chess.engine.Movement.ChessPieces.King
{
    public class KingCastleValidator : IMoveValidator
    {

        public bool ValidateMove(BoardMove move, IBoardState boardState)
        {
            var king = boardState.GetItem(move.From).Item;
            var kingIsValid = king.EntityType == ChessPieceName.King; // && !king.MoveHistory.Any()

            var rookLoc = move.MoveType == MoveType.CastleKingSide
                ? BoardLocation.At($"H{move.From.Rank}")
                : BoardLocation.At($"A{move.From.Rank}");

            var rook = boardState.GetItem(rookLoc);

            if (rook == null) return false;

            var rookIsValid = rook.Item.EntityType == ChessPieceName.Rook
                              && rook.Item.Player == king.Player; // && !rook.MoveHistory.Any()

            var pathBetween = CalcPathBetweenKingAndCastle(move, king);

            var destinationIsEmptyValidator = new DestinationIsEmptyValidator();
            var pathIsEmpty = pathBetween.All(loc 
                => destinationIsEmptyValidator.ValidateMove(
                    new BoardMove(move.From, loc, MoveType.MoveOnly), 
                    boardState));

            var destinationNotUnderAttackValidator = new DestinationNotUnderAttackValidator();
            var pathNotUnderAttack = pathBetween.All(loc 
                => destinationNotUnderAttackValidator.ValidateMove(
                    new BoardMove(move.From, loc, MoveType.MoveOnly),
                    boardState));
            
            return kingIsValid && rookIsValid && pathIsEmpty && pathNotUnderAttack;
        }

        private static List<BoardLocation> CalcPathBetweenKingAndCastle(BoardMove move, ChessPieceEntity king)
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

            pathBetween.RemoveAll(location => location == null);
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