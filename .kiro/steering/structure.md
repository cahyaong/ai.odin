# AI.Odin Project Structure

## Root Directory Organization

```
├── .ai-context/          # AI assistant context and documentation
├── .ai-template/         # Templates for AI-generated content
├── .ai-workspace/        # AI workspace files and tasks
├── .kiro/               # Kiro IDE configuration and steering
├── Asset/               # Game assets (sprites, fonts, palettes)
├── Documentation/       # Project documentation
├── External/            # External dependencies and submodules
├── Source/              # Main source code directory
└── README.md           # Project overview and setup
```

## Source Directory Structure

### Core Projects

**`Source/Odin.Engine/`** - Platform-agnostic game logic
- `Contract/` - Interfaces and contracts (`IGameController`, `ICamera`, `ITimeTracker`, `IGameState`)
- `Contract.Data/` - Core data structures (`Point`, `Vector`, `Size`, `Universe`, `Cell`, `Statistics`)
- `ECS.Component/` - Pure data components with no behavior
  - `IComponent.cs` - Base component interface
  - `ComponentConstant.cs` - Component type identifiers
- `ECS.Component.Logic/` - Game logic components
  - `IntelligenceComponent` - AI state and decision timing
  - `PhysicsComponent` - Position and velocity
  - `VitalityComponent` - Health and energy
  - `TraitComponent` - Genetic characteristics
  - `HarvestableComponent` - Resource nodes
  - `RejuvenationComponent` - Health recovery
- `ECS.Entity/` - Entity management
  - `EntityManager` - Central entity management with component indexing
  - `Entity` - Base entity implementation
  - `MotionState` - Entity behavioral states (Idle, Walking, Running, Dead)
- `ECS.Coordinator/` - Entity and component factories
  - `EntityFactory` - Creates entities from blueprints
  - `ComponentFactory` - Creates components with dependency injection
  - `IEntityManager` - Entity management interface
- `ECS.System/` - Base system classes
  - `BaseFixedSystem` - Fixed-timestep system base
  - `BaseVariableSystem` - Variable-timestep system base
  - `SystemMetadataAttribute` - Execution order control
- `ECS.System.Logic/` - Behavior systems
  - `DecisionMakingSystem` - AI state transitions
  - `MovementSystem` - Physics updates
  - `MetabolismSystem` - Energy consumption
  - `GrowthSystem` - Resource regeneration
  - `DebuggingSystem` - Performance monitoring
- `ECS.Blueprint/` - Entity templates
  - `EntityBlueprint` - Entity creation templates
  - `ComponentBlueprint` - Component configuration
  - `SpriteSheetBlueprint` - Visual asset definitions
  - `AnimationBlueprint` - Animation configurations
- `GameController.cs` - Main game loop orchestration with system execution
- `GameState.cs` - Global game state management

**`Source/Odin.Client.Godot/`** - Godot engine integration
- `Common/` - Application infrastructure
  - `AppBootstrapper` - Dependency injection setup with Autofac
  - `TimeTracker` - Frame timing and game loop synchronization
  - `Constant` - Pixel scaling and configuration constants
  - `DataExtensions` - Utility extension methods
- `Common.Art/` - Art and visual assets
  - `entity-placeholder-8x8.png` - 8x8 pixel entity sprites
  - `entity-placeholder-16x16.png` - 16x16 pixel entity sprites
- `Common.Font/` - Font resources
  - FiraCode family (Bold, Light, Medium, Regular, Retina, SemiBold)
- `Common.UI/` - User interface components
  - `HeadUpDisplay` - Main UI container
  - `StatisticsOverlay` - Real-time performance metrics
  - `DiagnosticsOverlay` - Debug information display
  - `Camera` - Viewport and camera control
- `ECS.Component/` - Godot-specific components
  - `RenderingComponent` - Visual data and animation state
- `ECS.Coordinator/` - Godot-specific factories
  - `EntityCoordinator` - Manages entity lifecycle in Godot
  - `ComponentFactory` - Creates Godot components
  - `EntityPool` - Entity object pooling
  - `SpriteSheetFactory` - Sprite sheet management
- `ECS.Entity/` - Godot entity implementation
  - `RenderableEntity` - Visual entity with Godot Node2D
  - `RenderableEntity.tscn` - Entity scene template
- `ECS.System/` - Rendering and input systems
  - `RenderingSystem` - Bridges engine entities to Godot visuals
  - `EntitySpawningSystem` - Manages entity creation in scenes
  - `InputHandlingSystem` - Processes user input
- `Stage/` - Game scenes
  - `Universe.tscn` - Main game scene
- `project.godot` - Godot project configuration

**`Source/Odin.Glue/`** - Serialization and data management
- `Common.Blueprint/` - Entity blueprint definitions (`.ngaoblueprint` files)
  - `entity-bush.ngaoblueprint` - Bush entity template
  - `entity-human.ngaoblueprint` - Human entity template
  - `spritesheet-bush.ngaoblueprint` - Bush sprite configuration
  - `spritesheet-humanoid.ngaoblueprint` - Humanoid sprite configuration
- `Serializer/` - YAML serialization framework
  - `YamlSerializationExtensions` - Main serialization API
  - `ParameterYamlConverter` - Parameter type conversion
  - `ScalarOrSequenceYamlConverter` - Flexible scalar/sequence handling
  - `ScalarYamlConverter` - Custom scalar type conversion
- `Serializer.Scalar/` - Custom scalar value handlers
  - `IScalarHandler` - Scalar handler interface
  - `CellScalarHandler` - Cell data parsing
  - `SizeScalarHandler` - Size data parsing
  - `LookupScalarHandler` - Lookup table parsing
  - `ScalarExtensions` - Scalar processing utilities
- `EmbeddedDataStore.cs` - Embedded resource management for blueprints
- `OdinMime.cs` - File type and MIME type definitions

**`Source/Odin.Glue.UnitTest/`** - Testing framework
- Unit tests for serialization and data management

## File Naming Conventions

### Project Files
- **Solution**: `nGratis.AI.Odin.sln`
- **Projects**: `nGratis.AI.Odin.[ProjectName].csproj`
- **Namespace**: `nGratis.AI.Odin.[ProjectName]`

### Code Organization
- **Components**: `[Name]Component.cs` (e.g., `IntelligenceComponent.cs`)
- **Systems**: `[Name]System.cs` (e.g., `MovementSystem.cs`)
- **Contracts**: `I[Name].cs` for interfaces
- **Blueprints**: `[Name]Blueprint.cs`
- **Tests**: `[ClassName]Tests.cs`

### Asset Files
- **Sprites**: `.aseprite` format for pixel art
- **Fonts**: Organized in `FONT_[Name]/` directories
- **Palettes**: `.gpl` format with usage documentation

## ECS Architecture Organization

### Component Categories

**Logic Components (Odin.Engine/ECS.Component.Logic/):**
- **Survival**: `VitalityComponent` (health, energy), `TraitComponent` (genetic characteristics)
- **AI**: `IntelligenceComponent` (AI state, decision timing, target positions)
- **Physics**: `PhysicsComponent` (position, velocity)
- **Resources**: `HarvestableComponent` (resource nodes with regeneration)
- **Recovery**: `RejuvenationComponent` (health recovery mechanics)

**Rendering Components (Odin.Client.Godot/ECS.Component/):**
- **Visual**: `RenderingComponent` (Godot-specific visual data and animation state)

**Planned Components (Roadmap):**
- **Genetics**: `GeneticTraitsComponent` (22 inherited traits)
- **Learning**: `NEATGenomeComponent`, `ExperienceBufferComponent`, `LearningProfileComponent`
- **Social**: `SocialContextComponent`, `CulturalKnowledgeComponent`, `MultiGenerationalRelationshipComponent`
- **Economic**: `EconomicNetworkComponent`, `ProfessionalAdvancementComponent`
- **Environmental**: `EnvironmentalAwarenessComponent`, `WeatherAffectedComponent`

### System Categories

**Core Logic Systems (Odin.Engine/ECS.System.Logic/):**
- **AI**: `DecisionMakingSystem` (state transitions, behavior logic)
- **Physics**: `MovementSystem` (position updates, velocity calculations)
- **Survival**: `MetabolismSystem` (energy consumption, death mechanics)
- **Resources**: `GrowthSystem` (resource regeneration, environmental expansion)
- **Monitoring**: `DebuggingSystem` (performance metrics, diagnostics)

**Rendering Systems (Odin.Client.Godot/ECS.System/):**
- **Visual**: `RenderingSystem` (bridges engine entities to Godot visuals)
- **Lifecycle**: `EntitySpawningSystem` (manages entity creation in scenes)
- **Input**: `InputHandlingSystem` (processes user input for interaction)

**Planned Systems (26 Game Mechanics + 16 ML Systems):**
- **Core Mechanics (1-6)**: Survival, resources, building, social, combat, technology
- **Advanced (7-15)**: Environment, ecology, production, god powers, physics, psychology
- **Specialized (16-21)**: City-builder, economic, professional, political, moral, creature training
- **Emergent (22-26)**: Meta-economics, multi-generational, archaeological, narrative, cross-colony
- **ML Systems (1-16)**: Genetic algorithms, NEAT evolution, DQN, PPO, social learning, meta-learning

### ECS Design Principles
- **Pure Data Components**: Components contain only data, no behavior
- **System Processing**: All logic resides in systems that process entities
- **Component Composition**: Entities are composed of multiple components
- **Performance Optimization**: Systems process entities in bulk for efficiency

## Configuration and Settings

### Build Configuration
- `paket.dependencies` - Package dependencies
- `paket.lock` - Locked package versions
- `CodeMaid.config` - Code formatting rules
- `.DotSettings` files - ReSharper/Rider settings

### Development Tools
- `.vscode/` - VS Code configuration
- `.vs/` - Visual Studio configuration
- `dotnet-tools.json` - .NET CLI tools

## External Dependencies
- `External/cop.olympus/` - External project dependency
- Git submodules for external code
- Paket for NuGet package management

## Asset Organization
- **Sprites**: 8x8 and 16x16 pixel art in Aseprite format
- **Fonts**: Mono10 and FiraCode for UI
- **Palettes**: Resurrect-64 color palette for consistent art style

## Documentation Structure
- `.ai-context/` - AI-generated documentation and analysis
- `Documentation/` - Human-written documentation
- Inline code documentation using XML comments
- README files for major components

## Code Style Guidelines
- **C# Conventions**: Follow standard C# naming conventions
- **Namespace Organization**: Mirror folder structure in namespaces
- **Component Suffix**: All ECS components end with "Component"
- **System Suffix**: All ECS systems end with "System"
- **Interface Prefix**: All interfaces start with "I"
- **Async Methods**: Use "Async" suffix for async methods