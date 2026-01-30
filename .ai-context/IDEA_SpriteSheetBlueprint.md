# IDEA: SpriteSheet Blueprint

**Last Updated:** January 6, 2026

---

## Table of Contents

- [IDEA: SpriteSheet Blueprint](#idea-spritesheet-blueprint)
  - [Table of Contents](#table-of-contents)
  - [1. Overview](#1-overview)
    - [1.1 Core Concept](#11-core-concept)
    - [1.2 Design Goals](#12-design-goals)
  - [2. Architecture](#2-architecture)
    - [2.1 Blueprint Structure](#21-blueprint-structure)
    - [2.2 Coordinate System](#22-coordinate-system)
    - [2.3 Integration Points](#23-integration-points)
  - [3. Data Structures](#3-data-structures)
    - [3.1 Core Blueprint Data](#31-core-blueprint-data)
    - [3.2 Factory and Loading Infrastructure](#32-factory-and-loading-infrastructure)
    - [3.3 Client Integration](#33-client-integration)
    - [3.4 Blueprint Files](#34-blueprint-files)
  - [4. Design Decisions](#4-design-decisions)
    - [4.1 Key Technical Decisions](#41-key-technical-decisions)
    - [4.2 Integration Strategy](#42-integration-strategy)
  - [5. Success Criteria](#5-success-criteria)
    - [5.1 Functional Goals](#51-functional-goals)
    - [5.2 Production Verification](#52-production-verification)
  - [6. Future Enhancements](#6-future-enhancements)
    - [6.1 Animation Metadata](#61-animation-metadata)
    - [6.2 Error Handling Improvements](#62-error-handling-improvements)
    - [6.3 Multiple Entity Types](#63-multiple-entity-types)
    - [6.4 Advanced Features](#64-advanced-features)
  - [7. Artifacts](#7-artifacts)
    - [7.1 New Artifacts](#71-new-artifacts)
    - [7.2 Modified Artifacts](#72-modified-artifacts)
    - [7.3 Reused Infrastructure](#73-reused-infrastructure)
  - [8. Quality Assessment](#8-quality-assessment)
    - [8.1 Quality Metrics](#81-quality-metrics)
    - [8.2 Overall Evaluation](#82-overall-evaluation)

---

## 1. Overview

### 1.1 Core Concept

The SpriteSheet Blueprint System provides a data-driven animation configuration framework that enables artists to modify entity animations through YAML blueprints without requiring code changes.

### 1.2 Design Goals

The blueprint-based spritesheet templating system achieves the following:
- Loads animation configurations from YAML files
- Generates sprite frames dynamically from blueprints
- Integrates seamlessly with existing ECS architecture
- Provides thread-safe caching for optimal performance
- Enables reusable animation templates across entity types

## 2. Architecture

### 2.1 Blueprint Structure

The system uses a two-file architecture:

**Spritesheet Template Blueprints**: Reusable animation definitions stored in `.ngaoblueprint` files. Each template defines:
- Sprite dimensions (e.g., 16x16 pixels)
- Animation definitions with coordinate ranges
- Animation identifiers matching entity state enum values

**Entity Blueprints**: Entity-specific configurations referencing spritesheet templates through data container parameters. Each entity blueprint specifies:
- Spritesheet template reference via blueprint identifier
- Texture resource name
- Other data container configurations

### 2.2 Coordinate System

The implementation uses Cell-based coordinates (`R:0, C:0` format) rather than Point coordinates (`X:0, Y:0` format) to maintain consistency with existing codebase patterns. This decision:
- Aligns with Universe grid cell terminology
- Leverages existing cell scalar handler for YAML serialization
- Avoids introducing duplicate coordinate concepts

### 2.3 Integration Points

The system integrates at three key points in the architecture:

**Data Layer (Glue Module)**: Embedded data store loads spritesheet blueprints from embedded resources at application startup.

**Factory Layer (Client Coordinator Module)**: Data factory extracts spritesheet parameters during data container creation and delegates sprite frame generation to spritesheet factory.

**Entity Layer (Client Entity Module)**: Renderable entity receives generated sprite frames and applies them to its animated sprite node.

## 3. Data Structures

### 3.1 Core Blueprint Data

Three primary blueprint structures are defined:

**SpriteSheet Blueprint**: Top-level record containing:
- `Id`: Unique identifier for the spritesheet template
- `SpriteSize`: Size scalar defining frame dimensions (Width, Height)
- `AnimationBlueprints`: Collection of animation definitions

**Animation Blueprint**: Animation definition record containing:
- `Id`: Animation name matching entity state values
- `StartingCell`: Beginning cell coordinate for frame range
- `EndingCell`: Ending cell coordinate for frame range

**Rendering Data Blueprint**: Type-safe parameter extraction wrapper for data blueprints, providing:
- Spritesheet blueprint identifier accessor
- Texture resource name accessor
- Compile-time safety over raw parameter dictionary access

All blueprint structures use existing YAML scalar serialization infrastructure without requiring new converters.

### 3.2 Factory and Loading Infrastructure

**SpriteSheet Factory**: Implements sprite frame generation with two-level caching:

First Level - Texture Cache: Caches loaded texture resources using a thread-safe hash table to avoid redundant file I/O operations.

Second Level - SpriteFrames Cache: Caches generated sprite frames using a thread-safe hash table to avoid redundant frame generation for identical configurations.

The factory performs these operations:
- Loads textures from resource paths
- Converts Cell coordinate ranges to pixel-based rectangular regions
- Generates animation frame sequences (horizontal, vertical, or rectangular)
- Creates complete sprite frames with all animations configured

**Embedded Data Store**: Extended to load spritesheet blueprints:
- Added spritesheet blueprint loading method
- Utilizes existing embedded resource scanning infrastructure
- Loads blueprints at application startup before entity creation

**Data Factory**: Integrates blueprint system into data container creation:
- Loads all spritesheet blueprints during initialization
- Extracts spritesheet parameters from entity blueprints using rendering data blueprint wrapper
- Calls spritesheet factory to generate or retrieve cached sprite frames
- Applies generated sprite frames to renderable entity instances

### 3.3 Client Integration

**Renderable Entity**: Added spritesheet management:

Added an update spritesheet method that:
- Validates animated sprite node existence
- Applies sprite frames to the animated sprite
- Sets default animation to "Idle" state
- Starts animation playback automatically

The existing animation state update method already handles animation transitions based on entity state, requiring no modifications.

### 3.4 Blueprint Files

Production-ready blueprint files in the Glue Blueprint folder:

**spritesheet-humanoid.ngaoblueprint**: Defines standard humanoid animation template with four animation states:
- Idle: Cells (R:0, C:0) to (R:0, C:3) - 4 frames
- Walking: Cells (R:1, C:0) to (R:1, C:3) - 4 frames
- Running: Cells (R:2, C:0) to (R:2, C:3) - 4 frames
- Dead: Cells (R:3, C:0) to (R:3, C:3) - 4 frames

All animations use 16x16 pixel sprite dimensions, matching the existing placeholder texture structure.

**entity-human.ngaoblueprint**: References spritesheet template:
- Added rendering data container with parameters
- Specified spritesheet blueprint identifier: 'Humanoid'
- Specified texture name: 'entity-placeholder-16x16'

## 4. Design Decisions

### 4.1 Key Technical Decisions

**Cell Coordinates Over Point Coordinates**: Chose Cell-based system to align with existing Universe grid terminology and leverage existing scalar handlers.

**Two-File Blueprint Structure**: Separated template definitions from entity-specific configurations to enable template reuse across multiple entity types.

**Thread-Safe Caching**: Used thread-safe hash tables for both texture and sprite frame caches to support concurrent entity creation without locking overhead.

**Data Factory Integration**: Placed blueprint loading in data factory rather than rendering processor to ensure all data containers receive fully-configured sprites before processor execution begins.

**Rendering Data Blueprint Wrapper**: Created type-safe accessor structure instead of using raw string-based parameter access to enable compile-time validation.

**No New MIME Types**: Reused existing `.ngaoblueprint` extension for spritesheet templates to leverage existing file scanning and serialization infrastructure.

### 4.2 Integration Strategy

Integrated with existing systems rather than replacing them:
- Leveraged existing ECS data container creation pipeline
- Used existing YAML scalar serialization system
- Maintained existing entity state-based animation control
- Preserved existing entity spawning workflow

## 5. Success Criteria

### 5.1 Functional Goals

All success criteria should be met:

- **Functional**: New entities can be created using only YAML blueprint files without code changes
- **Performance**: Thread-safe caching with no performance regression compared to hardcoded approach
- **Maintainable**: Artists can modify animations by editing YAML files without touching source code
- **Extensible**: System supports adding new entity types by creating new blueprints with existing templates
- **Compatible**: Works seamlessly with existing placeholder textures
- **Reusable**: Multiple entity types can share the same animation template with different textures

### 5.2 Production Verification

Verified working data flow:
1. Blueprint loading from embedded resources at startup
2. Data container creation with spritesheet parameter extraction
3. Sprite frame generation with multi-level caching
4. Entity rendering with correct animations
5. State-based animation transitions during gameplay

## 6. Future Enhancements

### 6.1 Animation Metadata

Add speed and loop control to animation blueprints to eliminate hardcoded values in spritesheet factory:

Extend animation blueprint with:
- `Speed` property (default: 8.0) for frames per second
- `Loop` property (default: true) for animation repetition

This would enable per-animation speed control (e.g., slow idle animations vs fast running animations) through blueprint configuration.

### 6.2 Error Handling Improvements

Add fallback mechanisms for missing resources:
- Implement placeholder texture generation for missing texture files
- Add comprehensive error logging with resource paths
- Provide clear error messages for blueprint validation failures

### 6.3 Multiple Entity Types

The current system already supports multiple entity types. Future work involves creating additional entity blueprints:
- Wizard using Humanoid template with different texture
- Elf character variant
- Orc enemy type
- Dwarf character variant

All would reference the same Humanoid animation template with entity-specific textures.

### 6.4 Advanced Features

Long-term enhancements not currently needed:
- Template inheritance system for animation composition
- Per-entity animation overrides without creating new templates
- Multiple spritesheet composition for complex entities
- Runtime blueprint hot-reload for development workflow
- Animation event callbacks for gameplay integration

## 7. Artifacts

### 7.1 New Artifacts

**Blueprint Definitions**:
- SpriteSheet Blueprint: Core blueprint record in Engine Blueprint module
- Animation Blueprint: Animation definition record in Engine Blueprint module
- Rendering Data Blueprint: Type-safe parameter wrapper in Engine Blueprint module

**Factory Implementation**:
- SpriteSheet Factory: Sprite frame generation with caching in Client Coordinator module

**Blueprint Files**:
- `spritesheet-humanoid.ngaoblueprint`: Humanoid animation template in Glue Blueprint folder

### 7.2 Modified Artifacts

**Integration Points**:
- Data Factory: Added blueprint loading and integration
- Renderable Entity: Added update spritesheet method
- Embedded Data Store: Added spritesheet blueprint loading
- `entity-human.ngaoblueprint`: Added spritesheet parameters

### 7.3 Reused Infrastructure

**Existing Infrastructure**:
- YAML Serialization Extensions: Existing scalar handlers sufficient
- Rendering Data Container: Existing parameter system worked as-is
- Rendering Processor: Data factory handled all integration

## 8. Quality Assessment

### 8.1 Quality Metrics

**Code Quality**: Follows established patterns and ECS principles

**Performance**: Optimized multi-level caching with thread-safe concurrent access

**Maintainability**: Clear separation of concerns with data-driven configuration

**Extensibility**: Easy to add new entity types by creating blueprint files

**Time Investment**: Leverages existing YAML and ECS infrastructure

### 8.2 Overall Evaluation

The feature achieves requirements by:
- Providing production-ready thread-safe caching
- Integrating seamlessly with existing architecture
- Enabling data-driven workflow for artists
- Supporting future extensibility without code changes
- Maintaining clean ECS architectural principles
