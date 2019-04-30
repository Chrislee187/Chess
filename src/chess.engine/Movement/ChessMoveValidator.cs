using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public interface IMoveValidator
    {
        Path ValidPath(Path possiblePath, BoardState boardState);
    }

    public class ChessMoveValidator : IMoveValidator
    {
        private delegate bool ChessBoardMovePredicate(ChessMove move
        , BoardState boardState);

        private readonly IDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>> _moveValidationFactory =
            new Dictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>>()
            {
                {ChessMoveType.KingMove, new ChessBoardMovePredicate[] {DestinationIsEmpty, DestinationNotUnderAttack}},
                {ChessMoveType.MoveOnly, new ChessBoardMovePredicate[] {DestinationIsEmpty}},
                {ChessMoveType.MoveOrTake, new ChessBoardMovePredicate[] {DestinationIsEmptyOrContainsEnemy}},
                {ChessMoveType.TakeOnly, new ChessBoardMovePredicate[] {DestinationContainsEnemy}},
                {ChessMoveType.TakeEnPassant, new ChessBoardMovePredicate[] {EnPassantIsPossible}}
            };


        public Path ValidPath(Path possiblePath, BoardState boardState)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                if (!_moveValidationFactory.TryGetValue(move.ChessMoveType, out var moveTests))
                {
                    throw new ArgumentOutOfRangeException(nameof(move.ChessMoveType), move.ChessMoveType, $"No Move Validator implemented for {move.ChessMoveType}");
                }

                if (!moveTests.All(t => t(move, boardState)))
                {
                    break;
                }

                validPath.Add(move);
            }

            return validPath;
        }
        // TODO: Move these into the board state class, lose the statics, add tests
        // Path Validations should hang off board state?
        private static bool EnPassantIsPossible(ChessMove move, BoardState boardState)
        {
            var normalTakeOk = DestinationContainsEnemy(move, boardState);

            var piece = boardState.Entities[move.From];

            var passingPieceLocation = move.To.MoveBack(piece.Player);

            if (!boardState.Entities.TryGetValue(passingPieceLocation, out var passingPiece)) return false;
            if (passingPiece.Player == piece.Player) return false;
            if (passingPiece.EntityType != ChessPieceName.Pawn) return false;

            var enpassantOk = CheckPawnUsedDoubleMove(move.To);

            return normalTakeOk || enpassantOk;
        }

        private static bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }

        private static bool DestinationContainsEnemy(ChessMove move, BoardState boardState)
        {
            var sourcePiece = boardState.Entities[move.From];
            Guard.NotNull(sourcePiece);

            if (!boardState.Entities.TryGetValue(move.To, out var destinationPiece))
            {
                return false;
            }

            return sourcePiece.Player != destinationPiece.Player;
        }

        private static bool DestinationIsEmptyOrContainsEnemy(ChessMove move, BoardState boardState) 
            => DestinationIsEmpty(move, boardState) || DestinationContainsEnemy(move, boardState);

        private static bool DestinationIsEmpty(ChessMove move, BoardState boardState) 
            => !boardState.Entities.ContainsKey(move.To) || boardState.Entities[move.To] == null;

        private static bool DestinationNotUnderAttack(ChessMove move, BoardState boardState)
        {
            var pieceEntity = boardState.Entities[move.From];
            var enemyColour = pieceEntity.Player.Enemy();
            var enemyLocationKeys = boardState.Entities.Where(kvp => kvp.Value?.Player == enemyColour).Select(kvp => kvp.Key);

            var enemyPaths = boardState.Paths.Where(kvp => enemyLocationKeys.Contains(kvp.Key) && kvp.Value != null).ToList();
            var enemyPaths2 = enemyPaths.SelectMany(kvp => kvp.Value).SelectMany(p => p).ToList();

            return !enemyPaths2.Any(enemyMove 
                        => Equals(enemyMove.To, move.To));

        }

    }
}