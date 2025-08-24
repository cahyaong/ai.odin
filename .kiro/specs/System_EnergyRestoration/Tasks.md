# Implementation Plan

- [ ] 1. Enhance VitalityComponent with maximum energy support
  - Add MaxEnergy property with default value of 100.0f
  - Add IsEnergyFull helper property for quick energy state checking
  - Add EnergyPercentage helper property for energy ratio calculations
  - Update constructor to initialize Energy to 50.0f (half capacity)
  - _Requirements: 1.4, 2.2, 2.3_

- [ ] 2. Implement HarvestableComponent with berry bush properties
  - Add TargetId property for resource type identification (default "Berry")
  - Add Amount property for current harvestable berry count (default 8)
  - Add IsEmpty property for efficient empty state checking (default false)
  - Add HarvestRadius property for proximity-based harvesting (default 16.0f)
  - _Requirements: 1.1, 1.2, 1.3, 4.1, 4.4_

- [ ] 3. Implement RejuvenationComponent with energy restoration properties
  - Add Energy property for energy restoration value (default 10.0f)
  - Add IsConsumed property for consumption state tracking (default false)
  - Ensure component follows existing IComponent interface pattern
  - _Requirements: 2.1, 4.2, 4.4_

- [ ] 4. Create unit tests for enhanced components
  - Write tests for VitalityComponent energy clamping behavior
  - Write tests for HarvestableComponent empty state management
  - Write tests for RejuvenationComponent default value initialization
  - Write tests for component property validation and boundary conditions
  - _Requirements: 1.4, 2.2, 2.3, 4.4_

- [ ] 5. Enhance MetabolismSystem with berry harvesting logic
  - Add ProcessBerryHarvestingAndConsumption method to existing MetabolismSystem
  - Implement proximity detection using Vector2.Distance for harvest radius checking
  - Add energy need validation (skip harvesting if entity energy is full)
  - Implement berry consumption logic with immediate energy restoration
  - Add energy clamping to prevent exceeding MaxEnergy capacity
  - _Requirements: 1.1, 1.5, 2.1, 2.2, 2.3, 3.1, 5.1, 5.3_

- [ ] 6. Integrate spatial queries for efficient bush discovery
  - Use EntityManager.FindEntitiesWithComponent<HarvestableComponent> for bush queries
  - Implement distance-based filtering for entities within harvest radius
  - Add early exit optimization when no harvestable entities exist
  - Ensure spatial queries maintain 60 FPS performance with 1000+ entities
  - _Requirements: 1.1, 3.3, 3.4, 5.2_

- [ ] 7. Implement harvest state management and validation
  - Add bush berry count decrement logic when berries are harvested
  - Implement IsEmpty flag updates when Amount reaches zero
  - Add validation to skip empty bushes during harvesting attempts
  - Implement one-berry-per-tick limitation to prevent overconsumption
  - _Requirements: 1.2, 1.3, 1.5, 5.4_

- [ ] 8. Add blueprint parameter support for component configuration
  - Update component creation to support MaxEnergy parameter in VitalityComponent
  - Add blueprint parameter parsing for HarvestableComponent properties
  - Add blueprint parameter parsing for RejuvenationComponent energy values
  - Implement default value fallbacks when blueprint parameters are missing
  - _Requirements: 4.1, 4.2, 4.3, 4.4_

- [ ] 9. Create comprehensive integration tests for energy restoration system
  - Write test for normal berry harvesting scenario with energy restoration
  - Write test for multiple entities competing for same bush resources
  - Write test for empty bush handling and system performance impact
  - Write test for full energy entities skipping berry harvesting
  - Write performance test ensuring 1000+ entities process within frame budget
  - _Requirements: 1.1, 1.2, 1.3, 2.1, 3.3, 5.1, 5.2, 5.4, 5.5_

- [ ] 10. Add error handling and defensive programming measures
  - Add null safety checks for component access in MetabolismSystem
  - Add boundary validation for energy values and distance calculations
  - Add NaN and infinite value protection in proximity calculations
  - Implement graceful degradation when blueprint parameters are invalid
  - Add logging for debugging berry harvesting behavior and edge cases
  - _Requirements: 3.1, 3.2, 4.3, 4.4_

- [ ] 11. Update existing blueprint files with energy restoration configuration
  - Enhance entity-human.ngaoblueprint with MaxEnergy parameter
  - Verify entity-bush.ngaoblueprint has correct HarvestableComponent parameters
  - Ensure Blueberry blueprint has proper RejuvenationComponent configuration
  - Test blueprint loading and component creation with new parameters
  - _Requirements: 4.1, 4.2, 4.4_

- [ ] 12. Integrate energy restoration with existing ECS architecture
  - Ensure MetabolismSystem processes both energy consumption and restoration in single tick
  - Verify component filtering works correctly with enhanced RequiredComponentTypes
  - Test system execution order maintains proper energy state consistency
  - Validate that energy restoration integrates with existing AI decision-making systems
  - _Requirements: 2.4, 3.1, 3.2, 5.5_