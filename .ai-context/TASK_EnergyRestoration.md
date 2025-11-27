# Energy Restoration Mechanic Implementation Status

## Overview

This document tracks the implementation of the **Energy Restoration Mechanic** - a dual-mode mechanic providing both passive and active energy recovery for agents in AI.Odin. This system works in conjunction with the already-complete **MetabolismSystem** (energy consumption) to create a complete survival loop.

**Current Status: PLANNING COMPLETE - Ready for Implementation**  
**Last Updated: November 27, 2025**

---

## 🎯 System Architecture Overview

### Complete Energy Lifecycle

```
+-----------------------------------------------------------------+
|  ENERGY CONSUMPTION (COMPLETE - MetabolismSystem)               |
|  - Continuous drain based on activity level                     |
|  - Idle: 2 energy/tick                                          |
|  - Walking: 3 energy/tick                                       |
|  - Running: 5 energy/tick                                       |
|  - Death when energy reaches 0                                  |
+-----------------------------------------------------------------+
                               |
                               v
+-----------------------------------------------------------------+
|  SURVIVAL DECISION MAKING (TO IMPLEMENT)                        |
|  - Low Energy Threshold: < 30 energy                            |
|  - Safe Energy Threshold: > 70 energy                           |
|  - Resource seeking behavior                                    |
|  - Fallback resting behavior                                    |
+-----------------------------------------------------------------+
                               |
                +--------------+---------------+
                |                              |
                v                              v
+------------------------------+   +------------------------------+
|  PASSIVE RESTORATION         |   |  ACTIVE RESTORATION          |
|  (TO IMPLEMENT)              |   |  (TO IMPLEMENT)              |
|                              |   |                              |
|  RejuvenationSystem          |   |  HarvestingSystem            |
|  - Resting state             |   |  - Proximity-based           |
|  - Slow regen                |   |  - Instant energy            |
|  - +1 energy/10 ticks        |   |  - +10 energy/berry          |
|  - No food required          |   |  - Resource depletion        |
+------------------------------+   +------------------------------+
```

---

## ✅ COMPLETED: Metabolism System (Energy Consumption)

### Working Implementation

**MetabolismSystem** - `Source/Odin.Engine/ECS.System.Logic/MetabolismSystem.cs`
- ✅ Continuous energy depletion based on agent activity
- ✅ State-based consumption rates (Idle/Walking/Running)
- ✅ Death transition when energy reaches zero
- ✅ Blueprint-driven configuration via TraitComponent
- ✅ Clean integration with ECS architecture

### Components (Energy Consumption)

**VitalityComponent** - `Source/Odin.Engine/ECS.Component.Logic/VitalityComponent.cs`
- ✅ `IsDead` (bool) - Death state tracking
- ✅ `Energy` (float) - Current energy level
- ❌ **NEEDS:** `MaxEnergy` (float) - Maximum energy capacity

**TraitComponent** - `Source/Odin.Engine/ECS.Component.Logic/TraitComponent.cs`
- ✅ `EnergyConsumptionRateByEntityStateLookup` - Consumption rates per state
- ✅ Blueprint integration working

**Success Metrics (Achieved):**
- ✅ Entities consume energy based on behavioral state
- ✅ Death occurs when energy depletes
- ✅ Blueprint-driven configuration working
- ✅ Performance: <0.1ms per 100 entities

---

## 🚧 TO IMPLEMENT: Energy Restoration Mechanic

### Two-Mode Restoration Architecture

#### **Mode 1: Passive Restoration (Resting)**

**Purpose:** Fallback energy recovery when no food is available

**RejuvenationSystem** - `Source/Odin.Engine/ECS.System.Logic/RejuvenationSystem.cs` (NEW)
- Restores energy when agent is in `MotionState.Resting`
- Slow but reliable: +1 energy per 10 ticks (configurable)
- Requires agent to be stationary (zero velocity)
- Energy capped at `VitalityComponent.MaxEnergy`
- Skips dead entities

**Requirements:**
- Agent must be in Resting state
- Positive restoration rate in RejuvenationComponent
- Not at maximum energy
- Not dead

#### **Mode 2: Active Restoration (Harvesting)**

**Purpose:** Primary energy recovery through resource consumption

**HarvestingSystem** - `Source/Odin.Engine/ECS.System.Logic/HarvestingSystem.cs` (NEW)
- Transfers energy from harvestable resources to agent
- Fast recovery: +10 energy per berry (instant)
- Proximity-based: agent must be within harvest range
- Cooldown mechanism to prevent instant harvesting
- Decrements resource quantity
- Clears target when resource depleted

**Requirements:**
- Agent has target entity ID set
- Target has HarvestableComponent with Amount > 0
- Agent within harvest range of target
- Harvest cooldown expired
- Not at maximum energy
- Not dead

#### **Enhanced Decision Making**

**DecisionMakingSystem Extensions** - `Source/Odin.Engine/ECS.System.Logic/DecisionMakingSystem.cs`
- **Low Energy Detection (< 30 energy):**
  - Search for nearest harvestable with available resources
  - If found: Set TargetEntityId and navigate toward it
  - If not found: Switch to Resting state
  
- **Safe Energy Reached (> 70 energy):**
  - Clear target entity
  - Resume normal AI behaviors
  
- **Resting State Enforcement:**
  - Ensure zero velocity when resting
  - Continue until safe energy threshold

---

## 📋 Component Requirements

### Components to Enhance

**VitalityComponent** - ENHANCE
```csharp
public class VitalityComponent : IComponent
{
    public bool IsDead { get; set; }
    public float Energy { get; set; }
    public float MaxEnergy { get; set; } = 100.0f; // NEW
    
    // Helper properties
    public bool IsEnergyFull => Energy >= MaxEnergy;
    public float EnergyPercentage => Energy / MaxEnergy;
}
```

**RejuvenationComponent** - IMPLEMENT
```csharp
public class RejuvenationComponent : IComponent
{
    public float RestorationRate { get; set; } = 0.1f; // Energy per tick while resting
    public uint LastHarvestTick { get; set; } = 0;     // For harvest cooldown
}
```

**HarvestableComponent** - ENHANCE
```csharp
public class HarvestableComponent : IComponent
{
    // Existing properties (already implemented):
    public string ResourceBlueprintId { get; init; }
    public ushort AmountMax { get; init; }
    public ushort Amount { get; set; }
    public ushort RemainingTick { get; set; }
    public bool IsFull => Amount >= AmountMax;
    
    // NEW properties needed:
    public float EnergyValue { get; set; } = 10.0f;      // Energy per harvest
    public float HarvestRadius { get; set; } = 16.0f;    // Proximity required
}
```

**IntelligenceComponent** - ENHANCE
```csharp
public class IntelligenceComponent : IComponent
{
    // Existing properties...
    
    // NEW property:
    public uint? TargetEntityId { get; set; } = null; // Track harvest target
}
```

**MotionState enum** - ENHANCE
```csharp
public enum MotionState
{
    Unknown,
    Idling,
    Walking,
    Running,
    Dying,
    Resting  // NEW - for passive energy restoration
}
```

---

## 🎮 Energy Restoration Flow

### Complete Survival Cycle

```
1. Agent Spawns
   └─> Initial Energy: 50 / MaxEnergy: 100

2. Activity Phase
   └─> Energy drains continuously (MetabolismSystem)
       - Idle: -2/tick
       - Walking: -3/tick
       - Running: -5/tick

3. LOW ENERGY THRESHOLD (< 30 energy)
   └─> DecisionMakingSystem activates survival mode
       
   3a. Resource Available?
       YES → Navigate to berry bush
           → HarvestingSystem activates
           → Harvest berry: +10 energy (instant)
           → Repeat until Energy > 70
           
   3b. No Resource Nearby?
       NO → Switch to Resting state
          → RejuvenationSystem activates
          → Slow restoration: +0.1 energy/tick
          → Continue until Energy > 70 or food found

4. SAFE ENERGY THRESHOLD (> 70 energy)
   └─> Resume normal AI behaviors
   └─> Return to Activity Phase (step 2)

5. Energy Reaches 0
   └─> Death (MetabolismSystem)
```

---

## 📂 Blueprint Configuration

### Human Entity Configuration
**File:** `Source/Odin.Glue/Common.Blueprint/entity-human.ngaoblueprint`

```yaml
- id: 'Human'
  component-blueprints:
    - id: 'Trait'
      parameter:
        'EnergyConsumptionRateByEntityStateLookup': <Lookup> (Idle:2, Walking:3, Running:5)
    - id: 'Vitality'
      parameter:
        'Energy': 50.0
        'MaxEnergy': 100.0  # NEW
    - id: 'Rejuvenation'  # NEW
      parameter:
        'RestorationRate': 0.1
    - id: 'Physics'
    - id: 'Intelligence'
    - id: 'Rendering'
      parameter:
        'SpritesheetBlueprintId': 'Humanoid'
        'TextureName': 'entity-placeholder-16x16'
```

### Berry Bush Configuration
**File:** `Source/Odin.Glue/Common.Blueprint/entity-bush.ngaoblueprint`

```yaml
- id: 'BlueberryBush'
  component-blueprints:
    - id: 'Vitality'
    - id: 'Physics'
    - id: 'Harvestable'
      parameter:
        'ResourceBlueprintId': 'Berry'
        'AmountMax': 8
        'EnergyValue': 10.0    # NEW
        'HarvestRadius': 16.0  # NEW
    - id: 'Rendering'
      parameter:
        'SpriteSheetBlueprintId': 'Bush'
        'TextureName': 'entity-placeholder-8x8'

- id: 'Blueberry'
  component-blueprints:
    - id: 'Rejuvenation'
      parameter:
        'Energy': 10  # Note: This might be renamed to EnergyValue for consistency
```

---

## 🧪 Comprehensive Testing Strategy

### Property-Based Testing (FsCheck)

The implementation includes 13 property tests ensuring system correctness:

**Resting System Properties:**
1. Property 1: Resting increases energy proportionally to ticks
2. Property 2: Resting state enforces stationary behavior
3. Property 3: Energy never exceeds maximum capacity
4. Property 4: Transitioning from resting stops restoration

**Harvesting System Properties:**
5. Property 5: Successful harvest transfers correct energy
6. Property 6: Depleted resources cannot be harvested
7. Property 7: Harvest decrements resource quantity
8. Property 8: Harvest range validation

**Decision-Making Properties:**
9. Property 9: Low energy triggers survival behaviors
10. Property 10: Safe energy resumes normal behaviors
11. Property 11: Food-seeking targets nearest available resource

**Component Integrity:**
12. Property 13: Component filtering is correct
13. Property 14: Energy updates modify Vitality Component

### Unit Tests

**Edge Cases:**
- Agent at max energy does not gain additional energy
- Agent with zero restoration rate does not regenerate
- Harvest fails when out of range
- Harvest fails during cooldown
- System skips dead entities
- System clears target when resource depleted

### Integration Tests

**Decision-Making Scenarios:**
- Low-energy agent with nearby resource chooses harvesting
- Low-energy agent without nearby resource chooses resting
- Agent above safe threshold resumes normal behaviors

---

## 🎨 Visual Feedback (Optional Enhancement)

**Godot Client Integration:**
- Resting animation state when `MotionState.Resting`
- Harvest particle effect or animation trigger
- Low energy visual indicator (color tint or icon)
- Energy bar UI element

**File:** `Source/Odin.Client.Godot/ECS.System/RenderingSystem.cs`
- Add handling for new Resting motion state
- Trigger visual effects based on energy level
- Update animations based on harvesting state

---

## 📊 Implementation Plan

### Detailed Task Breakdown

**Complete implementation plan available in:**
`.kiro/specs/mechanic--energy-restoration/tasks.md`

**10-Step Implementation:**
1. Extend components for energy restoration
2. Implement RejuvenationSystem (passive restoration)
3. Implement HarvestingSystem (active restoration)
4. Extend DecisionMakingSystem (survival behaviors)
5. Update blueprint files
6. Register systems in dependency injection
7. Checkpoint - Ensure all tests pass
8. Add visual feedback (Godot client)
9. Create test project infrastructure
10. Final checkpoint - Ensure all tests pass

**Estimated Development Time:** 2-3 days
**Priority:** P0 (Highest - Core Survival Mechanic)

---

## 🎯 Success Criteria

### Functional Requirements
- ✅ Agents can rest to slowly restore energy
- ✅ Agents can harvest berries for fast energy restoration
- ✅ Low energy triggers appropriate survival behavior
- ✅ Safe energy threshold returns to normal behavior
- ✅ Resource depletion and cooldowns working correctly
- ✅ Energy properly capped at maximum

### Performance Requirements
- ✅ <2ms processing time for 100 entities with both systems
- ✅ Efficient resource proximity checks
- ✅ No memory leaks from entity targeting

### Quality Requirements
- ✅ All property tests passing
- ✅ All unit tests passing
- ✅ Integration tests validating decision scenarios
- ✅ Visual feedback working in Godot client

---

## 🔍 Technical Considerations

### Performance Optimizations

**Proximity Calculations:**
- Current approach: O(n*m) where n=agents, m=harvestable resources
- Future optimization: Spatial partitioning for resource lookup
- Acceptable for current scale (<100 entities)

**Energy Thresholds:**
- Constants defined for now (LOW=30, SAFE=70)
- Future: Blueprint-configurable per entity type
- Allows different survival strategies per species

**Harvest Cooldown:**
- Tick-based timing prevents spam harvesting
- Configurable via blueprint for different resource types
- Prevents instant energy refill exploits

### Design Decisions

**Why Two Restoration Modes?**
- Resting: Guarantees agents never starve if food unavailable
- Harvesting: Primary mechanic - faster, more interesting
- Together: Complete survival system with meaningful choices

**Why Energy Thresholds?**
- Low threshold (30%): Early warning, plenty of time to seek food
- Safe threshold (70%): Hysteresis prevents oscillation
- Gap prevents constant state switching

**Why Instant Berry Energy?**
- Simpler implementation (no berry entity lifecycle)
- Immediate feedback for player observation
- Can be enhanced later with inventory system

---

## 🔮 Future Enhancements

For comprehensive roadmap of energy restoration enhancements and code implementations, see:

**Design & Architecture:**
- **ROADMAP_GameMechanic.md** - System 1 (Survival Mechanics) and System 2 (Resource Management)
  - P1: Stomach Component & Digestion System
  - P2: Variable Digestion Rates
  - P3: Multiple Energy Types (Energy + Calories)
  - P4: Inventory System
  - P5: Advanced Food Mechanics (integrated with Cooking & Crafting)
  - P6: Environmental Factors (integrated with Environmental Systems)

**Code Implementations:**
- **SNIPPET_GameMechanic.md** - Complete component and system implementations
  - StomachComponent and FoodItem classes
  - Enhanced MetabolismSystem with digestion
  - Enhanced VitalityComponent with dual energy
  - InventoryComponent structure
  - Stomach-aware DecisionMakingSystem and HarvestingSystem

---

## 🚀 Next Steps

1. **Review this document** - Ensure alignment with project vision
2. **Toggle to Act Mode** - Begin implementation when ready
3. **Follow task plan** - `.kiro/specs/mechanic--energy-restoration/tasks.md`
4. **Test incrementally** - Run tests after each step
5. **Visual polish** - Add Godot client feedback last

**Status:** Ready for implementation - all planning complete!

---

## 📚 Related Documentation

- **SUMMARY_Goal.md** - Project learning platform perspective
- **ROADMAP_GameMechanic.md** - Complete 26-system roadmap
- **AUDIT_CodeQuality.md** - Performance and quality guidelines
- **.kiro/specs/mechanic--energy-restoration/tasks.md** - Detailed implementation steps

---

*Last Updated: November 27, 2025*  
*Status: Planning Complete - Ready for Implementation*  
*Context: This builds on the completed MetabolismSystem to create a complete survival loop*
