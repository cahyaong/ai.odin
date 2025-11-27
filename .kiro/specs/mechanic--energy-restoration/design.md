# Design Document

## Overview

The Energy Restoration System implements a complete survival loop for autonomous agents by enabling energy recovery through two complementary mechanisms: passive resting and active resource harvesting. This system transforms the existing one-way energy consumption into a dynamic balance where agents must intelligently manage their energy through behavioral choices.

The design leverages the existing ECS architecture with minimal new components, primarily extending the already-defined `RejuvenationComponent` and integrating with `VitalityComponent`, `HarvestableComponent`, and the `DecisionMakingSystem`. The system introduces two new ECS systems: `RejuvenationSystem` for passive energy restoration and `HarvestingSystem` for active resource consumption.

This design prioritizes performance through batch processing, maintains clean separation of concerns following ECS principles, and provides extensibility for future enhancements like food quality variations, cooking mechanics, and social food sharing.

## Architecture

### System Architecture

The Energy Restoration System consists of two primary systems that operate within the existing ECS framework:

**RejuvenationSystem (Fixed-timestep)**
- Processes entities with `RejuvenationComponent` and `VitalityComponent`
- Executes after `MetabolismSystem` in the system execution order
- Applies passive energy restoration when agents are in resting state
- Respects maximum energy capacity constraints
- Operates at fixed intervals for deterministic simulation

**HarvestingSystem (Variable-timestep)**
- Processes harvest requests from the `DecisionMakingSystem`
- Validates harvest conditions (range, resource availability)
- Transfers energy from harvestable entities to agents
- Updates `HarvestableComponent` quantities
- Emits harvest events for visual feedback and statistics

### Component Integration

The system integrates with existing components:

**VitalityComponent (Existing)**
```csharp
- CurrentEnergy: float (modified by both systems)
- MaxEnergy: float (used for capacity checks)
```

**RejuvenationComponent (Existing - Extended)**
```csharp
- IsResting: bool (indicates resting state)
- BaseRestorationRate: float (energy per second while resting)
- LastHarvestTime: float (cooldown tracking)
```

**HarvestableComponent (Existing)**
```csharp
- CurrentQuantity: int (decremented on harvest)
- MaxQuantity: int (capacity limit)
- EnergyValue: float (energy gained per harvest)
- RegenerationRate: float (used by GrowthSystem)
```

**IntelligenceComponent (Existing - Extended)**
```csharp
- TargetEntityId: int (for tracking harvest target)
- CurrentState: MotionState (includes new Resting state)
```

### Data Flow

1. **Energy Depletion Flow** (Existing)
   - Agent performs actions → `MetabolismSystem` → Decreases `VitalityComponent.CurrentEnergy`

2. **Passive Restoration Flow** (New)
   - Agent enters resting state → `DecisionMakingSystem` sets `IntelligenceComponent.CurrentState = Idle` and `RejuvenationComponent.IsResting = true`
   - `RejuvenationSystem` detects resting state → Increases `VitalityComponent.CurrentEnergy` based on `BaseRestorationRate`
   - Energy reaches max or agent becomes active → Restoration stops

3. **Active Harvesting Flow** (New)
   - Agent detects low energy → `DecisionMakingSystem` identifies nearest harvestable entity
   - Agent navigates to resource → `MovementSystem` updates position
   - Agent reaches harvest range → `DecisionMakingSystem` requests harvest
   - `HarvestingSystem` validates and processes → Transfers energy, decrements resource quantity
   - Harvest complete → Agent resumes normal behavior or seeks next resource

## Components and Interfaces

### Component Extensions

**RejuvenationComponent (Extended)**
```csharp
public class RejuvenationComponent : IComponent
{
    public RejuvenationComponent()
    {
        this.RestorationRate = 0f;
        this.LastHarvestTick = 0;
    }

    public float RestorationRate { get; set; }
    public uint LastHarvestTick { get; set; }
}
```

**VitalityComponent (Note: Already exists)**
```csharp
public class VitalityComponent : IComponent
{
    public bool IsDead { get; set; }
    public float Energy { get; set; }
    // Note: MaxEnergy will be added as a new field
}
```

**HarvestableComponent (Note: Already exists)**
```csharp
public class HarvestableComponent : IComponent
{
    public string ResourceBlueprintId { get; init; }
    public ushort AmountMax { get; init; }
    public ushort Amount { get; set; }
    public ushort RemainingTick { get; set; }
    public bool IsFull => this.Amount >= this.AmountMax;
    // Note: EnergyValue will be added as a new field
}
```

**IntelligenceComponent (Extended)**
```csharp
public class IntelligenceComponent : IComponent
{
    public Point TargetPosition { get; set; }
    public ushort RemainingTick { get; set; }
    public int TargetEntityId { get; set; } // New: for tracking harvest target
}
```

**MotionState (Extended)**
```csharp
public enum MotionState
{
    Unknown = 0,
    Idling,
    Walking,
    Running,
    Immobilized,
    Resting  // New state for energy restoration
}
```

### System Implementations

**RejuvenationSystem**
```csharp
public class RejuvenationSystem : BaseFixedSystem
{
    private const float MaxEnergy = 100f; // TODO: Move to configuration or component
    
    public RejuvenationSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(RejuvenationComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        
        if (vitalityComponent.IsDead)
        {
            return;
        }

        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        // Only restore energy when resting (stationary)
        if (physicsComponent.MotionState != MotionState.Resting)
        {
            return;
        }

        var rejuvenationComponent = entity.FindComponent<RejuvenationComponent>();
        
        // Apply energy restoration (tick-based, not time-based)
        var energyGain = rejuvenationComponent.RestorationRate;
        vitalityComponent.Energy = Math.Min(
            vitalityComponent.Energy + energyGain,
            MaxEnergy);
    }
}
```

**HarvestingSystem**
```csharp
public class HarvestingSystem : BaseFixedSystem
{
    private const float HarvestRange = 2.0f; // TODO: Move to configuration
    private const uint HarvestCooldownTicks = 10; // TODO: Move to configuration
    
    public HarvestingSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(RejuvenationComponent),
        typeof(IntelligenceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        
        if (vitalityComponent.IsDead)
        {
            return;
        }

        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        
        // Check if agent is targeting a resource
        if (intelligenceComponent.TargetEntityId <= 0)
        {
            return;
        }

        var targetEntity = this.EntityManager.FindEntity(intelligenceComponent.TargetEntityId);
        
        if (targetEntity == null)
        {
            intelligenceComponent.TargetEntityId = 0;
            return;
        }

        var harvestableComponent = targetEntity.FindComponent<HarvestableComponent>();
        
        if (harvestableComponent == null || harvestableComponent.Amount <= 0)
        {
            intelligenceComponent.TargetEntityId = 0;
            return;
        }

        var rejuvenationComponent = entity.FindComponent<RejuvenationComponent>();
        
        // Check cooldown
        if (tick - rejuvenationComponent.LastHarvestTick < HarvestCooldownTicks)
        {
            return;
        }

        var agentPhysics = entity.FindComponent<PhysicsComponent>();
        var resourcePhysics = targetEntity.FindComponent<PhysicsComponent>();
        
        // Check range
        var distance = (agentPhysics.Position - resourcePhysics.Position).Magnitude();
        
        if (distance > HarvestRange)
        {
            return;
        }

        // Perform harvest
        // TODO: Get energy value from resource blueprint or component
        const float energyPerHarvest = 15f;
        const float maxEnergy = 100f;
        
        vitalityComponent.Energy = Math.Min(
            vitalityComponent.Energy + energyPerHarvest,
            maxEnergy);
            
        harvestableComponent.Amount--;
        rejuvenationComponent.LastHarvestTick = tick;
        
        // Clear target after successful harvest
        intelligenceComponent.TargetEntityId = 0;
    }
}
```

## Data Models

### Configuration Constants

For the initial implementation, configuration values will be defined as constants within the systems. Future iterations can move these to a configuration system or component properties.

**RejuvenationSystem Constants**
```csharp
private const float MaxEnergy = 100f;
// RestorationRate is per-entity via RejuvenationComponent
```

**HarvestingSystem Constants**
```csharp
private const float HarvestRange = 2.0f;
private const uint HarvestCooldownTicks = 10;
private const float EnergyPerHarvest = 15f; // TODO: Move to resource blueprint
```

**DecisionMakingSystem Constants (to be added)**
```csharp
private const float LowEnergyThreshold = 30.0f;
private const float SafeEnergyThreshold = 70.0f;
```

### Blueprint Extensions

**EntityBlueprint (Extended)**
```yaml
# entity-human.ngaoblueprint
components:
  rejuvenation:
    restoration_rate: 0.5  # Energy per tick while resting
  vitality:
    energy: 100.0
    # Note: IsDead is initialized to false by default
```

**HarvestableBlueprint (Extended)**
```yaml
# entity-bush.ngaoblueprint
components:
  harvestable:
    resource_blueprint_id: "berry"
    amount_max: 10
    amount: 10
    # Note: RemainingTick is managed by GrowthSystem
    # Note: EnergyValue will be added in future iteration
```


## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system—essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Passive Energy Restoration Properties

**Property 1: Resting increases energy proportionally to time**
*For any* agent in resting state with energy below maximum, advancing time by delta seconds should increase the agent's energy by exactly `BaseRestorationRate * RestorationMultiplier * delta` (capped at maximum energy).
**Validates: Requirements 1.1**

**Property 2: Resting state enforces stationary behavior**
*For any* agent in resting state, the agent's velocity must be zero and motion state must be Idle.
**Validates: Requirements 1.2, 1.5**

**Property 3: Energy never exceeds maximum capacity**
*For any* agent, after any energy restoration operation (resting or harvesting), the agent's current energy must be less than or equal to maximum energy.
**Validates: Requirements 1.3, 2.4**

**Property 4: Transitioning from resting stops restoration**
*For any* agent that transitions from resting to active state, advancing time should not increase the agent's energy.
**Validates: Requirements 1.4**

### Active Harvesting Properties

**Property 5: Successful harvest transfers correct energy**
*For any* agent and harvestable resource where harvest conditions are met, performing a harvest should increase the agent's energy by exactly the resource's energy value (capped at maximum energy).
**Validates: Requirements 2.1**

**Property 6: Depleted resources cannot be harvested**
*For any* harvestable resource with zero quantity, attempting to harvest should fail and the agent's energy should remain unchanged.
**Validates: Requirements 2.2**

**Property 7: Harvest decrements resource quantity**
*For any* successful harvest operation, the harvestable entity's quantity should decrease by exactly one unit.
**Validates: Requirements 2.3**

**Property 8: Harvest range validation**
*For any* agent and harvestable resource, if the distance between them is less than or equal to harvest range and the resource has quantity greater than zero and cooldown has elapsed, then CanHarvest should return true.
**Validates: Requirements 2.5**

### AI Decision-Making Properties

**Property 9: Low energy triggers survival behaviors**
*For any* agent with energy below the low energy threshold, the Decision Making System should output either a harvesting behavior (if resources are accessible) or a resting behavior (if no resources are accessible).
**Validates: Requirements 3.1, 3.2, 3.3**

**Property 10: Safe energy resumes normal behaviors**
*For any* agent with energy above the safe energy threshold, the Decision Making System should not output survival-specific behaviors (resting or food-seeking).
**Validates: Requirements 3.4**

**Property 11: Food-seeking targets nearest available resource**
*For any* agent in food-seeking state with multiple harvestable resources in range, the agent's target should be the nearest resource with quantity greater than zero.
**Validates: Requirements 3.5**

### Configuration and Integration Properties

**Property 12: Restoration rates are applied correctly**
*For any* agent with a specific restoration rate value, after resting for one tick, the energy gain should equal exactly that restoration rate value (capped at maximum energy).
**Validates: Requirements 4.1**

**Property 13: Component filtering is correct**
*For any* set of entities with various component combinations, the systems should process only those entities that have all required components.
**Validates: Requirements 5.1**

**Property 14: Energy updates modify Vitality Component**
*For any* agent processed by the Rejuvenation System while resting, the Vitality Component's current energy value should be different after processing than before (unless already at maximum).
**Validates: Requirements 5.3**



## Error Handling

### Validation Errors

**Invalid Harvest Attempts**
- **Condition**: Agent attempts to harvest when out of range, resource is depleted, or cooldown hasn't elapsed
- **Handling**: `TryHarvest` returns false, no state changes occur, no exceptions thrown
- **Recovery**: Agent can retry after addressing the condition (moving closer, waiting for cooldown, finding different resource)

**Component Missing**
- **Condition**: System attempts to process entity without required components
- **Handling**: Entity is skipped in batch processing, logged as warning in debug builds
- **Recovery**: Automatic - entity is ignored until components are added

**Invalid Configuration**
- **Condition**: Configuration values are negative, zero, or otherwise invalid
- **Handling**: Throw `OdinException` during system initialization with descriptive message
- **Recovery**: Application fails fast - configuration must be corrected before restart

### Runtime Errors

**Energy Overflow**
- **Condition**: Calculation would result in energy exceeding maximum
- **Handling**: Clamp energy to maximum capacity, no exception thrown
- **Recovery**: Automatic - energy is capped at valid value

**Concurrent Harvesting**
- **Condition**: Multiple agents attempt to harvest the same resource simultaneously
- **Handling**: First agent succeeds, subsequent attempts fail if resource becomes depleted
- **Recovery**: Automatic - failed agents seek alternative resources

**System Execution Order Violation**
- **Condition**: Systems execute in incorrect order due to misconfiguration
- **Handling**: Log error and continue execution (degraded behavior)
- **Recovery**: Manual - fix SystemMetadata attributes and restart

### Edge Cases

**Zero Restoration Rate**
- **Condition**: Agent has restoration rate of zero
- **Handling**: No energy restoration occurs, agent can still harvest
- **Recovery**: Intentional behavior for certain agent types

**Instant Death During Harvest**
- **Condition**: Agent dies from other causes while harvesting
- **Handling**: Harvest completes if already initiated, otherwise cancelled
- **Recovery**: Automatic - dead agents are filtered from system processing

**Resource Regeneration During Harvest**
- **Condition**: Resource regenerates while agent is harvesting
- **Handling**: Harvest uses quantity available at harvest initiation time
- **Recovery**: Automatic - regeneration continues normally after harvest

## Testing Strategy

### Unit Testing Approach

The Energy Restoration System will use unit tests to verify specific examples, edge cases, and integration points:

**RejuvenationSystem Tests**
- Test resting agent with 50% energy restores to 100% over time
- Test agent at 100% energy does not gain additional energy while resting
- Test agent with zero restoration rate does not gain energy
- Test restoration multiplier correctly scales energy gain
- Test system skips entities without required components

**HarvestingSystem Tests**
- Test successful harvest with agent at 50% energy and resource at full quantity
- Test harvest fails when agent is out of range
- Test harvest fails when resource is depleted
- Test harvest fails during cooldown period
- Test harvest event is emitted with correct parameters
- Test multiple agents harvesting different resources simultaneously

**DecisionMakingSystem Integration Tests**
- Test low-energy agent with nearby resource chooses harvesting
- Test low-energy agent without nearby resource chooses resting
- Test agent above safe threshold resumes normal behaviors
- Test agent targets nearest available resource when multiple exist

**Configuration Tests**
- Test system respects custom restoration rates
- Test system respects custom energy thresholds
- Test system respects custom harvest range
- Test system respects custom cooldown values

### Property-Based Testing Approach

The Energy Restoration System will use property-based testing to verify universal properties across all inputs. We will use **FsCheck** (the .NET property-based testing library) integrated with xUnit. Each property-based test will run a minimum of 100 iterations with randomly generated inputs.

**Property Test Structure**
```csharp
[Property(MaxTest = 100)]
public Property PropertyName(/* generated inputs */)
{
    // Arrange: Set up test conditions
    // Act: Execute system behavior
    // Assert: Verify property holds
    return property.ToProperty();
}
```

**Generator Strategy**
- **Agents**: Generate with random energy levels (0-100), positions, and restoration rates
- **Resources**: Generate with random quantities (0-10), energy values (5-20), and positions
- **Time Deltas**: Generate realistic time steps (0.016-1.0 seconds)
- **Configurations**: Generate valid configuration ranges for all parameters

**Property Test Coverage**
- Each correctness property from the design document will have one corresponding property-based test
- Tests will be tagged with comments explicitly referencing the property number and requirement
- Property tests will focus on invariants and universal rules rather than specific examples

**Example Property Test**
```csharp
// Feature: core-energy-restoration, Property 1: Resting increases energy proportionally to time
// Validates: Requirements 1.1
[Property(MaxTest = 100)]
public Property RestingIncreasesEnergyProportionally()
{
    return Prop.ForAll(
        GenerateRestingAgent(),
        GenerateTimeDelta(),
        (agent, deltaTime) =>
        {
            var initialEnergy = agent.Vitality.CurrentEnergy;
            var expectedGain = agent.Rejuvenation.BaseRestorationRate 
                * agent.Rejuvenation.RestorationMultiplier 
                * deltaTime;
            var expectedFinal = Math.Min(initialEnergy + expectedGain, agent.Vitality.MaxEnergy);
            
            _rejuvenationSystem.ProcessFixed(deltaTime);
            
            return Math.Abs(agent.Vitality.CurrentEnergy - expectedFinal) < 0.001f;
        });
}
```

### Integration Testing

**System Interaction Tests**
- Test complete survival loop: energy depletion → low energy detection → resource seeking → harvesting → energy restoration → normal behavior
- Test resting fallback: energy depletion → low energy detection → no resources available → resting → energy restoration
- Test resource competition: multiple agents seeking same resource → first agent harvests → other agents seek alternatives
- Test regeneration cycle: resource depleted → agents rest → resource regenerates → agents resume harvesting

**Performance Tests**
- Verify 1000 agents with energy restoration maintain 60 FPS
- Verify batch processing efficiency with varying entity counts
- Verify spatial queries for resource finding scale appropriately

### Test Organization

Tests will be organized in the `Odin.Engine.UnitTest` project (to be created):
```
Odin.Engine.UnitTest/
  ECS.System.Logic/
    RejuvenationSystemTests.cs          # Unit tests
    RejuvenationSystemProperties.cs     # Property-based tests
    HarvestingSystemTests.cs            # Unit tests
    HarvestingSystemProperties.cs       # Property-based tests
    DecisionMakingSystemTests.cs        # Integration tests
```

