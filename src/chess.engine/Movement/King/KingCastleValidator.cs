using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using board.engine.Movement.Validators;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;

namespace chess.engine.Movement.King
{
    public class KingCastleValidator : IMoveValidator<ChessPieceEntity> 
    {

        private static List<BoardLocation> CalcPathBetweenKingAndCastle(BoardMove move, ChessPieceEntity king)
        {
            // TODO: This needs tests
            var pathBetween = new List<BoardLocation>();

            BoardLocation KingSide(Colours c, int i) 
                => c == Colours.White 
                    ? move.From.MoveRight(c, i) 
                    : move.From.MoveLeft(c, i);

            BoardLocation QueenSide(Colours c, int i)
                => c == Colours.White
                    ? move.From.MoveLeft(c, i)
                    : move.From.MoveRight(c, i);


            var kingOwner = king.Player;

            if (move.From.X < move.To.X)
            {
                pathBetween.Add(KingSide(kingOwner, 1));
                pathBetween.Add(KingSide(kingOwner, 2));
            }
            else
            {
                pathBetween.Add(QueenSide(kingOwner, 1));
                pathBetween.Add(QueenSide(kingOwner, 2));
            }

            pathBetween.RemoveAll(location => location == null);
            return pathBetween;
        }
        
        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState)
        {
            var kingEntity = roBoardState.GetItem(move.From);
            if (kingEntity == null) return false;

            var king = kingEntity.Item;
            var kingIsValid = king.Piece.Equals(ChessPieceName.King); // && !king.MoveHistory.Any()
            if (!kingIsValid) return false;

            var rookLoc = move.MoveType == (int)ChessMoveTypes.CastleKingSide
                ? $"H{move.From.Y}".ToBoardLocation()
                : $"A{move.From.Y}".ToBoardLocation();

            var rook = roBoardState.GetItem(rookLoc);
            if (rook == null) return false;

            var rookIsValid = rook.Item.Is(king.Player, ChessPieceName.Rook); // && !rook.MoveHistory.Any()

            if (!rookIsValid) return false;

            var pathBetween = CalcPathBetweenKingAndCastle(move, king);

            var destinationIsEmptyValidator = new DestinationIsEmptyValidator<ChessPieceEntity>();
            var pathIsEmpty = pathBetween.All(loc
                => destinationIsEmptyValidator.ValidateMove(
                    new BoardMove(move.From, loc, (int)DefaultActions.MoveOnly),
                    roBoardState));

            var destinationNotUnderAttackValidator = new DestinationNotUnderAttackValidator<ChessPieceEntity>();
            var pathNotUnderAttack = pathBetween.All(loc
                => destinationNotUnderAttackValidator.ValidateMove(
                    new BoardMove(move.From, loc, (int)DefaultActions.MoveOnly),
                    roBoardState));

            return pathIsEmpty && pathNotUnderAttack;
        }
    }
}