using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class ChessMoveValidator
    {

        private delegate bool ChessBoardMovePredicate(ChessMove move
        , IDictionary<BoardLocation, ChessPieceEntity> entities
        , IDictionary<BoardLocation, IEnumerable<Path>> paths);

        public Path ValidPath(Path possiblePath, 
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                var tests = new List<ChessBoardMovePredicate>();

                switch (move.ChessMoveType)
                {
                    case ChessMoveType.KingMove:
                        // TODO: Needs additional code to check for being in check or not
                        tests.Add(DestinationIsEmpty);
                        tests.Add(DestinationNotUnderAttack);
                        break;
                    case ChessMoveType.MoveOnly:
                        tests.Add(DestinationIsEmpty);
                        break;
                    case ChessMoveType.TakeOnly:
                        tests.Add(DestinationContainsEnemy);
                        break;
                    case ChessMoveType.TakeEnPassant:
                        tests.Add(EnPassantIsPossible);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(move.ChessMoveType), move.ChessMoveType, $"NotImplemented ChessMoveType");
                }

                if (!tests.All(t => t(move, entities, paths)))
                {
                    break;
                }

                validPath.Add(move);
            }

            return validPath;
        }

        private bool EnPassantIsPossible(ChessMove move,
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths)
        {
            var normalTakeOk = DestinationContainsEnemy(move, entities, paths);

            var piece = entities[move.From];

            var passingPieceLocation = move.To.MoveBack(piece.Player);
            var passingPiece = entities[passingPieceLocation];

            if (passingPiece == null) return false;
            if (passingPiece.Player == piece.Player) return false;
            if (passingPiece.EntityType != ChessPieceName.Pawn) return false;

            var enpassantOk = CheckPawnUsedDoubleMove(move.To);

            return normalTakeOk || enpassantOk;
        }

        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }

        // Valid Path Tests

        private bool DestinationContainsEnemy(ChessMove move,
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths)
        {
            var sourcePiece = entities[move.From];
            Guard.NotNull(sourcePiece);

            if (!entities.TryGetValue(move.To, out var destinationPiece))
            {
                return false;
            }

            return sourcePiece.Player != destinationPiece.Player;
        }
        private bool DestinationIsEmpty(ChessMove move,
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths) => LocationIsEmpty(move.To, entities);
        private bool LocationIsEmpty(BoardLocation location,
            IDictionary<BoardLocation, ChessPieceEntity> entities) 
            => !entities.ContainsKey(location) || entities[location] == null;
        private bool DestinationNotUnderAttack(ChessMove move,
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths)
        {
            var pieceEntity = entities[move.From];
            var enemyColour = pieceEntity.Player.Enemy();
            var enemyLocationKeys = entities.Where(kvp => kvp.Value?.Player == enemyColour).Select(kvp => kvp.Key);

            var enemyPaths = paths.Where(kvp => enemyLocationKeys.Contains(kvp.Key) && kvp.Value != null).ToList();
            var enemyPaths2 = enemyPaths.SelectMany(kvp => kvp.Value).SelectMany(p => p).ToList();

            return !enemyPaths2.Any(enemyMove 
                        => Equals(enemyMove.To, move.To));

        }

    }
}