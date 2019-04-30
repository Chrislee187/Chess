using System;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{

    public class ChessBoardEngine : ILiveBoardActions
    {
        public readonly BoardState BoardState;

        private readonly IGameSetup _gameSetup;
        private readonly IMoveValidator _moveValidator;
        private readonly IRefreshAllPaths _allPathCalculator;

        public ChessBoardEngine(IGameSetup gameSetup, IMoveValidator moveValidator) : this(gameSetup, moveValidator, new DefaultRefreshAllPaths())
        {
        }

        public ChessBoardEngine(IGameSetup gameSetup, IMoveValidator moveValidator, IRefreshAllPaths allPathCalculator)
        {
            _moveValidator = moveValidator;
            BoardState = new BoardState(_moveValidator);
            _gameSetup = gameSetup;
            _allPathCalculator = allPathCalculator;
            _gameSetup.SetupPieces(this);
        }

        public void ResetBoard()
        {
            ClearBoard();
            _gameSetup.SetupPieces(this);
        }
        public void ClearBoard()
        {
            BoardState.Clear();
        }

        public ChessBoardEngine AddEntity(ChessPieceEntity create, string startingLocation) =>
            AddEntity(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            BoardState.SetEntity(startingLocation, create);
            BoardState.ClearPaths(startingLocation);
            return this;
        }
        
        public ActiveBoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);
        public ActiveBoardPiece PieceAt(BoardLocation location)
        {
            var piece = BoardState.GetEntityOrNull(location);
            if (piece == null) return null;

            var validPaths = BoardState.GetOrCreatePaths(piece, location);

            return new ActiveBoardPiece(piece, validPaths);
        }

        public BoardPiece[,] Board
        {
            get
            {
                var pieces = new BoardPiece[8, 8];
                foreach (ChessFile file in Enum.GetValues(typeof(ChessFile)))
                {
                    for (int rank = 8; rank > 0; rank--)
                    {
                        var entity = BoardState.GetEntityOrNull(BoardLocation.At(file, rank));
                        pieces[(int)file - 1, rank - 1] = entity == null
                            ? null
                            : new BoardPiece(entity.Player, entity.EntityType);
                    }
                }

                return pieces;
            }
        }

        //TODO: Need an abstraction around Move, to not be Chess specific
        // so will need some default actions (move entity, remove entity) but can be extended with custom ones, (enpassant, castle)
        public void Move(ChessMove validMove)
        {
            Action<ChessMove, ILiveBoardActions> action;
            switch (validMove.ChessMoveType)
            {
                case ChessMoveType.MoveOnly:
                    action = MoveOnlyAction;
                    break;
                case ChessMoveType.TakeOnly:
                    action = TakeOnlyAction;
                    break;
                case ChessMoveType.KingMove:
                    action = MoveOnlyAction;
                    break;
                case ChessMoveType.MoveOrTake:
                    action = MoveOrTakeAction;
                    break;
                default:
                    throw new NotImplementedException($"MoveType: {validMove.ChessMoveType} not implemented");
            }

            action(validMove, this);

            _allPathCalculator.RefreshAllPaths(BoardState);
        }

        void MoveOnlyAction(ChessMove move, ILiveBoardActions actions)
        {
            var piece = actions.GetEntity(move.From);
            actions.ClearSquare(move.From);
            actions.PlaceEntity(move.To, piece);
        }
        void TakeOnlyAction(ChessMove move, ILiveBoardActions actions)
        {
            TakePieceAction(move.To, actions);

            MoveOnlyAction(move, actions);
        }
        void MoveOrTakeAction(ChessMove move, ILiveBoardActions actions)
        {
            var dest = actions.GetEntity(move.To);
            
            if (dest != null)
            {
                TakePieceAction(move.To, actions);
            }

            MoveOnlyAction(move, actions);
        }
        void TakePieceAction(BoardLocation loc, ILiveBoardActions actions)
        {

            // TODO: Record lost piece etc.
            actions.ClearSquare(loc);
        }

        #region Board Actions
        ChessPieceEntity ILiveBoardActions.GetEntity(BoardLocation loc) => BoardState.GetEntityOrNull(loc);

        void ILiveBoardActions.PlaceEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            BoardState.SetEntity(loc, entity);
            BoardState.SetPaths(loc, BoardState.GeneratePossiblePaths(entity, loc));
        }

        void ILiveBoardActions.ClearSquare(BoardLocation loc)
        {
            BoardState.SetEntity(loc, null);
            BoardState.SetPaths(loc, null);
        }
        #endregion

    }
    public class DefaultRefreshAllPaths : IRefreshAllPaths
    {
        public void RefreshAllPaths(BoardState boardState)
        {
            foreach (var kvp in boardState.Entities)
            {
                boardState.SetPaths(kvp.Key, null);
                boardState.GetOrCreatePaths(kvp.Value, kvp.Key);
            }

        }
    }

}