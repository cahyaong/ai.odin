# SNIPPET: Scenario Loading System

**Last Updated:** January 6, 2026

---

## Table of Contents

- [SNIPPET: Scenario Loading System](#snippet-scenario-loading-system)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
  - [2. Phase 1: Core Scenario Blueprint System](#2-phase-1-core-scenario-blueprint-system)
    - [2.1 Data Model Implementation](#21-data-model-implementation)
      - [ScenarioBlueprint - Core Scenario Definition](#scenarioblueprint---core-scenario-definition)
      - [EntitySpawning - Entity Population Definition](#entityspawning---entity-population-definition)
      - [ResourceDistribution - World Resource Configuration](#resourcedistribution---world-resource-configuration)
    - [2.2 Core Scenario Loading Implementation](#22-core-scenario-loading-implementation)
      - [IScenarioLoader - Interface Definition](#iscenarioloader---interface-definition)
      - [ScenarioLoader - Core Loading Implementation](#scenarioloader---core-loading-implementation)
  - [3. Phase 2: Placement Strategy Implementation](#3-phase-2-placement-strategy-implementation)
    - [3.1 Placement Strategy Interface and Factory](#31-placement-strategy-interface-and-factory)
      - [IPlacementStrategy - Strategy Interface](#iplacementstrategy---strategy-interface)
      - [RandomPlacementStrategy - Random Placement Implementation](#randomplacementstrategy---random-placement-implementation)
      - [GridPlacementStrategy - Grid Formation Implementation](#gridplacementstrategy---grid-formation-implementation)
      - [ClusterPlacementStrategy - Cluster Formation Implementation](#clusterplacementstrategy---cluster-formation-implementation)
  - [4. Phase 3: System Integration \& Scenario Management](#4-phase-3-system-integration--scenario-management)
    - [4.1 Scenario Initialization System](#41-scenario-initialization-system)
      - [ScenarioInitializationSystem - Main Scenario Execution System](#scenarioinitializationsystem---main-scenario-execution-system)
    - [4.2 Enhanced Game State Extensions](#42-enhanced-game-state-extensions)
      - [GameState Extensions for Scenario Support](#gamestate-extensions-for-scenario-support)
    - [4.3 YAML Serialization Extensions](#43-yaml-serialization-extensions)
      - [ScenarioYamlConverter - YAML Type Converter](#scenarioyamlconverter---yaml-type-converter)
      - [Updated YamlSerializationExtensions](#updated-yamlserializationextensions)
    - [4.4 EmbeddedDataStore Extensions](#44-embeddeddatastore-extensions)
      - [Updated EmbeddedDataStore](#updated-embeddeddatastore)
      - [Updated OdinMime](#updated-odinmime)
  - [5. Example Scenario Blueprint Files](#5-example-scenario-blueprint-files)
    - [5.1 Basic Survival Scenario](#51-basic-survival-scenario)
    - [5.2 Large Population Research Scenario](#52-large-population-research-scenario)
    - [5.3 Tutorial Progression Scenario](#53-tutorial-progression-scenario)

---

## 1. Overview

This document contains code implementations for the scenario loading system.

---

## 2. Phase 1: Core Scenario Blueprint System

### 2.1 Data Model Implementation

#### ScenarioBlueprint - Core Scenario Definition

```csharp
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

#### EntitySpawning - Entity Population Definition

```csharp
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

#### ResourceDistribution - World Resource Configuration

```csharp
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

### 2.2 Core Scenario Loading Implementation

#### IScenarioLoader - Interface Definition

```csharp
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

#### ScenarioLoader - Core Loading Implementation

```csharp
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
        var universe = this.CreateUniverseFromScenario(scenario);

        if (scenario.Metadata?.ReproducibleSeed.HasValue == true)
        {
            this._random = new Random((int)scenario.Metadata.ReproducibleSeed.Value);
        }

        this.SpawnEntitiesFromScenario(scenario, universe);

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

        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 0.1f, 
            CurrentPhase = "Initializing Universe",
            StatusMessage = $"Setting up world: {scenario.Universe.Size.Width}x{scenario.Universe.Size.Height}"
        });

        var universe = this.CreateUniverseFromScenario(scenario);

        if (scenario.Metadata?.ReproducibleSeed.HasValue == true)
        {
            this._random = new Random((int)scenario.Metadata.ReproducibleSeed.Value);
        }

        var totalEntities = scenario.EntitySpawnings.Sum(spawning => spawning.Quantity);

        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 0.2f, 
            CurrentPhase = "Spawning Entities",
            TotalEntities = totalEntities,
            StatusMessage = $"Creating {totalEntities} entities..."
        });

        await this.SpawnEntitiesFromScenarioAsync(scenario, universe, progress);

        progress?.Report(new ScenarioLoadingProgress 
        { 
            OverallProgress = 1.0f, 
            CurrentPhase = "Completed",
            EntitiesCreated = totalEntities,
            TotalEntities = totalEntities,
            StatusMessage = "Scenario loaded successfully"
        });

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
        var scenario = scenarios.FirstOrDefault(scenarioBlueprint => scenarioBlueprint.Id == scenarioId);
        
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
        var initialSpawnings = scenario.EntitySpawnings
            .Where(spawning => spawning.SpawnTiming == SpawnTiming.Initial);

        foreach (var entitySpawning in initialSpawnings)
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

        var initialSpawnings = scenario.EntitySpawnings
            .Where(spawning => spawning.SpawnTiming == SpawnTiming.Initial);

        foreach (var entitySpawning in initialSpawnings)
        {
            await this.SpawnEntityGroupAsync(entitySpawning, universe, progress, entitiesCreated, totalEntities);
            entitiesCreated += entitySpawning.Quantity;
        }
    }

    private void SpawnEntityGroup(EntitySpawning spawning, IUniverse universe)
    {
        var placementStrategy = this.GetPlacementStrategy(spawning.PlacementStrategy);
        var positions = placementStrategy.GeneratePositions(spawning.Quantity, universe);

        for (var entityIndex = 0; entityIndex < spawning.Quantity; entityIndex++)
        {
            var entity = this._entityFactory.CreateEntity(spawning.EntityBlueprintId);
            var physicsComponent = entity.FindComponent<PhysicsComponent>();

            if (physicsComponent != null && entityIndex < positions.Count())
            {
                physicsComponent.Position = positions.ElementAt(entityIndex);
            }

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

        for (uint batchStart = 0; batchStart < spawning.Quantity; batchStart += batchSize)
        {
            var batchCount = Math.Min(batchSize, spawning.Quantity - batchStart);
            
            for (var batchOffset = 0; batchOffset < batchCount; batchOffset++)
            {
                var entityIndex = batchStart + batchOffset;
                var entity = this._entityFactory.CreateEntity(spawning.EntityBlueprintId);
                var physicsComponent = entity.FindComponent<PhysicsComponent>();

                if (physicsComponent != null && entityIndex < positions.Count())
                {
                    physicsComponent.Position = positions.ElementAt((int)entityIndex);
                }

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

            var currentEntitiesCreated = baseEntitiesCreated + batchStart + batchCount;

            progress?.Report(new ScenarioLoadingProgress 
            { 
                OverallProgress = 0.2f + (0.7f * currentEntitiesCreated / totalEntities),
                CurrentPhase = "Spawning Entities",
                EntitiesCreated = currentEntitiesCreated,
                TotalEntities = totalEntities,
                StatusMessage = $"Created {currentEntitiesCreated}/{totalEntities} entities"
            });

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
            var component = entity.Components
                .FirstOrDefault(comp => comp.GetType().Name.StartsWith(componentId, StringComparison.OrdinalIgnoreCase));
            
            if (component != null)
            {
                this.ApplyParameterToComponent(component, parameter);
            }
        }
    }

    private void ApplyTraitVariations(IEntity entity, StatisticalDistribution distribution)
    {
        var component = entity.Components
            .FirstOrDefault(comp => comp.GetType().Name.StartsWith(distribution.ComponentId, StringComparison.OrdinalIgnoreCase));

        if (component != null)
        {
            var property = component.GetType().GetProperty(distribution.PropertyName);
            var canWriteProperty = property != null && property.CanWrite;
            
            if (canWriteProperty)
            {
                var randomValue = this.GenerateRandomValue(distribution);
                property!.SetValue(component, Convert.ChangeType(randomValue, property.PropertyType));
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

## 3. Phase 2: Placement Strategy Implementation

### 3.1 Placement Strategy Interface and Factory

#### IPlacementStrategy - Strategy Interface

```csharp
public interface IPlacementStrategy
{
    bool CanHandle(PlacementType placementType);
    
    IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe);
    
    IEnumerable<Point> GeneratePositions(uint quantity, IUniverse universe, PlacementStrategy strategy);
}
```

#### RandomPlacementStrategy - Random Placement Implementation

```csharp
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

        var maxAttempts = (int)quantity * 10;
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

#### GridPlacementStrategy - Grid Formation Implementation

```csharp
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

        var columns = formation.Columns ?? (uint)Math.Ceiling(Math.Sqrt(quantity));
        var rows = formation.Rows ?? (uint)Math.Ceiling((double)quantity / columns);

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

#### ClusterPlacementStrategy - Cluster Formation Implementation

```csharp
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

        var clusterCenters = this.GenerateClusterCenters(cluster.ClusterCount, boundary);
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

        for (var clusterIndex = 0; clusterIndex < clusterCount; clusterIndex++)
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

        for (var clusterIndex = 0; clusterIndex < clusterCount; clusterIndex++)
        {
            distribution[clusterIndex] = baseCount;
            
            if (clusterIndex < remainder)
            {
                distribution[clusterIndex]++;
            }

            if (densityVariation > 0)
            {
                var variation = (float)(this._random.NextDouble() - 0.5) * 2 * densityVariation;
                var adjustment = (int)(distribution[clusterIndex] * variation);
                distribution[clusterIndex] = (uint)Math.Max(1, (int)distribution[clusterIndex] + adjustment);
            }
        }

        return distribution;
    }

    private IEnumerable<Point> GenerateClusterPositions(uint quantity, Point center, float radius)
    {
        var positions = new List<Point>();

        for (var positionIndex = 0; positionIndex < quantity; positionIndex++)
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

## 4. Phase 3: System Integration & Scenario Management

### 4.1 Scenario Initialization System

#### ScenarioInitializationSystem - Main Scenario Execution System

```csharp
[SystemMetadata(OrderingIndex = -100)]
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
        gameState.SetScenarioEnvironment(environment);
    }

    private void InitializeScenarioResources(ScenarioBlueprint scenario, IGameState gameState)
    {
        if (scenario.Resources == null)
        {
            return;
        }

        foreach (var resourceDistribution in scenario.Resources)
        {
            this.SpawnResourceEntities(resourceDistribution, gameState.Universe);
        }
    }

    private void InitializeScenarioEvents(ScenarioBlueprint scenario)
    {
        if (scenario.Events == null)
        {
            return;
        }

        foreach (var environmentalEvent in scenario.Events)
        {
            this.ScheduleEnvironmentalEvent(environmentalEvent);
        }
    }

    private void SpawnResourceEntities(ResourceDistribution distribution, IUniverse universe)
    {
    }

    private void ScheduleEnvironmentalEvent(EnvironmentalEvent environmentalEvent)
    {
    }
}
```

### 4.2 Enhanced Game State Extensions

#### GameState Extensions for Scenario Support

```csharp
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

### 4.3 YAML Serialization Extensions

#### ScenarioYamlConverter - YAML Type Converter

```csharp
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

#### Updated YamlSerializationExtensions

```csharp
private static readonly IDeserializer Deserializer = new DeserializerBuilder()
    .WithNamingConvention(YamlSerializationExtensions.NamingConvention)
    .WithTypeConverter(ParameterYamlConverter.Instance)
    .WithTypeConverter(ScalarYamlConverter.Instance)
    .WithTypeConverter(new ScenarioYamlConverter()) // Add scenario converter
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<EntityBlueprint>())
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<SpriteSheetBlueprint>())
    .WithTypeConverter(new ScalarOrSequenceYamlConverter<ScenarioBlueprint>()) // Add scenario support
    .Build();

public static IEnumerable<ScenarioBlueprint> LoadScenarioBlueprints(this IDataStore dataStore)
{
    return dataStore
        .FindResources(OdinMime.ScenarioBlueprint)
        .Select(resource => resource.Content.DeserializeFromYaml<ScenarioBlueprint>());
}
```

### 4.4 EmbeddedDataStore Extensions

#### Updated EmbeddedDataStore

```csharp
public IEnumerable<ScenarioBlueprint> LoadScenarioBlueprints()
{
    return this
        .FindResources(OdinMime.ScenarioBlueprint)
        .Select(resource => resource.Content.DeserializeFromYaml<ScenarioBlueprint>());
}
```

#### Updated OdinMime

```csharp
public static class OdinMime
{
    public static readonly MimeType EntityBlueprint = new("application", "x-ngao-entity-blueprint");
    public static readonly MimeType SpriteSheetBlueprint = new("application", "x-ngao-spritesheet-blueprint");
    public static readonly MimeType ScenarioBlueprint = new("application", "x-ngao-scenario-blueprint");

    public static MimeType? GetMimeType(FileInfo fileInfo)
    {
        return fileInfo.Extension switch
        {
            ".ngaoblueprint" => EntityBlueprint,
            ".ngasprite" => SpriteSheetBlueprint,
            ".ngascenario" => ScenarioBlueprint,
            _ => null
        };
    }
}
```

---

## 5. Example Scenario Blueprint Files

### 5.1 Basic Survival Scenario

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

### 5.2 Large Population Research Scenario

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

### 5.3 Tutorial Progression Scenario

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
