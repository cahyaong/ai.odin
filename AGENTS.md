# AGENTS.md

**Last Updated:** December 21, 2025

---

## 1. Project Overview

**ai.odin** is an Artificial Life Simulator platform where artificial agents evolve and thrive, enabling experimentation and refinement of AI and ML techniques. The project uses an Entity-Component-System (ECS) architecture for flexible and performant simulation.

### 1.1 Architecture

- **Odin.Engine**: Core simulation engine containing ECS implementation, game logic, and contracts
- **Odin.Client.Godot**: Godot 4.5.1-based visualization client with C# integration
- **Odin.Glue**: Data serialization layer handling YAML blueprints and embedded resources
- **External/cop.olympus**: External dependency providing foundational contracts

### 1.2 Key Concepts

- **Universe**: The game world representation with configurable dimensions
- **Scenarios**: Configurable simulation setups defined via ScenarioBlueprint
- **Entity-Component-System (ECS)**: Entities are composed of data Components, processed by Systems
- **Time Tracking**: Systems process both variable-duration (frame-based) and fixed-duration (tick-based) updates
- **Blueprints**: YAML-based configuration data for entities, scenarios, animations, and sprites

## 2. Development Environment Setup

### 2.1 Required Tools

- **.NET SDK**: Version 10.0 or later
- **Godot Engine**: Version 4.5.1
- **IDE**: Visual Studio 2022 (recommended) or any C# IDE
- **Environment Variable**: `$(GODOT_EXE)` must point to Godot executable path on Windows

### 2.2 Dependency Management

This project uses **Paket** for dependency management:

- Dependencies defined in `Source/paket.dependencies`
- Project references in `Source/<ProjectName>/paket.references`
- Lock file at `Source/paket.lock`

### 2.3 Initial Setup

1. Clone the repository with submodules: `git clone --recursive <repo-url>`
2. Set environment variable: `set GODOT_EXE=<_PATH_TO_YOUR_GODOT_EXE_>`
3. Restore dependencies: `dotnet restore Source/nGratis.AI.Odin.sln`
4. Build the solution: `dotnet build Source/nGratis.AI.Odin.sln`

## 3. Build Commands

### 3.1 Primary Build Method

```bash
# Build entire solution
dotnet build Source/nGratis.AI.Odin.sln

# Build specific project
dotnet build Source/Odin.Engine/nGratis.AI.Odin.Engine.csproj

# Build with specific configuration
dotnet build Source/nGratis.AI.Odin.sln -c Release
```

### 3.2 Available Build Configurations

- **Debug**: Development build with debug symbols
- **Release**: Optimized release build
- **ExportDebug**: Godot export configuration (debug)
- **ExportRelease**: Godot export configuration (release)

### 3.3 Alternative Build Methods

- **Visual Studio**: Use `Run.DebuggingWithGodot` launch configuration
- **Godot Editor**: Open scene and click "Play" button

### 3.4 Paket Commands

```bash
# Update dependencies
dotnet paket update

# Install dependencies
dotnet paket install

# Add new package to project
dotnet paket add <package-name> --project Source/<ProjectName>
```

## 4. Testing

### 4.1 Running Tests

```bash
# Run all unit tests
dotnet test Source/nGratis.AI.Odin.sln

# Run tests for specific project
dotnet test Source/Odin.Glue.UnitTest/nGratis.AI.Odin.Glue.UnitTest.csproj

# Run tests with detailed output
dotnet test Source/nGratis.AI.Odin.sln --verbosity normal
```

### 4.2 Test Organization

- **Unit Tests**: Located in `Source/Odin.Glue.UnitTest/`
- **Test Framework**: xUnit
- **Naming Convention**: `<ClassName>Tests.<MethodName>.cs`
- **Integration Tests**: Not yet implemented (future enhancement)

### 4.3 Testing Guidelines

- Write unit tests for new components and systems
- Test blueprints and serialization in Glue layer
- Ensure tests are deterministic and independent
- Mock external dependencies appropriately

## 5. Code Style & Conventions

Follow standard C# coding conventions with the specific requirements described in the following subsections.

### 5.1 Naming Conventions

- **Private fields**: MUST be prefixed with underscore (e.g., `_fieldName`)
- **Public properties**: PascalCase (e.g., `PropertyName`)
- **Methods**: PascalCase (e.g., `MethodName`)
- **Local variables**: camelCase (e.g., `variableName`)
- **Constants**: PascalCase (e.g., `MaxValue`)
- **Interfaces**: Prefix with 'I' (e.g., `IGameController`)

### 5.2 File Headers

All code files MUST include this copyright header:

```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileName.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Day, Month DD, YYYY HH:MM:SS AM/PM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------
```

### 5.3 Code Organization

- **Namespace**: Must match folder hierarchy (e.g., `nGratis.AI.Odin.Engine.ECS.Component`)
- **One class per file**: File name matches class name
- **Ordering**: Fields → Constructors → Properties → Methods
- **Access modifiers**: Always explicit, even for private members

### 5.4 General Guidelines

- Use `var` for local variables when type is obvious
- Prefer expression-bodied members for simple properties/methods
- Use nullable reference types (`string?`) where appropriate
- Add XML documentation for public APIs
- Keep methods focused and single-purpose
- Avoid magic numbers; use named constants

## 6. Documentation Conventions

For complete Markdown formatting standards, see `.ai-toolbox/RULE_Markdown.md`.

### 6.1 Quick Reference

**Essential Rules:**
- **Last Updated Date**: Format as `**Last Updated:** Month DD, YYYY` after title
- **Heading Numbering**: H2 sequential (1, 2, 3...), H3 relative to parent (1.1, 1.2...)
- **Folder Paths**: Must end with `/`
- **File Paths**: No trailing `/`
- **Examples**: Use Restaurant application (non-project-specific)
- **Emojis**: Always followed by UPPERCASE text (✅ CORRECT, ❌ WRONG)
- **Code Snippets**: Complete or explained in prose (no ellipsis comments)
- **Headings**: Natural language (no code identifiers)

### 6.2 Validation

To validate any Markdown file:
```
Use the "Format Markdown" skill from .ai-toolbox/SKILL_FormatMarkdown.md
```

See `.ai-toolbox/RULE_Markdown.md` for complete details on all formatting rules.

### 6.3 AI Toolbox Resources

The `.ai-toolbox/` folder contains reusable skills and rules for AI agents.

#### Available Skills

**SKILL_FormatMarkdown.md** - Validates and formats Markdown documentation
- Detects file types (IDEA, ROADMAP, SNIPPET, SUMMARY, AUDIT, SKILL, RULE, Kiro, Standard)
- Applies all formatting rules from RULE_Markdown.md
- Generates comprehensive validation reports
- Includes self-verification protocol to ensure accuracy
- Usage: Invoke when modifying any Markdown file to ensure compliance

#### Available Rules

**RULE_Markdown.md** - Complete Markdown formatting standards
- File type detection guidelines
- General validation rules (dates, headings, paths, formatting, emojis)
- File-specific rules (SNIPPET/ROADMAP pairing, code format)
- Priority levels for issues (CRITICAL, HIGH, MEDIUM)
- Validation checklist for systematic verification

#### Usage Pattern

1. Read RULE_Markdown.md to understand formatting standards
2. Use SKILL_FormatMarkdown.md to validate/format files
3. Follow the self-verification protocol after making changes

## 7. Common Patterns

### 7.1 Dependency Injection

- Engine uses constructor injection
- Register services during application bootstrapping
- `AppBootstrapper.cs` handles Godot-side registration

### 7.2 Error Handling

- Use `OdinException` for domain-specific errors
- Include descriptive error messages
- Validate inputs early in public APIs

### 7.3 Performance Considerations

- Use `ImmutableArray` for collections that don't change
- Entity pooling via `EntityPool` reduces allocations
- Systems ordered by `OrderingIndex` for cache efficiency
- Track performance with `DebuggingStatistics`

## 8. AI-Assisted Development

### 8.1 Kiro Guidelines

For Kiro documentation formatting conventions, see `.ai-toolbox/RULE_Markdown.md` Section 3.3.

**Summary**: All Markdown files in `.kiro/` folder must follow standard formatting rules including:
- Last Updated date format
- Heading numbering (H2 sequential, H3 relative to parent)
- All general validation rules from RULE_Markdown.md Section 2

### 8.2 Context Management System

The `.ai-context/` folder contains curated context documents that provide deep understanding of features, architecture decisions, and implementation details. These documents are collaboratively created by AI agents and engineers to maintain comprehensive project knowledge.

This context system enables:
- Rapid onboarding for new AI agents
- Consistent implementation across features
- Preservation of architectural decisions
- Comprehensive code references

#### Document Types

**IDEA_*** - Vision & Strategy Documents  
Purpose: Long-term project vision and strategic direction  
When to Read: Before major architectural decisions or new feature planning

**ROADMAP_*** - Feature Planning Documents  
Purpose: Strategic implementation plans with phases and priorities  
When to Read: Before implementing a new feature

**SNIPPET_*** - Code Implementation References  
Purpose: Conceptual pseudo-code illustrating implementation ideas  
Format: Pseudo-code without comments, using statements, or namespace declarations  
Critical: MUST have 1:1 section mapping with corresponding ROADMAP_* file  
When to Read: During active implementation of a feature

**SUMMARY_*** - Quick Reference Guides  
Purpose: Current state snapshots and onboarding guides  
When to Read: First file to read when starting work on the project

**AUDIT_*** - Assessment Reports  
Purpose: Quality assessments and improvement recommendations  
When to Read: During refactoring or architecture review

#### Usage Guidelines

**When Starting Work:**
1. Read SUMMARY_CodebaseAnalysis.md - Understand current project state
2. Read IDEA_Overall.md - Understand project vision and goals
3. Find relevant ROADMAP_* - Understand feature architecture and phases
4. Find corresponding SNIPPET_* - Get implementation code references
5. Check AUDIT reports - Be aware of quality considerations

**When Implementing:**
1. Follow ROADMAP phases sequentially
2. Use SNIPPET code as implementation reference
3. Ensure implementation matches SNIPPET architecture
4. Reference SUMMARY for integration with existing code

**When Creating Documents:**
1. All major features should have ROADMAP + SNIPPET pair
2. Maintain 1:1 section mapping between ROADMAP and SNIPPET
3. Update SUMMARY files when significant changes are made
4. Create AUDIT reports for quality assessments when needed

#### Document Lifecycle

```
IDEA Document (Vision)
    ↓
ROADMAP Document (Plan) ←→ SNIPPET Document (Code)
    ↓                           ↓
Implementation (tracked in .kiro/ folder)
```

**Critical 1:1 Mapping Rule**: ROADMAP_* and SNIPPET_* files MUST maintain section alignment:
- Each ROADMAP section with implementation must have matching SNIPPET section
- Section numbers must match exactly (e.g., ROADMAP 2.1 = SNIPPET 2.1)
- Architecture in ROADMAP must match implementation in SNIPPET
- SNIPPET files MAY have gaps in section numbering to maintain alignment

Example: ROADMAP has 2.1, 2.2, 2.3, 2.4 but SNIPPET only has 2.1, 2.2, 2.4 (gap at 2.3 is intentional).

#### Naming Convention

```
<PREFIX>_<FeatureName>.md

Examples:
- IDEA_Overall.md
- ROADMAP_ScenarioLoader.md
- SNIPPET_ScenarioLoader.md
- SUMMARY_CodebaseAnalysis.md
- AUDIT_AssetManagement.md
```

#### SNIPPET File Code Format

- Use pseudo-code to illustrate concepts, not production-ready code
- Omit comments, using statements, and namespace declarations
- Use descriptive variable names (avoid abbreviations)
- Opening curly bracket `{` MUST be on new line (C# standard)

**What to Exclude**: Copyright notices, XML comments, using/import statements, namespace declarations, obvious implementation comments, file path annotations, Godot scene files, test code

**What to Include**: Class/method signatures, essential properties/fields, core logic, descriptive variable names

## 9. ECS Architecture Guidelines

### 9.1 Entity-Component-System Pattern

The project follows a strict ECS architecture.

#### Components

- **Purpose**: Pure data containers with NO logic
- **Location**: `Source/Odin.Engine/ECS.Component/` and `Source/Odin.Engine/ECS.Component.Logic/`
- **Implementation**: Implement `IComponent` interface
- **Example**: `PhysicsComponent`, `IntelligenceComponent`, `RenderingComponent`
- **Rule**: Components should only contain fields/properties, no methods

#### Systems

- **Purpose**: Process entities that have specific component combinations
- **Location**: `Source/Odin.Engine/ECS.System/` and `Source/Odin.Engine/ECS.System.Logic/`
- **Processing**: Implement variable-duration and fixed-duration update methods
- **Ordering**: Use `[SystemMetadata(OrderingIndex = n)]` attribute to control execution order
- **Example**: `EntitySpawningSystem`, `RenderingSystem`, `InputHandlingSystem`

#### Entities

- **Management**: Created via `IEntityFactory`
- **Coordination**: Managed by `EntityCoordinator`
- **Pooling**: Use `EntityPool` for performance
- **Client-side**: `RenderableEntity` for Godot visualization

#### Blueprints

- **Purpose**: Configuration data loaded from YAML files
- **Location**: `Source/Odin.Engine/ECS.Blueprint/`
- **Types**: `ScenarioBlueprint`, `EntityBlueprint`, `UniverseBlueprint`, `ComponentBlueprint`, `AnimationBlueprint`, `SpriteSheetBlueprint`
- **Loading**: Handled by Glue layer serializers

### 9.2 Adding New Components

1. Create component class in appropriate `ECS.Component/` folder
2. Implement `IComponent` interface
3. Add required properties (data only, no logic)
4. Register component in `ComponentFactory` if needed
5. Create system to process the component if it requires behavior

### 9.3 Adding New Systems

1. Create system class in appropriate `ECS.System/` folder
2. Implement variable and/or fixed duration processing
3. Add `[SystemMetadata(OrderingIndex = n)]` for execution order
4. Register system in dependency injection container
5. System will be automatically ordered and executed by `GameController`

### 9.4 Entity States

Entities have behavioral states defined in `EntityState` enum:
- **Idle**: Static, no movement
- **Walking**: Slow movement with basic AI
- **Running**: Fast movement with animated sprites  
- **Dead**: Inactive state

State transitions are managed by `DecisionMakingSystem`.

### 9.5 System Implementation Patterns

- Systems inherit from `BaseFixedSystem` for consistent timestep processing
- Systems declare `RequiredComponentTypes` for automatic entity filtering
- Entity queries are optimized through component type indexing
- System execution order controlled via `SystemMetadataAttribute`

**Key Systems:**
- `DecisionMakingSystem`: AI state transitions (Idle → Walking/Running)
- `MovementSystem`: Physics updates and position changes
- `RenderingSystem`: Visual updates and animations
- `DebuggingSystem`: Performance monitoring (FPS, entity counts, system metrics)

### 9.6 Blueprint File Format

- Entity blueprints stored in `.ngaoblueprint` files
- YAML-based serialization via YamlDotNet
- Loaded via `EmbeddedDataStore`

## 10. Project Structure

### 10.1 Source Organization

```
Source/
├── Odin.Engine/                      # Core simulation engine
│   ├── Contract/                     # Core interfaces (IGameController, IGameState, etc.)
│   ├── Contract.Data/                # Data contracts (Universe, Cell, Point, etc.)
│   ├── ECS.Blueprint/                # Blueprint definitions for configuration
│   ├── ECS.Component/                # Component definitions (data containers)
│   ├── ECS.Component.Logic/          # Logic-specific components
│   ├── ECS.Coordinator/              # Entity coordination and factories
│   ├── ECS.Entity/                   # Entity definitions
│   ├── ECS.System/                   # System implementations (behavior)
│   └── ECS.System.Logic/             # Logic-specific systems
│
├── Odin.Client.Godot/                # Godot client for visualization
│   ├── Common/                       # Shared utilities and extensions
│   ├── Common.Art/                   # Sprite and texture assets
│   ├── Common.Font/                  # Font assets (FiraCode)
│   ├── Common.UI/                    # UI components (HUD, overlays, camera)
│   ├── ECS.Component/                # Client-side components
│   ├── ECS.Coordinator/              # Client-side coordination
│   ├── ECS.Entity/                   # Godot entity implementations
│   ├── ECS.System/                   # Client-side systems
│   ├── Stage/                        # Godot scenes (Universe.tscn)
│   └── project.godot                 # Godot project configuration
│
├── Odin.Glue/                        # Data and serialization layer
│   ├── Common.Blueprint/             # Common blueprint definitions
│   ├── Serializer/                   # YAML serialization
│   ├── Serializer.Scalar/            # Scalar value serializers
│   └── EmbeddedDataStore.cs          # Embedded resource management
│
└── Odin.Glue.UnitTest/               # Unit tests
    └── YamlSerializationExtensionsTests.*.cs
```

### 10.2 Key Files

- **GameController.cs**: Main game loop coordinator
- **GameState.cs**: Current simulation state
- **EntityCoordinator.cs**: Manages entity lifecycle
- **ScenarioBlueprint.cs**: Scenario configuration structure
- **AppBootstrapper.cs**: Application initialization (Godot)

## 11. Godot-Specific Guidelines

### 11.1 Version & Setup

- **Godot Version**: 4.5.1 (Mono/.NET version)
- **C# Integration**: Uses Godot 4's C# bindings
- **Scene Structure**: Main scene at `Source/Odin.Client.Godot/Stage/Universe.tscn`
- **Project File**: `Source/Odin.Client.Godot/project.godot`

### 11.2 Asset Management

#### Sprites and Textures

- **Location**: `Source/Odin.Client.Godot/Common.Art/`
- **Source Assets**: Original assets in `Asset/` (Aseprite format)
- **Naming**: Descriptive names (e.g., `entity-placeholder-8x8.png`)
- **Import**: Godot auto-generates `.import` files

#### Fonts

- **Location**: `Source/Odin.Client.Godot/Common.Font/`
- **Included Fonts**: FiraCode (Bold, Light, Medium, Regular, Retina, SemiBold)
- **Custom Fonts**: Mono10 fonts in `Asset/FONT_Mono10/`
- **Import**: TTF files with corresponding `.import` files

#### Color Palette

- **Palette**: Resurrect 64 color palette
- **File**: `Asset/PALETTE_resurrect-64.gpl`
- **Usage**: Reference for consistent art style

### 11.3 Godot C# Integration Patterns

#### Node Structure

- Inherit from Godot nodes (e.g., `Node2D`, `Node`, `Control`)
- Use `[GlobalClass]` attribute for nodes that should appear in Godot editor
- Scene files (`.tscn`) link to C# scripts via `.uid` files

#### Resource Loading

- Use `GD.Load<T>()` for loading resources
- Prefer `PackedScene` for instantiating scene templates
- Use resource paths: `res://path/to/resource`

#### Entity Rendering

- `RenderableEntity.cs`: Godot Node2D representing visual entities
- `RenderingComponent.cs`: Component storing rendering state
- `RenderingSystem.cs`: Updates Godot nodes based on component data
- Sprite sheets managed by `SpriteSheetFactory`

### 11.4 UI Components

- **HeadUpDisplay**: Main HUD (`.tscn` + `.cs`)
- **DiagnosticsOverlay**: Debug information display
- **StatisticsOverlay**: Performance metrics
- **Camera**: Custom camera controller
- **SimulationController**: Play/pause/speed controls

## 12. Common Tasks

### 12.1 Creating a New Scenario

1. Define scenario blueprint in YAML format
2. Specify `UniverseBlueprint` (world size, properties)
3. Add `EntityPopulationBlueprint` entries for entities to spawn
4. Place YAML file in appropriate data location
5. Load via `EmbeddedDataStore` or file system
6. Scenario will be processed by `EntitySpawningSystem`

### 12.2 Adding a New Entity Type

1. Create `EntityBlueprint` with required components
2. Define component blueprints for entity behavior
3. Add corresponding `ComponentBlueprint.Rendering` if visual
4. Register any new components in `ComponentFactory`
5. Create entity instances via `IEntityFactory`

### 12.3 Working with YAML Serialization

- **Serializers**: Located in `Source/Odin.Glue/Serializer/`
- **Custom Types**: Extend `YamlSerializationExtensions`
- **Testing**: Add tests in `YamlSerializationExtensionsTests`
- **Conventions**: Use clear property names, nested structures for hierarchy

### 12.4 Debugging with Godot

1. Set `$(GODOT_EXE)` environment variable
2. Use Visual Studio launch configuration `Run.DebuggingWithGodot`
3. Breakpoints work in C# code called from Godot
4. Use `GD.Print()` for Godot console output
5. Check Godot's Output panel for errors and logs

## 13. Resources & Credits

### 13.1 External Resources

- **Color Palette**: [Resurrect 64](https://lospec.com/palette-list/resurrect-64)
- **Fonts**: [FiraCode](https://github.com/tonsky/FiraCode), [Mono10](https://jdjimenez.itch.io/mono10)

### 13.2 License

This project is licensed under the MIT License. See LICENSE file for details.

### 13.3 Author

Cahya Ong — cahya.ong@gmail.com

## 14. Future Enhancements

### 14.1 Planned Features

- Integration testing framework
- Additional scenario types
- Performance profiling tools
- Extended entity behaviors
- More visualization options

### 14.2 Known Limitations

- Integration tests not yet implemented
- Windows-specific Godot integration (macOS/Linux support planned)
- Limited scenario library (more examples needed)

## 15. Quick Reference

### 15.1 Most Common Commands

```bash
# Build everything
dotnet build Source/nGratis.AI.Odin.sln

# Run tests
dotnet test Source/nGratis.AI.Odin.sln

# Clean build artifacts
dotnet clean Source/nGratis.AI.Odin.sln

# Restore packages
dotnet restore Source/nGratis.AI.Odin.sln
```

### 15.2 Key Interfaces

- `IGameController`: Main game loop control
- `IGameState`: Current simulation state
- `IComponent`: Base for all components
- `ISystem`: Base for all systems
- `IEntityFactory`: Creates entities and universe
- `ITimeTracker`: Manages simulation time

### 15.3 Important Attributes

- `[SystemMetadata(OrderingIndex = n)]`: Control system execution order
- `[GlobalClass]`: Make Godot node visible in editor
