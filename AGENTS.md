# AGENTS.md

**Last Updated:** January 3, 2026

---

## Table of Contents

- [AGENTS.md](#agentsmd)
  - [Table of Contents](#table-of-contents)
  - [1. Project Overview](#1-project-overview)
  - [2. Quick Start](#2-quick-start)
    - [2.1 Requirements](#21-requirements)
    - [2.2 Setup](#22-setup)
    - [2.3 Common Commands](#23-common-commands)
  - [3. Project Structure](#3-project-structure)
  - [4. ECS Architecture](#4-ecs-architecture)
    - [4.1 Pattern Overview](#41-pattern-overview)
    - [4.2 Adding New Components](#42-adding-new-components)
    - [4.3 Adding New Systems](#43-adding-new-systems)
    - [4.4 Entity States](#44-entity-states)
    - [4.5 Key Systems](#45-key-systems)
  - [5. Godot Integration](#5-godot-integration)
    - [5.1 Setup](#51-setup)
    - [5.2 Assets](#52-assets)
    - [5.3 C# Patterns](#53-c-patterns)
    - [5.4 UI Components](#54-ui-components)
    - [5.5 Debugging](#55-debugging)
  - [6. Common Tasks](#6-common-tasks)
    - [6.1 Create Scenario](#61-create-scenario)
    - [6.2 Add Entity Type](#62-add-entity-type)
    - [6.3 YAML Serialization](#63-yaml-serialization)
  - [7. Standards](#7-standards)
    - [7.1 Code Style](#71-code-style)
    - [7.2 Documentation](#72-documentation)
    - [7.3 AI Toolbox](#73-ai-toolbox)
  - [8. AI Context System](#8-ai-context-system)
    - [8.1 Document Types](#81-document-types)
    - [8.2 Key Rules](#82-key-rules)
  - [9. Resources](#9-resources)
    - [9.1 External](#91-external)
    - [9.2 License](#92-license)
    - [9.3 Author](#93-author)
    - [9.4 Future Work](#94-future-work)

---

## 1. Project Overview

**ai.odin** is an Artificial Life Simulator platform where artificial agents evolve and thrive, enabling experimentation with AI and ML techniques. Uses Entity-Component-System (ECS) architecture.

| Project                | Purpose                                             |
|------------------------|-----------------------------------------------------|
| **Odin.Engine**        | Core simulation engine (ECS, game logic, contracts) |
| **Odin.Client.Godot**  | Godot 4.5.1 visualization client (C#)               |
| **Odin.Glue**          | YAML serialization and embedded resources           |
| **cop.olympus**        | External foundational contracts                     |

**Key Concepts:**
- **Universe**: Game world with configurable dimensions
- **Scenarios**: Simulation setups defined via `ScenarioBlueprint`
- **ECS**: Entities composed of data Components, processed by Systems
- **Blueprints**: YAML configuration for entities, scenarios, animations

## 2. Quick Start

### 2.1 Requirements

| Tool         | Version      | Notes                    |
|--------------|--------------|--------------------------|
| .NET SDK     | 10.0+        |                          |
| Godot Engine | 4.5.1        | Mono/.NET version        |
| Environment  | `GODOT_EXE`  | Path to Godot executable |

**Dependency Management:** Paket (`Source/paket.dependencies`, `Source/paket.lock`)

### 2.2 Setup

```bash
git clone --recursive <repo-url>
set GODOT_EXE=<PATH_TO_GODOT>
dotnet restore Source/nGratis.AI.Odin.sln
dotnet build Source/nGratis.AI.Odin.sln
```

### 2.3 Common Commands

```bash
# Build
dotnet build Source/nGratis.AI.Odin.sln
dotnet build Source/nGratis.AI.Odin.sln -c Release

# Test
dotnet test Source/nGratis.AI.Odin.sln

# Dependencies
dotnet paket update
dotnet paket install
```

**Build Configurations:** Debug, Release, ExportDebug, ExportRelease

**Alternative:** Visual Studio (`Run.DebuggingWithGodot`) or Godot Editor (Play button)

## 3. Project Structure

```
Source/
├── Odin.Engine/              # Core simulation
│   ├── Contract/             # Interfaces (IGameController, IGameState)
│   ├── Contract.Data/        # Data types (Universe, Point, Vector)
│   ├── ECS.Blueprint/        # YAML blueprints
│   ├── ECS.Component/        # Data containers
│   ├── ECS.Component.Logic/  # Logic components
│   ├── ECS.Coordinator/      # Entity management
│   ├── ECS.System/           # Behavior processors
│   └── ECS.System.Logic/     # Logic systems
├── Odin.Client.Godot/        # Visualization
│   ├── Common/               # Utilities, TimeTracker
│   ├── Common.Art/           # Sprites, textures
│   ├── Common.Font/          # FiraCode fonts
│   ├── Common.UI/            # HUD, overlays, camera
│   ├── ECS.*/                # Client-side ECS
│   └── Stage/                # Scenes (Universe.tscn)
├── Odin.Glue/                # Serialization
│   ├── Serializer/           # YAML serializers
│   └── EmbeddedDataStore.cs  # Resource loading
└── Odin.Glue.UnitTest/       # Tests (xUnit)
```

**Key Files:** `GameController.cs`, `GameState.cs`, `EntityCoordinator.cs`, `ScenarioBlueprint.cs`, `AppBootstrapper.cs`

## 4. ECS Architecture

### 4.1 Pattern Overview

| Element        | Purpose              | Location           | Key Rule                                    |
|----------------|----------------------|--------------------|---------------------------------------------|
| **Components** | Pure data containers | `ECS.Component/`   | NO logic, only fields/properties            |
| **Systems**    | Process entities     | `ECS.System/`      | Use `[SystemMetadata(OrderingIndex = n)]`   |
| **Entities**   | Component containers | `ECS.Entity/`      | Created via `IEntityFactory`                |
| **Blueprints** | YAML configuration   | `ECS.Blueprint/`   | Loaded by Glue serializers                  |

### 4.2 Adding New Components

1. Create class in `ECS.Component/`, implement `IComponent`
2. Add data properties only (no methods)
3. Register in `ComponentFactory` if needed
4. Create processing System if behavior required

### 4.3 Adding New Systems

1. Create class in `ECS.System/`
2. Implement variable/fixed duration processing
3. Add `[SystemMetadata(OrderingIndex = n)]` attribute
4. Register in DI container (auto-executed by `GameController`)

### 4.4 Entity States

| State   | Behavior                 |
|---------|--------------------------|
| Idle    | Static, no movement      |
| Walking | Slow movement, basic AI  |
| Running | Fast movement, animated  |
| Dead    | Inactive                 |

State transitions managed by `DecisionMakingSystem`.

### 4.5 Key Systems

- `DecisionMakingSystem`: AI state transitions
- `MovementSystem`: Physics and position updates
- `RenderingSystem`: Visual updates and animations
- `DebuggingSystem`: Performance monitoring

## 5. Godot Integration

### 5.1 Setup

| Item         | Value                                          |
|--------------|------------------------------------------------|
| Version      | 4.5.1 (Mono/.NET)                              |
| Main Scene   | `Source/Odin.Client.Godot/Stage/Universe.tscn` |
| Project File | `Source/Odin.Client.Godot/project.godot`       |

### 5.2 Assets

| Type       | Location                         | Notes                       |
|------------|----------------------------------|-----------------------------|
| Sprites    | `Common.Art/`                    | `.png` with `.import` files |
| Fonts      | `Common.Font/`                   | FiraCode family             |
| Source Art | `Asset/`                         | Aseprite format             |
| Palette    | `Asset/PALETTE_resurrect-64.gpl` | Resurrect 64                |

### 5.3 C# Patterns

**Node Structure:** Inherit Godot nodes, use `[GlobalClass]` for editor visibility

**Resource Loading:** `GD.Load<T>()`, `PackedScene`, paths as `res://path/to/resource`

**Entity Rendering:** `RenderableEntity.cs` (Node2D), `RenderingComponent.cs` (state), `RenderingSystem.cs` (updates)

### 5.4 UI Components

`HeadUpDisplay`, `DiagnosticsOverlay`, `StatisticsOverlay`, `Camera`, `SimulationController`

### 5.5 Debugging

1. Set `GODOT_EXE` environment variable
2. Use VS launch config `Run.DebuggingWithGodot`
3. Breakpoints work in C# code
4. Use `GD.Print()` for console output

## 6. Common Tasks

### 6.1 Create Scenario

1. Define `ScenarioBlueprint` YAML with `UniverseBlueprint` and `EntityPopulationBlueprint`
2. Place in data location, load via `EmbeddedDataStore`
3. `EntitySpawningSystem` processes spawning

### 6.2 Add Entity Type

1. Create `EntityBlueprint` with component blueprints
2. Add `ComponentBlueprint.Rendering` if visual
3. Register new components in `ComponentFactory`
4. Create via `IEntityFactory`

### 6.3 YAML Serialization

- Serializers: `Source/Odin.Glue/Serializer/`
- Custom types: Extend `YamlSerializationExtensions`
- Tests: `YamlSerializationExtensionsTests`

## 7. Standards

### 7.1 Code Style

See `.ai-toolbox/rules/RULE_CSharp.md` for complete standards.

**Quick Reference:**
- Private fields: `_camelCase`
- Opening brace: New line for classes/methods/control flow
- Access modifiers: Always explicit
- Namespace: Must match folder hierarchy

### 7.2 Documentation

See `.ai-toolbox/rules/RULE_Markdown.md` and `.ai-toolbox/rules/RULE_Document.md`.

**Quick Reference:**
- Last Updated: `**Last Updated:** Month DD, YYYY`
- Headings: H2 sequential (1, 2, 3...), H3 relative (1.1, 1.2...)
- Paths: Folders end with `/`, files do not

### 7.3 AI Toolbox

| File                      | Purpose                                    |
|---------------------------|--------------------------------------------|
| `rules/RULE_CSharp.md`    | C# coding standards                        |
| `rules/RULE_Markdown.md`  | Markdown formatting                        |
| `rules/RULE_Document.md`  | Document content rules                     |
| `rules/RULE_Persona.md`   | Expert persona reference                   |
| `skills/format-markdown/` | Markdown validation                        |
| `skills/review-markdown/` | Markdown optimization for token reduction  |
| `skills/audit-codebase/`  | Codebase auditing                          |

## 8. AI Context System

The `.ai-context/` folder contains curated documents for feature planning and implementation.

### 8.1 Document Types

| Prefix       | Purpose                                     | When to Read                 |
|--------------|---------------------------------------------|------------------------------|
| `IDEA_*`     | Game feature discussions and ideas          | Before implementing features |
| `ROADMAP_*`  | Prioritized development features (future)   | During sprint planning       |
| `SNIPPET_*`  | Code references                             | During implementation        |
| `SUMMARY_*`  | Quick references                            | First when starting work     |
| `AUDIT_*`    | Quality assessments                         | During refactoring           |

### 8.2 Key Rules

**IDEA/SNIPPET Alignment:** Section numbers MUST match exactly (e.g., IDEA 2.1 = SNIPPET 2.1). Gaps allowed if no code for that section.

**Workflow:**
1. Read `SUMMARY_*` for project state
2. Read `IDEA_*` for feature discussions
3. Use `SNIPPET_*` during coding
4. Reference `AUDIT_*` for quality

See `.ai-toolbox/rules/RULE_Document.md` for complete document standards.

## 9. Resources

### 9.1 External

- **Palette**: [Resurrect 64](https://lospec.com/palette-list/resurrect-64)
- **Fonts**: [FiraCode](https://github.com/tonsky/FiraCode), [Mono10](https://jdjimenez.itch.io/mono10)

### 9.2 License

MIT License — See LICENSE file.

### 9.3 Author

Cahya Ong — cahya.ong@gmail.com

### 9.4 Future Work

**Planned:** Integration tests, additional scenarios, performance profiling, extended behaviors

**Limitations:** No integration tests yet, Windows-specific Godot integration, limited scenarios
