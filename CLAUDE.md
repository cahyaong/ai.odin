# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Development Commands

**Building the solution:**
```bash
dotnet build Source/nGratis.AI.Odin.sln
```

**Running tests:**
```bash
dotnet test Source/nGratis.AI.Odin.sln
```

**Running specific test project:**
```bash
dotnet test Source/Odin.Glue.UnitTest/nGratis.AI.Odin.Glue.UnitTest.csproj
```

**Package management (Paket):**
```bash
# From Source/ directory
paket install
paket update
```

**Running the Godot client:**
- Open the project in Godot using `Source/Odin.Client.Godot/project.godot`
- The main scene is `Stage/Universe.tscn`
- For debugging: Use the `Run.DebuggingWithGodot` profile in launchSettings.json

## Architecture Overview

This is an **Artificial Life Simulator** built with a clean **Entity-Component-System (ECS)** architecture:

### Core Projects Structure
- **Odin.Engine** - Core ECS framework and game logic (platform-agnostic)
- **Odin.Client.Godot** - Godot game engine integration and rendering
- **Odin.Glue** - Serialization, data management, and cross-cutting concerns
- **External/cop.olympus** - Shared infrastructure library (logging, validation, etc.)

### ECS Architecture

**Entities:** Managed by `EntityManager`, created via `EntityFactory` using blueprint templates

**Components (data-only):**
- `PhysicsComponent` - Position and velocity for movement
- `IntelligenceComponent` - AI state, decision timing, target positions
- `RenderingComponent` - Visual representation (Godot integration)

**Systems (behavior):**
- `DecisionMakingSystem` - AI state transitions (Idle → Walking/Running)
- `MovementSystem` - Physics updates and position changes  
- `RenderingSystem` - Visual updates and animations
- `DebuggingSystem` - Performance monitoring

**Key System Patterns:**
- Systems inherit from `BaseFixedSystem` for consistent timestep processing
- Systems declare `RequiredComponentTypes` for automatic entity filtering
- Entity queries are optimized through component type indexing
- System execution order controlled via `SystemMetadataAttribute`

### Entity States and Behavior
Entities have behavioral states defined in `EntityState` enum:
- `Idle` - Static, no movement
- `Walking` - Slow movement with basic AI
- `Running` - Fast movement with animated sprites
- `Dead` - Inactive state

### Data Management
- **Paket** for NuGet package management
- **YamlDotNet** for blueprint serialization
- Entity blueprints stored in `.ngaoblueprint` files
- Embedded data store for game assets

### Platform Integration
- **.NET 9.0** target framework
- **Godot 4.4** game engine for rendering and input
- Clean separation between engine logic and client presentation
- Reactive Extensions for event handling

## Development Notes

- Entity creation uses factory pattern with blueprint-based configuration
- Components are pure data containers - no behavior methods
- Systems process entities in bulk operations for performance
- Godot integration handles visual representation while engine manages logic
- Debug overlays provide FPS, entity counts, and system performance metrics