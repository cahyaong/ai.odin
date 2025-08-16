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
- **Entity-Component-System (ECS)**: Core architectural pattern
- **Dependency Injection**: Using Autofac for IoC
- **Reactive Programming**: System.Reactive for event handling
- **YAML Configuration**: Blueprint and configuration management
- **Clean Architecture**: Separation between Engine, Client, and Glue layers
- **State Management**: Hybrid approach with double buffering, CoW, and event sourcing