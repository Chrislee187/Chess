using System.Collections.Generic;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class BoardState : IBoardState //<ChessPieceEntity, Colours>
    {
        private readonly IDictionary<BoardLocation, LocatedItem<IBoardEntity<ChessPieceName, Colours>>> _items;
        private readonly IBoardActionFactory _actionFactory;
        private readonly IChessPathsValidator _chessPathsValidator;

        public BoardState(IChessPathsValidator chessPathsValidator, IBoardActionFactory actionFactory)
        {
            _items = new Dictionary<BoardLocation, LocatedItem<IBoardEntity<ChessPieceName, Colours>>>();
            _chessPathsValidator = chessPathsValidator;
            _actionFactory = actionFactory;
        }

        public void Clear() => _items.Clear();

        public IEnumerable<BoardLocation> GetAllItemLocations => _items.Keys;

        public void PlaceEntity(BoardLocation loc, IBoardEntity<ChessPieceName, Colours> entity, bool generateMoves = true) 
            => _items[loc] = new LocatedItem<IBoardEntity<ChessPieceName, Colours>>(loc, entity, 
                generateMoves 
                ? _chessPathsValidator.GeneratePossiblePaths(entity, loc) 
                : null);

        public LocatedItem<IBoardEntity<ChessPieceName, Colours>> GetItem(BoardLocation loc)
            => GetItems(loc).SingleOrDefault();

        public GameState CurrentGameState(Colours forPlayer)
        {
            var king = GetItems(forPlayer, ChessPieceName.King).First();
            var locatedItems = GetItems(forPlayer.Enemy());
            var enemiesAttackingKing = locatedItems.Where(itm
                => itm.Paths.ContainsMoveTo(king.Location)).ToList();

            var inCheck = enemiesAttackingKing.Any();

            if (inCheck)
            {
                return CheckForCheckMate(forPlayer, enemiesAttackingKing);
            }

            return GameState.InProgress;
        }

        public IEnumerable<BoardLocation> LocationsOf(Colours player, ChessPieceName piece)
        {
            return _items
                .Where(kvp => kvp.Value.Item.EntityType == piece
                              && kvp.Value.Item.Owner == player)
                .Select(kvp => kvp.Key);
        }

        public IEnumerable<BoardLocation> LocationsOf(Colours player)
        {
            return _items
                .Where(kvp => kvp.Value.Item.Owner == player)
                .Select(kvp => kvp.Key);
        }

        public bool IsEmpty(BoardLocation location) => !_items.ContainsKey(location);

        public void GeneratePaths(IBoardEntity<ChessPieceName, Colours> forEntity, BoardLocation at, bool removeMovesThatLeaveKingInCheck = true)
        {
            var item = GetItem(at);

            Guard.NotNull(item, $"Null item found at {at}!");

            var paths = _chessPathsValidator.GeneratePossiblePaths(forEntity, at);

            paths = _chessPathsValidator.RemoveInvalidMoves(paths, this, true);
            
            item.UpdatePaths(paths);
        }

        public object Clone()
        {
            // Clone the items, ignore paths as they will be regenerated
            var clonedItems = _items.Values.Select(e =>
                new LocatedItem<IBoardEntity<ChessPieceName, Colours>>((BoardLocation)e.Location.Clone(),
                    (IBoardEntity<ChessPieceName, Colours>)e.Item.Clone(), 
                    null));

            // TODO: ? Why do we not clone the paths instead of regening?

            var clonedState = new BoardState(_chessPathsValidator, _actionFactory);
            foreach (var clonedItem in clonedItems)
            {
                clonedState.PlaceEntity(clonedItem.Location.Clone() as BoardLocation, clonedItem.Item.Clone() as IBoardEntity<ChessPieceName, Colours>);
            }
            return clonedState;
        }

        public bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move) 
            => _chessPathsValidator.DoesMoveLeaveMovingPlayersKingInCheck(move, this);

        public IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer)
        {
            var friendlyItems = GetItems(forPlayer).Where(i => i.Item.EntityType != ChessPieceName.King);
            var friendlyDestinations = friendlyItems.SelectMany(fi => fi.Paths.FlattenMoves()).Select(m => m.To);
            return friendlyDestinations;
        }

        public GameState CheckForCheckMate(Colours forPlayer, List<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> enemiesAttackingKing)
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

        public IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(params BoardLocation[] locations)
        {
            return _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value).Cast<LocatedItem<IBoardEntity<ChessPieceName, Colours>>>();
        }
        public IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(ChessPieceName pieceType)
        {
            return _items.Values.Where(itm => itm.Item.EntityType == pieceType);
        }
        public IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(Colours colour)
        {
            return _items.Where(itm => itm.Value.Item.Owner == colour).Select(kvp => kvp.Value);
        }
        public IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(Colours colour, ChessPieceName piece)
        {
            return _items.Where(itm => itm.Value.Item.Owner == colour && itm.Value.Item.EntityType == piece).Select(kvp => kvp.Value);
        }

        public void Remove(BoardLocation loc)
        {
            _items.Remove(loc);
        }
    }

}