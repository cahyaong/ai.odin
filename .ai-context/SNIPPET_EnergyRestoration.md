# Energy Restoration Mechanic Implementation Snippets

**Last Updated:** January 3, 2026

---

## Table of Contents

- [Energy Restoration Mechanic Implementation Snippets](#energy-restoration-mechanic-implementation-snippets)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
  - [3. Implementation Snippets](#3-implementation-snippets)
    - [3.1 Data Model Extensions](#31-data-model-extensions)
    - [3.2 Passive Restoration Processor](#32-passive-restoration-processor)
    - [3.3 Active Harvesting Processor](#33-active-harvesting-processor)
    - [3.4 Decision Making Integration](#34-decision-making-integration)
  - [4. Integration Notes](#4-integration-notes)

---

## 1. Overview

This document contains code implementation snippets for the Energy Restoration mechanic within the Entity Component System (ECS) framework. These implementations complement the existing MetabolismSystem to create a complete survival loop.

---

## 3. Implementation Snippets

### 3.1 Data Model Extensions

**Enhanced Vitality Component:**
```csharp
public class VitalityComponent : IComponent
{
    public bool IsDead { get; set; }
    public float Energy { get; set; }
    public float MaxEnergy { get; set; } = 100.0f;
    
    public bool IsEnergyFull => Energy >= MaxEnergy;
    public float EnergyPercentage => MaxEnergy > 0 ? Energy / MaxEnergy : 0f;
}
```

**Rejuvenation Component:**
```csharp
public class RejuvenationComponent : IComponent
{
    public float RestorationRate { get; set; } = 0.1f;
    public uint LastHarvestTick { get; set; }
    public uint HarvestCooldownTicks { get; set; } = 10;
    
    public bool CanHarvest(uint currentTick)
    {
        return currentTick >= LastHarvestTick + HarvestCooldownTicks;
    }
}
```

**Enhanced Harvestable Component:**
```csharp
public class HarvestableComponent : IComponent
{
    public string ResourceBlueprintId { get; set; }
    public int Amount { get; set; }
    public int MaxAmount { get; set; }
    public float RegrowthRate { get; set; }
    public float EnergyValue { get; set; } = 10.0f;
    public float HarvestRadius { get; set; } = 16.0f;
    
    public bool HasResources => Amount > 0;
}
```

**Enhanced Intelligence Component:**
```csharp
public class IntelligenceComponent : IComponent
{
    public EntityState EntityState { get; set; }
    public Point? TargetPosition { get; set; }
    public uint? TargetEntityId { get; set; }
    
    public bool HasTarget => TargetEntityId.HasValue;
    
    public void ClearTarget()
    {
        TargetEntityId = null;
        TargetPosition = null;
    }
}
```

**Motion State Enumeration:**
```csharp
public enum MotionState
{
    Idling,
    Walking,
    Running,
    Resting  // Added for passive restoration
}
```

---

### 3.2 Passive Restoration Processor

**Rejuvenation System Implementation:**
```csharp
[SystemMetadata(OrderingIndex = 15)] // After MetabolismSystem, before DecisionMakingSystem
public class RejuvenationSystem : BaseFixedSystem
{
    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(RejuvenationComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitality = entity.FindComponent<VitalityComponent>();
        
        if (vitality.IsDead || vitality.IsEnergyFull)
        {
            return;
        }
        
        var physics = entity.FindComponent<PhysicsComponent>();
        
        if (physics.MotionState != MotionState.Resting)
        {
            return;
        }
        
        var rejuvenation = entity.FindComponent<RejuvenationComponent>();
        
        // Apply restoration
        var energyGain = rejuvenation.RestorationRate;
        vitality.Energy = Math.Min(vitality.Energy + energyGain, vitality.MaxEnergy);
        
        // Enforce stationary behavior while resting
        physics.Velocity = Vector.Zero;
        
        entity.SetComponent(vitality);
        entity.SetComponent(physics);
    }
}
```

---

### 3.3 Active Harvesting Processor

**Harvesting System Implementation:**
```csharp
[SystemMetadata(OrderingIndex = 16)] // After RejuvenationSystem, before DecisionMakingSystem
public class HarvestingSystem : BaseFixedSystem
{
    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(RejuvenationComponent),
        typeof(IntelligenceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitality = entity.FindComponent<VitalityComponent>();

        if (vitality.IsDead || vitality.IsEnergyFull)
        {
            return;
        }

        var intelligence = entity.FindComponent<IntelligenceComponent>();

        if (!intelligence.HasTarget)
        {
            return;
        }

        if (!this.EntityManager.TryGetEntity(intelligence.TargetEntityId.Value, out var targetEntity))
        {
            intelligence.ClearTarget();
            entity.SetComponent(intelligence);
            return;
        }

        var harvestable = targetEntity.FindComponent<HarvestableComponent>();

        if (harvestable == null || !harvestable.HasResources)
        {
            intelligence.ClearTarget();
            entity.SetComponent(intelligence);
            return;
        }

        var entityPhysics = entity.FindComponent<PhysicsComponent>();
        var targetPhysics = targetEntity.FindComponent<PhysicsComponent>();
        var distance = this.CalculateDistance(entityPhysics.Position, targetPhysics.Position);

        if (distance > harvestable.HarvestRadius)
        {
            return;
        }

        var rejuvenation = entity.FindComponent<RejuvenationComponent>();

        if (!rejuvenation.CanHarvest(tick))
        {
            return;
        }

        this.ExecuteHarvest(tick, entity, targetEntity, vitality, rejuvenation, harvestable, intelligence);
    }

    private void ExecuteHarvest(
        uint tick,
        IEntity entity,
        IEntity targetEntity,
        VitalityComponent vitality,
        RejuvenationComponent rejuvenation,
        HarvestableComponent harvestable,
        IntelligenceComponent intelligence)
    {
        // Transfer energy
        vitality.Energy = Math.Min(vitality.Energy + harvestable.EnergyValue, vitality.MaxEnergy);
        
        // Decrement resource
        harvestable.Amount--;
        
        // Update cooldown
        rejuvenation.LastHarvestTick = tick;
        
        // Clear target if resource depleted
        if (!harvestable.HasResources)
        {
            intelligence.ClearTarget();
        }
        
        // Persist changes
        entity.SetComponent(vitality);
        entity.SetComponent(rejuvenation);
        entity.SetComponent(intelligence);
        targetEntity.SetComponent(harvestable);
    }
    
    private float CalculateDistance(Point fromPosition, Point toPosition)
    {
        var deltaX = fromPosition.X - toPosition.X;
        var deltaY = fromPosition.Y - toPosition.Y;

        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
```

---

### 3.4 Decision Making Integration

**Survival Behavior Extension:**
```csharp
public class DecisionMakingSystem : BaseFixedSystem
{
    private const float LowEnergyThreshold = 30.0f;
    private const float SafeEnergyThreshold = 70.0f;
    
    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(VitalityComponent),
        typeof(IntelligenceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitality = entity.FindComponent<VitalityComponent>();

        if (vitality.IsDead)
        {
            return;
        }

        var intelligence = entity.FindComponent<IntelligenceComponent>();
        var physics = entity.FindComponent<PhysicsComponent>();

        if (vitality.Energy < LowEnergyThreshold)
        {
            this.ProcessSurvivalMode(entity, vitality, intelligence, physics);
            return;
        }

        if (vitality.Energy > SafeEnergyThreshold)
        {
            this.ProcessNormalMode(entity, intelligence, physics);
            return;
        }
    }

    private void ProcessSurvivalMode(
        IEntity entity,
        VitalityComponent vitality,
        IntelligenceComponent intelligence,
        PhysicsComponent physics)
    {
        var nearestResource = this.FindNearestHarvestableResource(entity, physics);

        if (nearestResource != null)
        {
            intelligence.TargetEntityId = nearestResource.Id;
            intelligence.TargetPosition = nearestResource.FindComponent<PhysicsComponent>().Position;
            physics.MotionState = MotionState.Walking;

            entity.SetComponent(intelligence);
            entity.SetComponent(physics);
            return;
        }

        intelligence.ClearTarget();
        physics.MotionState = MotionState.Resting;
        physics.Velocity = Vector.Zero;

        entity.SetComponent(intelligence);
        entity.SetComponent(physics);
    }

    private void ProcessNormalMode(
        IEntity entity,
        IntelligenceComponent intelligence,
        PhysicsComponent physics)
    {
        if (intelligence.HasTarget)
        {
            var targetEntity = this.EntityManager.GetEntity(intelligence.TargetEntityId.Value);

            if (targetEntity?.HasComponent<HarvestableComponent>() == true)
            {
                intelligence.ClearTarget();
                entity.SetComponent(intelligence);
            }
        }

        if (physics.MotionState == MotionState.Resting)
        {
            physics.MotionState = MotionState.Idling;
            entity.SetComponent(physics);
        }
    }

    private IEntity FindNearestHarvestableResource(IEntity entity, PhysicsComponent physics)
    {
        var resourceEntities = this.EntityManager
            .GetEntitiesWithComponent<HarvestableComponent>();

        IEntity nearest = null;
        var minDistance = float.MaxValue;

        foreach (var resourceEntity in resourceEntities)
        {
            var harvestable = resourceEntity.FindComponent<HarvestableComponent>();

            if (!harvestable.HasResources)
            {
                continue;
            }

            var resourcePhysics = resourceEntity.FindComponent<PhysicsComponent>();
            var distance = this.CalculateDistance(physics.Position, resourcePhysics.Position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = resourceEntity;
            }
        }

        return nearest;
    }

    private float CalculateDistance(Point fromPosition, Point toPosition)
    {
        var deltaX = fromPosition.X - toPosition.X;
        var deltaY = fromPosition.Y - toPosition.Y;

        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
```

---

## 4. Integration Notes

**System Ordering:**
- MetabolismSystem: OrderingIndex = 10 (energy consumption)
- RejuvenationSystem: OrderingIndex = 15 (passive restoration)
- HarvestingSystem: OrderingIndex = 16 (active restoration)
- DecisionMakingSystem: OrderingIndex = 20 (behavior coordination)

**Component Dependencies:**
- RejuvenationSystem requires: VitalityComponent, RejuvenationComponent, PhysicsComponent
- HarvestingSystem requires: VitalityComponent, RejuvenationComponent, IntelligenceComponent, PhysicsComponent
- DecisionMakingSystem requires: VitalityComponent, IntelligenceComponent, PhysicsComponent

**Blueprint Configuration:**
- Human entities need RejuvenationComponent added via entity blueprint
- Berry bush entities need EnergyValue and HarvestRadius in HarvestableComponent
- MaxEnergy should be set appropriately in VitalityComponent

*Note: These implementations follow the existing AI.Odin ECS architecture patterns and integrate seamlessly with the MetabolismSystem for complete energy lifecycle management.*
