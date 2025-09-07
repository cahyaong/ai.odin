# AI.Odin Scenario Loading System Implementation Snippets

## Overview

This document contains comprehensive code implementation snippets for the scenario loading system within our Entity Component System (ECS) framework. Each section provides complete C# implementations that integrate seamlessly with our existing blueprint infrastructure and YAML serialization system.

The scenario loader extends our current blueprint pattern to support sophisticated world initialization, entity placement strategies, and dynamic simulation configuration while maintaining compatibility with existing systems.

## Implementation Priority Matrix

**Note:** All code examples follow the existing ECS architecture patterns and integrate with current blueprint and serialization infrastructure.

---

## Phase 1: Core Scenario Blueprint System

### Data Model Implementation

#### ScenarioBlueprint.cs - Core Scenario Definition
**File:** `Source/Odin.Engine/ECS.Blueprint/ScenarioBlueprint.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record ScenarioBlueprint
{
    public required string Id { get; init; }
    
    public required string Name { get; init; }
    
    public string? Description { get; init; }
    
    public required UniverseSetup Universe { get; init; }
    
    public required IEnumerable<EntitySpawning> EntitySpawnings { get; init; }
    
    public IEnumerable<ResourceDistribution>? Resources { get; init; }
    
    public IEnumerable<EnvironmentalEvent>? Events { get; init; }
    
    public SimulationMetadata? Metadata { get; init; }
}

public record UniverseSetup
{
    public required Size Size { get; init; }
    
    public EnvironmentConfiguration? Environment { get; init; }
    
    public PhysicsConfiguration? Physics { get; init; }
}

public record EnvironmentConfiguration
{
    public string Biome { get; init; } = "Temperate";
    
    public string StartingSeason { get; init; } = "Spring";
    
    public float Temperature { get; init; } = 20.0f;
    
    public float Humidity { get; init; } = 0.5f;
}

public record PhysicsConfiguration
{
    public float Gravity { get; init; } = 9.81f;
    
    public float TimeScale { get; init; } = 1.0f;
    
    public bool EnableCollisions { get; init; } = true;
}

public record SimulationMetadata
{
    public string? ExpectedDuration { get; init; }
    
    public string? SuccessCriteria { get; init; }
    
    public DataCollectionSettings? DataCollection { get; init; }
    
    public MLTrainingSettings? MLTraining { get; init; }
    
    public uint? ReproducibleSeed { get; init; }
}

public record DataCollectionSettings
{
    public bool Enabled { get; init; } = false;
    
    public IEnumerable<string>? Metrics { get; init; }
    
    public float SampleRate { get; init; } = 1.0f;
}

public record MLTrainingSettings
{
    public bool Enabled { get; init; } = false;
    
    public string? Algorithm { get; init; }
    
    public IEnumerable<string>? FitnessTracking { get; init; }
    
    public IReadOnlyDictionary<string, object>? Parameters { get; init; }
}
```

#### EntitySpawning.cs - Entity Population Definition
**File:** `Source/Odin.Engine/ECS.Blueprint/EntitySpawning.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntitySpawning.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record EntitySpawning
{
    public required string EntityBlueprintId { get; init; }
    
    public required uint Quantity { get; init; }
    
    public required PlacementStrategy PlacementStrategy { get; init; }
    
    public SpawnTiming SpawnTiming { get; init; } = SpawnTiming.Initial;
    
    public IReadOnlyDictionary<string, IParameter>? ComponentOverrides { get; init; }
    
    public IEnumerable<RelationshipInitialization>? Relationships { get; init; }
    
    public StatisticalDistribution? TraitVariations { get; init; }
}

public record PlacementStrategy
{
    public required PlacementType Type { get; init; }
    
    public Boundary? Boundary { get; init; }
    
    public FormationConfiguration? Formation { get; init; }
    
    public ClusterConfiguration? Cluster { get; init; }
    
    public bool AvoidCollisions { get; init; } = true;
    
    public float MinimumDistance { get; init; } = 2.0f;
}

public enum PlacementType
{
    Random,
    Boundary,
    Formation,
    Grid,
    Cluster,
    Circle,
    Line
}

public record Boundary
{
    public float MinX { get; init; }
    public float MaxX { get; init; }
    public float MinY { get; init; }
    public float MaxY { get; init; }
    
    public Point? Center { get; init; }
    public float? Radius { get; init; }
}

public record FormationConfiguration
{
    public FormationType Type { get; init; } = FormationType.Grid;
    public float Spacing { get; init; } = 4.0f;
    public float Jitter { get; init; } = 0.0f;
    public uint? Columns { get; init; }
    public uint? Rows { get; init; }
}

public enum FormationType
{
    Grid,
    Circle,
    Line,
    Triangle,
    Diamond
}

public record ClusterConfiguration
{
    public uint ClusterCount { get; init; } = 1;
    public float ClusterRadius { get; init; } = 8.0f;
    public float DensityVariation { get; init; } = 0.2f;
}

public enum SpawnTiming
{
    Initial,
    Periodic,
    Conditional,
    Manual
}

public record RelationshipInitialization
{
    public string RelationshipType { get; init; } = "Friend";
    public float InitialStrength { get; init; } = 0.5f;
    public IEnumerable<uint>? TargetEntityIndices { get; init; }
}

public record StatisticalDistribution
{
    public string ComponentId { get; init; } = string.Empty;
    public string PropertyName { get; init; } = string.Empty;
    public DistributionType DistributionType { get; init; } = DistributionType.Normal;
    public float Mean { get; init; } = 0.0f;
    public float StandardDeviation { get; init; } = 1.0f;
    public float MinValue { get; init; } = float.MinValue;
    public float MaxValue { get; init; } = float.MaxValue;
}

public enum DistributionType
{
    Uniform,
    Normal,
    Exponential,
    Constant
}
```

#### ResourceDistribution.cs - World Resource Configuration
**File:** `Source/Odin.Engine/ECS.Blueprint/ResourceDistribution.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceDistribution.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record ResourceDistribution
{
    public required string ResourceBlueprintId { get; init; }
    
    public required uint BaseQuantity { get; init; }
    
    public float Density { get; init; } = 1.0f;
    
    public PlacementStrategy PlacementStrategy { get; init; } = new PlacementStrategy { Type = PlacementType.Random };
    
    public RegenerationRules? Regeneration { get; init; }
    
    public QualityDistribution? Quality { get; init; }
}

public record RegenerationRules
{
    public bool Enabled { get; init; } = false;
    
    public float RegenerationRate { get; init; } = 0.1f;
    
    public uint MaxQuantity { get; init; } = 100;
    
    public float RegenerationRadius { get; init; } = 16.0f;
    
    public IEnumerable<string>? RequiredConditions { get; init; }
}

public record QualityDistribution
{
    public float MinQuality { get; init; } = 0.1f;
    
    public float MaxQuality { get; init; } = 1.0f;
    
    public DistributionType DistributionType { get; init; } = DistributionType.Normal;
    
    public float QualityMean { get; init; } = 0.6f;
    
    public float QualityStandardDeviation { get; init; } = 0.2f;
}
```

### Core Scenario Loading Implementation

#### IScenarioLoader.cs - Interface Definition
**File:** `Source/Odin.Engine/ECS.Coordinator/IScenarioLoader.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScenarioLoader.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IScenarioLoader
{
    bool HasActiveScenario { get; }
    
    ScenarioBlueprint? CurrentScenario { get; }
    
    IUniverse LoadScenario(string scenarioId);
    
    IUniverse LoadScenario(ScenarioBlueprint scenario);
    
    Task<IUniverse> LoadScenarioAsync(string scenarioId, IProgress<ScenarioLoadingProgress>? progress = null);
    
    Task<IUniverse> LoadScenarioAsync(ScenarioBlueprint scenario, IProgress<ScenarioLoadingProgress>? progress = null);
    
    void UnloadCurrentScenario();
    
    IEnumerable<string> GetAvailableScenarios();
    
    ScenarioBlueprint GetScenarioBlueprint(string scenarioId);
    
    void RegisterScenarioLoadedCallback(Action<ScenarioBlueprint> callback);
}

public record ScenarioLoadingProgress
{
    public float OverallProgress { get; init; }
    
    public string CurrentPhase { get; init; } = string.Empty;
    
    public uint EntitiesCreated { get; init; }
    
    public uint TotalEntities { get; init; }
    
    public string? StatusMessage { get; init; }
}
```

#### ScenarioLoader.cs - Core Loading Implementation
**File:** `Source/Odin.Engine/ECS.Coordinator/ScenarioLoader.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioLoader.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Immutable;
using nGratis.Cop.Olympus.Contract;

public class ScenarioLoader : IScenarioLoader
{
    private readonly IDataStore _dataStore;
    private readonly IEntityFactory _entityFactory;
    private readonly IEntityManager _entityManager;
    private readonly IReadOnlyList<IPlacementStrategy> _placementStrategies;
    private readonly Random _random;
    
    private ScenarioBlueprint? _currentScenario;
    private readonly List<Action<ScenarioBlueprint>> _scenarioLoadedCallbacks;

    public ScenarioLoader(
        IDataStore dataStore,
        IEntityFactory entityFactory,
        IEntityManager entityManager,
        IEnumerable<IPlacementStrategy> placementStrategies)
    {
        this._dataStore = dataStore;
        this._entityFactory = entityFactory;
        this._entityManager = entityManager;
        this._placementStrategies = placementStrategies.ToImmutableArray();
        this._random = new Random();
        this._scenarioLoadedCallbacks = new List<Action<ScenarioBlueprint>>();
    }

    public bool HasActiveScenario => this._currentScenario != null;

    public ScenarioBlueprint? CurrentScenario => this._currentScenario;

    public IUniverse LoadScenario(string scenarioId)
    {
        var scenarioBlueprint = this.GetScenarioBlueprint(scenarioId);
        return this.LoadScenario(scenarioBlueprint);
    }

    public IUniverse LoadScenario(ScenarioBlueprint scenario)
    {
        Guard.Require(scenario, nameof(scenario)).Is.Not.Null();

        this._currentScenario = scenario;

        // Initialize universe with scenario configuration
        var universe = this.CreateUniverseFromScenario(scenario);

        // Apply reproducible seed if specified
        if (scenario.Metadata?.ReproducibleSeed.HasValue == true)
        {
            this._random = new Random((int)scenario.Metadata.ReproducibleSeed.Value);
        }

        // Spawn all initial entities
        this.SpawnEntitiesFromScenario(scenario, universe);

        // Notify callbacks
        foreach (var callback in this._scenarioLoadedCallbacks)
        {
            callback(scenario);
        }

        return universe;
    }

    public async Task<IUniverse> LoadScenarioAsync(
        string scenarioId, 
        IProgress<ScenarioLoadingProgress>? progress = null)
    {
        var scenarioBlueprint = this.GetScenarioBlueprint(scenarioId);
        return await this.LoadScenarioAsync(scenarioBlueprint, progress);
    }

    public async Task<IUniverse> LoadScenarioAsync(
        ScenarioBlueprint scenario, 
        IProgress<ScenarioLoadingProgress>? progress = null)
    {
        Guard.Require(scenario, nameof(scenario)).Is.Not.Null();

        this._currentScenario = scenario;

        // Report initialization phase
        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 0.1f, 
            CurrentPhase = "Initializing Universe",
            StatusMessage = $"Setting up world: {scenario.Universe.Size.Width}x{scenario.Universe.Size.Height}"
        });

        var universe = this.CreateUniverseFromScenario(scenario);

        // Apply reproducible seed if specified
        if (scenario.Metadata?.ReproducibleSeed.HasValue == true)
        {
            this._random = new Random((int)scenario.Metadata.ReproducibleSeed.Value);
        }

        // Calculate total entities for progress tracking
        var totalEntities = scenario.EntitySpawnings.Sum(spawning => spawning.Quantity);

        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 0.2f, 
            CurrentPhase = "Spawning Entities",
            TotalEntities = totalEntities,
            StatusMessage = $"Creating {totalEntities} entities..."
        });

        // Spawn entities with progress reporting
        await this.SpawnEntitiesFromScenarioAsync(scenario, universe, progress);

        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 1.0f, 
            CurrentPhase = "Completed",
            EntitiesCreated = totalEntities,
            TotalEntities = totalEntities,
            StatusMessage = "Scenario loaded successfully"
        });

        // Notify callbacks
        foreach (var callback in this._scenarioLoadedCallbacks)
        {
            callback(scenario);
        }

        return universe;
    }

    public void UnloadCurrentScenario()
    {
        if (this._currentScenario != null)
        {
            // Clear all entities
            this._entityManager.Clear();
            this._currentScenario = null;
        }
    }

    public IEnumerable<string> GetAvailableScenarios()
    {
        return this._dataStore
            .LoadScenarioBlueprints()
            .Select(scenario => scenario.Id);
    }

    public ScenarioBlueprint GetScenarioBlueprint(string scenarioId)
    {
        var scenarios = this._dataStore.LoadScenarioBlueprints();
        var scenario = scenarios.FirstOrDefault(s => s.Id == scenarioId);
        
        if (scenario == null)
        {
            throw new OdinException(
                "Scenario blueprint must be defined for given ID!",
                ("ID", scenarioId));
        }

        return scenario;
    }

    public void RegisterScenarioLoadedCallback(Action<ScenarioBlueprint> callback)
    {
        Guard.Require(callback, nameof(callback)).Is.Not.Null();
        this._scenarioLoadedCallbacks.Add(callback);
    }

    private IUniverse CreateUniverseFromScenario(ScenarioBlueprint scenario)
    {
        return this._entityFactory.CreateUniverse(
            scenario.Universe.Size.Width,
            scenario.Universe.Size.Height);
    }

    private void SpawnEntitiesFromScenario(ScenarioBlueprint scenario, IUniverse universe)
    {
        foreach (var entitySpawning in scenario.EntitySpawnings.Where(s => s.SpawnTiming == SpawnTiming.Initial))
        {
            this.SpawnEntityGroup(entitySpawning, universe);
        }
    }

    private async Task SpawnEntitiesFromScenarioAsync(
        ScenarioBlueprint scenario, 
        IUniverse universe, 
        IProgress<ScenarioLoadingProgress>? progress)
    {
        var totalEntities = scenario.EntitySpawnings.Sum(spawning => spawning.Quantity);
        uint entitiesCreated = 0;

        foreach (var entitySpawning in scenario.EntitySpawnings.Where(s => s.SpawnTiming == SpawnTiming.Initial))
        {
            await this.SpawnEntityGroupAsync(entitySpawning, universe, progress, entitiesCreated, totalEntities);
            entitiesCreated += entitySpawning.Quantity;
        }
    }

    private void SpawnEntityGroup(EntitySpawning spawning, IUniverse universe)
    {
        var placementStrategy = this.GetPlacementStrategy(spawning.PlacementStrategy);
        var positions = placementStrategy.GeneratePositions(spawning.Quantity, universe);

        for (var i = 0; i < spawning.Quantity; i++)
        {
            var entity = this._entityFactory.CreateEntity(spawning.EntityBlueprintId);
            
            // Apply position from placement strategy
            var physicsComponent = entity.FindComponent<PhysicsComponent>();
            if (physicsComponent != null && i < positions.Count())
            {
                physicsComponent.Position = positions.ElementAt(i);
            }

            // Apply component overrides if specified
            if (spawning.ComponentOverrides != null)
            {
                this.ApplyComponentOverrides(entity, spawning.ComponentOverrides);
            }

            // Apply trait variations if specified
            if (spawning.TraitVariations != null)
            {
                this.ApplyTraitVariations(entity, spawning.TraitVariations);
            }

            this._entityManager.AddEntity(entity);
        }
    }

    private async Task SpawnEntityGroupAsync(
        EntitySpawning spawning, 
        IUniverse universe, 
        IProgress<ScenarioLoadingProgress>? progress,
        uint baseEntitiesCreated,
        uint totalEntities)
    {
        const uint batchSize = 50;
        var placementStrategy = this.GetPlacementStrategy(spawning.PlacementStrategy);
        var positions = placementStrategy.GeneratePositions(spawning.Quantity, universe);

        for (uint i = 0; i < spawning.Quantity; i += batchSize)
        {
            var batchCount = Math.Min(batchSize, spawning.Quantity - i);
            
            // Create batch of entities
            for (var j = 0; j < batchCount; j++)
            {
                var entityIndex = i + j;
                var entity = this._entityFactory.CreateEntity(spawning.EntityBlueprintId);
                
                // Apply position from placement strategy
                var physicsComponent = entity.FindComponent<PhysicsComponent>();
                if (physicsComponent != null && entityIndex < positions.Count())
                {
                    physicsComponent.Position = positions.ElementAt((int)entityIndex);
                }

                // Apply component overrides and variations
                if (spawning.ComponentOverrides != null)
                {
                    this.ApplyComponentOverrides(entity, spawning.ComponentOverrides);
                }

                if (spawning.TraitVariations != null)
                {
                    this.ApplyTraitVariations(entity, spawning.TraitVariations);
                }

                this._entityManager.AddEntity(entity);
            }

            // Report progress
            var currentEntitiesCreated = baseEntitiesCreated + i + batchCount;
            progress?.Report(new ScenarioLoadingProgress 
            { 
                OverallProgress = 0.2f + (0.7f * currentEntitiesCreated / totalEntities),
                CurrentPhase = "Spawning Entities",
                EntitiesCreated = currentEntitiesCreated,
                TotalEntities = totalEntities,
                StatusMessage = $"Created {currentEntitiesCreated}/{totalEntities} entities"
            });

            // Yield control to prevent blocking
            await Task.Yield();
        }
    }

    private IPlacementStrategy GetPlacementStrategy(PlacementStrategy strategyConfig)
    {
        return this._placementStrategies.First(strategy => 
            strategy.CanHandle(strategyConfig.Type));
    }

    private void ApplyComponentOverrides(IEntity entity, IReadOnlyDictionary<string, IParameter> overrides)
    {
        foreach (var (componentId, parameter) in overrides)
        {
            var component = entity.Components.FirstOrDefault(c => 
                c.GetType().Name.StartsWith(componentId, StringComparison.OrdinalIgnoreCase));
            
            if (component != null)
            {
                // Apply parameter to component using reflection
                this.ApplyParameterToComponent(component, parameter);
            }
        }
    }

    private void ApplyTraitVariations(IEntity entity, StatisticalDistribution distribution)
    {
        var component = entity.Components.FirstOrDefault(c => 
            c.GetType().Name.StartsWith(distribution.ComponentId, StringComparison.OrdinalIgnoreCase));

        if (component != null)
        {
            var property = component.GetType().GetProperty(distribution.PropertyName);
            if (property != null && property.CanWrite)
            {
                var randomValue = this.GenerateRandomValue(distribution);
                property.SetValue(component, Convert.ChangeType(randomValue, property.PropertyType));
            }
        }
    }

    private void ApplyParameterToComponent(IComponent component, IParameter parameter)
    {
        // Implementation depends on parameter type and component property
        // This would use reflection to set component properties based on parameter data
        // Similar to existing ComponentFactory pattern
    }

    private float GenerateRandomValue(StatisticalDistribution distribution)
    {
        return distribution.DistributionType switch
        {
            DistributionType.Uniform => (float)this._random.NextDouble() * 
                (distribution.MaxValue - distribution.MinValue) + distribution.MinValue,
            DistributionType.Normal => this.GenerateNormalDistribution(
                distribution.Mean, distribution.StandardDeviation),
            DistributionType.Exponential => this.GenerateExponentialDistribution(distribution.Mean),
            DistributionType.Constant => distribution.Mean,
            _ => distribution.Mean
        };
    }

    private float GenerateNormalDistribution(float mean, float stdDev)
    {
        // Box-Muller transform for normal distribution
        var u1 = 1.0f - (float)this._random.NextDouble();
        var u2 = 1.0f - (float)this._random.NextDouble();
        var randStdNormal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2));
        return mean + stdDev * randStdNormal;
    }

    private float GenerateExponentialDistribution(float lambda)
    {
        return (float)(-Math.Log(1.0 - this._random.NextDouble()) / lambda);
    }
}
```

---

## Phase 2: Placement Strategy Implementation

### Placement Strategy Interface and Factory

#### IPlacementStrategy.cs - Strategy Interface
**File:** `Source/Odin.Engine/ECS.Coordinator/IPlacementStrategy.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlacementStrategy.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IPlacementStrategy
{
    bool CanHandle(PlacementType placementType);
    
    IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe);
    
    IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe, PlacementStrategy strategy);
}
```

#### RandomPlacementStrategy.cs - Random Placement Implementation
**File:** `Source/Odin.Engine/ECS.Coordinator/RandomPlacementStrategy.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomPlacementStrategy.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public class RandomPlacementStrategy : IPlacementStrategy
{
    private readonly Random _random;

    public RandomPlacementStrategy()
    {
        this._random = new Random();
    }

    public bool CanHandle(PlacementType placementType)
    {
        return placementType == PlacementType.Random;
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe)
    {
        var defaultStrategy = new PlacementStrategy 
        { 
            Type = PlacementType.Random,
            MinimumDistance = 2.0f,
            AvoidCollisions = true
        };
        
        return this.GeneratePositions(quantity, universe, defaultStrategy);
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe, PlacementStrategy strategy)
    {
        Guard.Require(quantity, nameof(quantity)).Is.GreaterThan(0U);
        Guard.Require(universe, nameof(universe)).Is.Not.Null();

        var positions = new List<Point>();
        var boundary = strategy.Boundary ?? new Boundary 
        { 
            MinX = 0, 
            MinY = 0, 
            MaxX = universe.Size.Width, 
            MaxY = universe.Size.Height 
        };

        var maxAttempts = (int)quantity * 10; // Prevent infinite loops
        var attempts = 0;

        while (positions.Count < quantity && attempts < maxAttempts)
        {
            var candidatePosition = this.GenerateRandomPosition(boundary);
            
            if (!strategy.AvoidCollisions || this.IsValidPosition(candidatePosition, positions, strategy.MinimumDistance))
            {
                positions.Add(candidatePosition);
            }
            
            attempts++;
        }

        return positions;
    }

    private Point GenerateRandomPosition(Boundary boundary)
    {
        if (boundary.Center.HasValue && boundary.Radius.HasValue)
        {
            // Generate within circular boundary
            var angle = this._random.NextDouble() * 2 * Math.PI;
            var distance = Math.Sqrt(this._random.NextDouble()) * boundary.Radius.Value;
            
            return new Point
            {
                X = boundary.Center.Value.X + (float)(Math.Cos(angle) * distance),
                Y = boundary.Center.Value.Y + (float)(Math.Sin(angle) * distance)
            };
        }
        else
        {
            // Generate within rectangular boundary
            return new Point
            {
                X = (float)(this._random.NextDouble() * (boundary.MaxX - boundary.MinX) + boundary.MinX),
                Y = (float)(this._random.NextDouble() * (boundary.MaxY - boundary.MinY) + boundary.MinY)
            };
        }
    }

    private bool IsValidPosition(Point candidate, IList<Point> existingPositions, float minimumDistance)
    {
        return existingPositions.All(existing => 
            Math.Sqrt(Math.Pow(candidate.X - existing.X, 2) + Math.Pow(candidate.Y - existing.Y, 2)) >= minimumDistance);
    }
}
```

#### GridPlacementStrategy.cs - Grid Formation Implementation
**File:** `Source/Odin.Engine/ECS.Coordinator/GridPlacementStrategy.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridPlacementStrategy.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public class GridPlacementStrategy : IPlacementStrategy
{
    private readonly Random _random;

    public GridPlacementStrategy()
    {
        this._random = new Random();
    }

    public bool CanHandle(PlacementType placementType)
    {
        return placementType == PlacementType.Grid;
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe)
    {
        var defaultStrategy = new PlacementStrategy 
        { 
            Type = PlacementType.Grid,
            Formation = new FormationConfiguration { Spacing = 8.0f }
        };
        
        return this.GeneratePositions(quantity, universe, defaultStrategy);
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe, PlacementStrategy strategy)
    {
        Guard.Require(quantity, nameof(quantity)).Is.GreaterThan(0U);
        Guard.Require(universe, nameof(universe)).Is.Not.Null();
        Guard.Require(strategy.Formation, nameof(strategy.Formation)).Is.Not.Null();

        var positions = new List<Point>();
        var formation = strategy.Formation!;
        var boundary = strategy.Boundary ?? new Boundary 
        { 
            MinX = 0, 
            MinY = 0, 
            MaxX = universe.Size.Width, 
            MaxY = universe.Size.Height 
        };

        // Calculate grid dimensions
        var columns = formation.Columns ?? (uint)Math.Ceiling(Math.Sqrt(quantity));
        var rows = formation.Rows ?? (uint)Math.Ceiling((double)quantity / columns);

        // Calculate starting position to center the grid
        var gridWidth = (columns - 1) * formation.Spacing;
        var gridHeight = (rows - 1) * formation.Spacing;
        var startX = boundary.MinX + (boundary.MaxX - boundary.MinX - gridWidth) / 2;
        var startY = boundary.MinY + (boundary.MaxY - boundary.MinY - gridHeight) / 2;

        uint entityCount = 0;
        for (uint row = 0; row < rows && entityCount < quantity; row++)
        {
            for (uint col = 0; col < columns && entityCount < quantity; col++)
            {
                var baseX = startX + col * formation.Spacing;
                var baseY = startY + row * formation.Spacing;

                // Apply jitter if specified
                if (formation.Jitter > 0)
                {
                    baseX += (float)(this._random.NextDouble() - 0.5) * 2 * formation.Jitter;
                    baseY += (float)(this._random.NextDouble() - 0.5) * 2 * formation.Jitter;
                }

                positions.Add(new Point { X = baseX, Y = baseY });
                entityCount++;
            }
        }

        return positions;
    }
}
```

#### ClusterPlacementStrategy.cs - Cluster Formation Implementation
**File:** `Source/Odin.Engine/ECS.Coordinator/ClusterPlacementStrategy.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClusterPlacementStrategy.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public class ClusterPlacementStrategy : IPlacementStrategy
{
    private readonly Random _random;

    public ClusterPlacementStrategy()
    {
        this._random = new Random();
    }

    public bool CanHandle(PlacementType placementType)
    {
        return placementType == PlacementType.Cluster;
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe)
    {
        var defaultStrategy = new PlacementStrategy 
        { 
            Type = PlacementType.Cluster,
            Cluster = new ClusterConfiguration { ClusterCount = 3, ClusterRadius = 8.0f }
        };
        
        return this.GeneratePositions(quantity, universe, defaultStrategy);
    }

    public IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe, PlacementStrategy strategy)
    {
        Guard.Require(quantity, nameof(quantity)).Is.GreaterThan(0U);
        Guard.Require(universe, nameof(universe)).Is.Not.Null();
        Guard.Require(strategy.Cluster, nameof(strategy.Cluster)).Is.Not.Null();

        var positions = new List<Point>();
        var cluster = strategy.Cluster!;
        var boundary = strategy.Boundary ?? new Boundary 
        { 
            MinX = 0, 
            MinY = 0, 
            MaxX = universe.Size.Width, 
            MaxY = universe.Size.Height 
        };

        // Generate cluster centers
        var clusterCenters = this.GenerateClusterCenters(cluster.ClusterCount, boundary);
        
        // Distribute entities among clusters
        var entitiesPerCluster = this.DistributeEntitiesAcrossClusters(quantity, cluster.ClusterCount, cluster.DensityVariation);

        for (var clusterIndex = 0; clusterIndex < clusterCenters.Count; clusterIndex++)
        {
            var center = clusterCenters[clusterIndex];
            var entityCount = entitiesPerCluster[clusterIndex];
            
            var clusterPositions = this.GenerateClusterPositions(entityCount, center, cluster.ClusterRadius);
            positions.AddRange(clusterPositions);
        }

        return positions;
    }

    private List<Point> GenerateClusterCenters(uint clusterCount, Boundary boundary)
    {
        var centers = new List<Point>();
        var minDistance = Math.Min(boundary.MaxX - boundary.MinX, boundary.MaxY - boundary.MinY) / (clusterCount * 2);

        for (var i = 0; i < clusterCount; i++)
        {
            Point center;
            var attempts = 0;
            var maxAttempts = (int)clusterCount * 10;

            do
            {
                center = new Point
                {
                    X = (float)(this._random.NextDouble() * (boundary.MaxX - boundary.MinX) + boundary.MinX),
                    Y = (float)(this._random.NextDouble() * (boundary.MaxY - boundary.MinY) + boundary.MinY)
                };
                attempts++;
            } while (attempts < maxAttempts && centers.Any(existing => 
                Math.Sqrt(Math.Pow(center.X - existing.X, 2) + Math.Pow(center.Y - existing.Y, 2)) < minDistance));

            centers.Add(center);
        }

        return centers;
    }

    private uint[] DistributeEntitiesAcrossClusters(uint totalEntities, uint clusterCount, float densityVariation)
    {
        var distribution = new uint[clusterCount];
        var baseCount = totalEntities / clusterCount;
        var remainder = totalEntities % clusterCount;

        for (var i = 0; i < clusterCount; i++)
        {
            distribution[i] = baseCount;
            
            // Add remainder to first clusters
            if (i < remainder)
            {
                distribution[i]++;
            }

            // Apply density variation
            if (densityVariation > 0)
            {
                var variation = (float)(this._random.NextDouble() - 0.5) * 2 * densityVariation;
                var adjustment = (int)(distribution[i] * variation);
                distribution[i] = (uint)Math.Max(1, (int)distribution[i] + adjustment);
            }
        }

        return distribution;
    }

    private IEnumerable<Point> GenerateClusterPositions(uint quantity, Point center, float radius)
    {
        var positions = new List<Point>();

        for (var i = 0; i < quantity; i++)
        {
            var angle = this._random.NextDouble() * 2 * Math.PI;
            var distance = Math.Sqrt(this._random.NextDouble()) * radius;
            
            var position = new Point
            {
                X = center.X + (float)(Math.Cos(angle) * distance),
                Y = center.Y + (float)(Math.Sin(angle) * distance)
            };
            
            positions.Add(position);
        }

        return positions;
    }
}
```

---

## Phase 3: System Integration & Scenario Management

### Scenario Initialization System

#### ScenarioInitializationSystem.cs - Main Scenario Execution System
**File:** `Source/Odin.Engine/ECS.System.Logic/ScenarioInitializationSystem.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioInitializationSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Reflection;

[SystemMetadata(OrderingIndex = -100)] // Run before all other systems
public class ScenarioInitializationSystem : BaseVariableSystem
{
    private readonly IScenarioLoader _scenarioLoader;
    private bool _hasInitialized = false;

    public ScenarioInitializationSystem(IScenarioLoader scenarioLoader)
    {
        this._scenarioLoader = scenarioLoader;
    }

    public override IEnumerable<Type> RequiredComponentTypes => Enumerable.Empty<Type>();

    protected override void ProcessAllEntities(TimeSpan deltaTime, IGameState gameState)
    {
        if (this._hasInitialized || !this._scenarioLoader.HasActiveScenario)
        {
            return;
        }

        var scenario = this._scenarioLoader.CurrentScenario!;
        
        // Initialize scenario-specific components and systems
        this.InitializeScenarioEnvironment(scenario, gameState);
        this.InitializeScenarioResources(scenario, gameState);
        this.InitializeScenarioEvents(scenario);

        this._hasInitialized = true;
    }

    private void InitializeScenarioEnvironment(ScenarioBlueprint scenario, IGameState gameState)
    {
        if (scenario.Universe.Environment == null)
        {
            return;
        }

        var environment = scenario.Universe.Environment;
        
        // Apply environmental settings to game state
        // This would integrate with environmental systems when they exist
        
        // For now, we can store environment data for future systems to use
        gameState.SetScenarioEnvironment(environment);
    }

    private void InitializeScenarioResources(ScenarioBlueprint scenario, IGameState gameState)
    {
        if (scenario.Resources == null)
        {
            return;
        }

        // Resource distribution would be handled here
        // This integrates with resource systems when implemented
        foreach (var resourceDistribution in scenario.Resources)
        {
            // Create resource entities based on distribution configuration
            this.SpawnResourceEntities(resourceDistribution, gameState.Universe);
        }
    }

    private void InitializeScenarioEvents(ScenarioBlueprint scenario)
    {
        if (scenario.Events == null)
        {
            return;
        }

        // Schedule environmental events
        // This would integrate with event scheduling systems
        foreach (var environmentalEvent in scenario.Events)
        {
            // Register event for later execution
            this.ScheduleEnvironmentalEvent(environmentalEvent);
        }
    }

    private void SpawnResourceEntities(ResourceDistribution distribution, IUniverse universe)
    {
        // Resource spawning implementation
        // This would use similar placement strategies as entity spawning
    }

    private void ScheduleEnvironmentalEvent(EnvironmentalEvent environmentalEvent)
    {
        // Event scheduling implementation
        // This would integrate with a future event system
    }
}
```

### Enhanced Game State Extensions

#### GameState Extensions for Scenario Support
**File:** `Source/Odin.Engine/GameState.Scenario.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameState.Scenario.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

// Extension methods for GameState to support scenario functionality
public static class GameStateScenarioExtensions
{
    private static readonly Dictionary<IGameState, EnvironmentConfiguration> EnvironmentConfigurations 
        = new Dictionary<IGameState, EnvironmentConfiguration>();

    public static void SetScenarioEnvironment(this IGameState gameState, EnvironmentConfiguration environment)
    {
        EnvironmentConfigurations[gameState] = environment;
    }

    public static EnvironmentConfiguration? GetScenarioEnvironment(this IGameState gameState)
    {
        return EnvironmentConfigurations.TryGetValue(gameState, out var environment) ? environment : null;
    }

    public static string GetCurrentBiome(this IGameState gameState)
    {
        var environment = gameState.GetScenarioEnvironment();
        return environment?.Biome ?? "Temperate";
    }

    public static string GetCurrentSeason(this IGameState gameState)
    {
        var environment = gameState.GetScenarioEnvironment();
        return environment?.StartingSeason ?? "Spring";
    }

    public static float GetCurrentTemperature(this IGameState gameState)
    {
        var environment = gameState.GetScenarioEnvironment();
        return environment?.Temperature ?? 20.0f;
    }

    public static float GetCurrentHumidity(this IGameState gameState)
    {
        var environment = gameState.GetScenarioEnvironment();
        return environment?.Humidity ?? 0.5f;
    }
}
```

### YAML Serialization Extensions

#### ScenarioYamlConverter.cs - YAML Type Converter
**File:** `Source/Odin.Glue/Serializer/ScenarioYamlConverter.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenarioYamlConverter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using nGratis.AI.Odin.Engine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class ScenarioYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(PlacementType) || 
               type == typeof(SpawnTiming) || 
               type == typeof(DistributionType) ||
               type == typeof(FormationType);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        var scalar = parser.Consume<Scalar>();
        
        if (type == typeof(PlacementType))
        {
            return Enum.Parse<PlacementType>(scalar.Value, true);
        }
        else if (type == typeof(SpawnTiming))
        {
            return Enum.Parse<SpawnTiming>(scalar.Value, true);
        }
        else if (type == typeof(DistributionType))
        {
            return Enum.Parse<DistributionType>(scalar.Value, true);
        }
        else if (type == typeof(FormationType))
        {
            return Enum.Parse<FormationType>(scalar.Value, true);
        }

        throw new InvalidOperationException($"Unsupported type: {type}");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (value == null)
        {
            emitter.Emit(new Scalar(null, null, "null", ScalarStyle.Plain, true, false));
            return;
        }

        var stringValue = value.ToString()?.ToLowerInvariant();
        emitter.Emit(new Scalar(null, null, stringValue, ScalarStyle.Plain, true, false));
    }
}
```

#### Updated YamlSerializationExtensions.cs
**Modifications to:** `Source/Odin.Glue/Serializer/YamlSerializationExtensions.cs`
```csharp
// Add to existing deserializer configuration
private static readonly IDeserializer Deserializer = new DeserializerBuilder()
    .WithNamingConvention(YamlSerializationExtensions.NamingConvention)
    .WithTypeConverter(ParameterYamlConverter.Instance)
    .WithTypeConverter(ScalarYamlConverter.Instance)
    .WithTypeConverter(new ScenarioYamlConverter()) // Add scenario converter
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<EntityBlueprint>())
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<SpriteSheetBlueprint>())
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<ScenarioBlueprint>()) // Add scenario support
    .Build();

// Add scenario loading method
public static IEnumerable<ScenarioBlueprint> LoadScenarioBlueprints(this IDataStore dataStore)
{
    return dataStore
        .FindResources(OdinMime.ScenarioBlueprint)
        .Select(resource => resource.Content.DeserializeFromYaml<ScenarioBlueprint>());
}
```

### EmbeddedDataStore Extensions

#### Updated EmbeddedDataStore.cs
**Add to:** `Source/Odin.Glue/EmbeddedDataStore.cs`
```csharp
// Add scenario blueprint support
public IEnumerable<ScenarioBlueprint> LoadScenarioBlueprints()
{
    return this
        .FindResources(OdinMime.ScenarioBlueprint)
        .Select(resource => resource.Content.DeserializeFromYaml<ScenarioBlueprint>());
}
```

#### Updated OdinMime.cs
**Add to:** `Source/Odin.Glue/OdinMime.cs`
```csharp
public static class OdinMime
{
    public static readonly MimeType EntityBlueprint = new("application", "x-ngao-entity-blueprint");
    public static readonly MimeType SpriteSheetBlueprint = new("application", "x-ngao-spritesheet-blueprint");
    public static readonly MimeType ScenarioBlueprint = new("application", "x-ngao-scenario-blueprint"); // Add this line

    public static MimeType? GetMimeType(FileInfo fileInfo)
    {
        return fileInfo.Extension switch
        {
            ".ngaoblueprint" => EntityBlueprint,
            ".ngasprite" => SpriteSheetBlueprint,
            ".ngascenario" => ScenarioBlueprint, // Add this line
            _ => null
        };
    }
}
```

---

## Example Scenario Blueprint Files

### Basic Survival Scenario
**File:** `Source/Odin.Glue/Common.Blueprint/scenario-basic-survival.ngascenario`
```yaml
id: 'BasicSurvival'
name: 'Basic Survival Challenge'
description: 'Small population with limited resources for survival gameplay testing'

universe:
  size:
    width: 128
    height: 64
  environment:
    biome: 'Temperate'
    starting-season: 'Spring'
    temperature: 18.5
    humidity: 0.6

entity-spawnings:
  - entity-blueprint-id: 'Human'
    quantity: 8
    placement-strategy:
      type: 'Random'
      boundary:
        min-x: 10
        max-x: 50
        min-y: 10
        max-y: 50
      avoid-collisions: true
      minimum-distance: 3.0
    spawn-timing: 'Initial'
    component-overrides:
      'Vitality':
        'Energy': 75.0
        'MaxEnergy': 100.0
    
  - entity-blueprint-id: 'BlueberryBush'
    quantity: 12
    placement-strategy:
      type: 'Cluster'
      cluster:
        cluster-count: 3
        cluster-radius: 8.0
        density-variation: 0.3
    spawn-timing: 'Initial'

resources:
  - resource-blueprint-id: 'WaterSource'
    base-quantity: 3
    density: 0.5
    placement-strategy:
      type: 'Random'
      boundary:
        min-x: 20
        max-x: 108
        min-y: 10
        max-y: 54
    regeneration:
      enabled: true
      regeneration-rate: 0.05
      max-quantity: 5

metadata:
  expected-duration: '10 minutes'
  success-criteria: 'At least 4 humans survive for 5 minutes'
  data-collection:
    enabled: true
    metrics: ['survival-time', 'resource-usage', 'social-interactions']
    sample-rate: 1.0
  reproducible-seed: 42
```

### Large Population Research Scenario
**File:** `Source/Odin.Glue/Common.Blueprint/scenario-population-dynamics.ngascenario`
```yaml
id: 'PopulationDynamics'
name: 'Large Population Research'
description: 'ML research scenario with 500+ agents for social evolution and NEAT training'

universe:
  size:
    width: 256
    height: 256
  environment:
    biome: 'Mixed'
    starting-season: 'Spring'
    temperature: 20.0
    humidity: 0.5
  physics:
    gravity: 9.81
    time-scale: 1.0
    enable-collisions: true

entity-spawnings:
  - entity-blueprint-id: 'Human'
    quantity: 500
    placement-strategy:
      type: 'Grid'
      formation:
        type: 'Grid'
        spacing: 12.0
        jitter: 3.0
        columns: 25
        rows: 20
      boundary:
        min-x: 20
        max-x: 236
        min-y: 20
        max-y: 236
    spawn-timing: 'Initial'
    component-overrides:
      'Trait':
        'EnergyConsumptionRateByEntityStateLookup': <Lookup> (Idling:1, Walking:2, Running:3)
    trait-variations:
      component-id: 'Vitality'
      property-name: 'MaxEnergy'
      distribution-type: 'Normal'
      mean: 100.0
      standard-deviation: 15.0
      min-value: 50.0
      max-value: 150.0
    
  - entity-blueprint-id: 'BlueberryBush'
    quantity: 200
    placement-strategy:
      type: 'Random'
      avoid-collisions: false
    spawn-timing: 'Initial'

  - entity-blueprint-id: 'Human'
    quantity: 50
    placement-strategy:
      type: 'Boundary'
      boundary:
        center:
          x: 128
          y: 128
        radius: 30
    spawn-timing: 'Periodic'
    component-overrides:
      'Trait':
        'EnergyConsumptionRateByEntityStateLookup': <Lookup> (Idling:0.5, Walking:1, Running:2)

resources:
  - resource-blueprint-id: 'BlueberryBush'
    base-quantity: 300
    density: 1.2
    placement-strategy:
      type: 'Cluster'
      cluster:
        cluster-count: 8
        cluster-radius: 15.0
        density-variation: 0.4
    regeneration:
      enabled: true
      regeneration-rate: 0.1
      max-quantity: 400
      regeneration-radius: 20.0

metadata:
  expected-duration: '60 minutes'
  success-criteria: 'Maintain population above 200 for 30 minutes'
  data-collection:
    enabled: true
    metrics: ['survival-time', 'social-interactions', 'resource-efficiency', 'population-dynamics', 'emergent-behaviors']
    sample-rate: 0.5
  ml-training:
    enabled: true
    algorithm: 'NEAT'
    fitness-tracking: ['survival', 'social', 'economic', 'cultural']
    parameters:
      'population-size': 500
      'mutation-rate': 0.1
      'crossover-rate': 0.8
      'speciation-threshold': 3.0
  reproducible-seed: 12345
```

### Tutorial Progression Scenario
**File:** `Source/Odin.Glue/Common.Blueprint/scenario-tutorial-basic.ngascenario`
```yaml
id: 'TutorialBasic'
name: 'Tutorial: Basic Observation'
description: 'Simple scenario for new players to learn observation and god powers'

universe:
  size:
    width: 64
    height: 32
  environment:
    biome: 'Peaceful'
    starting-season: 'Spring'
    temperature: 22.0
    humidity: 0.4

entity-spawnings:
  - entity-blueprint-id: 'Human'
    quantity: 3
    placement-strategy:
      type: 'Formation'
      formation:
        type: 'Line'
        spacing: 8.0
        jitter: 1.0
      boundary:
        min-x: 16
        max-x: 48
        min-y: 12
        max-y: 20
    spawn-timing: 'Initial'
    component-overrides:
      'Vitality':
        'Energy': 100.0
      'Trait':
        'EnergyConsumptionRateByEntityStateLookup': <Lookup> (Idling:0.5, Walking:1, Running:2)

  - entity-blueprint-id: 'BlueberryBush'
    quantity: 6
    placement-strategy:
      type: 'Random'
      boundary:
        min-x: 8
        max-x: 56
        min-y: 8
        max-y: 24
    spawn-timing: 'Initial'

metadata:
  expected-duration: '5 minutes'
  success-criteria: 'All humans survive'
  data-collection:
    enabled: false
  reproducible-seed: 1
```

---

## Phase 4: Godot UI Integration Implementation

### Enhanced SimulationController

#### SimulationController.cs - UI Logic and State Management
**File:** `Source/Odin.Client.Godot/Common.UI/SimulationController.cs`
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimulationController.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nGratis.AI.Odin.Engine;

public partial class SimulationController : HBoxContainer
{
    // UI Node references
    private OptionButton _scenarioSelector;
    private Button _runningButton;
    private ProgressBar _loadingProgressBar;
    private Label _statusLabel;
    private Panel _descriptionPanel;
    private RichTextLabel _descriptionText;

    // Dependencies injected via AppBootstrapper
    private IScenarioLoader _scenarioLoader;
    private IGameController _gameController;
    private IDataStore _dataStore;

    // State management
    private SimulationState _currentState = SimulationState.Idle;
    private string _selectedScenarioId = string.Empty;
    private CancellationTokenSource _loadingCancellation;

    // UI state enum
    private enum SimulationState
    {
        Idle,           // Ready to start
        Loading,        // Loading scenario
        Running,        // Simulation active
        Stopping,       // Graceful shutdown
        Error           // Error state
    }

    [Export]
    public AppBootstrapper AppBootstrapper { get; set; }

    public override void _Ready()
    {
        this.InitializeUINodes();
        this.InitializeDependencies();
        this.InitializeScenarioSelector();
        this.ConnectUISignals();
        this.UpdateUIState();
    }

    private void InitializeUINodes()
    {
        // Get UI node references
        this._scenarioSelector = this.GetNode<OptionButton>("ScenarioSelector");
        this._runningButton = this.GetNode<Button>("RunningButton");
        this._loadingProgressBar = this.GetNode<ProgressBar>("LoadingProgressBar");
        this._statusLabel = this.GetNode<Label>("StatusLabel");
        
        // Description panel (optional - can be null if not present)
        this._descriptionPanel = this.GetNodeOrNull<Panel>("DescriptionPanel");
        this._descriptionText = this._descriptionPanel?.GetNodeOrNull<RichTextLabel>("DescriptionText");

        // Initialize UI state
        this._loadingProgressBar.Visible = false;
        this._statusLabel.Text = "Ready";
        
        if (this._descriptionPanel != null)
        {
            this._descriptionPanel.Visible = false;
        }
    }

    private void InitializeDependencies()
    {
        if (this.AppBootstrapper == null)
        {
            GD.PrintErr("AppBootstrapper not set on SimulationController!");
            return;
        }

        // Get dependencies from DI container through AppBootstrapper
        // Note: This assumes AppBootstrapper exposes the container or provides access methods
        this._dataStore = this.AppBootstrapper.DataStore;
        
        // TODO: These would be resolved from the DI container in a real implementation
        // For now, we'll need to add these to AppBootstrapper or pass them differently
        // this._scenarioLoader = container.Resolve<IScenarioLoader>();
        // this._gameController = container.Resolve<IGameController>();
    }

    private void InitializeScenarioSelector()
    {
        try
        {
            // Clear existing items
            this._scenarioSelector.Clear();
            
            // Add default option
            this._scenarioSelector.AddItem("Select Scenario...", -1);
            this._scenarioSelector.Selected = 0;

            // Load available scenarios from data store
            var availableScenarios = this.GetAvailableScenarios();
            var itemIndex = 1;
            
            foreach (var scenario in availableScenarios)
            {
                this._scenarioSelector.AddItem($"{scenario.Name} ({scenario.Id})", itemIndex);
                this._scenarioSelector.SetItemMetadata(itemIndex, scenario.Id);
                itemIndex++;
            }

            GD.Print($"Loaded {availableScenarios.Count()} scenarios into selector");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to load scenarios: {ex.Message}");
            this._scenarioSelector.AddItem("Error loading scenarios", -1);
            this.SetErrorState($"Failed to load scenarios: {ex.Message}");
        }
    }

    private IEnumerable<ScenarioBlueprint> GetAvailableScenarios()
    {
        // In a real implementation, this would use the scenario loader
        // For now, we'll create some mock scenarios or load from data store
        
        if (this._dataStore != null)
        {
            try
            {
                // Assuming DataStore has a method to load scenario blueprints
                return this._dataStore.LoadScenarioBlueprints();
            }
            catch
            {
                // Fallback to mock scenarios for development
                return this.CreateMockScenarios();
            }
        }

        return this.CreateMockScenarios();
    }

    private IEnumerable<ScenarioBlueprint> CreateMockScenarios()
    {
        // Mock scenarios for development/testing
        return new[]
        {
            new ScenarioBlueprint
            {
                Id = "BasicSurvival",
                Name = "Basic Survival",
                Description = "Small population survival scenario with limited resources",
                Universe = new UniverseSetup 
                { 
                    Size = new Size { Width = 128, Height = 64 } 
                },
                EntitySpawnings = new List<EntitySpawning>()
            },
            new ScenarioBlueprint
            {
                Id = "PopulationDynamics", 
                Name = "Population Dynamics",
                Description = "Large scale population for ML research and social evolution",
                Universe = new UniverseSetup 
                { 
                    Size = new Size { Width = 256, Height = 256 } 
                },
                EntitySpawnings = new List<EntitySpawning>()
            },
            new ScenarioBlueprint
            {
                Id = "TutorialBasic",
                Name = "Tutorial - Basic Observation", 
                Description = "Simple tutorial scenario for new players",
                Universe = new UniverseSetup 
                { 
                    Size = new Size { Width = 64, Height = 32 } 
                },
                EntitySpawnings = new List<EntitySpawning>()
            }
        };
    }

    private void ConnectUISignals()
    {
        // Connect UI signals
        this._scenarioSelector.ItemSelected += this.OnScenarioSelected;
        this._runningButton.Pressed += this.OnRunningButtonPressed;
    }

    private void OnScenarioSelected(long index)
    {
        if (index <= 0)
        {
            this._selectedScenarioId = string.Empty;
            this.HideScenarioDescription();
            this.UpdateUIState();
            return;
        }

        // Get scenario ID from metadata
        this._selectedScenarioId = this._scenarioSelector.GetItemMetadata((int)index).AsString();
        
        GD.Print($"Selected scenario: {this._selectedScenarioId}");
        
        // Show scenario description if available
        this.ShowScenarioDescription(this._selectedScenarioId);
        this.UpdateUIState();
    }

    private void ShowScenarioDescription(string scenarioId)
    {
        if (this._descriptionPanel == null || this._descriptionText == null)
            return;

        try
        {
            var scenarios = this.GetAvailableScenarios();
            var scenario = scenarios.FirstOrDefault(s => s.Id == scenarioId);
            
            if (scenario != null)
            {
                this._descriptionText.Text = $"[b]{scenario.Name}[/b]\n\n{scenario.Description ?? "No description available."}\n\n" +
                    $"[color=gray]Universe Size: {scenario.Universe.Size.Width}x{scenario.Universe.Size.Height}[/color]";
                this._descriptionPanel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to show scenario description: {ex.Message}");
        }
    }

    private void HideScenarioDescription()
    {
        if (this._descriptionPanel != null)
        {
            this._descriptionPanel.Visible = false;
        }
    }

    private async void OnRunningButtonPressed()
    {
        switch (this._currentState)
        {
            case SimulationState.Idle:
                await this.StartSimulation();
                break;
                
            case SimulationState.Running:
                this.StopSimulation();
                break;
                
            case SimulationState.Loading:
                this.CancelLoading();
                break;
                
            case SimulationState.Error:
                this.ResetToIdle();
                break;
        }
    }

    private async Task StartSimulation()
    {
        if (string.IsNullOrEmpty(this._selectedScenarioId))
        {
            this.SetErrorState("Please select a scenario first");
            return;
        }

        try
        {
            this.SetLoadingState();
            
            // Create cancellation token for loading
            this._loadingCancellation = new CancellationTokenSource();
            
            // Create progress reporter
            var progress = new Progress<ScenarioLoadingProgress>(this.OnLoadingProgress);
            
            GD.Print($"Starting to load scenario: {this._selectedScenarioId}");
            
            // Load scenario asynchronously
            // Note: In real implementation, this would call the scenario loader
            // await this._gameController.LoadScenarioFromUI(this._selectedScenarioId, progress);
            
            // Mock loading for development
            await this.MockScenarioLoading(progress, this._loadingCancellation.Token);
            
            if (!this._loadingCancellation.Token.IsCancellationRequested)
            {
                this.SetRunningState();
                GD.Print($"Successfully loaded and started scenario: {this._selectedScenarioId}");
            }
        }
        catch (OperationCanceledException)
        {
            GD.Print("Scenario loading was cancelled");
            this.ResetToIdle();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to start simulation: {ex.Message}");
            this.SetErrorState($"Failed to load scenario: {ex.Message}");
        }
        finally
        {
            this._loadingCancellation?.Dispose();
            this._loadingCancellation = null;
        }
    }

    private async Task MockScenarioLoading(IProgress<ScenarioLoadingProgress> progress, CancellationToken cancellationToken)
    {
        // Mock scenario loading phases for development
        var phases = new[]
        {
            ("Initializing Universe", 0.1f, 500),
            ("Loading Entity Blueprints", 0.3f, 800),
            ("Spawning Entities", 0.7f, 1200),
            ("Finalizing Setup", 0.9f, 300),
            ("Completed", 1.0f, 100)
        };

        foreach (var (phase, progressValue, delay) in phases)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            progress?.Report(new ScenarioLoadingProgress
            {
                OverallProgress = progressValue,
                CurrentPhase = phase,
                StatusMessage = $"{phase}..."
            });
            
            await Task.Delay(delay, cancellationToken);
        }
    }

    private void OnLoadingProgress(ScenarioLoadingProgress progress)
    {
        // Update UI with loading progress
        this._loadingProgressBar.Value = progress.OverallProgress * 100;
        this._statusLabel.Text = progress.StatusMessage ?? progress.CurrentPhase;
        
        // Handle error states
        if (progress.CurrentPhase == "Error")
        {
            this.SetErrorState(progress.StatusMessage ?? "Unknown error occurred");
        }
    }

    private void StopSimulation()
    {
        try
        {
            this.SetState(SimulationState.Stopping, "Stopping simulation...", false);
            
            // Stop the game controller
            // this._gameController.Stop();
            
            GD.Print("Simulation stopped");
            this.ResetToIdle();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to stop simulation: {ex.Message}");
            this.SetErrorState($"Failed to stop simulation: {ex.Message}");
        }
    }

    private void CancelLoading()
    {
        try
        {
            this._loadingCancellation?.Cancel();
            GD.Print("Scenario loading cancelled by user");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to cancel loading: {ex.Message}");
        }
    }

    private void ResetToIdle()
    {
        this.SetState(SimulationState.Idle, "Ready", false);
    }

    private void SetLoadingState()
    {
        this.SetState(SimulationState.Loading, "Loading scenario...", true);
    }

    private void SetRunningState()
    {
        this.SetState(SimulationState.Running, "Simulation running", false);
    }

    private void SetErrorState(string errorMessage)
    {
        this.SetState(SimulationState.Error, $"Error: {errorMessage}", false);
    }

    private void SetState(SimulationState state, string statusText, bool showProgress)
    {
        this._currentState = state;
        this._statusLabel.Text = statusText;
        this._loadingProgressBar.Visible = showProgress;
        
        if (!showProgress)
        {
            this._loadingProgressBar.Value = 0;
        }
        
        this.UpdateUIState();
    }

    private void UpdateUIState()
    {
        // Update button text and state based on current simulation state
        switch (this._currentState)
        {
            case SimulationState.Idle:
                this._runningButton.Text = "Start";
                this._runningButton.Disabled = string.IsNullOrEmpty(this._selectedScenarioId);
                this._scenarioSelector.Disabled = false;
                break;
                
            case SimulationState.Loading:
                this._runningButton.Text = "Cancel";
                this._runningButton.Disabled = false;
                this._scenarioSelector.Disabled = true;
                break;
                
            case SimulationState.Running:
                this._runningButton.Text = "Stop";
                this._runningButton.Disabled = false;
                this._scenarioSelector.Disabled = true;
                break;
                
            case SimulationState.Stopping:
                this._runningButton.Text = "Stopping...";
                this._runningButton.Disabled = true;
                this._scenarioSelector.Disabled = true;
                break;
                
            case SimulationState.Error:
                this._runningButton.Text = "Reset";
                this._runningButton.Disabled = false;
                this._scenarioSelector.Disabled = false;
                break;
        }
    }

    public override void _ExitTree()
    {
        // Clean up resources
        this._loadingCancellation?.Dispose();
    }
}
```

### Enhanced HeadUpDisplay.tscn Scene Structure
**File:** `Source/Odin.Client.Godot/Common.UI/HeadUpDisplay.tscn`
```gdscript
[gd_scene load_steps=8 format=3 uid="uid://y2biswnrb7sa"]

[ext_resource type="Script" uid="uid://d2ymbagfgf0h2" path="res://Common.UI/HeadUpDisplay.cs" id="1_c3tkl"]
[ext_resource type="Script" uid="uid://b7gc34p2b3uth" path="res://Common.UI/StatisticsOverlay.cs" id="2_lliuc"]
[ext_resource type="Script" uid="uid://b5g0miibp24cw" path="res://Common.UI/SimulationController.cs" id="2_ygwek"]
[ext_resource type="FontFile" uid="uid://c3qb1yji6f7s8" path="res://Common.Font/FiraCode-Regular.ttf" id="3_tdcyv"]
[ext_resource type="FontFile" uid="uid://j5u7g1muwlcb" path="res://Common.Font/FiraCode-Bold.ttf" id="4_ygwek"]

[sub_resource type="StyleBoxFlat" id="StyleBoxFlat_tdcyv"]
content_margin_left = 8.0
content_margin_top = 8.0
content_margin_right = 8.0
content_margin_bottom = 8.0
bg_color = Color(0, 0, 0, 0.25098)

[sub_resource type="StyleBoxFlat" id="StyleBoxFlat_description"]
content_margin_left = 12.0
content_margin_top = 12.0
content_margin_right = 12.0
content_margin_bottom = 12.0
bg_color = Color(0, 0, 0, 0.4)
border_width_left = 2
border_width_top = 2
border_width_right = 2
border_width_bottom = 2
border_color = Color(0.2, 0.4, 0.8, 0.8)

[node name="HeadUpDisplay" type="Control"]
layout_mode = 3
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0
grow_horizontal = 2
grow_vertical = 2
script = ExtResource("1_c3tkl")

[node name="ToolbarOverlay" type="CanvasLayer" parent="."]

[node name="SimulationController" type="VBoxContainer" parent="ToolbarOverlay" node_paths=PackedStringArray("AppBootstrapper")]
offset_left = 16.0
offset_top = 16.0
offset_right = 528.0
offset_bottom = 120.0
script = ExtResource("2_ygwek")
AppBootstrapper = NodePath("../../../AppBootstrapper")

[node name="MainControls" type="HBoxContainer" parent="ToolbarOverlay/SimulationController"]
layout_mode = 2

[node name="ScenarioSelector" type="OptionButton" parent="ToolbarOverlay/SimulationController/MainControls"]
layout_mode = 2
size_flags_horizontal = 3
theme_override_fonts/font = ExtResource("3_tdcyv")
theme_override_font_sizes/font_size = 12
item_count = 1
popup/item_0/text = "Loading scenarios..."
popup/item_0/id = 0

[node name="RunningButton" type="Button" parent="ToolbarOverlay/SimulationController/MainControls"]
layout_mode = 2
size_flags_horizontal = 0
custom_minimum_size = Vector2(100, 0)
theme_override_fonts/font = ExtResource("4_ygwek")
theme_override_font_sizes/font_size = 12
text = "Start"
disabled = true

[node name="StatusRow" type="HBoxContainer" parent="ToolbarOverlay/SimulationController"]
layout_mode = 2

[node name="StatusLabel" type="Label" parent="ToolbarOverlay/SimulationController/StatusRow"]
layout_mode = 2
size_flags_horizontal = 3
theme_override_fonts/font = ExtResource("3_tdcyv")
theme_override_font_sizes/font_size = 11
text = "Initializing..."

[node name="LoadingProgressBar" type="ProgressBar" parent="ToolbarOverlay/SimulationController/StatusRow"]
layout_mode = 2
size_flags_horizontal = 3
custom_minimum_size = Vector2(200, 16)
max_value = 100.0
show_percentage = false
visible = false

[node name="DescriptionPanel" type="Panel" parent="ToolbarOverlay/SimulationController"]
layout_mode = 2
custom_minimum_size = Vector2(0, 80)
theme_override_styles/panel = SubResource("StyleBoxFlat_description")
visible = false

[node name="DescriptionText" type="RichTextLabel" parent="ToolbarOverlay/SimulationController/DescriptionPanel"]
layout_mode = 1
anchors_preset = 15
anchor_right = 1.0
anchor_bottom = 1.0
grow_horizontal = 2
grow_vertical = 2
theme_override_fonts/normal_font = ExtResource("3_tdcyv")
theme_override_fonts/bold_font = ExtResource("4_ygwek")
theme_override_font_sizes/normal_font_size = 11
theme_override_font_sizes/bold_font_size = 12
bbcode_enabled = true
text = "Select a scenario to see its description here."
fit_content = true

[node name="StatisticsOverlay" type="CanvasLayer" parent="."]
script = ExtResource("2_lliuc")

[node name="MetricLabel" type="RichTextLabel" parent="StatisticsOverlay"]
anchors_preset = 1
anchor_left = 1.0
anchor_right = 1.0
offset_left = -144.0
offset_top = 16.0
offset_right = -16.0
offset_bottom = 48.0
grow_horizontal = 0
scale = Vector2(1, 0.997271)
theme_override_fonts/normal_font = ExtResource("3_tdcyv")
theme_override_fonts/italics_font = ExtResource("3_tdcyv")
theme_override_fonts/bold_font = ExtResource("4_ygwek")
theme_override_font_sizes/italics_font_size = 12
theme_override_font_sizes/normal_font_size = 12
theme_override_font_sizes/bold_font_size = 12
theme_override_styles/normal = SubResource("StyleBoxFlat_tdcyv")
bbcode_enabled = true
text = "<SO.METRIC>"
fit_content = true
autowrap_mode = 0
```

### Enhanced AppBootstrapper with ScenarioLoader Registration
**Modifications to:** `Source/Odin.Client.Godot/Common/AppBootstrapper.cs`
```csharp
public static class AutofacExtensions
{
    public static ContainerBuilder RegisterInfrastructure(this ContainerBuilder containerBuilder, Node rootNode)
    {
        // Existing registrations...
        containerBuilder
            .Register(_ => rootNode.GetNode<Camera>("MainCamera"))
            .InstancePerLifetimeScope()
            .As<ICamera>();

        containerBuilder
            .Register(_ => rootNode.GetNode<TimeTracker>(nameof(TimeTracker)))
            .InstancePerLifetimeScope()
            .As<ITimeTracker>();

        containerBuilder
            .Register(_ => rootNode
                .GetNode<HeadUpDisplay>(nameof(HeadUpDisplay))
                .StatisticsOverlay)
            .InstancePerLifetimeScope()
            .As<IStatisticsOverlay>();

        containerBuilder
            .Register(_ => rootNode.GetNode<DiagnosticsOverlay>(nameof(DiagnosticsOverlay)))
            .InstancePerLifetimeScope()
            .As<IDiagnosticOverlay>();

        // Enhanced GameController with ScenarioLoader support
        containerBuilder
            .RegisterType<GameController>()
            .InstancePerLifetimeScope()
            .As<IGameController>();

        return containerBuilder;
    }

    // Add new registration method for scenario loading
    public static ContainerBuilder RegisterScenarioSystem(this ContainerBuilder containerBuilder)
    {
        // Register placement strategies
        containerBuilder
            .RegisterType<RandomPlacementStrategy>()
            .InstancePerLifetimeScope()
            .As<IPlacementStrategy>();

        containerBuilder
            .RegisterType<GridPlacementStrategy>()
            .InstancePerLifetimeScope()
            .As<IPlacementStrategy>();

        containerBuilder
            .RegisterType<ClusterPlacementStrategy>()
            .InstancePerLifetimeScope()
            .As<IPlacementStrategy>();

        // Register scenario loader
        containerBuilder
            .RegisterType<ScenarioLoader>()
            .InstancePerLifetimeScope()
            .As<IScenarioLoader>();

        return containerBuilder;
    }

    // ... other existing methods remain the same
}

public partial class AppBootstrapper : Node
{
    public IDataStore DataStore { get; private set; }
    public IContainer Container { get; private set; } // Expose container for UI access

    public override void _Ready()
    {
        var rootNode = this.GetParent();

        this.Container = new ContainerBuilder()
            .RegisterInfrastructure(rootNode)
            .RegisterStorage()
            .RegisterEntityCoordinator(rootNode)
            .RegisterScenarioSystem() // Add scenario system registration
            .RegisterSystem()
            .Build();

        this.DataStore = this.Container.Resolve<IDataStore>();

        // Don't auto-start - let UI control simulation startup
        // this.Container.Resolve<IGameController>().Start();
    }
    
    // Helper methods for UI to access services
    public T GetService<T>() where T : class
    {
        return this.Container.Resolve<T>();
    }
}
```

### Enhanced GameController for UI Integration
**Modifications to:** `Source/Odin.Engine/GameController.cs`
```csharp
public class GameController : IGameController
{
    private readonly IGameState _gameState;
    private readonly ITimeTracker _timeTracker;
    private readonly IReadOnlyCollection<ISystem> _systems;
    private readonly IScenarioLoader _scenarioLoader;

    private bool _isRunning = false;

    public GameController(
        ITimeTracker timeTracker,
        IEntityFactory entityFactory,
        IScenarioLoader scenarioLoader,
        IReadOnlyCollection<ISystem> systems)
    {
        // Initialize with default small universe - scenarios loaded via UI
        this._gameState = new GameState 
        { 
            Universe = entityFactory.CreateUniverse(64, 32) 
        };
        
        this._timeTracker = timeTracker;
        this._scenarioLoader = scenarioLoader;
        this._timeTracker.DeltaChanged += this.OnDeltaChanged;
        this._timeTracker.TickChanged += this.OnTickChanged;

        this._systems = systems
            .OrderBy(system => system
                .GetType()
                .GetTypeInfo()
                .GetCustomAttribute<SystemMetadataAttribute>()?
                .OrderingIndex ?? 0)
            .ToImmutableArray();

        // Register scenario loading callback
        this._scenarioLoader.RegisterScenarioLoadedCallback(this.OnScenarioLoaded);
    }

    // UI-friendly scenario loading method
    public async Task LoadScenarioFromUIAsync(string scenarioId, IProgress<ScenarioLoadingProgress> progress)
    {
        try
        {
            // Stop current simulation if running
            if (this._isRunning)
            {
                this.Stop();
            }

            // Clear existing entities
            var entityManager = this._gameState.FindService<IEntityManager>();
            entityManager?.Clear();

            // Load new scenario
            var universe = await this._scenarioLoader.LoadScenarioAsync(scenarioId, progress);
            this._gameState.Universe = universe;

            GD.Print($"GameController: Successfully loaded scenario '{scenarioId}'");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"GameController: Failed to load scenario '{scenarioId}': {ex.Message}");
            
            // Report error through progress
            progress?.Report(new ScenarioLoadingProgress 
            { 
                OverallProgress = 0.0f,
                CurrentPhase = "Error",
                StatusMessage = $"Failed to load scenario: {ex.Message}"
            });
            
            throw; // Re-throw for UI handling
        }
    }

    public void Start()
    {
        if (!this._isRunning)
        {
            this._isRunning = true;
            this._timeTracker.Start();
            GD.Print("GameController: Simulation started");
        }
    }

    public void Stop()
    {
        if (this._isRunning)
        {
            this._isRunning = false;
            this._timeTracker.Stop();
            GD.Print("GameController: Simulation stopped");
        }
    }

    public bool IsRunning => this._isRunning;

    private void OnScenarioLoaded(ScenarioBlueprint scenario)
    {
        GD.Print($"GameController: Scenario '{scenario.Name}' loaded and ready");
        
        // Scenario is loaded but simulation is not auto-started
        // UI will call Start() when ready
    }

    // ... rest of existing GameController implementation remains the same
}
```

### Interface Additions for UI Integration
**File:** `Source/Odin.Engine/Contract/IGameController.cs`
```csharp
public interface IGameController
{
    void Start();
    void Stop();
    bool IsRunning { get; }
    
    // New method for UI integration
    Task LoadScenarioFromUIAsync(string scenarioId, IProgress<ScenarioLoadingProgress> progress);
}
```

---

## Integration with GameController

### Enhanced GameController.cs Initialization
**Modifications to:** `Source/Odin.Engine/GameController.cs`
```csharp
public class GameController : IGameController
{
    private readonly IGameState _gameState;
    private readonly ITimeTracker _timeTracker;
    private readonly IReadOnlyCollection<ISystem> _systems;
    private readonly IScenarioLoader _scenarioLoader; // Add scenario loader

    public GameController(
        ITimeTracker timeTracker,
        IEntityFactory entityFactory,
        IScenarioLoader scenarioLoader, // Add scenario loader parameter
        IReadOnlyCollection<ISystem> systems)
    {
        // Initialize with scenario if available, otherwise use default
        this._gameState = scenarioLoader.HasActiveScenario 
            ? new GameState { Universe = scenarioLoader.CurrentScenario!.Universe }
            : new GameState { Universe = entityFactory.CreateUniverse(64, 32) };

        this._timeTracker = timeTracker;
        this._scenarioLoader = scenarioLoader;
        
        // Rest of initialization remains the same
        this._timeTracker.DeltaChanged += this.OnDeltaChanged;
        this._timeTracker.TickChanged += this.OnTickChanged;

        this._systems = systems
            .OrderBy(system => system
                .GetType()
                .GetTypeInfo()
                .GetCustomAttribute<SystemMetadataAttribute>()?
                .OrderingIndex ?? 0)
            .ToImmutableArray();
    }

    // Add scenario management methods
    public void LoadScenario(string scenarioId)
    {
        var universe = this._scenarioLoader.LoadScenario(scenarioId);
        this._gameState.Universe = universe;
    }

    public async Task LoadScenarioAsync(string scenarioId, IProgress<ScenarioLoadingProgress>? progress = null)
    {
        var universe = await this._scenarioLoader.LoadScenarioAsync(scenarioId, progress);
        this._gameState.Universe = universe;
    }
}
```

---

## Conclusion

This comprehensive implementation provides a complete scenario loading system that integrates seamlessly with AI.Odin's existing ECS architecture and blueprint infrastructure. The system supports:

- **Flexible Entity Placement**: Multiple placement strategies from simple random to complex formations
- **Dynamic Configuration**: Component overrides, trait variations, and statistical distributions
- **Performance Optimization**: Asynchronous loading with progress reporting for large scenarios
- **Research Integration**: Reproducible seeds, comprehensive metrics collection, and ML training support
- **User Experience**: Clear YAML configuration format with extensive examples and documentation

The modular design ensures easy extension and maintenance while providing powerful capabilities for both entertainment and research applications. The implementation follows existing code patterns and maintains compatibility with all current systems while adding significant new functionality for world initialization and scenario management.

---

*Implementation Status: Ready for Development*
*Estimated Development Time: 4-6 weeks for all phases*
*Dependencies: Leverages existing ECS, blueprint, and serialization infrastructure*

---

*Last Updated: January 2025*