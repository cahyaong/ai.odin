# IDEA: Energy Restoration Mechanic

**Last Updated:** January 5, 2026

---

## Table of Contents

- [IDEA: Energy Restoration Mechanic](#idea-energy-restoration-mechanic)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
    - [1.1 Vision](#11-vision)
  - [2. System Architecture](#2-system-architecture)
    - [2.1 Complete Energy Lifecycle](#21-complete-energy-lifecycle)
    - [2.2 Two-Mode Restoration Design](#22-two-mode-restoration-design)
      - [Mode 1: Passive Restoration (Resting)](#mode-1-passive-restoration-resting)
      - [Mode 2: Active Restoration (Harvesting)](#mode-2-active-restoration-harvesting)
    - [2.3 Decision-Making Integration](#23-decision-making-integration)
    - [2.4 Data Flow](#24-data-flow)
  - [3. Data Model](#3-data-model)
    - [3.1 Vitality Data Extensions](#31-vitality-data-extensions)
    - [3.2 Rejuvenation Data](#32-rejuvenation-data)
    - [3.3 Harvestable Resource Data](#33-harvestable-resource-data)
    - [3.4 Intelligence Data](#34-intelligence-data)
    - [3.5 Motion State Extensions](#35-motion-state-extensions)
  - [4. Processing Systems](#4-processing-systems)
    - [4.1 Passive Restoration Processor](#41-passive-restoration-processor)
    - [4.2 Active Harvesting Processor](#42-active-harvesting-processor)
    - [4.3 Decision Making Processor](#43-decision-making-processor)
  - [5. Configuration](#5-configuration)
    - [5.1 Entity Configuration](#51-entity-configuration)
    - [5.2 Visual Feedback](#52-visual-feedback)
  - [6. Testing Strategy](#6-testing-strategy)
    - [6.1 Property-Based Testing](#61-property-based-testing)
    - [6.2 Unit Tests](#62-unit-tests)
    - [6.3 Integration Tests](#63-integration-tests)
  - [7. Technical Considerations](#7-technical-considerations)
    - [7.1 Performance Optimization](#71-performance-optimization)
    - [7.2 Design Decisions](#72-design-decisions)
    - [7.3 Architectural Principles](#73-architectural-principles)
  - [8. Success Criteria](#8-success-criteria)
    - [8.1 Functional Requirements](#81-functional-requirements)
    - [8.2 Performance Requirements](#82-performance-requirements)
    - [8.3 Quality Requirements](#83-quality-requirements)
  - [9. Future Enhancements](#9-future-enhancements)
    - [9.1 Advanced Features](#91-advanced-features)
    - [9.2 Optimization Opportunities](#92-optimization-opportunities)
    - [9.3 Research Opportunities](#93-research-opportunities)

---

## 1. Overview

### 1.1 Vision

The Energy Restoration Mechanic provides a dual-mode energy recovery system that complements the existing metabolism processor (energy consumption) to create a complete survival loop for agents. This feature enables agents to:

- **Passively restore energy** through resting when resources are unavailable
- **Actively restore energy** through harvesting food resources
- **Make intelligent survival decisions** based on energy levels and resource availability

This mechanic transforms energy from a simple drain into a meaningful survival challenge where agents must balance activity with recovery, demonstrating adaptive behavior in resource-constrained environments.

## 2. System Architecture

### 2.1 Complete Energy Lifecycle

The energy restoration mechanic integrates with the existing metabolism processor to form a complete survival cycle:

```
+-----------------------------------------------------------------+
|  ENERGY CONSUMPTION (Metabolism Processor)                      |
|  - Continuous drain based on activity level                     |
|  - Idle: 2 energy/tick                                          |
|  - Walking: 3 energy/tick                                       |
|  - Running: 5 energy/tick                                       |
|  - Death when energy reaches 0                                  |
+-----------------------------------------------------------------+
                               |
                               v
+-----------------------------------------------------------------+
|  SURVIVAL DECISION MAKING                                       |
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
|                              |   |                              |
|  Rejuvenation Processor      |   |  Harvesting Processor        |
|  - Resting state             |   |  - Proximity-based           |
|  - Slow regen                |   |  - Instant energy            |
|  - +1 energy/10 ticks        |   |  - +10 energy/berry          |
|  - No food required          |   |  - Resource depletion        |
+------------------------------+   +------------------------------+
```

### 2.2 Two-Mode Restoration Design

#### Mode 1: Passive Restoration (Resting)

**Purpose:** Fallback energy recovery when no food is available

**Mechanism:**
- Activates when agent enters Resting motion state
- Provides slow but reliable energy restoration
- Rate: +1 energy per 10 ticks (configurable via blueprint)
- Requires agent to be stationary (zero velocity)
- Energy capped at maximum capacity
- No resource consumption required

**Use Cases:**
- Agent cannot find food resources
- Agent is waiting for resources to respawn
- Emergency fallback to prevent starvation

#### Mode 2: Active Restoration (Harvesting)

**Purpose:** Primary energy recovery through resource consumption

**Mechanism:**
- Activates when agent is near harvestable resource
- Transfers energy from resource to agent
- Rate: +10 energy per berry (instant, configurable)
- Proximity requirement: within harvest radius
- Cooldown mechanism prevents spam harvesting
- Decrements resource quantity
- Clears target when resource depleted

**Use Cases:**
- Primary energy restoration method
- More efficient than resting (10x faster)
- Requires environmental resources
- Demonstrates resource management behavior

### 2.3 Decision-Making Integration

The decision-making processor orchestrates survival behavior based on energy thresholds:

**Low Energy State (< 30 energy):**
1. Detect energy below threshold
2. Search for nearest harvestable resource with available quantity
3. If resource found:
   - Set target entity identifier
   - Navigate toward resource
   - Harvesting processor handles energy transfer
4. If no resource found:
   - Switch to Resting motion state
   - Rejuvenation processor provides slow recovery
   - Continue until resource appears or energy safe

**Safe Energy State (> 70 energy):**
1. Detect energy above threshold
2. Clear target entity identifier
3. Resume normal AI behaviors
4. Return to standard activity patterns

**Threshold Hysteresis:**
- 40-point gap between thresholds (30% to 70%)
- Prevents rapid state oscillation
- Allows time for food-seeking or resting
- Creates predictable behavior patterns

### 2.4 Data Flow

**Complete Survival Cycle:**
```
1. Agent Spawns
   └─> Initial Energy: 50 / MaxEnergy: 100

2. Activity Phase
   └─> Energy drains continuously (Metabolism Processor)
       - Rate depends on motion state
       - Continuous background process

3. LOW ENERGY THRESHOLD (< 30 energy)
   └─> Decision-Making Processor activates survival mode
       
   3a. Resource Available?
       YES → Navigate to berry bush
           → Harvesting Processor activates when in range
           → Harvest berry: +10 energy (instant)
           → Resource quantity decrements
           → Repeat until Energy > 70
           
   3b. No Resource Nearby?
       NO → Switch to Resting state
          → Rejuvenation Processor activates
          → Slow restoration: +0.1 energy/tick
          → Continue until Energy > 70 or food found

4. SAFE ENERGY THRESHOLD (> 70 energy)
   └─> Clear harvest target
   └─> Resume normal AI behaviors
   └─> Return to Activity Phase (step 2)

5. Energy Reaches 0
   └─> Death (Metabolism Processor handles)
```

## 3. Data Model

### 3.1 Vitality Data Extensions

Extend existing vitality data container to support energy restoration:

- Maximum energy capacity tracking (default 100.0)
- Helper calculations: full energy detection, energy percentage
- Enables energy capping and UI visualization

### 3.2 Rejuvenation Data

New data container for passive restoration:

- Restoration rate: energy gained per tick while resting
- Harvest cooldown tracking: timestamp of last harvest action
- Configurable via blueprint for different entity types

### 3.3 Harvestable Resource Data

Extend existing resource data with energy properties:

- Energy value: amount of energy provided when consumed (default 10.0)
- Harvest radius: proximity required for harvesting (default 16.0)

### 3.4 Intelligence Data

Extend intelligence data for target tracking:

- Target tracking: nullable reference to target entity
- Enables food-seeking behavior coordination

### 3.5 Motion State Extensions

Add Resting state for passive restoration:

- Distinct from Idling - intentional recovery behavior
- Triggers rejuvenation processing

## 4. Processing Systems

### 4.1 Passive Restoration Processor

**Objective:** Resting-based energy recovery

**Ordering:** Executes after metabolism processing, before decision-making

**Required Data:** Vitality data, rejuvenation data

**Processing Logic:**
1. Query entities with vitality and rejuvenation data
2. Filter for entities in Resting motion state
3. Skip dead entities and entities at maximum energy
4. Calculate energy gain: restoration rate × elapsed ticks
5. Apply energy increase, capped at maximum
6. Enforce stationary behavior (zero velocity)

**Configuration:**
- Restoration rate configurable per entity type via blueprint
- Default: 0.1 energy per tick (10 ticks = 1 energy)
- Balances with metabolism rates (2-5 energy per tick drain)

### 4.2 Active Harvesting Processor

**Objective:** Resource-based energy restoration

**Ordering:** Executes after rejuvenation processing, before decision-making

**Required Data:** Vitality data, rejuvenation data, intelligence data

**Processing Logic:**
1. Query entities with vitality, rejuvenation, and intelligence data
2. Skip dead entities and entities without target
3. Validate target entity has harvestable data
4. Check harvest conditions:
   - Target has available resources (amount > 0)
   - Agent within harvest radius
   - Harvest cooldown expired
   - Agent not at maximum energy
5. Execute harvest:
   - Transfer energy from resource to agent
   - Decrement resource amount
   - Update harvest cooldown timestamp
   - Cap energy at maximum
6. Clear target if resource depleted

**Proximity Detection:**
- Calculate distance between agent and resource positions
- Compare against harvestable harvest radius
- Current approach: O(n×m) acceptable for <100 entities
- Future optimization: Spatial partitioning

**Cooldown Mechanism:**
- Tick-based timing prevents spam harvesting
- Default: 10 ticks between harvests (configurable)
- Prevents instant energy refill exploits

### 4.3 Decision Making Processor

**Objective:** Survival behavior coordination

**Enhancement Areas:**

**Low Energy Detection:**
- Monitor vitality energy levels
- Threshold: < 30 energy (30% of max)
- Triggers survival mode decision tree

**Resource Search Logic:**
1. Query universe for entities with harvestable data
2. Filter for resources with available quantity
3. Calculate distances from agent position
4. Select nearest available resource
5. Set target entity identifier
6. Update motion state to navigate toward target

**Resting Fallback:**
- Activated when no resources found
- Set motion state to Resting
- Ensure zero velocity in physics data
- Continue until energy safe or resource appears

**Safe Energy Restoration:**
- Threshold: > 70 energy (70% of max)
- Clear target entity identifier
- Reset to normal AI behavior patterns
- Resume standard decision-making logic

**State Transition Rules:**
```
Normal State:
  └─> Energy < 30? → Survival Mode
  
Survival Mode:
  ├─> Resource Available? → Food Seeking
  │   └─> In Range? → Harvesting
  │   └─> Not In Range? → Navigating
  │
  └─> No Resource? → Resting
      └─> Energy > 70? → Normal State
```

## 5. Configuration

### 5.1 Entity Configuration

**Human Entity:**
- Vitality: Add maximum energy parameter (100.0)
- Rejuvenation: Restoration rate (0.1)

**Berry Bush:**
- Harvestable: Energy value (10.0), harvest radius (16.0)

**Blueprint Serialization:**
- Uses existing YAML serialization
- Loaded via embedded data store
- Validated during deserialization
- Default values provided for backward compatibility

### 5.2 Visual Feedback

**Rendering Updates:**
- Add handling for Resting motion state
- Trigger resting animation when state detected
- Different from Idling animation (intentional recovery pose)

**Energy Level Indicators:**
- Low energy visual tint (red hue when < 30 energy)
- Safe energy indicator restoration (normal color > 70)
- Color gradient based on energy percentage

**Harvesting Effects:**
- Particle effect or animation on successful harvest
- Visual feedback for energy transfer
- Resource depletion animation

**HUD Integration:**
- Energy percentage display
- Current restoration mode indicator
- Target resource highlighting

## 6. Testing Strategy

### 6.1 Property-Based Testing

**Resting Properties:**
1. **Resting Increases Energy:** Agent with energy < max in Resting state gains energy proportional to elapsed ticks
2. **Resting Enforces Stationary:** Agent in Resting state has zero velocity
3. **Energy Never Exceeds Maximum:** Restoration capped at maximum energy
4. **State Transition Stops Restoration:** Non-Resting state stops energy gain

**Harvesting Properties:**
5. **Harvest Transfers Energy:** Agent near resource gains energy equal to resource energy value
6. **Depleted Resources Cannot Harvest:** Resource with amount = 0 prevents energy transfer
7. **Harvest Decrements Resource:** Resource amount decrements by 1 per harvest
8. **Range Validation:** Agent outside harvest radius cannot harvest

**Decision-Making Properties:**
9. **Low Energy Triggers Survival:** Energy < 30 activates survival mode
10. **Safe Energy Resumes Normal:** Energy > 70 in survival mode returns to normal behavior
11. **Food Seeking Targets Nearest:** Multiple resources available selects nearest

**Data Integrity:**
12. **Data Filtering:** Only entities with required data are processed
13. **Energy Updates Modify Vitality:** Energy modifications reflect in vitality data

### 6.2 Unit Tests

**Edge Cases:**
- Agent at max energy does not gain additional energy
- Agent with zero restoration rate does not regenerate
- Harvest fails when out of range
- Harvest fails during cooldown period
- Processor skips dead entities correctly
- Processor clears target when resource fully depleted
- Resting with positive restoration rate increases energy
- Harvesting with available resource transfers energy
- Energy capping prevents overflow

**Processor Behavior:**
- Rejuvenation processor only processes Resting entities
- Harvesting processor validates target entity exists
- Decision-making processor prioritizes survival mode
- Cooldown mechanism prevents rapid harvesting
- Resource depletion clears agent target

### 6.3 Integration Tests

**Decision-Making Scenarios:**
- Low-energy agent with nearby resource chooses harvesting
- Low-energy agent without nearby resource chooses resting
- Agent above safe threshold resumes normal behaviors
- Agent transitions from harvesting to normal when safe
- Agent transitions from resting to normal when safe
- Agent finds new resource when current depletes

**Complete Survival Cycle:**
- Agent spawns, consumes energy through activity
- Reaches low threshold, seeks food
- Harvests resource, gains energy
- Returns to normal behavior above safe threshold
- Complete cycle repeats with resource respawn

**Processor Interactions:**
- Metabolism processor drains energy while harvesting processor restores
- Rejuvenation processor provides fallback when harvesting inactive
- Decision-making processor coordinates state transitions
- Resource regrowth processor interacts with harvest depletion

## 7. Technical Considerations

### 7.1 Performance Optimization

**Proximity Calculations:**
- Current approach: O(n×m) where n=agents, m=harvestable resources
- Acceptable for current scale (<100 entities)
- Future optimization: Spatial partitioning (quadtree or grid)
- Trade-off: Simplicity vs. scalability
- Target performance: <2ms for 100 entities with both processors

**Entity Queries:**
- Processors use data type filtering
- Efficient lookup via entity coordinator indexing
- Minimal overhead per entity processed
- Dead entity filtering reduces unnecessary work

**Memory Considerations:**
- Minimal additional memory per entity (new data properties)
- No entity pooling changes required
- Target entity references use integer identifiers (4 bytes)
- No string allocations in hot paths

### 7.2 Design Decisions

**Why Two Restoration Modes?**
- **Resting:** Guarantees agents never starve if food unavailable
- **Harvesting:** Primary mechanic - faster, more interesting gameplay
- **Together:** Complete survival system with meaningful choices
- **Balance:** Resting prevents death, harvesting enables thriving

**Why Energy Thresholds?**
- **Low Threshold (30%):** Early warning, plenty of time to seek food
- **Safe Threshold (70%):** Hysteresis prevents oscillation between states
- **Gap (40%):** Prevents constant state switching, creates stable behaviors
- **Future:** Could be blueprint-configurable per entity type

**Why Instant Berry Energy?**
- **Simplicity:** No berry entity lifecycle management
- **Immediate Feedback:** Clear cause-effect for observation
- **Performance:** No additional entities to track
- **Future Enhancement:** Can add stomach/digestion module later

**Why Cooldown Mechanism?**
- **Prevents Exploits:** Stops instant energy refill when standing on resource
- **Realistic Pacing:** Mimics time required to consume food
- **Balance:** Forces agents to manage resources, not camp on them
- **Tunable:** Configurable per resource type via blueprint

### 7.3 Architectural Principles

**ECS Pattern Adherence:**
- Data containers remain pure data
- Processors contain all behavior logic
- No circular dependencies between processors
- Processor ordering via metadata attributes

**Separation of Concerns:**
- Rejuvenation processor: Passive restoration only
- Harvesting processor: Active restoration only
- Decision-making processor: Behavior coordination only
- Each processor has single, clear responsibility

**Blueprint-Driven Design:**
- All tunable parameters in YAML blueprints
- No hardcoded game constants in processors
- Entity behavior customizable without code changes
- Supports multiple entity types with different strategies

**Testability:**
- Property-based tests validate mathematical properties
- Unit tests cover edge cases and error conditions
- Integration tests validate processor interactions
- All processors independently testable

## 8. Success Criteria

### 8.1 Functional Requirements

**Core Mechanics:**
- Agents can rest to slowly restore energy
- Agents can harvest berries for fast energy restoration
- Low energy triggers appropriate survival behavior
- Safe energy threshold returns agents to normal behavior
- Resource depletion and cooldowns work correctly
- Energy properly capped at maximum capacity

**Decision Making:**
- Agents seek food when energy low and food available
- Agents rest when energy low and no food available
- Agents resume normal behavior when energy safe
- Target tracking works correctly for food-seeking
- Target cleared when resource depleted

**Processor Interactions:**
- Metabolism processor and restoration processors balance correctly
- Multiple agents can harvest same resource sequentially
- Resource respawn integrates with harvest depletion
- Visual feedback reflects energy restoration states

### 8.2 Performance Requirements

**Processing Time:**
- <2ms total processing time for 100 entities with both processors
- Rejuvenation processor: <0.5ms for 50 resting entities
- Harvesting processor: <1.0ms for 50 harvesting entities
- Decision-making processor: <0.5ms overhead for survival logic

**Memory Usage:**
- No memory leaks from entity targeting
- Minimal memory increase per entity (<50 bytes)
- No unbounded collection growth
- Efficient entity reference management

**Scalability:**
- Processors perform acceptably up to 200 entities
- Proximity checks remain efficient
- No O(n²) algorithms in hot paths
- Resource queries optimized via data filtering

### 8.3 Quality Requirements

**Testing Coverage:**
- All property tests passing (13 properties)
- All unit tests passing (edge cases and behaviors)
- Integration tests validating decision scenarios
- No regressions in existing metabolism tests

**Code Quality:**
- Follows project coding standards
- Clear separation of concerns
- Comprehensive documentation
- No magic numbers (all constants in blueprints)

**Visual Feedback:**
- Resting animation state working in client
- Harvest visual effects functioning (if implemented)
- Energy level indicators accurate (if implemented)
- UI elements responsive and informative (if implemented)

**Documentation:**
- Implementation matches this idea document
- Code comments explain complex logic
- Blueprint parameters documented
- Test cases document expected behaviors

## 9. Future Enhancements

### 9.1 Advanced Features

**Stomach Module & Digestion Processing**
- Multi-stage energy restoration (ingestion → digestion → energy)
- Stomach capacity and fullness tracking
- Food item queue with digestion rates
- Gradual energy release over time
- More realistic eating behavior

**Variable Digestion Rates**
- Different food types digest at different speeds
- High-energy foods take longer to digest
- Low-energy foods digest quickly
- Creates food choice trade-offs

**Multiple Energy Types**
- Separate Energy (immediate) and Calories (stored)
- Metabolism converts calories to energy
- Enables true hunger vs. exhaustion distinction
- Supports advanced survival mechanics

**Inventory Module**
- Agents can carry food items
- Eat from inventory when energy low
- Enables food hoarding behavior
- Storage capacity limits

**Advanced Food Mechanics**
- Integrated with Cooking & Crafting modules
- Food quality affects restoration rates
- Recipe-based food creation
- Spoilage and preservation

**Environmental Factors**
- Integrated with Environmental modules
- Temperature affects metabolism rates
- Weather impacts foraging success
- Shelter provides resting bonuses

### 9.2 Optimization Opportunities

**Spatial Partitioning:**
- Implement quadtree or grid for resource lookups
- Reduces proximity calculation complexity
- Scales to larger entity counts
- Trade-off: Additional memory overhead

**Configurable Thresholds:**
- Move LOW_ENERGY and SAFE_ENERGY to blueprints
- Per-entity-type survival strategies
- Enables species diversity (risk-takers vs. conservative)
- Supports experimentation and tuning

**Smart Resource Tracking:**
- Cache nearest resource per agent
- Invalidate cache only when resources change
- Reduces redundant distance calculations
- Improves decision-making performance

**Predictive Behaviors:**
- Agents anticipate energy needs based on planned activity
- Seek food before reaching low threshold
- More intelligent resource management
- Requires enhanced AI module

### 9.3 Research Opportunities

**Machine Learning Integration:**
- Train agents to optimize survival strategies
- Learn when to rest vs. seek food
- Discover efficient resource usage patterns

**Social Behaviors:**
- Food sharing between agents
- Resource competition and territories
- Cooperative hunting or gathering
- Group survival strategies

**Ecosystem Dynamics:**
- Resource availability affects population
- Predator-prey energy transfer
- Food chain simulation
- Evolutionary pressure on strategies
