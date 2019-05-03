using System.Collections.Generic;
using System.Linq;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class BoardState
    {
        private readonly IPathValidator _pathValidator;
        private readonly IDictionary<BoardLocation, LocatedItem<ChessPieceEntity>> _items;

        public BoardState(IPathValidator pathValidator)
        {
            _pathValidator = pathValidator;
            _items = new Dictionary<BoardLocation, LocatedItem<ChessPieceEntity>>();
        }

        private Path ValidatePath(Path possiblePath) 
            => _pathValidator.ValidatePath(possiblePath, this);

        public IEnumerable<BoardLocation> LocationsInUse => _items.Keys;

        public void Clear() => _items.Clear();

        public void PlaceEntity(BoardLocation loc, ChessPieceEntity entity, Paths paths = null) 
            => _items[loc] = new LocatedItem<ChessPieceEntity>(loc, entity, paths);

        public LocatedItem<ChessPieceEntity> GetItem(BoardLocation loc)
            => Get(loc).Single();

        public void UpdatePaths(ChessPieceEntity forEntity, BoardLocation at)
        {
            var item = GetItem(at);

            Guard.NotNull(item, $"Null item found at {at}!");

            var paths = GeneratePossiblePaths(forEntity, at);

            paths = RemoveInvalidMoves(paths);
            item.UpdatePaths(paths);
        }
        public Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new Paths();

            foreach (var pathGen in entity.PathGenerators)
            {
                var movesFrom = pathGen.PathsFrom(boardLocation, entity.Player);
                paths.AddRange(movesFrom);
            }

            return paths;
        }

        private Paths RemoveInvalidMoves(Paths possiblePaths)
        {
            var validPaths = new Paths();

            foreach (var possiblePath in possiblePaths)
            {
                var validPath = ValidatePath(possiblePath);

                if (validPath.Any())
                {
                    validPaths.Add(validPath);
                }
            }

            return validPaths;
        }
        
        public IEnumerable<BoardLocation> LocationsOf(Colours player, ChessPieceName piece)
        {
            return _items
                .Where(kvp => kvp.Value.Item.EntityType == piece
                              && kvp.Value.Item.Player == player)
                .Select(kvp => kvp.Key);
        }
        public IEnumerable<BoardLocation> LocationsOf(Colours player)
        {
            return _items
                .Where(kvp => kvp.Value.Item.Player == player)
                .Select(kvp => kvp.Key);
        }


        public GameState CurrentGameState(Colours forPlayer)
        {
            var kingLoc = LocationsOf(forPlayer, ChessPieceName.King).First();
            var enemiesAttackingKing = _items.Values.Where(itm
                    => itm.Paths.SelectMany(p => p).Any(m => m.To.Equals(kingLoc))).ToList();

            var inCheck = enemiesAttackingKing.Any();

            if (inCheck)
            {
                return CheckForCheckMate(forPlayer, kingLoc, enemiesAttackingKing);
            }

            return GameState.InProgress;
        }

        private GameState CheckForCheckMate(Colours forPlayer, BoardLocation kingLoc, List<LocatedItem<ChessPieceEntity>> enemiesAttackingKing)
        {
            var state = GameState.Check;
            var kingCannotMove = !_items[kingLoc].Paths.Any(); // Move validator will ensure we can't move into check

            // Get possible destinations of all friendly support pieces
            var friendlyDestinations = FriendlySupportLocations(forPlayer, kingLoc);

            bool canBlock = enemiesAttackingKing.All(enemy =>
            {
                // Get the path from the item that it is attack along
                var attackingPath = enemy.Paths.Single(attackPath => attackPath.Any(p => p.To.Equals(kingLoc)));

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

        private IEnumerable<BoardLocation> FriendlySupportLocations(Colours forPlayer, BoardLocation kingLoc)
        {
            var friendlyItems = _items.Where(i => i.Key != kingLoc).Select(i => i.Value);
            var friendlyDestinations = friendlyItems.SelectMany(fi => fi.Paths.FlattenMoves()).Select(m => m.To);
            return friendlyDestinations;
        }


        public bool MoveIsATake(ChessMove move)
        {
            if (IsEmpty(move.To)) return false;

            var movePlayerColour = GetItem(move.From).Item.Player;
            var takeEntity = GetItem(move.To).Item;
            var moveIsATake = takeEntity != null && takeEntity.Player != movePlayerColour;
            return moveIsATake;
        }


        public bool MoveLeavesKingInCheck(ChessMove move)
        {
            // TODO: Is there anyway to accurately do this without making the move and
            // recalculating all the paths???
            return false;
        }

        public bool IsEmpty(BoardLocation moveTo) => !_items.ContainsKey(moveTo);

        public IEnumerable<LocatedItem<ChessPieceEntity>> Get(params BoardLocation[] locations)
        {
            return _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value);
        }
        public IEnumerable<LocatedItem<ChessPieceEntity>> Get(ChessPieceName pieceType)
        {
            return _items.Values.Where(itm => itm.Item.EntityType == pieceType);
        }

        public void Remove(BoardLocation loc)
        {
            _items.Remove(loc);
        }
    }

}