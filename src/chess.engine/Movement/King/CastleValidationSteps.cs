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
    // TODO: Rename this to something more common, ChessValidationSteps - create a nice Mock builder for it
    // IsLocationUnderCheck etc. from PlayerStateService and refactor accordingly
    // IsLocationUnderAttack etc. like Check but includes the king in the scans
    // Add IsPawnAllowedToEnpassant()
    // Add IsPawnAllowedToTakeWithEnPassant()

    public class CastleValidationSteps : ICastleValidationSteps
    {
        // TODO: Needs integration tests at least, going to be clumsy to unit test with mocks
        public bool IsPathClearFromAttacks(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState, IEnumerable<BoardLocation> pathBetween)
        {
            // TODO: Use IsLocationUnderAttack() (see above) instead of the the validator
            var destinationNotUnderAttackValidator = new DestinationNotUnderAttackValidator<ChessPieceEntity>();
            var pathNotUnderAttack = pathBetween.All(loc
                => destinationNotUnderAttackValidator.ValidateMove(
                    new BoardMove(move.From, loc, (int)DefaultActions.MoveOnly),
                    roBoardState));
            return pathNotUnderAttack;
        }

        public bool IsPathBetweenClear(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> roBoardState,
            Colours kingColour, out IEnumerable<BoardLocation> pathBetween)
        {
            pathBetween = CalcPathBetweenKingAndCastle(move, kingColour);

            return pathBetween.All(loc
                => roBoardState.GetItem(loc) == null);

        }

        public bool IsRookAllowedToCastle(BoardMove move,
            IReadOnlyBoardState<ChessPieceEntity> roBoardState, Colours player)
        {
            var rookLoc = move.MoveType == (int)ChessMoveTypes.CastleKingSide
                ? $"H{move.From.Y}".ToBoardLocation()
                : $"A{move.From.Y}".ToBoardLocation();

            var rook = roBoardState.GetItem(rookLoc);
            if (rook == null) return false;

            return rook.Item.Is(player, ChessPieceName.Rook)
                   && rook.Item.LocationHistory.Count() == 1;
        }

        public bool IsKingAllowedToCastle(BoardMove move,
            IReadOnlyBoardState<ChessPieceEntity> roBoardState,
            out ChessPieceEntity king)
        {
            var kingItem = roBoardState.GetItem(move.From);
            king = null;
            if (kingItem == null) return false;
            king = kingItem.Item;

            return king.Piece.Equals(ChessPieceName.King)
                   && kingItem.Location.Equals(Pieces.King
                       .StartPositionFor(king.Player))
                   && king.LocationHistory.Count() == 1;
        }

        private static IEnumerable<BoardLocation> CalcPathBetweenKingAndCastle(BoardMove move, Colours kingColour)
        {
            // TODO: This could be pulled out and tested in isolation indeed just be static lists
            var pathBetween = new List<BoardLocation>();

            BoardLocation KingSide(Colours c, int i)
                => c == Colours.White
                    ? move.From.MoveRight(c, i)
                    : move.From.MoveLeft(c, i);

            BoardLocation QueenSide(Colours c, int i)
                => c == Colours.White
                    ? move.From.MoveLeft(c, i)
                    : move.From.MoveRight(c, i);


            var kingOwner = kingColour;

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
    }
}