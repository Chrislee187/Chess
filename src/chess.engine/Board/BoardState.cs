using System.Collections.Generic;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class BoardState<TEntity> : IBoardState<TEntity> where TEntity : IBoardEntity
    {
        private readonly IDictionary<BoardLocation, LocatedItem<TEntity>> _items;
        private readonly IBoardActionFactory<TEntity> _actionFactory;
        private readonly IPathsValidator<TEntity> _pathsValidator;

        public BoardState(IPathsValidator<TEntity> pathsValidator, 
            IBoardActionFactory<TEntity> actionFactory)
        {
            _items = new Dictionary<BoardLocation, LocatedItem<TEntity>>();
            _pathsValidator = pathsValidator;
            _actionFactory = actionFactory;
        }

        public void Clear() => _items.Clear();

        public IEnumerable<BoardLocation> GetAllItemLocations => _items.Keys;

        public void PlaceEntity(BoardLocation loc, TEntity entity, bool generateMoves = true) 
            => _items[loc] = new LocatedItem<TEntity>(loc, entity, 
                generateMoves 
                ? _pathsValidator.GeneratePossiblePaths(entity, loc) 
                : null);

        public LocatedItem<TEntity> GetItem(BoardLocation loc)
            => GetItems(loc).SingleOrDefault();

        // TODO: This should be here, pull in to something with a simple dependency on the IBoardState interface
        public GameState CurrentGameState(Colours currentPlayer, Colours enemy)
        {
            var king = GetItems(currentPlayer, ChessPieceName.King).First();

            var locatedItems = GetItems(enemy);
            
            var enemiesAttackingKing = locatedItems.Where(itm
                => itm.Paths.ContainsMoveTo(king.Location)).ToList();

            var inCheck = enemiesAttackingKing.Any();

            if (inCheck)
            {
                return CheckForCheckMate(currentPlayer, enemiesAttackingKing);
            }

            return GameState.InProgress;
        }

        // TODO: ChessPieceName, not generic
        public IEnumerable<BoardLocation> LocationsOf(Colours owner, ChessPieceName piece)
        {
            return _items
                .Where(kvp => kvp.Value.Item.EntityName.Equals(piece)
                              && kvp.Value.Item.Owner.Equals(owner))
                .Select(kvp => kvp.Key);
        }

        public IEnumerable<BoardLocation> LocationsOf(Colours owner)
        {
            return _items
                .Where(kvp => kvp.Value.Item.Owner.Equals(owner))
                .Select(kvp => kvp.Key);
        }

        public bool IsEmpty(BoardLocation location) => !_items.ContainsKey(location);

        public void GeneratePaths(TEntity forEntity, BoardLocation at, bool removeMovesThatLeaveKingInCheck = true)
        {
            var item = GetItem(at);

            Guard.NotNull(item, $"Null item found at {at}!");

            var paths = _pathsValidator.GeneratePossiblePaths(forEntity, at);

            paths = _pathsValidator.RemoveInvalidMoves(paths, this, true);
            
            item.UpdatePaths(paths);
        }

        public object Clone()
        {
            // Clone the items, ignore paths as they will be regenerated
            var clonedItems = _items.Values.Select(e =>
                new LocatedItem<TEntity>((BoardLocation)e.Location.Clone(),
                    (TEntity)e.Item.Clone(), 
                    null));

            // TODO: ? Why do we not clone the paths instead of regening?

            var clonedState = new BoardState<TEntity>(_pathsValidator, _actionFactory);
            foreach (var clonedItem in clonedItems)
            {
                clonedState.PlaceEntity(clonedItem.Location.Clone() as BoardLocation, (TEntity) clonedItem.Item.Clone());
            }
            return clonedState;
        }

        public bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move) 
            => _pathsValidator.DoesMoveLeaveMovingPlayersKingInCheck(move, this);

        public IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer)
        {
            var friendlyItems = GetItems(forPlayer).Where(i => !i.Item.EntityName.Equals(ChessPieceName.King));
            var friendlyDestinations = friendlyItems.SelectMany(fi => fi.Paths.FlattenMoves()).Select(m => m.To);
            return friendlyDestinations;
        }

        public GameState CheckForCheckMate(Colours forPlayer, List<LocatedItem<TEntity>> enemiesAttackingKing)
        {
            var state = GameState.Check;
            var king = GetItems(forPlayer, ChessPieceName.King).Single();
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = GetAllMoveDestinations(forPlayer);

            bool canBlock = enemiesAttackingKing.All(enemy =>
            {
                var attackingPath = enemy.Paths
                    .Single(attackPath => attackPath.Any(p => p.To.Equals(king.Location)));

                // Check if any friendly pieces can move to the path or take the item
                return friendlyDestinations.Any(fd => fd.Equals(enemy.Location)
                                                      || attackingPath.Any(move => move.To.Equals(fd))
                );
            });

            if (kingCannotMove && !canBlock)
            {
                state = GameState.Checkmate;
            }

            return state;
        }

        public IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations)
        {
            return _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value).Cast<LocatedItem<TEntity>>();
        }
        public IEnumerable<LocatedItem<TEntity>> GetItems(ChessPieceName pieceType)
        {
            return _items.Values.Where(itm => itm.Item.EntityName == pieceType.ToString());
        }
        public IEnumerable<LocatedItem<TEntity>> GetItems(Colours owner)
        {
            return _items.Where(itm => itm.Value.Item.Owner.Equals(owner)).Select(kvp => kvp.Value);
        }
        public IEnumerable<LocatedItem<TEntity>> GetItems(Colours owner, ChessPieceName piece)
        {
            return _items.Where(itm => itm.Value.Item.Owner.Equals(owner) 
                                       && itm.Value.Item.EntityName == piece.ToString()
                                       ).Select(kvp => kvp.Value);
        }

        public void Remove(BoardLocation loc)
        {
            _items.Remove(loc);
        }
    }

}