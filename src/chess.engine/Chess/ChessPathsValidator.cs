using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public class ChessPathsValidator : IPathsValidator<ChessPieceEntity>
    {
        private readonly IPathValidator<ChessPieceEntity> _pathValidator;
        private readonly IBoardActionFactory<ChessPieceEntity> _actionFactory;

        public ChessPathsValidator(IPathValidator<ChessPieceEntity> pathValidator, IBoardActionFactory<ChessPieceEntity> actionFactory)
        {
            _actionFactory = actionFactory;
            _pathValidator = pathValidator;
        }
        public Paths RemoveInvalidMoves(Paths possiblePaths, IBoardState<ChessPieceEntity> boardState, bool removeMovesThatLeaveKingInCheck)
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
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Owner))
            );

            return paths;
        }

        private Paths RemoveMovesThatLeaveKingInCheck(Paths possiblePaths, IBoardState<ChessPieceEntity> boardState)
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
        public bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move, IBoardState<ChessPieceEntity> boardState)
        {
            var clonedState = (IBoardState<ChessPieceEntity>) boardState.Clone();

            // Execute the move directly, we've already validated it on the original board
            var playerColour = clonedState.GetItem(move.From).Item.Owner;
            var action = _actionFactory.Create(move.MoveType, clonedState);
            action.Execute(move);

            var inCheck = clonedState.CurrentGameState(playerColour, playerColour.Enemy()) != GameState.InProgress;
            return inCheck;
        }
    }
}