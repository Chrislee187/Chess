using System;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public interface IChessPathsValidator
    {
        Paths RemoveInvalidMoves(Paths possiblePaths, IBoardState boardState, bool removeMovesThatLeaveKingInCheck);
        Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation);
        bool DoesMoveLeaveMovingPlayersKingInCheck(ChessMove move, IBoardState boardState);
    }
    public class ChessPathsValidator : IChessPathsValidator
    {
        private readonly IPathValidator _pathValidator;
        private readonly IBoardActionFactory _actionFactory;

        public ChessPathsValidator(IPathValidator pathValidator, IBoardActionFactory actionFactory)
        {
            _actionFactory = actionFactory;
            _pathValidator = pathValidator;
        }
        public Paths RemoveInvalidMoves(Paths possiblePaths, IBoardState boardState, bool removeMovesThatLeaveKingInCheck)
        {
            var validPaths = new Paths();

            foreach (var possiblePath in possiblePaths)
            {
                var testedPath = _pathValidator.ValidatePath(possiblePath, boardState);

                if (testedPath.Any())
                {

                    validPaths.Add(testedPath);
                }
            }
            if (removeMovesThatLeaveKingInCheck)
            {
                validPaths = RemoveMovesThatLeaveKingInCheck(validPaths, boardState);
            }

            return validPaths;
        }

        public Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new Paths();
            paths.AddRange(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Player))
            );

            return paths;
        }

        private Paths RemoveMovesThatLeaveKingInCheck(Paths possiblePaths, IBoardState boardState)
        {
            var validPaths = new Paths();

            foreach (var possiblePath in possiblePaths)
            {
                var testedPath = new Path();
                foreach (var move in possiblePath)
                {
                    var inCheck = DoesMoveLeaveMovingPlayersKingInCheck(move, boardState);

                    if (!inCheck)
                    {
                        testedPath.Add(move);
                    }
                }

                if (testedPath.Any())
                {
                    validPaths.Add(testedPath);
                }
            }

            return validPaths;
        }
        public bool DoesMoveLeaveMovingPlayersKingInCheck(ChessMove move, IBoardState boardState)
        {
            var clonedState = (BoardState) boardState.Clone();

            // Execute the move directly, we've already validated it on the original board
            var playerColour = clonedState.GetItem(move.From).Item.Player;
            var action = _actionFactory.Create(move.ChessMoveType, clonedState);
            action.Execute(move);

            var inCheck = clonedState.CurrentGameState(playerColour) != GameState.InProgress;
            return inCheck;
        }


    }


}