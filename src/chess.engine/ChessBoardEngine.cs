using System;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{

    public class ChessBoardEngine : IBoardState
    {
        public readonly BoardState BoardState;

        private readonly IGameSetup _gameSetup;
        private readonly IPathValidator _pathValidator;
        private readonly IRefreshAllPaths _allPathCalculator;

        public ChessBoardEngine(IGameSetup gameSetup, IPathValidator pathValidator) : this(gameSetup, pathValidator, new DefaultRefreshAllPaths())
        {
        }

        public ChessBoardEngine(IGameSetup gameSetup, IPathValidator pathValidator, IRefreshAllPaths allPathCalculator)
        {
            _pathValidator = pathValidator;
            BoardState = new BoardState(_pathValidator);
            _gameSetup = gameSetup;
            _allPathCalculator = allPathCalculator;
            _gameSetup.SetupPieces(this);
            _allPathCalculator.RefreshAllPaths(BoardState);
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

        //TODO: Need an abstraction around MoveType's and Actions, to not be Chess specific
        // so will need some default state (move entity, remove entity) but can be extended with custom ones, (enpassant, castle)
        public void Move(ChessMove validMove)
        {
            Action<ChessMove> action;
            switch (validMove.ChessMoveType)
            {
                case ChessMoveType.MoveOnly:
                    action = (move) => new MoveOnlyAction(this).Execute(move);
                    break;
                case ChessMoveType.TakeOnly:
                    action = (move) => new TakeOnlyAction(this).Execute(move);
                    break;
                case ChessMoveType.KingMove:
                    action = (move) => new MoveOrTakeAction(this).Execute(move);
                    break;
                case ChessMoveType.MoveOrTake:
                    action = (move) => new MoveOrTakeAction(this).Execute(move);
                    break;
                case ChessMoveType.CastleQueenSide:
                case ChessMoveType.CastleKingSide:
                    action = CastleAction;
                    break;
                default:
                    throw new NotImplementedException($"MoveType: {validMove.ChessMoveType} not implemented");
            }

            action(validMove);

            _allPathCalculator.RefreshAllPaths(BoardState);
        }

        void CastleAction(ChessMove move)
        {
            throw new NotImplementedException();
        }

        #region Board Actions
        ChessPieceEntity IBoardState.GetEntity(BoardLocation loc) => BoardState.GetEntityOrNull(loc);

        void IBoardState.SetEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            BoardState.SetEntity(loc, entity);
            BoardState.SetPaths(loc, BoardState.GeneratePossiblePaths(entity, loc));
        }

        void IBoardState.ClearLocation(BoardLocation loc)
        {
            BoardState.SetEntity(loc, null);
            BoardState.SetPaths(loc, null);
        }
        #endregion

        private class DefaultRefreshAllPaths : IRefreshAllPaths
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
}