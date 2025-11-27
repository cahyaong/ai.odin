# Implementation Plan

- [ ] 1. Extend existing components for energy restoration
  - Extend `RejuvenationComponent` with `RestorationRate` and `LastHarvestTick` properties
  - Add `Resting` state to `MotionState` enum in `MotionState.cs`
  - Add `MaxEnergy` property to `VitalityComponent`
  - Add `EnergyValue` property to `HarvestableComponent`
  - Add `TargetEntityId` property to `IntelligenceComponent` for tracking harvest targets
  - _Requirements: 1.1, 1.2, 1.5, 2.5, 4.1, 4.4, 4.5_

- [ ]* 1.1 Write property test for component data integrity
  - **Property 13: Component filtering is correct**
  - **Validates: Requirements 5.1**

- [ ] 2. Implement RejuvenationSystem for passive energy restoration
  - Create `RejuvenationSystem` class extending `BaseFixedSystem`
  - Set `RequiredComponentTypes` to include `VitalityComponent`, `RejuvenationComponent`, and `PhysicsComponent`
  - Implement `ProcessEntity` to check for `MotionState.Resting`
  - Apply energy restoration per tick based on `RejuvenationComponent.RestorationRate`
  - Clamp energy to maximum capacity (use constant for now)
  - Skip dead entities
  - _Requirements: 1.1, 1.3, 5.1, 5.2, 5.3, 5.4_

- [ ]* 2.1 Write property test for resting energy restoration
  - **Property 1: Resting increases energy proportionally to ticks**
  - **Validates: Requirements 1.1**

- [ ]* 2.2 Write property test for resting state invariants
  - **Property 2: Resting state enforces stationary behavior**
  - **Validates: Requirements 1.2, 1.5**

- [ ]* 2.3 Write property test for energy capping
  - **Property 3: Energy never exceeds maximum capacity**
  - **Validates: Requirements 1.3, 2.4**

- [ ]* 2.4 Write property test for state transition behavior
  - **Property 4: Transitioning from resting stops restoration**
  - **Validates: Requirements 1.4**

- [ ]* 2.5 Write property test for component updates
  - **Property 14: Energy updates modify Vitality Component**
  - **Validates: Requirements 5.3**

- [ ]* 2.6 Write unit tests for RejuvenationSystem edge cases
  - Test agent at max energy does not gain additional energy
  - Test agent with zero restoration rate does not gain energy
  - Test system skips dead entities

- [ ] 3. Implement HarvestingSystem for active resource consumption
  - Create `HarvestingSystem` class extending `BaseFixedSystem`
  - Set `RequiredComponentTypes` to include `VitalityComponent`, `RejuvenationComponent`, `IntelligenceComponent`, and `PhysicsComponent`
  - Implement `ProcessEntity` to check if agent has a target entity ID
  - Validate target entity exists and has `HarvestableComponent` with available resources
  - Check harvest cooldown using tick-based timing
  - Check harvest range using position distance calculation
  - Transfer energy from resource to agent (use constant for now)
  - Decrement resource `Amount` by 1
  - Update `LastHarvestTick` and clear `TargetEntityId`
  - Skip dead entities
  - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5_

- [ ]* 3.1 Write property test for harvest energy transfer
  - **Property 5: Successful harvest transfers correct energy**
  - **Validates: Requirements 2.1**

- [ ]* 3.2 Write property test for depleted resource handling
  - **Property 6: Depleted resources cannot be harvested**
  - **Validates: Requirements 2.2**

- [ ]* 3.3 Write property test for resource quantity decrement
  - **Property 7: Harvest decrements resource quantity**
  - **Validates: Requirements 2.3**

- [ ]* 3.4 Write property test for harvest range validation
  - **Property 8: Harvest range validation**
  - **Validates: Requirements 2.5**

- [ ]* 3.5 Write unit tests for HarvestingSystem edge cases
  - Test harvest fails when agent is out of range
  - Test harvest fails during cooldown period
  - Test system skips dead entities
  - Test system clears target when resource is depleted

- [ ] 4. Extend DecisionMakingSystem for survival behaviors
  - Add low energy detection using constant threshold (30.0f)
  - When energy is low, search for nearest harvestable entity with `Amount > 0`
  - If harvestable found, set `TargetEntityId` and navigate toward it
  - If no harvestable found, set `MotionState` to `Resting`
  - Add safe energy threshold check (70.0f) to resume normal behaviors
  - Ensure resting agents have zero velocity
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_

- [ ]* 4.1 Write property test for low energy behavior triggering
  - **Property 9: Low energy triggers survival behaviors**
  - **Validates: Requirements 3.1, 3.2, 3.3**

- [ ]* 4.2 Write property test for safe energy behavior resumption
  - **Property 10: Safe energy resumes normal behaviors**
  - **Validates: Requirements 3.4**

- [ ]* 4.3 Write property test for resource targeting
  - **Property 11: Food-seeking targets nearest available resource**
  - **Validates: Requirements 3.5**

- [ ]* 4.4 Write integration tests for decision-making scenarios
  - Test low-energy agent with nearby resource chooses harvesting
  - Test low-energy agent without nearby resource chooses resting
  - Test agent above safe threshold resumes normal behaviors

- [ ] 5. Update blueprint files with energy restoration parameters
  - Update `entity-human.ngaoblueprint` with `restoration_rate` in rejuvenation component
  - Update `entity-bush.ngaoblueprint` (note: energy value will be added in future iteration)
  - Ensure vitality component has initial energy value
  - _Requirements: 4.1, 4.2_

- [ ] 6. Register new systems in dependency injection
  - Register `RejuvenationSystem` in `AppBootstrapper`
  - Register `HarvestingSystem` in `AppBootstrapper`
  - Verify systems are added to the system collection
  - _Requirements: 5.2_

- [ ] 7. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 8. Add visual feedback for energy restoration (Godot client)
  - Extend `RenderingComponent` with resting animation state
  - Add harvest particle effect or animation trigger
  - Add low energy visual indicator (color tint or icon)
  - Update `RenderingSystem` to handle new visual states based on `MotionState.Resting`
  - _Requirements: 6.1, 6.2, 6.3_

- [ ]* 8.1 Write unit tests for rendering state updates
  - Test resting state triggers correct animation
  - Test low energy state applies visual indicator

- [ ] 9. Create test project infrastructure
  - Create `Odin.Engine.UnitTest` project if it doesn't exist
  - Add FsCheck NuGet package for property-based testing via Paket
  - Add xUnit and FluentAssertions packages via Paket
  - Set up test project structure with folders for system tests
  - _Requirements: All testing requirements_

- [ ] 10. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.
