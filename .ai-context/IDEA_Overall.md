# God Simulation Sandbox Game Project Summary

## Project Overview

This document summarizes the planning and architectural decisions for creating a god simulation sandbox game using the `Godot` game engine with `C#` scripting. The project draws primary inspiration from `WorldBox`.

### About the Game Concept
This god simulation sandbox game allows players to:
- Create and shape diverse worlds with various terrains and biomes
- Populate worlds with different civilizations (humans, elves, orcs, dwarves)
- Use divine powers (lightning, disasters, blessings)
- Observe emergent behaviors as civilizations develop, interact, and evolve
- Watch wars, alliances, technological progress, and cultural development unfold

### Technical Goals
- Implement the core mechanics of a god simulation sandbox game in `Godot` with `C#`
- Implement Entity Component System (`ECS`) architecture for modularity
- Integrate `NEAT` (NeuroEvolution of Augmenting Topologies) AI for evolving entity behaviors
- Balance procedural systems with AI-driven decision making
- Create scalable simulation supporting hundreds/thousands of entities

## Architecture Overview

### Core Systems Architecture

```
World Management
├── Chunk-based Grid System (16x16 tiles per chunk)
├── Procedural Terrain Generation (Perlin/Simplex noise)
├── Biome Distribution (temperature/moisture based)
└── Resource Placement System

Entity Component System (ECS)
├── Entity Manager (entity lifecycle, component queries)
├── Component Types (data containers)
├── System Types (behavior processors)
└── SystemsManager (update coordination)

Simulation Engine
├── Time Management (pause/play/fast-forward)
├── Physics and Collision
├── Event Queue Processing
└── Performance Optimization (spatial partitioning, LOD)
```

### Key Components Defined

**Core Entity Components:**
- `PositionComponent`: Location and movement tracking
- `MovementComponent`: Speed, direction, pathfinding
- `HealthComponent`: Health management and death states
- `HungerComponent`: Food needs and starvation mechanics
- `AIComponent`: Decision states and timing

**NEAT AI Components:**
- `NeatBrainComponent`: Neural network, genome, fitness tracking
- `ReproductionComponent`: Mating behaviors and evolution

**Specialized Components:**
- `CivilizationComponent`: Group membership and cultural traits
- `CombatComponent`: Fighting capabilities and combat state
- `SocialComponent`: Relationships and social behaviors
- `FarmingComponent`: Agricultural activities

**System Implementations**

**Movement System:**
- A* pathfinding integration
- Collision detection and avoidance
- Destination-based movement with waypoint following

**Hunger System:**
- Food consumption over time
- Starvation damage when food depleted
- Integration with foraging behaviors

**NEAT AI System:**
- Neural network processing for decision making
- Fitness evaluation based on survival metrics
- Population evolution every 10 minutes of game time
- Input processing (environmental sensors, internal state)
- Output interpretation (movement, actions, social behaviors)

## NEAT Integration Details

### Neural Network Architecture
- **Inputs (8)**: Hunger level, health, directional food detection, danger detection, exploration factor
- **Outputs (5)**: Movement directions (N/S/E/W), eating action
- **Evolution**: Population of 100 genomes, tournament selection, crossover and mutation

**NEAT Implementation Components**

**Genome Structure:**
- Node genes (input, hidden, output neurons)
- Connection genes (weighted links with innovation numbers)
- Mutation operators (weight adjustment, node addition, connection addition)

**Fitness Evaluation:**
- Survival factor (health maintenance)
- Hunger management (food acquisition efficiency)
- Decision accuracy (successful action rate)
- Combined fitness: 40% survival + 40% hunger + 20% decisions

**Evolution Process:**
- Elite preservation (top 20%)
- Tournament selection for breeding
- Crossover between fit parents
- Mutation rates: 80% weight changes, 5% node addition, 10% connection addition

## System Balance: NEAT vs Procedural

### NEAT-Controlled Systems
**Best for emergent, adaptive behaviors:**
- **Foraging Strategies**: Food-seeking patterns and efficiency optimization
- **Social Interactions**: Alliance formation, trade decisions, mate selection
- **Combat Decisions**: When to fight, flee, or negotiate
- **Resource Management**: Balancing immediate vs long-term needs

### Procedural Systems
**Best for deterministic, balanced mechanics:**
- **Aging and Lifecycle**: Predictable aging effects and natural death
- **Physical Laws**: Movement physics, collision detection
- **Resource Growth**: Crop maturation, forest regeneration
- **Basic Mechanics**: Damage calculation, building construction

### Hybrid Systems
**Combining both approaches:**
- **Reproduction**: `NEAT` controls mate selection and timing, procedural handles pregnancy/birth
- **Civilization Development**: Procedural for early stages, `NEAT` for advanced decisions
- **Combat**: `NEAT` for tactical decisions, procedural for damage and mechanics

## Development Roadmap

### Phase 1: Core Simulation Foundation (MVP)
**Priority: Establish working simulation loop**
1. Basic world generation (terrain, biomes, resources)
2. Essential entity systems (humans with hunger/health/movement)
3. Time controls (pause/play/speed)
4. Camera system (zoom, pan, selection)

### Phase 2: Depth and Engagement
**Priority: Add compelling AI and interactions**
1. NEAT system implementation for human AI
2. Additional races (elves, dwarves, orcs) with unique traits
3. Basic civilization mechanics (buildings, technology, territories)
4. Divine powers (lightning, rain, earthquakes, blessings)
5. Essential UI (statistics, inspection, power selection, save/load)

### Phase 3: Content and Complexity
**Priority: Rich simulation depth**
1. Advanced civilization features (wars, diplomacy, trade, culture)
2. Environmental complexity (weather, seasons, disasters, resource cycles)
3. Extended powers and creation tools
4. Deep simulation (religion, history, genetics, diseases)

### Critical Early Focus Areas
1. **Engaging World Generation**: Visually interesting and diverse worlds
2. **Complete Entity Lifecycle**: Birth → resource gathering → reproduction → death
3. **Essential UX**: Time controls and camera for observation
4. **Core Divine Powers**: Lightning, terrain modification, creature spawning
5. **NEAT for Humans**: Showcase emergent, evolving behaviors as key differentiator

## Technical Implementation Notes

### Performance Considerations
- Object pooling for frequently created/destroyed entities
- Spatial partitioning (quadtrees) for efficient entity queries
- Variable update frequencies based on entity importance
- Chunk-based rendering with view frustum culling
- `LOD` (Level of Detail) system for distant objects

### Optimization Strategies
- `NEAT` updates at reduced frequency (0.5s intervals) to manage computational load
- Statistical modeling for large populations vs detailed simulation for key entities
- Efficient component queries using type-based entity indexing

### Code Organization
```
/Scripts
  /Components      # All ECS component definitions
  /Systems         # All system implementations
  /NEAT           # Neural network and evolution code
  /World          # World generation and management
  /UI             # User interface systems
  /Utils          # Pathfinding, math utilities, etc.
```

## Key Design Decisions

### Why ECS Architecture?
- Separates data (components) from behavior (systems)
- Enables efficient queries for entities with specific component combinations
- Allows modular addition/removal of features
- Scales well with large numbers of entities

### Why NEAT for AI?
- Creates genuinely emergent behaviors that surprise players
- Entities become more sophisticated over time through evolution
- Computationally efficient once trained
- Provides variable difficulty as entities adapt to challenges

### Hybrid Approach Rationale
- Procedural systems ensure game balance and predictable mechanics
- `NEAT` systems create emergent complexity and replay value
- Computational resources focused where they provide most value
- Development complexity managed by using simpler approaches where appropriate

## Next Steps for Implementation

1. **Repository Setup**: Initialize `Godot` project with proper `C#` configuration
2. **Core ECS Framework**: Implement entity manager and basic component/system structure
3. **World Generation**: Create basic terrain generation and tile system
4. **Basic Entities**: Implement humans with movement and hunger systems
5. **NEAT Integration**: Add neural network framework and evolution mechanics
6. **Iterative Development**: Build and test incrementally, focusing on core simulation loop

## Context for Claude Code

This project represents a complex simulation game requiring:
- Real-time performance optimization
- Complex `AI` system integration
- Modular, maintainable architecture
- Scalable entity management
- Rich emergent gameplay

The codebase will benefit from AI assistance in:
- Implementing `ECS` patterns consistently
- Optimizing `NEAT` algorithm performance
- Creating efficient spatial data structures
- Debugging complex simulation behaviors
- Maintaining code quality across multiple interconnected systems

Key challenge areas where human oversight is critical:
- Performance bottlenecks in real-time simulation
- Balancing `NEAT` evolution parameters
- Ensuring emergent behaviors remain interesting vs chaotic
- Managing complexity as features are added