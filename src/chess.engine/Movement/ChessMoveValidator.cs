using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class ChessMoveValidator
    {
        private readonly ChessGameEngine.ChessPieceEntityProvider _pieceEntityProvider;

        private delegate bool ChessBoardMovePredicate(ChessMove move);
        public ChessMoveValidator(ChessGameEngine.ChessPieceEntityProvider pieceEntityProvider)
        {
            _pieceEntityProvider = pieceEntityProvider;
        }
        public Path ValidPath(Path possiblePath)
        {
            var validPath = new Path();
            foreach (var move in possiblePath)
            {
                var tests = new List<ChessBoardMovePredicate>();

                switch (move.ChessMoveType)
                {
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

                if (!tests.All(t => t(move)))
                {
                    break;
                }

                validPath.Add(move);
            }

            return validPath;
        }

        private bool EnPassantIsPossible(ChessMove move)
        {
            var normalTakeOk = DestinationContainsEnemy(move);

            var piece = _pieceEntityProvider(move.From);

            var passingPieceLocation = move.To.MoveBack(piece.Player);
            var passingPiece = _pieceEntityProvider(passingPieceLocation);

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

        private bool DestinationContainsEnemy(ChessMove move)
        {
            var sourcePiece = _pieceEntityProvider(move.From);
            Guard.NotNull(sourcePiece);

            var destinationPiece = _pieceEntityProvider(move.To);
            if (destinationPiece == null) return false;

            return sourcePiece.Player != destinationPiece.Player;
        }
        private bool DestinationIsEmpty(ChessMove move) => LocationIsEmpty(move.To);
        private bool LocationIsEmpty(BoardLocation location) => _pieceEntityProvider(location) == null;

    }
}