# AI.Odin Technology Stack

## Core Technologies
- **.NET 9.0**: Primary runtime and framework
- **C#**: Main programming language for all components
- **Godot 4.4**: Game engine for rendering and client presentation
- **Paket**: Dependency management system

## Build System
- **Visual Studio Solution**: `Source/nGratis.AI.Odin.sln`
- **MSBuild**: Standard .NET build system
- **Paket**: Package management with `paket.dependencies` and `paket.lock`

## Key Dependencies

### Core Libraries
- **Autofac**: Dependency injection container for IoC
- **System.Reactive**: Reactive programming with observables
- **YamlDotNet**: YAML serialization for blueprints and configuration
- **JetBrains.Annotations**: Code analysis and documentation

### Testing Framework
- **xUnit**: Primary testing framework
- **FluentAssertions**: Assertion library for readable tests
- **Moq**: Mocking framework for unit tests
- **Microsoft.NET.Test.Sdk**: Test SDK for .NET

## Build Configurations
- **Debug**: Development builds with full debugging
- **Release**: Optimized production builds
- **ExportDebug**: Godot export builds for debugging
- **ExportRelease**: Godot export builds for release

## Common Commands

### Building
```bash
# Build entire solution
dotnet build Source/nGratis.AI.Odin.sln

# Build specific project
dotnet build Source/Odin.Engine/nGratis.AI.Odin.Engine.csproj

# Build for release
dotnet build Source/nGratis.AI.Odin.sln -c Release
```

### Testing
```bash
# Run all tests
dotnet test Source/nGratis.AI.Odin.sln

# Run tests with coverage
dotnet test Source/nGratis.AI.Odin.sln --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test Source/Odin.Glue.UnitTest/nGratis.AI.Odin.Glue.UnitTest.csproj
```

### Package Management
```bash
# Restore packages
paket restore

# Update packages
paket update

# Install new package
paket add [package-name] --project [project-name]
```

## Godot Integration
- **Project File**: `Source/Odin.Client.Godot/project.godot`
- **Export Configurations**: ExportDebug and ExportRelease build configurations
- **C# Integration**: Godot project references .NET assemblies
- **Main Scene**: `res://Stage/Universe.tscn`
- **Input Mapping**: WASD for camera panning, E/Q for zoom

## Architecture Patterns
- **Entity-Component-System (ECS)**: Core architectural pattern with component-based indexing
- **Dependency Injection**: Using Autofac for IoC across all projects
- **Reactive Programming**: System.Reactive for event handling
- **YAML Configuration**: Blueprint and configuration management with YamlDotNet
- **Clean Architecture**: Separation between Engine, Client, and Glue layers
- **State Management**: Hybrid approach planned (double buffering, CoW, event sourcing)
- **Factory Pattern**: EntityFactory and ComponentFactory for object creation
- **Metadata-Driven Systems**: SystemMetadataAttribute for execution order control

## Current Implementation Status

### ✅ Production-Ready Features
- **ECS Foundation**: Complete entity-component-system with EntityManager
- **Autonomous Agents**: AI decision-making with state transitions (Idle/Walking/Running/Dead)
- **Survival Mechanics**: Energy consumption system with death states
- **Resource System**: Harvestable components with regeneration (GrowthSystem)
- **Godot Integration**: Full rendering pipeline with sprite-based entities
- **Blueprint System**: YAML-based entity templates with embedded resources
- **Performance Monitoring**: Real-time FPS and system execution metrics
- **Dependency Injection**: Complete Autofac setup across all projects

### 🚧 Active Development (P0 Priority)
- **Enhanced Metabolism**: Multi-resource survival (hunger, thirst, temperature)
- **Resource Management**: Berry bushes and food production mechanics
- **ML Data Collection**: Experience buffers for future training
- **Genetic Traits**: Expanded trait system for NEAT evolution

### 📋 Planned Features (Roadmap)
- **P1**: NEAT neural evolution with speciation and behavioral clustering
- **P1**: Advanced environmental systems (weather, disasters, seasons)
- **P2**: Reinforcement learning (DQN, PPO) with hierarchical decision-making
- **P2**: Social dynamics and cultural evolution systems
- **P3**: Multi-generational dynasties and archaeological systems
- **P3**: AI-driven narrative generation and content creation tools

## Development Roadmap

### Phase Priorities
1. **P0 - Foundation**: Core mechanics (survival, resources, building, social, combat, tech)
2. **P1 - Advanced**: NEAT evolution, environmental systems, production chains
3. **P2 - Specialized**: RL systems, social learning, professional advancement
4. **P3 - Emergent**: Meta-learning, multi-generational, narrative generation

### Performance Targets
- **Entity Count**: 1000+ agents with full AI processing
- **Frame Rate**: 60 FPS sustained performance
- **ML Processing**: Real-time neural network inference
- **Memory Usage**: Efficient component storage with spatial partitioning