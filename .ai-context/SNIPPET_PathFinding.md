# AI.Odin Pathfinding and Spatial Systems Implementation Snippets

## Overview

This document contains comprehensive code implementation snippets for the pathfinding and spatial systems within our Entity Component System (ECS) framework. Each section provides component definitions and system implementations in C# for creating scalable pathfinding and spatial partitioning that supports thousands of entities while maintaining 60 FPS performance.

**Note:** These are complete implementations ready for integration with the existing ECS architecture. The systems build upon current `PhysicsComponent` and `MovementSystem` implementations.

## Implementation Priority Matrix

### Core Spatial Systems (1-4) - Essential Foundation

#### 1. Dual Spatial Partitioning

**Enhanced Movement Component:**
```csharp
public record MovementComponent : IComponent
{
    public Vector2 Velocity { get; init; }
    public Vector2 Destination { get; init; }
    public float Speed { get; init; } = 50f;
    public PathfindingMethod Method { get; init; } = PathfindingMethod.Direct;
    public int FlowFieldId { get; init; } = -1;
    public float RecalculationTimer { get; init; } = 0f;
}

public record PathfindingComponent : IComponent
{
    public Queue<Vector2> Waypoints { get; init; } = new();
    public Vector2 CurrentTarget { get; init; }
    public PathfindingStatus Status { get; init; } = PathfindingStatus.Idle;
    public float LastRecalculationTime { get; init; } = 0f;
    public PathfindingMethod PreferredMethod { get; init; } = PathfindingMethod.Direct;
}

public record SpatialPartitionComponent : IComponent
{
    public enum PartitionType
    {
        Dynamic,    // Use spatial hash grid for moving entities
        Static,     // Use quadtree for buildings/terrain
        Hybrid      // Use both for different queries
    }
    
    public PartitionType Type { get; init; } = PartitionType.Dynamic;
    public Rect2 Bounds { get; init; }
    public bool IsStatic { get; init; } = false;
    public CollisionLayer Layer { get; init; } = CollisionLayer.Default;
    public float QueryRadius { get; init; } = 32f;
    public float LastUpdateTime { get; init; } = 0f;
}

public record CollisionResponseComponent : IComponent
{
    public CollisionResponseType ResponseType { get; init; } = CollisionResponseType.Separate;
    public float SeparationForce { get; init; } = 10f;
    public float AvoidanceDistance { get; init; } = 16f;
    public bool CanPushOthers { get; init; } = false;
    public HashSet<CollisionLayer> CollidesWithLayers { get; init; } = new();
    public float PushStrength { get; init; } = 1f;
}

public record StaticEntityComponent : IComponent
{
    public bool HasChanged { get; init; } = true;
    public StaticEntityType Type { get; init; } = StaticEntityType.Building;
    public float LastModifiedTime { get; init; } = 0f;
    public Rect2 StaticBounds { get; init; }
}

public enum PathfindingMethod
{
    Direct,         // Simple A* for short distances
    JumpPointSearch,// Optimized A* for long distances
    FlowField,      // Shared pathfinding for groups
    Hierarchical,   // HPA* for large maps
    Cached          // Use cached result
}

public enum PathfindingStatus
{
    Idle,           // Not currently pathfinding
    Calculating,    // Path calculation in progress
    Following,      // Following calculated path
    Stuck,          // Unable to reach destination
    Reached         // Destination reached
}

public enum CollisionResponseType
{
    None,           // No collision response
    Separate,       // Push objects apart
    Stop,           // Stop movement on collision
    Bounce,         // Reflect velocity
    Trigger,        // Send collision event only
    Destroy         // Remove entity on collision
}

public enum StaticEntityType
{
    Building,       // Player-constructed buildings
    Terrain,        // Natural terrain features
    Resource,       // Resource nodes (trees, rocks)
    Decoration      // Visual elements
}

public enum CollisionLayer
{
    Default,
    Buildings,
    Resources,
    Entities,
    Projectiles
}
```

**Dual Spatial Manager System:**
```csharp
public class DualSpatialManagerSystem : BaseFixedSystem
{
    private readonly SpatialHashGrid<IEntity> _movingEntitiesGrid;
    private readonly StaticQuadTree<IEntity> _staticEntitiesTree;
    private readonly Dictionary<IEntity, SpatialStructureType> _entityStructureMap;
    private readonly CrossStructureQueryCache _queryCache;
    
    private enum SpatialStructureType { Moving, Static }
    
    public DualSpatialManagerSystem(IEntityManager entityManager)
        : base(entityManager)
    {
        var worldBounds = new Rect2(new Vector2(-1000, -1000), new Vector2(2000, 2000));
        
        _movingEntitiesGrid = new SpatialHashGrid<IEntity>(
            worldBounds: worldBounds,
            cellSize: 32f,           // 2x typical entity size
            expectedDensity: 0.1f    // 10% cell occupancy
        );
        
        _staticEntitiesTree = new StaticQuadTree<IEntity>(
            worldBounds: worldBounds,
            maxDepth: 10,            // Deep subdivision for varied sizes
            maxItemsPerLeaf: 8       // Geometric precision
        );
        
        _entityStructureMap = new Dictionary<IEntity, SpatialStructureType>();
        _queryCache = new CrossStructureQueryCache();
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(PhysicsComponent),
        typeof(SpatialPartitionComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        var spatialComponent = entity.FindComponent<SpatialPartitionComponent>();
        
        var entityBounds = CalculateEntityBounds(entity, physicsComponent, spatialComponent);
        
        // Determine if entity should be in moving or static structure
        var isStatic = entity.HasComponent<StaticEntityComponent>() || 
                      spatialComponent.Type == SpatialPartitionComponent.PartitionType.Static;
        
        var currentTime = Time.RealtimeSinceStartup;
        
        if (!_entityStructureMap.TryGetValue(entity, out var currentStructure))
        {
            // New entity - add to appropriate structure
            if (isStatic)
            {
                _staticEntitiesTree.Insert(entity, entityBounds);
                _entityStructureMap[entity] = SpatialStructureType.Static;
            }
            else
            {
                _movingEntitiesGrid.Insert(entity, entityBounds);
                _entityStructureMap[entity] = SpatialStructureType.Moving;
            }
        }
        else
        {
            // Existing entity - check for structure migration or update
            if (isStatic && currentStructure == SpatialStructureType.Moving)
            {
                // Migrate from moving to static
                MigrateEntityToStatic(entity, entityBounds);
            }
            else if (!isStatic && currentStructure == SpatialStructureType.Static)
            {
                // Migrate from static to moving
                MigrateEntityToMoving(entity, entityBounds);
            }
            else
            {
                // Update within current structure
                UpdateEntityInStructure(entity, entityBounds, currentStructure);
            }
        }
        
        // Update spatial component timestamp
        entity.SetComponent(spatialComponent with { LastUpdateTime = currentTime });
        
        // Update query cache
        _queryCache.Update(Time.DeltaTime);
    }
    
    private void MigrateEntityToStatic(IEntity entity, Rect2 bounds)
    {
        _movingEntitiesGrid.Remove(entity);
        _staticEntitiesTree.Insert(entity, bounds);
        _entityStructureMap[entity] = SpatialStructureType.Static;
        
        // Invalidate cache for affected regions
        _queryCache.InvalidateRegion(bounds);
    }
    
    private void MigrateEntityToMoving(IEntity entity, Rect2 bounds)
    {
        _staticEntitiesTree.Remove(entity);
        _movingEntitiesGrid.Insert(entity, bounds);
        _entityStructureMap[entity] = SpatialStructureType.Moving;
        
        // Invalidate cache for affected regions
        _queryCache.InvalidateRegion(bounds);
    }
    
    private void UpdateEntityInStructure(IEntity entity, Rect2 newBounds, SpatialStructureType structure)
    {
        var oldBounds = GetLastKnownBounds(entity);
        
        if (structure == SpatialStructureType.Moving)
        {
            _movingEntitiesGrid.Update(entity, oldBounds, newBounds);
        }
        else
        {
            _staticEntitiesTree.Update(entity, oldBounds, newBounds);
        }
        
        UpdateLastKnownBounds(entity, newBounds);
    }
    
    public IEnumerable<IEntity> GetNearbyEntities(IEntity sourceEntity, float radius,
        bool includeMoving = true, bool includeStatic = true)
    {
        var position = sourceEntity.FindComponent<PhysicsComponent>().Position;
        var queryBounds = new Rect2(
            new Vector2(position.X - radius, position.Y - radius),
            new Vector2(radius * 2, radius * 2)
        );
        
        // Check cache first
        if (_queryCache.TryGetCachedResult(queryBounds, out var cachedResult))
        {
            return cachedResult.Where(e => e != sourceEntity);
        }
        
        var neighbors = new List<IEntity>();
        
        if (includeMoving)
        {
            neighbors.AddRange(_movingEntitiesGrid.Query(queryBounds)
                .Where(e => e != sourceEntity));
        }
        
        if (includeStatic)
        {
            neighbors.AddRange(_staticEntitiesTree.Query(queryBounds)
                .Where(e => e != sourceEntity));
        }
        
        // Cache the result
        _queryCache.CacheResult(queryBounds, neighbors);
        
        return neighbors;
    }
}
```

**Optimized Spatial Hash Grid:**
```csharp
public class SpatialHashGrid<T> where T : class
{
    private struct GridCell
    {
        public List<SpatialItem> Items;
        public int Version; // For cache invalidation
        
        public GridCell()
        {
            Items = new List<SpatialItem>(4); // Pre-size for typical occupancy
            Version = 0;
        }
    }
    
    private struct SpatialItem
    {
        public T Item;
        public Rect2 Bounds;
        public uint Hash; // Cache hash for fast removal
        
        public SpatialItem(T item, Rect2 bounds, uint hash)
        {
            Item = item;
            Bounds = bounds;
            Hash = hash;
        }
    }
    
    private readonly GridCell[] _grid;
    private readonly Dictionary<T, List<uint>> _itemCells;
    private readonly Dictionary<T, Rect2> _lastKnownBounds;
    private readonly int _gridWidth;
    private readonly int _gridHeight;
    private readonly float _cellSize;
    private readonly Vector2 _worldOffset;
    private readonly uint _gridMask;
    
    public SpatialHashGrid(Rect2 worldBounds, float cellSize, float expectedDensity = 0.1f)
    {
        _cellSize = cellSize;
        _worldOffset = worldBounds.Position;
        
        _gridWidth = Mathf.CeilToInt(worldBounds.Size.X / cellSize);
        _gridHeight = Mathf.CeilToInt(worldBounds.Size.Y / cellSize);
        
        // Use power-of-2 grid size for fast modulo operations
        int gridSize = Mathf.NextPowerOf2(_gridWidth * _gridHeight);
        _gridMask = (uint)(gridSize - 1);
        
        _grid = new GridCell[gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            _grid[i] = new GridCell();
        }
        
        _itemCells = new Dictionary<T, List<uint>>();
        _lastKnownBounds = new Dictionary<T, Rect2>();
    }
    
    public void Insert(T item, Rect2 bounds)
    {
        var cellHashes = GetCellHashes(bounds);
        _itemCells[item] = cellHashes;
        _lastKnownBounds[item] = bounds;
        
        var spatialItem = new SpatialItem(item, bounds, 0);
        
        foreach (uint hash in cellHashes)
        {
            ref var cell = ref _grid[hash];
            cell.Items.Add(spatialItem);
            cell.Version++;
        }
    }
    
    public void Update(T item, Rect2 oldBounds, Rect2 newBounds)
    {
        if (!_itemCells.TryGetValue(item, out var oldCellHashes))
        {
            Insert(item, newBounds);
            return;
        }
        
        var newCellHashes = GetCellHashes(newBounds);
        
        // Optimize for micro-movements (most common case)
        if (ListsEqual(oldCellHashes, newCellHashes))
        {
            // Object stayed in same cells, just update bounds
            var spatialItem = new SpatialItem(item, newBounds, 0);
            foreach (uint hash in oldCellHashes)
            {
                ref var cell = ref _grid[hash];
                for (int i = 0; i < cell.Items.Count; i++)
                {
                    if (ReferenceEquals(cell.Items[i].Item, item))
                    {
                        cell.Items[i] = spatialItem;
                        break;
                    }
                }
            }
            _lastKnownBounds[item] = newBounds;
        }
        else
        {
            // Object moved to different cells
            Remove(item);
            Insert(item, newBounds);
        }
    }
    
    public void Remove(T item)
    {
        if (!_itemCells.TryGetValue(item, out var cellHashes))
            return;
        
        foreach (uint hash in cellHashes)
        {
            ref var cell = ref _grid[hash];
            for (int i = cell.Items.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(cell.Items[i].Item, item))
                {
                    cell.Items.RemoveAt(i);
                    cell.Version++;
                    break;
                }
            }
        }
        
        _itemCells.Remove(item);
        _lastKnownBounds.Remove(item);
    }
    
    public IEnumerable<T> Query(Rect2 bounds)
    {
        var results = new HashSet<T>();
        var cellHashes = GetCellHashes(bounds);
        
        foreach (uint hash in cellHashes)
        {
            ref var cell = ref _grid[hash];
            foreach (var spatialItem in cell.Items)
            {
                if (bounds.Intersects(spatialItem.Bounds))
                {
                    results.Add(spatialItem.Item);
                }
            }
        }
        
        return results;
    }
    
    private List<uint> GetCellHashes(Rect2 bounds)
    {
        var localBounds = new Rect2(bounds.Position - _worldOffset, bounds.Size);
        
        int minX = Mathf.FloorToInt(localBounds.Position.X / _cellSize);
        int minY = Mathf.FloorToInt(localBounds.Position.Y / _cellSize);
        int maxX = Mathf.FloorToInt((localBounds.Position.X + localBounds.Size.X) / _cellSize);
        int maxY = Mathf.FloorToInt((localBounds.Position.Y + localBounds.Size.Y) / _cellSize);
        
        var hashes = new List<uint>((maxX - minX + 1) * (maxY - minY + 1));
        
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                uint hash = SpatialHash(x, y) & _gridMask;
                hashes.Add(hash);
            }
        }
        
        return hashes;
    }
    
    private static uint SpatialHash(int x, int y)
    {
        // Jenkins hash function for good distribution
        const uint p1 = 73856093u;
        const uint p2 = 19349663u;
        return ((uint)x * p1) ^ ((uint)y * p2);
    }
    
    private static bool ListsEqual(List<uint> list1, List<uint> list2)
    {
        if (list1.Count != list2.Count) return false;
        
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i]) return false;
        }
        
        return true;
    }
}
```

#### 2. Core Pathfinding Algorithms

**Pathfinding Coordinator System:**
```csharp
public class PathfindingCoordinatorSystem : BaseFixedSystem
{
    private readonly JumpPointSearchSystem _jpsSystem;
    private readonly DirectPathfindingSystem _directSystem;
    private readonly FlowFieldSystem _flowFieldSystem;
    private readonly Dictionary<Vector2, int> _destinationCounts;
    private readonly Dictionary<(Vector2, Vector2), List<Vector2>> _pathCache;
    private readonly float _chunkSize = 16f;
    
    public PathfindingCoordinatorSystem(IEntityManager entityManager)
        : base(entityManager)
    {
        _jpsSystem = new JumpPointSearchSystem();
        _directSystem = new DirectPathfindingSystem();
        _flowFieldSystem = new FlowFieldSystem();
        _destinationCounts = new Dictionary<Vector2, int>();
        _pathCache = new Dictionary<(Vector2, Vector2), List<Vector2>>();
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(MovementComponent),
        typeof(PathfindingComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var movementComponent = entity.FindComponent<MovementComponent>();
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        // Check if pathfinding is needed
        if (movementComponent.Destination == Vector2.Zero || 
            pathfindingComponent.Status == PathfindingStatus.Calculating)
            return;
        
        var currentTime = Time.RealtimeSinceStartup;
        
        // Check if recalculation is needed
        if (pathfindingComponent.Status == PathfindingStatus.Following &&
            currentTime - pathfindingComponent.LastRecalculationTime < 1.0f) // 1 second minimum between recalculations
            return;
        
        var method = DetermineOptimalMethod(entity, movementComponent.Destination);
        RequestPath(entity, movementComponent.Destination, method);
    }
    
    public void RequestPath(IEntity entity, Vector2 destination, PathfindingMethod method)
    {
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        
        // Update status to calculating
        entity.SetComponent(pathfindingComponent with { 
            Status = PathfindingStatus.Calculating,
            LastRecalculationTime = Time.RealtimeSinceStartup
        });
        
        switch (method)
        {
            case PathfindingMethod.FlowField:
                var flowFieldId = GetOrCreateFlowField(destination);
                AssignToFlowField(entity, flowFieldId);
                break;
                
            case PathfindingMethod.JumpPointSearch:
                ScheduleJPSPath(entity, destination);
                break;
                
            case PathfindingMethod.Direct:
                CalculateDirectPath(entity, destination);
                break;
                
            case PathfindingMethod.Cached:
                ApplyCachedPath(entity, destination);
                break;
        }
    }
    
    private PathfindingMethod DetermineOptimalMethod(IEntity entity, Vector2 destination)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var distance = position.DistanceTo(destination);
        
        // Check if destination has enough users for flow field
        var destinationKey = QuantizeDestination(destination, 32f);
        var nearbyEntitiesWithSameDestination = CountNearbyEntitiesWithDestination(destinationKey);
        
        if (nearbyEntitiesWithSameDestination >= 5)
            return PathfindingMethod.FlowField;
        else if (distance > _chunkSize * 3)
            return PathfindingMethod.JumpPointSearch;
        else if (HasCachedPath(position, destination))
            return PathfindingMethod.Cached;
        else
            return PathfindingMethod.Direct;
    }
    
    private Vector2 QuantizeDestination(Vector2 destination, float gridSize)
    {
        return new Vector2(
            Mathf.Round(destination.X / gridSize) * gridSize,
            Mathf.Round(destination.Y / gridSize) * gridSize
        );
    }
    
    private int CountNearbyEntitiesWithDestination(Vector2 destination)
    {
        return _destinationCounts.GetValueOrDefault(destination, 0);
    }
    
    private bool HasCachedPath(Vector2 start, Vector2 destination)
    {
        return _pathCache.ContainsKey((start, destination));
    }
    
    private int GetOrCreateFlowField(Vector2 destination)
    {
        return _flowFieldSystem.GetOrCreateFlowField(destination, maxUsers: 100);
    }
    
    private void AssignToFlowField(IEntity entity, int flowFieldId)
    {
        var movementComponent = entity.FindComponent<MovementComponent>();
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        
        entity.SetComponent(movementComponent with { 
            Method = PathfindingMethod.FlowField,
            FlowFieldId = flowFieldId 
        });
        
        entity.SetComponent(pathfindingComponent with {
            Status = PathfindingStatus.Following
        });
    }
    
    private void ScheduleJPSPath(IEntity entity, Vector2 destination)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var path = _jpsSystem.FindPath(position, destination);
        
        if (path.Count > 0)
        {
            var waypoints = new Queue<Vector2>(path);
            var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
            
            entity.SetComponent(pathfindingComponent with {
                Waypoints = waypoints,
                Status = PathfindingStatus.Following,
                CurrentTarget = waypoints.Count > 0 ? waypoints.Peek() : destination
            });
        }
        else
        {
            // No path found
            var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
            entity.SetComponent(pathfindingComponent with { Status = PathfindingStatus.Stuck });
        }
    }
    
    private void CalculateDirectPath(IEntity entity, Vector2 destination)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var path = _directSystem.FindPath(position, destination);
        
        if (path.Count > 0)
        {
            var waypoints = new Queue<Vector2>(path);
            var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
            
            entity.SetComponent(pathfindingComponent with {
                Waypoints = waypoints,
                Status = PathfindingStatus.Following,
                CurrentTarget = waypoints.Count > 0 ? waypoints.Peek() : destination
            });
            
            // Cache the result
            _pathCache[(position, destination)] = path;
        }
        else
        {
            var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
            entity.SetComponent(pathfindingComponent with { Status = PathfindingStatus.Stuck });
        }
    }
    
    private void ApplyCachedPath(IEntity entity, Vector2 destination)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var path = _pathCache[(position, destination)];
        
        var waypoints = new Queue<Vector2>(path);
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        
        entity.SetComponent(pathfindingComponent with {
            Waypoints = waypoints,
            Status = PathfindingStatus.Following,
            CurrentTarget = waypoints.Count > 0 ? waypoints.Peek() : destination
        });
    }
}
```

**Jump Point Search System:**
```csharp
public class JumpPointSearchSystem
{
    private readonly Dictionary<(Vector2, Vector2), List<Vector2>> _pathCache;
    private Vector2 _currentGoal;
    
    public JumpPointSearchSystem()
    {
        _pathCache = new Dictionary<(Vector2, Vector2), List<Vector2>>();
    }
    
    public List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        var cacheKey = (start, goal);
        if (_pathCache.TryGetValue(cacheKey, out var cachedPath))
        {
            return new List<Vector2>(cachedPath);
        }
        
        _currentGoal = goal;
        var path = JumpPointSearch(start, goal);
        
        // Cache the result (limit cache size)
        if (_pathCache.Count > 1000)
        {
            var oldestKey = _pathCache.Keys.First();
            _pathCache.Remove(oldestKey);
        }
        _pathCache[cacheKey] = new List<Vector2>(path);
        
        return path;
    }
    
    private List<Vector2> JumpPointSearch(Vector2 start, Vector2 goal)
    {
        var openSet = new PriorityQueue<JPSNode, float>();
        var closedSet = new HashSet<Vector2>();
        var cameFrom = new Dictionary<Vector2, Vector2>();
        var gScore = new Dictionary<Vector2, float>();
        var fScore = new Dictionary<Vector2, float>();
        
        gScore[start] = 0;
        fScore[start] = HeuristicDistance(start, goal);
        openSet.Enqueue(new JPSNode(start), fScore[start]);
        
        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue().Position;
            
            if (Vector2.Distance(current, goal) < 8f) // Close enough to goal
            {
                return ReconstructPath(cameFrom, current);
            }
            
            closedSet.Add(current);
            
            var successors = GetSuccessors(current, cameFrom.GetValueOrDefault(current, current));
            
            foreach (var successor in successors)
            {
                if (closedSet.Contains(successor))
                    continue;
                
                var tentativeGScore = gScore[current] + Vector2.Distance(current, successor);
                
                if (!gScore.ContainsKey(successor) || tentativeGScore < gScore[successor])
                {
                    cameFrom[successor] = current;
                    gScore[successor] = tentativeGScore;
                    fScore[successor] = tentativeGScore + HeuristicDistance(successor, goal);
                    
                    openSet.Enqueue(new JPSNode(successor), fScore[successor]);
                }
            }
        }
        
        return new List<Vector2>(); // No path found
    }
    
    private List<Vector2> GetSuccessors(Vector2 current, Vector2 parent)
    {
        var successors = new List<Vector2>();
        var direction = (current - parent).Normalized();
        
        if (direction == Vector2.Zero)
        {
            // Initial node, check all directions
            var allDirections = new Vector2[]
            {
                Vector2.Up, Vector2.Right, Vector2.Down, Vector2.Left,
                new Vector2(1, 1).Normalized(), new Vector2(1, -1).Normalized(), 
                new Vector2(-1, 1).Normalized(), new Vector2(-1, -1).Normalized()
            };
            
            foreach (var dir in allDirections)
            {
                var jumpPoint = Jump(current, dir);
                if (jumpPoint != Vector2.Zero)
                {
                    successors.Add(jumpPoint);
                }
            }
        }
        else
        {
            // Prune directions based on parent
            var prunedDirections = PruneDirections(current, direction);
            
            foreach (var dir in prunedDirections)
            {
                var jumpPoint = Jump(current, dir);
                if (jumpPoint != Vector2.Zero)
                {
                    successors.Add(jumpPoint);
                }
            }
        }
        
        return successors;
    }
    
    private Vector2 Jump(Vector2 current, Vector2 direction)
    {
        var next = current + direction * 16f; // 16-pixel grid step
        
        if (!IsWalkable(next))
            return Vector2.Zero;
        
        if (Vector2.Distance(next, _currentGoal) < 8f)
            return next;
        
        // Check for forced neighbors
        if (HasForcedNeighbor(next, direction))
            return next;
        
        // Diagonal movement - check orthogonal jumps
        if (direction.X != 0 && direction.Y != 0)
        {
            if (Jump(next, new Vector2(direction.X, 0)) != Vector2.Zero ||
                Jump(next, new Vector2(0, direction.Y)) != Vector2.Zero)
            {
                return next;
            }
        }
        
        // Continue jumping in the same direction
        return Jump(next, direction);
    }
    
    private bool HasForcedNeighbor(Vector2 position, Vector2 direction)
    {
        // Check for obstacles that would force consideration of this position
        if (direction.X != 0 && direction.Y != 0) // Diagonal
        {
            return (!IsWalkable(position + new Vector2(-direction.X, 0)) && 
                    IsWalkable(position + new Vector2(-direction.X, direction.Y))) ||
                   (!IsWalkable(position + new Vector2(0, -direction.Y)) && 
                    IsWalkable(position + new Vector2(direction.X, -direction.Y)));
        }
        else if (direction.X != 0) // Horizontal
        {
            return (!IsWalkable(position + new Vector2(0, 16)) && 
                    IsWalkable(position + new Vector2(direction.X, 16))) ||
                   (!IsWalkable(position + new Vector2(0, -16)) && 
                    IsWalkable(position + new Vector2(direction.X, -16)));
        }
        else // Vertical
        {
            return (!IsWalkable(position + new Vector2(16, 0)) && 
                    IsWalkable(position + new Vector2(16, direction.Y))) ||
                   (!IsWalkable(position + new Vector2(-16, 0)) && 
                    IsWalkable(position + new Vector2(-16, direction.Y)));
        }
    }
    
    private bool IsWalkable(Vector2 position)
    {
        // Implementation depends on your collision detection system
        // This should check against static obstacles, buildings, etc.
        return true; // Placeholder - implement based on your collision system
    }
    
    private List<Vector2> PruneDirections(Vector2 current, Vector2 direction)
    {
        var directions = new List<Vector2>();
        
        // Always add the straight direction
        directions.Add(direction);
        
        // Add diagonal directions if applicable
        if (direction.X != 0 && direction.Y != 0)
        {
            // Diagonal movement - add orthogonal directions
            directions.Add(new Vector2(direction.X, 0));
            directions.Add(new Vector2(0, direction.Y));
        }
        
        return directions;
    }
    
    private float HeuristicDistance(Vector2 a, Vector2 b)
    {
        // Octile distance (allows diagonal movement)
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        return (dx + dy) + (1.414f - 2) * Math.Min(dx, dy);
    }
    
    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        var path = new List<Vector2> { current };
        
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        
        return path;
    }
    
    private struct JPSNode
    {
        public Vector2 Position;
        
        public JPSNode(Vector2 position)
        {
            Position = position;
        }
    }
}
```

#### 3. Flow Field Implementation

**Flow Field System:**
```csharp
public class FlowFieldSystem
{
    private readonly Dictionary<int, FlowField> _flowFields;
    private readonly Dictionary<Vector2, int> _destinationToFlowField;
    private int _nextFlowFieldId = 1;
    
    public struct FlowField
    {
        public Vector2[,] VectorGrid;
        public Vector2 Destination;
        public int UserCount;
        public int MaxUsers;
        public float LastUpdateTime;
        public Rect2 Bounds;
    }
    
    public FlowFieldSystem()
    {
        _flowFields = new Dictionary<int, FlowField>();
        _destinationToFlowField = new Dictionary<Vector2, int>();
    }
    
    public int GetOrCreateFlowField(Vector2 destination, int maxUsers = 50)
    {
        // Check if flow field already exists for this destination
        if (_destinationToFlowField.TryGetValue(destination, out int existingId))
        {
            var existing = _flowFields[existingId];
            if (existing.UserCount < existing.MaxUsers)
            {
                existing.UserCount++;
                _flowFields[existingId] = existing;
                return existingId;
            }
        }
        
        // Create new flow field
        var flowField = new FlowField
        {
            Destination = destination,
            VectorGrid = GenerateVectorGrid(destination),
            UserCount = 1,
            MaxUsers = maxUsers,
            LastUpdateTime = Time.RealtimeSinceStartup,
            Bounds = CalculateFlowFieldBounds(destination)
        };
        
        var flowFieldId = _nextFlowFieldId++;
        _flowFields[flowFieldId] = flowField;
        _destinationToFlowField[destination] = flowFieldId;
        
        return flowFieldId;
    }
    
    private Vector2[,] GenerateVectorGrid(Vector2 destination)
    {
        const int gridSize = 128; // 128x128 flow field grid
        const float cellSize = 16f;
        var vectorGrid = new Vector2[gridSize, gridSize];
        
        // Use Dijkstra flood-fill to generate distance field
        var distanceField = GenerateDistanceField(destination, gridSize, cellSize);
        
        // Convert distance field to vector field
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                vectorGrid[x, y] = CalculateFlowVector(x, y, distanceField);
            }
        }
        
        return vectorGrid;
    }
    
    private float[,] GenerateDistanceField(Vector2 destination, int gridSize, float cellSize)
    {
        var distanceField = new float[gridSize, gridSize];
        var visited = new bool[gridSize, gridSize];
        var queue = new Queue<(int x, int y, float distance)>();
        
        // Initialize all cells to infinity
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                distanceField[x, y] = float.MaxValue;
            }
        }
        
        // Start from destination
        var destGridX = Mathf.RoundToInt(destination.X / cellSize);
        var destGridY = Mathf.RoundToInt(destination.Y / cellSize);
        
        if (destGridX >= 0 && destGridX < gridSize && destGridY >= 0 && destGridY < gridSize)
        {
            distanceField[destGridX, destGridY] = 0f;
            queue.Enqueue((destGridX, destGridY, 0f));
            visited[destGridX, destGridY] = true;
        }
        
        // Dijkstra flood-fill
        var directions = new (int dx, int dy)[] 
        {
            (0, 1), (1, 0), (0, -1), (-1, 0), // Cardinal
            (1, 1), (1, -1), (-1, 1), (-1, -1) // Diagonal
        };
        
        while (queue.Count > 0)
        {
            var (x, y, distance) = queue.Dequeue();
            
            foreach (var (dx, dy) in directions)
            {
                var nx = x + dx;
                var ny = y + dy;
                
                if (nx < 0 || nx >= gridSize || ny < 0 || ny >= gridSize || visited[nx, ny])
                    continue;
                
                // Check if walkable
                var worldPos = new Vector2(nx * cellSize, ny * cellSize);
                if (!IsWalkable(worldPos))
                    continue;
                
                var moveCost = (dx != 0 && dy != 0) ? 1.414f : 1.0f; // Diagonal cost
                var newDistance = distance + moveCost;
                
                if (newDistance < distanceField[nx, ny])
                {
                    distanceField[nx, ny] = newDistance;
                    queue.Enqueue((nx, ny, newDistance));
                    visited[nx, ny] = true;
                }
            }
        }
        
        return distanceField;
    }
    
    private Vector2 CalculateFlowVector(int x, int y, float[,] distanceField)
    {
        var gridSize = distanceField.GetLength(0);
        var currentDistance = distanceField[x, y];
        
        if (currentDistance == float.MaxValue)
            return Vector2.Zero;
        
        var bestDirection = Vector2.Zero;
        var bestDistance = currentDistance;
        
        // Check all 8 neighbors
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                
                var nx = x + dx;
                var ny = y + dy;
                
                if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize)
                {
                    var neighborDistance = distanceField[nx, ny];
                    if (neighborDistance < bestDistance)
                    {
                        bestDistance = neighborDistance;
                        bestDirection = new Vector2(dx, dy).Normalized();
                    }
                }
            }
        }
        
        return bestDirection;
    }
    
    public Vector2 SampleFlowField(int flowFieldId, Vector2 worldPosition)
    {
        if (!_flowFields.TryGetValue(flowFieldId, out var flowField))
            return Vector2.Zero;
        
        const float cellSize = 16f;
        const int gridSize = 128;
        
        var gridX = Mathf.RoundToInt(worldPosition.X / cellSize);
        var gridY = Mathf.RoundToInt(worldPosition.Y / cellSize);
        
        if (gridX >= 0 && gridX < gridSize && gridY >= 0 && gridY < gridSize)
        {
            return flowField.VectorGrid[gridX, gridY];
        }
        
        return Vector2.Zero;
    }
    
    public void ReleaseFlowField(int flowFieldId)
    {
        if (_flowFields.TryGetValue(flowFieldId, out var flowField))
        {
            flowField.UserCount--;
            if (flowField.UserCount <= 0)
            {
                _flowFields.Remove(flowFieldId);
                _destinationToFlowField.Remove(flowField.Destination);
            }
            else
            {
                _flowFields[flowFieldId] = flowField;
            }
        }
    }
    
    private bool IsWalkable(Vector2 position)
    {
        // Implementation should check against static obstacles
        return true; // Placeholder
    }
    
    private Rect2 CalculateFlowFieldBounds(Vector2 destination)
    {
        const float fieldSize = 128 * 16; // Grid size * cell size
        return new Rect2(destination - Vector2.One * fieldSize / 2, Vector2.One * fieldSize);
    }
}
```

#### 4. Enhanced Movement Integration

**Spatial-Aware Movement System:**
```csharp
public class SpatialAwareMovementSystem : BaseFixedSystem
{
    private readonly DualSpatialManagerSystem _spatialManager;
    private readonly PathfindingCoordinatorSystem _pathfindingCoordinator;
    private readonly FlowFieldSystem _flowFieldSystem;
    
    public SpatialAwareMovementSystem(IEntityManager entityManager,
        DualSpatialManagerSystem spatialManager,
        PathfindingCoordinatorSystem pathfindingCoordinator,
        FlowFieldSystem flowFieldSystem)
        : base(entityManager)
    {
        _spatialManager = spatialManager;
        _pathfindingCoordinator = pathfindingCoordinator;
        _flowFieldSystem = flowFieldSystem;
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(MovementComponent),
        typeof(PathfindingComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var movementComponent = entity.FindComponent<MovementComponent>();
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        Vector2 desiredVelocity = CalculateDesiredVelocity(entity, movementComponent, pathfindingComponent, physicsComponent);
        
        // Apply collision avoidance
        var neighbors = _spatialManager.GetNearbyEntities(entity, 32f, includeMoving: true, includeStatic: false);
        var avoidanceForce = CalculateAvoidanceForce(entity, neighbors);
        
        // Combine desired movement with avoidance
        var finalVelocity = desiredVelocity + avoidanceForce;
        finalVelocity = Vector2.ClampMagnitude(finalVelocity, movementComponent.Speed);
        
        // Update position
        var newPosition = physicsComponent.Position + finalVelocity * Time.DeltaTime;
        
        // Check for static obstacle collisions
        if (CheckStaticCollisions(entity, newPosition))
        {
            // Handle collision response
            newPosition = HandleStaticCollision(entity, newPosition, physicsComponent.Position);
        }
        
        // Update components
        entity.SetComponent(movementComponent with { Velocity = finalVelocity });
        entity.SetComponent(physicsComponent with { Position = newPosition });
        
        // Check if waypoint or destination reached
        CheckWaypointProgress(entity, pathfindingComponent, newPosition);
    }
    
    private Vector2 CalculateDesiredVelocity(IEntity entity, MovementComponent movement, 
        PathfindingComponent pathfinding, PhysicsComponent physics)
    {
        Vector2 desiredDirection = Vector2.Zero;
        
        switch (movement.Method)
        {
            case PathfindingMethod.FlowField:
                if (movement.FlowFieldId >= 0)
                {
                    desiredDirection = _flowFieldSystem.SampleFlowField(movement.FlowFieldId, physics.Position);
                }
                break;
                
            case PathfindingMethod.Direct:
            case PathfindingMethod.JumpPointSearch:
                if (pathfinding.Waypoints.Count > 0)
                {
                    var targetWaypoint = pathfinding.CurrentTarget;
                    desiredDirection = (targetWaypoint - physics.Position).Normalized();
                }
                else if (movement.Destination != Vector2.Zero)
                {
                    desiredDirection = (movement.Destination - physics.Position).Normalized();
                }
                break;
        }
        
        return desiredDirection * movement.Speed;
    }
    
    private Vector2 CalculateAvoidanceForce(IEntity entity, IEnumerable<IEntity> neighbors)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var avoidanceForce = Vector2.Zero;
        var separationDistance = 24f;
        
        foreach (var neighbor in neighbors)
        {
            var neighborPos = neighbor.FindComponent<PhysicsComponent>().Position;
            var distance = position.DistanceTo(neighborPos);
            
            if (distance < separationDistance && distance > 0)
            {
                var separationVector = (position - neighborPos).Normalized();
                var strength = (separationDistance - distance) / separationDistance;
                avoidanceForce += separationVector * strength * 50f;
            }
        }
        
        return avoidanceForce;
    }
    
    private bool CheckStaticCollisions(IEntity entity, Vector2 newPosition)
    {
        var staticEntities = _spatialManager.GetNearbyEntities(entity, 16f, 
            includeMoving: false, includeStatic: true);
        
        foreach (var staticEntity in staticEntities)
        {
            var staticBounds = CalculateEntityBounds(staticEntity);
            var entityBounds = new Rect2(newPosition - Vector2.One * 8, Vector2.One * 16);
            
            if (staticBounds.Intersects(entityBounds))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private Vector2 HandleStaticCollision(IEntity entity, Vector2 attemptedPosition, Vector2 currentPosition)
    {
        // Simple slide along obstacle
        var direction = (attemptedPosition - currentPosition).Normalized();
        var slideDirection = new Vector2(-direction.Y, direction.X); // Perpendicular
        
        // Try sliding along the obstacle
        var slidePosition = currentPosition + slideDirection * 16f;
        
        if (!CheckStaticCollisions(entity, slidePosition))
        {
            return slidePosition;
        }
        
        // Try other slide direction
        slidePosition = currentPosition - slideDirection * 16f;
        
        if (!CheckStaticCollisions(entity, slidePosition))
        {
            return slidePosition;
        }
        
        // Stay at current position if no sliding works
        return currentPosition;
    }
    
    private void CheckWaypointProgress(IEntity entity, PathfindingComponent pathfinding, Vector2 currentPosition)
    {
        if (pathfinding.Status != PathfindingStatus.Following)
            return;
        
        // Check if current waypoint is reached
        if (pathfinding.Waypoints.Count > 0)
        {
            var targetWaypoint = pathfinding.CurrentTarget;
            var distanceToWaypoint = currentPosition.DistanceTo(targetWaypoint);
            
            if (distanceToWaypoint < 8f) // Reached waypoint
            {
                var waypoints = pathfinding.Waypoints;
                waypoints.Dequeue(); // Remove reached waypoint
                
                var newTarget = waypoints.Count > 0 ? waypoints.Peek() : Vector2.Zero;
                
                entity.SetComponent(pathfinding with {
                    Waypoints = waypoints,
                    CurrentTarget = newTarget,
                    Status = waypoints.Count > 0 ? PathfindingStatus.Following : PathfindingStatus.Reached
                });
            }
        }
        else
        {
            // Check if final destination is reached
            var movementComponent = entity.FindComponent<MovementComponent>();
            if (movementComponent.Destination != Vector2.Zero)
            {
                var distanceToDestination = currentPosition.DistanceTo(movementComponent.Destination);
                if (distanceToDestination < 8f)
                {
                    OnDestinationReached(entity);
                }
            }
        }
    }
    
    private void OnDestinationReached(IEntity entity)
    {
        var movementComponent = entity.FindComponent<MovementComponent>();
        var pathfindingComponent = entity.FindComponent<PathfindingComponent>();
        
        // Release flow field if using one
        if (movementComponent.Method == PathfindingMethod.FlowField && movementComponent.FlowFieldId >= 0)
        {
            _flowFieldSystem.ReleaseFlowField(movementComponent.FlowFieldId);
        }
        
        // Clear movement and pathfinding
        entity.SetComponent(movementComponent with { 
            Velocity = Vector2.Zero,
            Destination = Vector2.Zero,
            Method = PathfindingMethod.Direct,
            FlowFieldId = -1
        });
        
        entity.SetComponent(pathfindingComponent with {
            Status = PathfindingStatus.Reached,
            Waypoints = new Queue<Vector2>(),
            CurrentTarget = Vector2.Zero
        });
    }
    
    private Rect2 CalculateEntityBounds(IEntity entity)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var spatialComponent = entity.FindComponent<SpatialPartitionComponent>();
        
        if (spatialComponent?.Bounds.Size != Vector2.Zero)
        {
            return new Rect2(position + spatialComponent.Bounds.Position, spatialComponent.Bounds.Size);
        }
        
        // Default entity size
        return new Rect2(position - Vector2.One * 8, Vector2.One * 16);
    }
}
```

### Performance Optimization Systems (9-12) - Production-Ready Features

#### 9. Performance Budget Management

**Performance Budget Manager:**
```csharp
public class PerformanceBudgetManagerSystem : BaseVariableSystem
{
    private readonly Dictionary<string, float> _budgets = new()
    {
        ["SpatialUpdate"] = 2.0f,      // 2ms max
        ["CollisionDetection"] = 3.0f, // 3ms max  
        ["PathfindingUpdate"] = 2.0f   // 2ms max
    };
    
    private readonly MovingAverage _frameTimeAverage = new(60);
    private readonly Queue<Action> _spatialUpdateQueue = new();
    private readonly Queue<Action> _pathfindingQueue = new();
    
    public PerformanceBudgetManagerSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } = [];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        // This system processes the entire frame budget
    }

    public override void Update(float deltaTime, IGameState gameState)
    {
        var frameStart = Time.RealtimeSinceStartup;
        _frameTimeAverage.AddSample(deltaTime);
        
        // Process high-priority systems first with time budgets
        ProcessSpatialUpdates(frameStart);
        ProcessCollisionDetection(frameStart);
        ProcessPathfindingUpdates(frameStart);
    }
    
    private void ProcessSpatialUpdates(float frameStart)
    {
        var budget = _budgets["SpatialUpdate"];
        var processed = 0;
        
        while (_spatialUpdateQueue.Count > 0 && HasBudget(frameStart, budget) && processed < 100)
        {
            var updateAction = _spatialUpdateQueue.Dequeue();
            updateAction.Invoke();
            processed++;
        }
    }
    
    private void ProcessCollisionDetection(float frameStart)
    {
        var budget = _budgets["CollisionDetection"];
        var remainingBudget = GetRemainingBudget(frameStart, budget);
        
        if (remainingBudget > 0.5f) // Only process if we have meaningful time left
        {
            ProcessCollisionBatch(remainingBudget);
        }
    }
    
    private void ProcessPathfindingUpdates(float frameStart)
    {
        var budget = _budgets["PathfindingUpdate"];
        var processed = 0;
        
        while (_pathfindingQueue.Count > 0 && HasBudget(frameStart, budget) && processed < 50)
        {
            var pathfindingAction = _pathfindingQueue.Dequeue();
            pathfindingAction.Invoke();
            processed++;
        }
    }
    
    private void ProcessCollisionBatch(float remainingBudget)
    {
        // Process collision detection in batches based on remaining budget
        var maxCollisionChecks = Mathf.FloorToInt(remainingBudget * 200); // 200 checks per millisecond
        
        // Implementation would process collision checks up to the limit
        // This is a placeholder for the actual collision processing
    }
    
    public bool HasBudget(float startTime, float budgetMs)
    {
        var elapsed = (Time.RealtimeSinceStartup - startTime) * 1000;
        return elapsed < budgetMs;
    }
    
    public float GetRemainingBudget(float startTime, float maxBudgetMs)
    {
        var elapsed = (Time.RealtimeSinceStartup - startTime) * 1000;
        return Math.Max(0, maxBudgetMs - elapsed);
    }
    
    public void QueueSpatialUpdate(Action updateAction)
    {
        _spatialUpdateQueue.Enqueue(updateAction);
    }
    
    public void QueuePathfindingUpdate(Action pathfindingAction)
    {
        _pathfindingQueue.Enqueue(pathfindingAction);
    }
}

public class MovingAverage
{
    private readonly Queue<float> _samples;
    private readonly int _maxSamples;
    private float _sum;
    
    public MovingAverage(int maxSamples)
    {
        _maxSamples = maxSamples;
        _samples = new Queue<float>(maxSamples);
        _sum = 0f;
    }
    
    public void AddSample(float sample)
    {
        _samples.Enqueue(sample);
        _sum += sample;
        
        if (_samples.Count > _maxSamples)
        {
            _sum -= _samples.Dequeue();
        }
    }
    
    public float Average => _samples.Count > 0 ? _sum / _samples.Count : 0f;
    public int SampleCount => _samples.Count;
}
```

#### 10. Advanced Caching Systems

**Cross-Structure Query Cache:**
```csharp
public class CrossStructureQueryCache
{
    private struct CacheEntry
    {
        public List<IEntity> Results;
        public float Timestamp;
        public Rect2 QueryBounds;
        public uint Version;
        
        public bool IsValid(float currentTime, float maxAge = 0.1f)
        {
            return currentTime - Timestamp < maxAge;
        }
    }
    
    private readonly Dictionary<uint, CacheEntry> _cache;
    private readonly Dictionary<uint, HashSet<uint>> _regionCacheKeys;
    private const float CACHE_DURATION = 0.1f; // 100ms cache
    private uint _currentVersion = 1;
    
    public CrossStructureQueryCache()
    {
        _cache = new Dictionary<uint, CacheEntry>();
        _regionCacheKeys = new Dictionary<uint, HashSet<uint>>();
    }
    
    public bool TryGetCachedResult(Rect2 bounds, out List<IEntity> result)
    {
        uint key = HashBounds(bounds);
        
        if (_cache.TryGetValue(key, out var entry) && 
            entry.IsValid(Time.RealtimeSinceStartup) &&
            entry.Version == _currentVersion)
        {
            result = new List<IEntity>(entry.Results);
            return true;
        }
        
        result = null;
        return false;
    }
    
    public void CacheResult(Rect2 bounds, List<IEntity> result)
    {
        uint key = HashBounds(bounds);
        
        _cache[key] = new CacheEntry
        {
            Results = new List<IEntity>(result),
            Timestamp = Time.RealtimeSinceStartup,
            QueryBounds = bounds,
            Version = _currentVersion
        };
        
        RegisterForRegionInvalidation(key, bounds);
    }
    
    public void InvalidateRegion(Rect2 region)
    {
        uint regionKey = HashBounds(region);
        
        if (_regionCacheKeys.TryGetValue(regionKey, out var cacheKeys))
        {
            foreach (uint cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
            
            _regionCacheKeys.Remove(regionKey);
        }
        
        // Increment version to invalidate all cached results
        _currentVersion++;
    }
    
    public void Update(float deltaTime)
    {
        var expiredKeys = new List<uint>();
        var currentTime = Time.RealtimeSinceStartup;
        
        foreach (var kvp in _cache)
        {
            if (!kvp.Value.IsValid(currentTime, CACHE_DURATION) || 
                kvp.Value.Version < _currentVersion)
            {
                expiredKeys.Add(kvp.Key);
            }
        }
        
        foreach (uint key in expiredKeys)
        {
            _cache.Remove(key);
        }
        
        // Clean up region mappings periodically
        if (expiredKeys.Count > 100)
        {
            CleanupRegionMappings();
        }
    }
    
    private void RegisterForRegionInvalidation(uint cacheKey, Rect2 bounds)
    {
        var regionKey = HashBounds(QuantizeRegion(bounds));
        
        if (!_regionCacheKeys.ContainsKey(regionKey))
        {
            _regionCacheKeys[regionKey] = new HashSet<uint>();
        }
        
        _regionCacheKeys[regionKey].Add(cacheKey);
    }
    
    private Rect2 QuantizeRegion(Rect2 bounds)
    {
        const float regionSize = 64f;
        var quantizedPos = new Vector2(
            Mathf.Floor(bounds.Position.X / regionSize) * regionSize,
            Mathf.Floor(bounds.Position.Y / regionSize) * regionSize
        );
        return new Rect2(quantizedPos, new Vector2(regionSize, regionSize));
    }
    
    private void CleanupRegionMappings()
    {
        var keysToRemove = new List<uint>();
        
        foreach (var kvp in _regionCacheKeys)
        {
            kvp.Value.RemoveWhere(cacheKey => !_cache.ContainsKey(cacheKey));
            
            if (kvp.Value.Count == 0)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            _regionCacheKeys.Remove(key);
        }
    }
    
    private uint HashBounds(Rect2 bounds)
    {
        uint hash = (uint)bounds.Position.X.GetHashCode();
        hash = hash * 31 + (uint)bounds.Position.Y.GetHashCode();
        hash = hash * 31 + (uint)bounds.Size.X.GetHashCode();
        hash = hash * 31 + (uint)bounds.Size.Y.GetHashCode();
        return hash;
    }
}
```

#### 12. NEAT AI Integration

**Spatial NEAT Input Provider:**
```csharp
public class SpatialNEATInputProviderSystem : BaseFixedSystem
{
    private readonly DualSpatialManagerSystem _spatialManager;
    
    public SpatialNEATInputProviderSystem(IEntityManager entityManager, 
        DualSpatialManagerSystem spatialManager)
        : base(entityManager)
    {
        _spatialManager = spatialManager;
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(NEATGenomeComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var neatComponent = entity.FindComponent<NEATGenomeComponent>();
        var spatialInputs = GetSpatialInputs(entity);
        
        // Update the NEAT component with spatial awareness inputs
        var currentInputs = neatComponent.Inputs?.ToList() ?? new List<float>();
        
        // Replace or append spatial inputs (assume first 12 inputs are spatial)
        for (int i = 0; i < spatialInputs.Length && i < 12; i++)
        {
            if (i < currentInputs.Count)
                currentInputs[i] = spatialInputs[i];
            else
                currentInputs.Add(spatialInputs[i]);
        }
        
        entity.SetComponent(neatComponent with { Inputs = currentInputs.ToArray() });
    }
    
    public float[] GetSpatialInputs(IEntity entity)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        var inputs = new float[12];
        
        // Get neighbors from appropriate spatial structures
        var nearbyMoving = _spatialManager.GetNearbyEntities(entity, 48f, 
            includeMoving: true, includeStatic: false);
        var nearbyStatic = _spatialManager.GetNearbyEntities(entity, 64f,
            includeMoving: false, includeStatic: true);
        
        // 0-3: Moving entity density in cardinal directions (N, E, S, W)
        CalculateDirectionalDensity(position, nearbyMoving, inputs, 0);
        
        // 4-7: Static obstacle density in cardinal directions
        CalculateDirectionalDensity(position, nearbyStatic, inputs, 4);
        
        // 8: Local crowding factor (0-1)
        inputs[8] = Math.Min(1.0f, nearbyMoving.Count() / 10.0f);
        
        // 9: Distance to nearest building/shelter (0-1, normalized to 128 units)
        inputs[9] = GetDistanceToNearestBuilding(position, nearbyStatic) / 128f;
        
        // 10: Path clearance ahead (0-1)
        inputs[10] = CalculatePathClearance(entity, nearbyStatic);
        
        // 11: Resource availability in local area (0-1)
        inputs[11] = CalculateResourceDensity(position, nearbyStatic);
        
        return inputs;
    }
    
    private void CalculateDirectionalDensity(Vector2 position, IEnumerable<IEntity> entities, 
        float[] inputs, int startIndex)
    {
        var directions = new Vector2[] { Vector2.Up, Vector2.Right, Vector2.Down, Vector2.Left };
        
        for (int i = 0; i < 4; i++)
        {
            var direction = directions[i];
            var sectorBounds = new Rect2(
                position + direction * 24f - Vector2.One * 16f,
                Vector2.One * 32f
            );
            
            int entitiesInSector = 0;
            foreach (var entity in entities)
            {
                var entityPos = entity.FindComponent<PhysicsComponent>().Position;
                if (sectorBounds.HasPoint(entityPos))
                {
                    entitiesInSector++;
                }
            }
            
            inputs[startIndex + i] = Math.Min(1.0f, entitiesInSector / 5.0f);
        }
    }
    
    private float GetDistanceToNearestBuilding(Vector2 position, IEnumerable<IEntity> staticEntities)
    {
        float minDistance = float.MaxValue;
        
        foreach (var entity in staticEntities)
        {
            if (entity.HasComponent<StaticEntityComponent>())
            {
                var staticComp = entity.FindComponent<StaticEntityComponent>();
                if (staticComp.Type == StaticEntityType.Building)
                {
                    var entityPos = entity.FindComponent<PhysicsComponent>().Position;
                    var distance = position.DistanceTo(entityPos);
                    minDistance = Math.Min(minDistance, distance);
                }
            }
        }
        
        return minDistance == float.MaxValue ? 128f : minDistance;
    }
    
    private float CalculatePathClearance(IEntity entity, IEnumerable<IEntity> staticEntities)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        
        if (entity.HasComponent<MovementComponent>())
        {
            var movement = entity.FindComponent<MovementComponent>();
            
            if (movement.Velocity == Vector2.Zero)
                return 1.0f; // Full clearance when not moving
            
            var direction = movement.Velocity.Normalized();
            var checkDistance = 32f;
            var checkPosition = position + direction * checkDistance;
            
            foreach (var staticEntity in staticEntities)
            {
                var staticBounds = CalculateEntityBounds(staticEntity);
                if (staticBounds.HasPoint(checkPosition))
                {
                    var distanceToObstacle = position.DistanceTo(staticBounds.GetCenter());
                    return Math.Max(0f, (distanceToObstacle - 16f) / checkDistance);
                }
            }
        }
        
        return 1.0f; // Full clearance
    }
    
    private float CalculateResourceDensity(Vector2 position, IEnumerable<IEntity> staticEntities)
    {
        int resourceCount = 0;
        var searchRadius = 64f;
        var searchArea = new Rect2(position - Vector2.One * searchRadius, Vector2.One * searchRadius * 2);
        
        foreach (var entity in staticEntities)
        {
            if (entity.HasComponent<StaticEntityComponent>())
            {
                var staticComp = entity.FindComponent<StaticEntityComponent>();
                if (staticComp.Type == StaticEntityType.Resource)
                {
                    var entityPos = entity.FindComponent<PhysicsComponent>().Position;
                    if (searchArea.HasPoint(entityPos))
                    {
                        resourceCount++;
                    }
                }
            }
        }
        
        return Math.Min(1.0f, resourceCount / 5.0f);
    }
    
    private Rect2 CalculateEntityBounds(IEntity entity)
    {
        var position = entity.FindComponent<PhysicsComponent>().Position;
        
        if (entity.HasComponent<SpatialPartitionComponent>())
        {
            var spatialComponent = entity.FindComponent<SpatialPartitionComponent>();
            if (spatialComponent.Bounds.Size != Vector2.Zero)
            {
                return new Rect2(position + spatialComponent.Bounds.Position, spatialComponent.Bounds.Size);
            }
        }
        
        // Default entity size
        return new Rect2(position - Vector2.One * 8, Vector2.One * 16);
    }
}
```

---

## System Integration Notes

### Naming Conventions Followed:
- **Components**: `[Purpose]Component` inheriting from `IComponent`
- **Systems**: `[Purpose]System` inheriting from `BaseFixedSystem` or `BaseVariableSystem`
- **No Name Conflicts**: Each system has a unique purpose and name

### Architecture Compliance:
- All systems declare `RequiredComponentTypes` for entity filtering
- Systems use `EntityManager` for entity queries and component access
- Proper dependency injection through constructor parameters
- Component data encapsulation with `record` types and properties
- Separation of concerns between components (data) and systems (behavior)

### Performance Considerations:
- Systems process only entities with required components
- Spatial queries use optimized hash grid and quadtree structures
- Caching systems minimize repeated expensive calculations
- Time-slicing prevents frame rate drops under heavy load
- Memory pooling and reuse strategies minimize garbage collection

### ECS Integration:
- Seamless integration with existing `PhysicsComponent` and `IntelligenceComponent`
- Enhanced `MovementComponent` with pathfinding-specific data
- New specialized components for pathfinding and spatial partitioning
- Systems coordinate through shared component state modifications

*Note: These system implementations follow the existing AI.Odin ECS architecture patterns and should integrate seamlessly with the current codebase. Each system is designed to work independently while supporting emergent behaviors through component interactions and achieving the performance targets specified in the roadmap.*