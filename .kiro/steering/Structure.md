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
- `Contract/` - Interfaces and contracts (`IGameController`, `ICamera`, `ITimeTracker`)
- `Contract.Data/` - Core data structures (`Point`, `Vector`, `Size`, `Universe`)
- `ECS.Component/` - Pure data components (`IntelligenceComponent`, `PhysicsComponent`)
- `ECS.Entity/` - Entity management (`EntityManager`, `Entity`, `EntityFactory`)
- `ECS.System/` - Behavior systems (`DecisionMakingSystem`, `MovementSystem`)
- `ECS.Template/` - Entity blueprints (`EntityBlueprint`, `ComponentBlueprint`)
- `GameController.cs` - Main game loop orchestration
- `GameState.cs` - Global game state management

**`Source/Odin.Client.Godot/`** - Godot engine integration
- `Common/` - Application infrastructure (`AppBootstrapper`, `TimeTracker`)
- `Common.Art/` - Art and visual assets
- `Common.Font/` - Font resources
- `Common.UI/` - User interface (`HeadUpDisplay`, `StatisticsOverlay`)
- `ECS.Component/` - Godot-specific components (`RenderingComponent`)
- `ECS.Entity/` - Godot entity factories
- `ECS.System/` - Rendering and input systems
- `Stage/` - Game scenes and stages
- `project.godot` - Godot project configuration

**`Source/Odin.Glue/`** - Serialization and data management
- `Common.Blueprint/` - Entity blueprint definitions (`.ngaoblueprint` files)
- `Serializer/` - YAML serialization framework
- `Serializer.Scalar/` - Custom scalar value handlers
- `EmbeddedDataStore.cs` - Embedded resource management
- `OdinMime.cs` - File type definitions

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
- **Core Components**: Position, movement, health, hunger
- **AI Components**: Intelligence, decision-making, neural networks
- **Rendering Components**: Visual representation, animation
- **Game Mechanics**: Metabolism, social interactions, resources

### System Categories
- **Core Systems**: Movement, physics, decision-making
- **Rendering Systems**: Visual updates, UI management
- **AI Systems**: Machine learning, evolution, behavior
- **Game Systems**: Survival mechanics, social dynamics

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