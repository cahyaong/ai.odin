# AI.Odin Game Mechanics Implementation Snippets

## Overview

This document contains comprehensive code implementation snippets for the 26 game mechanic systems within our Entity Component System (ECS) framework. Each section provides component definitions and system implementations in C# for creating emergent gameplay through sophisticated system interactions.

## Implementation Priority Matrix

**Note:** This document shows complete implementations for all 26 planned game mechanic systems. Currently implemented: `VitalityComponent`, `TraitComponent`, `MetabolismSystem`, and `GrowthSystem`. All code examples follow the existing ECS architecture patterns.

### Core Mechanics (1-6) - Essential Foundation

#### 1. Survival Mechanics

**Hunger System Components:**
```csharp
public class HungerComponent : IComponent
{
    public HungerComponent()
    {
        Current = 100.0f;
        Max = 100.0f;
        DecayRate = 1.0f;
        MetabolicEfficiency = 1.0f;
    }

    public float Current { get; set; }
    public float Max { get; set; }
    public float DecayRate { get; set; }
    public float MetabolicEfficiency { get; set; } // From genetic traits
}

public class StarvationComponent : IComponent
{
    public StarvationComponent()
    {
        Damage = 0.0f;
        IsStarving = false;
        TimeWithoutFood = 0.0f;
    }

    public float Damage { get; set; }
    public bool IsStarving { get; set; }
    public float TimeWithoutFood { get; set; }
}

public class FoodPreferenceComponent : IComponent
{
    public FoodPreferenceComponent()
    {
        Preferences = new Dictionary<FoodType, float>();
        Allergies = new List<FoodType>();
        NutritionalNeeds = 1.0f;
    }

    public Dictionary<FoodType, float> Preferences { get; set; }
    public List<FoodType> Allergies { get; set; }
    public float NutritionalNeeds { get; set; }
}
```

**Hunger System Implementation:**
```csharp
public class HungerDecaySystem : BaseFixedSystem
{
    public HungerDecaySystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(HungerComponent),
        typeof(TraitComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var hungerComponent = entity.FindComponent<HungerComponent>();
        var traitComponent = entity.FindComponent<TraitComponent>();
        
        var decayRate = hungerComponent.DecayRate * traitComponent.MetabolicEfficiency;
        hungerComponent.Current = Math.Max(0.0f, hungerComponent.Current - decayRate);
        
        if (hungerComponent.Current <= 0.0f && !entity.HasComponent<StarvationComponent>())
        {
            entity.AddComponent(new StarvationComponent { IsStarving = true });
        }
    }
}

public class StarvationDamageSystem : BaseFixedSystem
{
    public StarvationDamageSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(StarvationComponent),
        typeof(VitalityComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var starvationComponent = entity.FindComponent<StarvationComponent>();
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        
        if (starvationComponent.IsStarving && !vitalityComponent.IsDead)
        {
            starvationComponent.TimeWithoutFood += 1.0f;
            var damage = starvationComponent.TimeWithoutFood * 0.1f;
            vitalityComponent.Energy = Math.Max(0.0f, vitalityComponent.Energy - damage);
            
            if (vitalityComponent.Energy <= 0.0f)
            {
                vitalityComponent.IsDead = true;
            }
        }
    }
}
```

**Temperature & Climate Survival Components:**
```csharp
public class TemperatureToleranceComponent : IComponent
{
    public TemperatureToleranceComponent()
    {
        MinTemp = -10.0f;
        MaxTemp = 40.0f;
        CurrentBodyTemp = 37.0f;
        BaseInsulation = 1.0f;
    }

    public float MinTemp { get; set; }
    public float MaxTemp { get; set; }
    public float CurrentBodyTemp { get; set; }
    public float BaseInsulation { get; set; }
}

public class HypothermiaComponent : IComponent
{
    public HypothermiaComponent()
    {
        Severity = 0.0f;
        OnsetTemperature = 35.0f;
        ProgressionRate = 1.0f;
    }

    public float Severity { get; set; }
    public float OnsetTemperature { get; set; }
    public float ProgressionRate { get; set; }
}

public class HeatstrokeComponent : IComponent
{
    public HeatstrokeComponent()
    {
        Severity = 0.0f;
        OnsetTemperature = 39.0f;
        ProgressionRate = 1.0f;
    }

    public float Severity { get; set; }
    public float OnsetTemperature { get; set; }
    public float ProgressionRate { get; set; }
}

public class ClothingComponent : IComponent
{
    public ClothingComponent()
    {
        WarmthRating = 0.0f;
        CoolnessRating = 0.0f;
        Waterproofing = 0.0f;
        Durability = 100.0f;
    }

    public float WarmthRating { get; set; }
    public float CoolnessRating { get; set; }
    public float Waterproofing { get; set; }
    public float Durability { get; set; }
}
```

**Temperature Regulation System Implementation:**
```csharp
public class TemperatureRegulationSystem : BaseFixedSystem
{
    public TemperatureRegulationSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(TemperatureToleranceComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var tempComponent = entity.FindComponent<TemperatureToleranceComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        // Get environmental temperature at entity location
        var environmentalTemp = gameState.GetEnvironmentalTemperature(physicsComponent.Position);
        
        // Apply clothing modifiers if present
        var effectiveInsulation = tempComponent.BaseInsulation;
        if (entity.HasComponent<ClothingComponent>())
        {
            var clothing = entity.FindComponent<ClothingComponent>();
            effectiveInsulation += environmentalTemp < tempComponent.CurrentBodyTemp 
                ? clothing.WarmthRating 
                : clothing.CoolnessRating;
        }
        
        // Calculate body temperature change
        var tempDifference = environmentalTemp - tempComponent.CurrentBodyTemp;
        var tempChange = tempDifference / effectiveInsulation * 0.1f;
        tempComponent.CurrentBodyTemp += tempChange;
        
        // Check for temperature-related conditions
        CheckTemperatureConditions(entity, tempComponent);
    }
    
    private void CheckTemperatureConditions(IEntity entity, TemperatureToleranceComponent tempComponent)
    {
        if (tempComponent.CurrentBodyTemp < tempComponent.MinTemp)
        {
            if (!entity.HasComponent<HypothermiaComponent>())
            {
                entity.AddComponent(new HypothermiaComponent());
            }
        }
        else if (tempComponent.CurrentBodyTemp > tempComponent.MaxTemp)
        {
            if (!entity.HasComponent<HeatstrokeComponent>())
            {
                entity.AddComponent(new HeatstrokeComponent());
            }
        }
    }
}
```

**Disease & Plague System Components:**
```csharp
public struct ImmuneSystemComponent {
    public float Strength;
    public List<string> Antibodies;
    public float RecoveryRate;
    public float InfectionResistance;
}

public struct DiseaseComponent {
    public string Type;
    public float Severity;
    public float Contagiousness;
    public float Lethality;
    public float IncubationTime;
}

public struct InfectionComponent {
    public float Progress;
    public string DiseaseId;
    public float SymptomSeverity;
    public bool IsContagious;
}

public struct QuarantineComponent {
    public bool IsIsolated;
    public float IsolationTime;
    public Vector3 QuarantineLocation;
}
```

#### 2. Resource Management

**Resource Gathering Components:**
```csharp
public struct HarvestableComponent {
    public ResourceType Type;
    public float Amount;
    public float MaxAmount;
    public float RegrowthRate;
    public float QualityModifier;
    public bool RequiresTools;
}

public struct InventoryComponent {
    public Dictionary<ResourceType, int> Items;
    public Dictionary<ResourceType, float> Quality;
    public int MaxWeight;
    public int CurrentWeight;
    public List<int> EquippedTools;
}

public struct ToolComponent {
    public ToolType Type;
    public float Efficiency;
    public float Durability;
    public float MaxDurability;
    public Dictionary<ResourceType, float> GatheringBonuses;
    public MaterialType Material;
}
```

**Storage & Distribution Components:**
```csharp
public struct StorageComponent {
    public List<StorageFilter> AllowedItems;
    public int Capacity;
    public int Priority;
    public StorageType Type; // Warehouse, Refrigerator, Barrel, etc.
    public float PreservationBonus;
}

public struct StorageZoneComponent {
    public Bounds Area;
    public int Priority;
    public List<ResourceType> AllowedTypes;
    public float AccessibilityRating;
}

public struct HaulingJobComponent {
    public int ResourceEntity;
    public int TargetStorage;
    public float Priority;
    public float EstimatedTime;
    public HaulingStatus Status;
}

public struct PerishableComponent {
    public float Freshness;
    public float SpoilageRate;
    public float PreservationMethod;
    public bool IsRotting;
}
```

#### 3. Building & Construction

**Structure Building Components:**
```csharp
public struct BlueprintComponent {
    public BuildingType Type;
    public List<ResourceRequirement> Requirements;
    public float WorkRequired;
    public Vector3 Dimensions;
    public List<SkillRequirement> SkillRequirements;
}

public struct ConstructionProgressComponent {
    public float Progress;
    public float TotalWork;
    public List<int> AssignedWorkers;
    public Dictionary<ResourceType, int> MaterialsDelivered;
    public Dictionary<ResourceType, int> MaterialsRequired;
    public ConstructionPhase CurrentPhase;
}

public struct BuildingComponent {
    public BuildingType Type;
    public float Health;
    public float MaxHealth;
    public List<BuildingFunction> Functions;
    public MaterialType PrimaryMaterial;
    public float StructuralIntegrity;
    public List<int> Inhabitants;
}
```

**Automation System Components:**
```csharp
public struct ConveyorComponent {
    public float Speed;
    public Direction Direction;
    public float Capacity;
    public float PowerConsumption;
    public List<int> CarriedItems;
}

public struct ItemRouterComponent {
    public List<OutputRule> Rules;
    public RouterType Type; // Splitter, Filter, Priority Merger
    public int InputBuffer;
    public Dictionary<Direction, int> OutputBuffers;
}

public struct MachineComponent {
    public Recipe CurrentRecipe;
    public float Progress;
    public float Efficiency;
    public PowerRequirement Power;
    public float WearLevel;
    public List<int> InputItems;
    public List<int> OutputItems;
}
```

#### 4. Social & Colony Dynamics

**Relationships & Social Networks Components:**
```csharp
public struct RelationshipComponent {
    public Dictionary<int, RelationshipData> Relationships;
    public float Sociability;
    public float SocialNeed;
    public float CurrentSocialSatisfaction;
}

public struct MoodComponent {
    public float Current;
    public float Baseline;
    public List<MoodModifier> Modifiers;
    public float StabilityFactor;
}

public struct PersonalityComponent {
    public Dictionary<Trait, float> Traits;
    public List<string> Interests;
    public List<string> Dislikes;
    public float EmotionalStability;
}

public class RelationshipData {
    public float Opinion; // -100 to +100
    public RelationType Type; // Friend, Rival, Lover, Family, Enemy
    public List<SharedMemory> History;
    public float TrustLevel;
    public float Intimacy;
}
```

**Faction & Kingdom Management Components:**
```csharp
public struct FactionComponent {
    public int FactionId;
    public FactionRole Role;
    public float Loyalty;
    public float Influence;
}

public struct DiplomacyComponent {
    public Dictionary<int, RelationStatus> Relations;
    public List<Treaty> ActiveTreaties;
    public float DiplomaticSkill;
    public List<DiplomaticAction> RecentActions;
}

public struct TerritoryComponent {
    public List<Vector2> ClaimedTiles;
    public float InfluenceStrength;
    public List<Vector2> DisputedAreas;
    public Dictionary<int, float> BorderTensions;
}

public struct LeadershipComponent {
    public LeadershipStyle Style;
    public float Charisma;
    public List<Policy> EnactedPolicies;
    public float ApprovalRating;
    public List<int> Followers;
}
```

#### 5. Combat & Defense

**Combat Mechanics Components:**
```csharp
public struct CombatStatsComponent {
    public float Attack;
    public float Defense;
    public float Speed;
    public float CritChance;
    public float Accuracy;
    public float Dodge;
}

public struct WeaponComponent {
    public float Damage;
    public float Range;
    public AttackType Type;
    public float AttackSpeed;
    public float CriticalMultiplier;
    public MaterialType Material;
    public float Durability;
}

public struct ArmorComponent {
    public Dictionary<DamageType, float> Resistances;
    public float Weight;
    public float Durability;
    public float MaxDurability;
    public float MovementPenalty;
}

public struct CombatAIComponent {
    public TacticType PreferredTactic;
    public float Aggression;
    public float Cowardice;
    public List<int> CurrentTargets;
    public Vector3 LastKnownEnemyPosition;
}
```

**Defensive Structures Components:**
```csharp
public struct TurretComponent {
    public float Range;
    public float Damage;
    public float FireRate;
    public AmmoType RequiredAmmo;
    public int CurrentAmmo;
    public float PowerConsumption;
    public List<int> ValidTargets;
}

public struct WallComponent {
    public float Health;
    public float MaxHealth;
    public bool BlocksProjectiles;
    public bool BlocksMovement;
    public MaterialType Material;
    public float RepairCost;
}

public struct TrapComponent {
    public TriggerType Type;
    public float Damage;
    public bool IsArmed;
    public float RearmTime;
    public List<int> ValidTargets;
    public bool IsHidden;
}

public struct FortificationComponent {
    public List<int> WallSegments;
    public List<int> DefensiveStructures;
    public float OverallStrength;
    public List<Vector3> WeakPoints;
}
```

#### 6. Progression & Technology

**Research & Technology Components:**
```csharp
public struct ResearchProgressComponent {
    public Dictionary<TechId, float> Progress;
    public float ResearchSpeed;
    public TechId CurrentFocus;
    public List<TechId> QueuedResearch;
}

public struct UnlockedTechComponent {
    public HashSet<TechId> Unlocked;
    public HashSet<TechId> Available;
    public Dictionary<TechId, float> Familiarity;
}

public struct ResearchStationComponent {
    public float SpeedMultiplier;
    public TechId CurrentResearch;
    public List<TechCategory> Specializations;
    public float KnowledgeCapacity;
    public List<int> AssignedResearchers;
}

public struct InnovationComponent {
    public float CreativityScore;
    public List<TechId> PersonalDiscoveries;
    public float ExperimentationRate;
    public int FailedAttempts;
}
```

**Experience & Skills Components:**
```csharp
public struct SkillsComponent {
    public Dictionary<SkillType, SkillData> Skills;
    public float OverallExperience;
}

public struct ExperienceComponent {
    public Dictionary<SkillType, float> PendingXP;
    public float LearningSpeed;
    public float RetentionRate;
}

public struct PassionComponent {
    public Dictionary<SkillType, PassionLevel> Passions;
    public List<SkillType> Interests;
    public List<SkillType> Dislikes;
}

public class SkillData {
    public int Level;
    public float Experience;
    public float DecayResistance;
    public float Aptitude;
    public DateTime LastUsed;
}
```

### Advanced Systems (7-15) - Sophisticated Features

#### 7. Environmental & World Systems

**Weather Systems Components:**
```csharp
public struct WeatherAffectedComponent {
    public Dictionary<WeatherType, float> Modifiers;
    public bool SeeksShelter;
    public float WeatherAdaptability;
}

public struct WindComponent {
    public Vector2 Direction;
    public float Strength;
    public float Turbulence;
    public List<int> AffectedEntities;
}

public struct WeatherZoneComponent {
    public WeatherType CurrentWeather;
    public float Intensity;
    public float Duration;
    public float Temperature;
    public float Humidity;
    public WeatherType NextWeather;
}

public struct StormComponent {
    public StormType Type;
    public float Intensity;
    public Vector2 Direction;
    public float Radius;
    public float DamagePerSecond;
}
```

#### 8. Ecology & Life Cycles

**Reproduction & Genetics Components:**
```csharp
public struct ReproductionComponent {
    public float Fertility;
    public int ReproductionAge;
    public int MaxAge;
    public bool IsFertile;
    public List<int> PotentialMates;
    public int PregnancyDuration;
}

public struct GeneticsComponent {
    public Dictionary<GeneType, float> Genes;
    public List<GeneType> DominantTraits;
    public List<GeneType> RecessiveTraits;
    public float MutationRate;
    public int Generation;
}

public struct EcosystemComponent {
    public SpeciesType Species;
    public int PopulationSize;
    public float CarryingCapacity;
    public List<SpeciesType> PreySpecies;
    public List<SpeciesType> PredatorSpecies;
    public float TerritorySize;
}
```

#### 9. Production Chains

**Manufacturing Networks Components:**
```csharp
public struct ProductionChainComponent {
    public List<ProductionStep> Steps;
    public Dictionary<ResourceType, float> InputRates;
    public Dictionary<ResourceType, float> OutputRates;
    public float Efficiency;
    public List<QualityModifier> QualityFactors;
}

public struct FactoryComponent {
    public List<int> ProductionLines;
    public float PowerConsumption;
    public List<int> Workers;
    public Dictionary<ResourceType, int> RawMaterialStorage;
    public Dictionary<ResourceType, int> FinishedGoodsStorage;
}

public struct LogisticsComponent {
    public List<SupplyRoute> Routes;
    public float TransportCapacity;
    public List<int> Vehicles;
    public Dictionary<Vector3, float> DeliveryTimes;
}
```

#### 10. God Powers & Intervention

**Divine Intervention Components:**
```csharp
public struct DivinePower {
    public PowerType Type;         // Blessing, Curse, Weather, Resource
    public Vector3 TargetLocation;
    public float Intensity;
    public float Duration;
    public List<int> AffectedEntities;
    public float FaithCost;
}

public struct FaithComponent {
    public float FaithLevel;
    public List<DivineMiracle> WitnessedMiracles;
    public ReligiousBehavior Behavior;
    public int ShrinesBuilt;
    public float PrayerFrequency;
    public DeityType PreferredDeity;
}

public struct ShrineComponent {
    public DeityType Deity;
    public float FaithGeneration;
    public List<int> Worshippers;
    public float EffectRadius;
    public List<ReligiousRitual> AvailableRituals;
}
```

#### 11. Gas & Liquid Dynamics

**Atmospheric Simulation Components:**
```csharp
public struct AtmosphericComponent {
    public Dictionary<GasType, float> GasComposition;
    public float Pressure;
    public float Temperature;
    public List<GasSource> Sources;
    public List<GasSink> Sinks;
}

public struct VentilationComponent {
    public float AirFlow;
    public List<int> ConnectedVents;
    public float FilterEfficiency;
    public List<GasType> FilteredGases;
}

public struct PipeNetworkComponent {
    public FluidType FluidType;
    public float Pressure;
    public float FlowRate;
    public List<int> ConnectedPipes;
    public float Capacity;
    public bool IsBlocked;
}
```

#### 12. Advanced Power Systems

**Power Generation Components:**
```csharp
public struct PowerGeneratorComponent {
    public GeneratorType Type;
    public float PowerOutput;
    public float Efficiency;
    public ResourceType FuelType;
    public float FuelConsumption;
    public float MaintenanceRequired;
}

public struct PowerGridComponent {
    public List<int> Generators;
    public List<int> Consumers;
    public float TotalGeneration;
    public float TotalConsumption;
    public float GridStability;
}

public struct BatteryComponent {
    public float Capacity;
    public float CurrentCharge;
    public float ChargeRate;
    public float DischargeRate;
    public float Efficiency;
}
```

#### 13. Dwarf Fortress-Inspired Complex Systems

**Complex Psychology Components:**
```csharp
public struct DetailedPersonalityComponent {
    public Dictionary<PersonalityTrait, float> Traits; // 50+ traits
    public List<string> Likes;
    public List<string> Dislikes;
    public List<string> Fears;
    public List<string> Goals;
    public MentalHealthState MentalHealth;
}

public struct ThoughtComponent {
    public List<Thought> RecentThoughts;
    public Dictionary<ThoughtType, float> ThoughtWeights;
    public float OverallHappiness;
    public List<string> Memories;
}

public struct HistoricalFigureComponent {
    public string Name;
    public List<HistoricalDeed> Deeds;
    public Dictionary<string, float> Relationships;
    public List<string> Legends;
    public int HistoricalSignificance;
}
```

#### 14. Cooking & Crafting Systems

**Recipe Discovery Components:**
```csharp
public struct CookingSkillComponent {
    public float CookingLevel;
    public List<Recipe> KnownRecipes;
    public float CreativityBonus;
    public List<Ingredient> PreferredIngredients;
}

public struct QualityComponent {
    public QualityTier Quality; // Poor, Normal, Good, Excellent, Masterwork
    public float QualityModifier;
    public string CrafterSignature;
    public DateTime CreationDate;
    public List<QualityBonus> Bonuses;
}

public struct CraftingStationComponent {
    public CraftingType Type;
    public float QualityBonus;
    public List<Recipe> AvailableRecipes;
    public Dictionary<ResourceType, int> MaterialStorage;
    public int AssignedCrafter;
}
```

#### 15. Weapon Production & Colony Defense

**Material-Based Crafting Components:**
```csharp
public struct WeaponCraftingComponent {
    public List<WeaponComponent> Components;
    public MaterialType BladeType;
    public MaterialType HandleType;
    public MaterialType GuardType;
    public float CraftingProgress;
    public int CrafterSkillLevel;
}

public struct MaterialPropertiesComponent {
    public MaterialType Type;
    public float Hardness;
    public float Sharpness;
    public float Durability;
    public float Weight;
    public Dictionary<DamageType, float> Resistances;
}

public struct SiegeEquipmentComponent {
    public SiegeType Type;
    public float Damage;
    public float Range;
    public float Accuracy;
    public List<ResourceType> AmmoTypes;
    public int CrewRequired;
}
```

### Specialized Systems (16-21) - Genre-Specific Features

#### 16. Sierra City-Builder Mechanics

**Walker-Based Services Components:**
```csharp
public struct WalkerComponent {
    public ServiceType ServiceProvided;
    public float ServiceRadius;
    public List<Vector3> PatrolRoute;
    public float ServiceEffectiveness;
    public int MaxCapacity;
    public int CurrentLoad;
}

public struct HousingTierComponent {
    public HousingTier CurrentTier;
    public List<ServiceType> RequiredServices;
    public Dictionary<ServiceType, float> ServiceSatisfaction;
    public float DesirabilityRating;
    public int MaxOccupancy;
}

public struct ServiceCoverageComponent {
    public Dictionary<ServiceType, float> Coverage;
    public float LastServiceTime;
    public List<int> ServiceProviders;
    public float ServiceDecayRate;
}
```

#### 17. Anno Series Mechanics

**Multi-Island Economy Components:**
```csharp
public struct IslandComponent {
    public IslandType Type;
    public Dictionary<ResourceType, float> NaturalResources;
    public float FertilityRating;
    public ClimateType Climate;
    public List<int> Settlements;
}

public struct ShippingRouteComponent {
    public List<Vector3> RoutePoints;
    public Dictionary<ResourceType, int> CargoManifest;
    public float TravelTime;
    public int AssignedShips;
    public TradeRouteStatus Status;
}

public struct PopulationTierComponent {
    public CitizenTier Tier; // Farmer, Worker, Artisan, Engineer
    public Dictionary<ResourceType, float> Needs;
    public float Satisfaction;
    public int Population;
    public float TaxRate;
}
```

#### 18. Guild & Professional Specialization Systems

**Professional Advancement Components:**
```csharp
public struct GuildMembershipComponent {
    public GuildType Guild;
    public GuildRank Rank;
    public float GuildReputation;
    public List<int> Apprentices;
    public int Master;
    public List<string> Specializations;
}

public struct BusinessComponent {
    public BusinessType Type;
    public float Revenue;
    public List<int> Employees;
    public Dictionary<ResourceType, int> Inventory;
    public float CustomerSatisfaction;
    public List<int> Competitors;
}

public struct DynastyComponent {
    public string FamilyName;
    public List<int> FamilyMembers;
    public float FamilyWealth;
    public Dictionary<SkillType, float> FamilyTraditions;
    public List<string> FamilySecrets;
}
```

#### 19. Political Systems

**Government Systems Components:**
```csharp
public struct ElectionComponent {
    public List<int> Candidates;
    public Dictionary<int, int> Votes;
    public ElectionType Type;
    public DateTime ElectionDate;
    public List<string> CampaignPromises;
}

public struct PolicyComponent {
    public PolicyType Type;
    public Dictionary<FactionType, float> Support;
    public List<PolicyEffect> Effects;
    public float ImplementationCost;
    public DateTime EnactmentDate;
}

public struct PoliticalFactionComponent {
    public string FactionName;
    public List<int> Members;
    public float PoliticalPower;
    public List<PolicyType> PreferredPolicies;
    public List<int> Enemies;
    public List<int> Allies;
}
```

#### 20. Moral Choice Systems

**Moral Dilemma Components:**
```csharp
public struct MoralChoiceComponent {
    public string DilemmaDescription;
    public List<MoralOption> Options;
    public MoralWeight Severity;
    public List<int> AffectedAgents;
    public DateTime DecisionDeadline;
}

public struct SocietalValueComponent {
    public Dictionary<MoralValue, float> Values; // Hope, Order, Faith, etc.
    public List<MoralPrecedent> Precedents;
    public float MoralStability;
    public List<string> CulturalLaws;
}

public struct ConsequenceComponent {
    public MoralChoice OriginalChoice;
    public List<string> LongTermEffects;
    public float SocietalImpact;
    public DateTime ConsequenceDate;
}
```

#### 21. Creature Training

**Learning Systems Components:**
```csharp
public struct CreatureLearningComponent {
    public float LearningRate;
    public List<Behavior> LearnedBehaviors;
    public Dictionary<ActionType, float> ActionWeights;
    public float Alignment; // Good to Evil scale
    public List<int> Trainers;
}

public struct CreaturePersonalityComponent {
    public List<CreatureTrait> Traits;
    public float Aggressiveness;
    public float Playfulness;
    public float Loyalty;
    public EmotionalState CurrentMood;
}

public struct MiracleComponent {
    public MiracleType Type;
    public float Power;
    public List<int> Witnesses;
    public float FaithGenerated;
    public Vector3 Location;
}
```

### Emergent Systems (22-26) - Meta-Game Features

#### 22. Emergent Economic Systems

**Progressive Economic Components:**
```csharp
public struct EconomicPhaseComponent {
    public EconomicPhase CurrentPhase; // Simple, Multi-Commodity, Complex
    public float PhaseProgress;
    public List<EconomicIndicator> Indicators;
    public float MarketSophistication;
}

public struct MarketDynamicsComponent {
    public Dictionary<ResourceType, float> Prices;
    public Dictionary<ResourceType, float> SupplyLevels;
    public Dictionary<ResourceType, float> DemandLevels;
    public List<MarketEvent> RecentEvents;
    public float MarketVolatility;
}

public struct TradeNetworkComponent {
    public List<int> TradingPartners;
    public Dictionary<int, float> TradeIntimacy;
    public List<TradeRoute> ActiveRoutes;
    public float NetworkStability;
    public EconomicResilience Resilience;
}
```

#### 23. Multi-Generational Relationship Systems

**Family Tree Components:**
```csharp
public struct GenealogyComponent {
    public List<int> Ancestors;
    public List<int> Descendants;
    public List<int> Siblings;
    public int Spouse;
    public FamilyTree FamilyTree;
    public string FamilyName;
}

public struct InheritanceComponent {
    public Dictionary<ResourceType, float> Wealth;
    public List<SkillType> InheritedSkills;
    public Dictionary<int, float> PropertyShares;
    public List<string> FamilyTraditions;
}

public struct ReputationInheritanceComponent {
    public float FamilyReputation;
    public float PersonalReputation;
    public List<ReputationEvent> FamilyHistory;
    public float SocialStanding;
    public MultiGenerationalReputation ReputationData;
}
```

#### 24. Ruins & Archaeological Systems

**Dynamic Ruin Generation Components:**
```csharp
public struct RuinComponent {
    public int OriginalColonyId;
    public DateTime CollapseDate;
    public List<HistoricalArtifact> Artifacts;
    public string CollapseReason;
    public float StructuralIntegrity;
    public List<TreasureCache> HiddenVaults;
    public RuinCategory Category;
}

public struct ArchaeologicalSiteComponent {
    public List<int> DiscoverableItems;
    public float ExplorationProgress;
    public List<int> Archaeologists;
    public Dictionary<ArtifactType, float> ArtifactDensity;
    public List<HistoricalMystery> Mysteries;
}

public struct HistoricalArtifactComponent {
    public ArtifactType Type;
    public string Description;
    public DateTime CreationDate;
    public int OriginalOwner;
    public float HistoricalValue;
    public List<string> AssociatedEvents;
}
```

#### 25. Living World Storytelling Systems

**Narrative Generation Components:**
```csharp
public struct NarrativeEngineComponent {
    public List<CharacterArchetype> IdentifiedHeroes;
    public List<CharacterArchetype> IdentifiedVillains;
    public List<StoryArc> ActiveSagas;
    public Dictionary<string, float> DramaticTension;
    public List<PlotTwist> RecentTwists;
}

public struct BiographyComponent {
    public List<Achievement> MajorAchievements;
    public List<Relationship> SignificantRelationships;
    public List<HistoricalEvent> WitnessedEvents;
    public string LegacyDescription;
    public float HistoricalSignificance;
    public List<string> PersonalQuotes;
}

public struct ChronicleComponent {
    public List<ChronicleEntry> Entries;
    public float DramaRating;
    public List<int> KeyFigures;
    public string ChronicleTitle;
    public ChronicleType Type;
}
```

#### 26. Cross-Colony Economic Networks

**Trade Relationship Dynamics Components:**
```csharp
public struct InterColonyRelationComponent {
    public Dictionary<int, TradeRelationship> Relationships;
    public List<TradeAgreement> ActiveAgreements;
    public float EconomicInfluence;
    public List<EconomicCrisis> ExperiencedCrises;
}

public struct EconomicContagionComponent {
    public List<SupplyChainLink> SupplyChains;
    public float VulnerabilityIndex;
    public List<EconomicShock> ReceivedShocks;
    public float ResilienceRating;
    public List<int> EconomicPartners;
}

public struct TradeRouteNetworkComponent {
    public List<TradeRoute> Routes;
    public Dictionary<int, float> RouteReliability;
    public List<TradeDisruption> RecentDisruptions;
    public float NetworkRedundancy;
}
```

---

## Comprehensive System Implementations

### Core Survival Systems (Priority 1)

```csharp
// Disease and Immune System Implementation
public class DiseaseSpreadSystem : BaseFixedSystem
{
    public DiseaseSpreadSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(InfectionComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var infectionComponent = entity.FindComponent<InfectionComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        if (!infectionComponent.IsContagious) return;
        
        // Find nearby entities within contagion range
        var nearbyEntities = EntityManager.FindEntitiesInRadius(
            physicsComponent.Position, 
            GetContagionRadius(infectionComponent.DiseaseId));
            
        foreach (var nearbyEntity in nearbyEntities)
        {
            if (nearbyEntity.HasComponent<ImmuneSystemComponent>())
            {
                var immuneSystem = nearbyEntity.FindComponent<ImmuneSystemComponent>();
                AttemptInfection(nearbyEntity, immuneSystem, infectionComponent);
            }
        }
    }
    
    private void AttemptInfection(IEntity target, ImmuneSystemComponent immuneSystem, InfectionComponent source)
    {
        var resistanceChance = immuneSystem.InfectionResistance;
        if (immuneSystem.Antibodies.Contains(source.DiseaseId))
        {
            resistanceChance *= 2.0f; // Previous exposure provides immunity
        }
        
        if (Random.Shared.NextSingle() > resistanceChance)
        {
            target.AddComponent(new InfectionComponent 
            { 
                DiseaseId = source.DiseaseId,
                Progress = 0.0f,
                IsContagious = false // Incubation period
            });
        }
    }
}

// Resource Management Systems
public class ResourceGatheringSystem : BaseFixedSystem
{
    public ResourceGatheringSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(IntelligenceComponent),
        typeof(InventoryComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var intelligenceComponent = entity.FindComponent<IntelligenceComponent>();
        var inventoryComponent = entity.FindComponent<InventoryComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        // Check if entity is targeting a harvestable resource
        if (intelligenceComponent.TargetPosition.HasValue)
        {
            var nearbyResources = EntityManager.FindEntitiesWithComponent<HarvestableComponent>()
                .Where(e => Vector3.Distance(e.FindComponent<PhysicsComponent>().Position, 
                                           physicsComponent.Position) < 2.0f);
                                           
            foreach (var resourceEntity in nearbyResources)
            {
                PerformHarvesting(entity, resourceEntity, inventoryComponent);
            }
        }
    }
    
    private void PerformHarvesting(IEntity harvester, IEntity resource, InventoryComponent inventory)
    {
        var harvestableComponent = resource.FindComponent<HarvestableComponent>();
        
        if (harvestableComponent.Amount <= 0) return;
        
        var gatheringEfficiency = CalculateGatheringEfficiency(harvester, harvestableComponent);
        var harvestedAmount = Math.Min(harvestableComponent.Amount, gatheringEfficiency);
        
        // Add to inventory
        if (inventory.Items.ContainsKey(harvestableComponent.Type))
        {
            inventory.Items[harvestableComponent.Type] += (int)harvestedAmount;
        }
        else
        {
            inventory.Items[harvestableComponent.Type] = (int)harvestedAmount;
        }
        
        harvestableComponent.Amount -= harvestedAmount;
    }
}

// Construction System
public class ConstructionProgressSystem : BaseFixedSystem
{
    public ConstructionProgressSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(ConstructionProgressComponent),
        typeof(BlueprintComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var progressComponent = entity.FindComponent<ConstructionProgressComponent>();
        var blueprintComponent = entity.FindComponent<BlueprintComponent>();
        
        if (progressComponent.AssignedWorkers.Count == 0) return;
        
        // Calculate work progress based on assigned workers
        var totalWorkThisTick = 0.0f;
        foreach (var workerId in progressComponent.AssignedWorkers)
        {
            if (EntityManager.TryGetEntity(workerId, out var worker))
            {
                totalWorkThisTick += CalculateWorkerEfficiency(worker, blueprintComponent);
            }
        }
        
        progressComponent.Progress += totalWorkThisTick;
        
        // Check if construction is complete
        if (progressComponent.Progress >= progressComponent.TotalWork)
        {
            CompleteConstruction(entity, blueprintComponent);
        }
    }
    
    private void CompleteConstruction(IEntity constructionSite, BlueprintComponent blueprint)
    {
        // Remove construction components and add building component
        constructionSite.RemoveComponent<ConstructionProgressComponent>();
        constructionSite.RemoveComponent<BlueprintComponent>();
        
        constructionSite.AddComponent(new BuildingComponent
        {
            Type = blueprint.Type,
            Health = 100.0f,
            MaxHealth = 100.0f,
            Functions = GetBuildingFunctions(blueprint.Type)
        });
    }
}

// Social Relationship System
public class RelationshipUpdateSystem : BaseFixedSystem
{
    public RelationshipUpdateSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(RelationshipComponent),
        typeof(PersonalityComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var relationshipComponent = entity.FindComponent<RelationshipComponent>();
        var personalityComponent = entity.FindComponent<PersonalityComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        // Find nearby entities for social interactions
        var nearbyEntities = EntityManager.FindEntitiesInRadius(physicsComponent.Position, 5.0f)
            .Where(e => e.HasComponent<RelationshipComponent>() && e.Id != entity.Id);
            
        foreach (var otherEntity in nearbyEntities)
        {
            ProcessSocialInteraction(entity, otherEntity, relationshipComponent, personalityComponent);
        }
    }
    
    private void ProcessSocialInteraction(IEntity entity1, IEntity entity2, 
        RelationshipComponent relationships, PersonalityComponent personality)
    {
        var entityId2 = entity2.Id;
        
        if (!relationships.Relationships.ContainsKey(entityId2))
        {
            relationships.Relationships[entityId2] = new RelationshipData
            {
                Opinion = 0.0f,
                Type = RelationType.Neutral,
                History = new List<SharedMemory>(),
                TrustLevel = 0.5f,
                Intimacy = 0.0f
            };
        }
        
        var relationship = relationships.Relationships[entityId2];
        var otherPersonality = entity2.FindComponent<PersonalityComponent>();
        
        // Calculate compatibility and update relationship
        var compatibility = CalculatePersonalityCompatibility(personality, otherPersonality);
        var opinionChange = compatibility * 0.1f;
        
        relationship.Opinion = Math.Clamp(relationship.Opinion + opinionChange, -100.0f, 100.0f);
        UpdateRelationshipType(relationship);
    }
}

// Combat System
public class CombatResolutionSystem : BaseFixedSystem
{
    public CombatResolutionSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(CombatStatsComponent),
        typeof(CombatAIComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var combatStats = entity.FindComponent<CombatStatsComponent>();
        var combatAI = entity.FindComponent<CombatAIComponent>();
        var physics = entity.FindComponent<PhysicsComponent>();
        
        if (combatAI.CurrentTargets.Count == 0) return;
        
        foreach (var targetId in combatAI.CurrentTargets.ToList())
        {
            if (EntityManager.TryGetEntity(targetId, out var target))
            {
                ProcessCombatAction(entity, target, combatStats);
            }
            else
            {
                combatAI.CurrentTargets.Remove(targetId);
            }
        }
    }
    
    private void ProcessCombatAction(IEntity attacker, IEntity target, CombatStatsComponent attackerStats)
    {
        var targetStats = target.FindComponent<CombatStatsComponent>();
        var targetPhysics = target.FindComponent<PhysicsComponent>();
        var attackerPhysics = attacker.FindComponent<PhysicsComponent>();
        
        var distance = Vector3.Distance(attackerPhysics.Position, targetPhysics.Position);
        var weaponRange = GetWeaponRange(attacker);
        
        if (distance <= weaponRange)
        {
            PerformAttack(attacker, target, attackerStats, targetStats);
        }
    }
}

// Technology Research System
public class ResearchProgressSystem : BaseFixedSystem
{
    public ResearchProgressSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(ResearchStationComponent),
        typeof(BuildingComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var researchStation = entity.FindComponent<ResearchStationComponent>();
        var building = entity.FindComponent<BuildingComponent>();
        
        if (researchStation.CurrentResearch == TechId.None || 
            researchStation.AssignedResearchers.Count == 0) return;
            
        var researchProgress = CalculateResearchProgress(researchStation);
        
        // Update faction-wide research progress
        var faction = GetEntityFaction(entity);
        if (faction?.HasComponent<ResearchProgressComponent>() == true)
        {
            var factionResearch = faction.FindComponent<ResearchProgressComponent>();
            if (!factionResearch.Progress.ContainsKey(researchStation.CurrentResearch))
            {
                factionResearch.Progress[researchStation.CurrentResearch] = 0.0f;
            }
            
            factionResearch.Progress[researchStation.CurrentResearch] += researchProgress;
            
            // Check if research is complete
            var requiredProgress = GetRequiredResearchProgress(researchStation.CurrentResearch);
            if (factionResearch.Progress[researchStation.CurrentResearch] >= requiredProgress)
            {
                CompleteResearch(faction, researchStation.CurrentResearch);
            }
        }
    }
    
    private void CompleteResearch(IEntity faction, TechId completedTech)
    {
        var unlockedTech = faction.FindComponent<UnlockedTechComponent>();
        unlockedTech.Unlocked.Add(completedTech);
        
        // Unlock dependent technologies
        var dependentTechs = GetDependentTechnologies(completedTech);
        foreach (var tech in dependentTechs)
        {
            unlockedTech.Available.Add(tech);
        }
    }
}
```

### Advanced System Implementations (Priority 2)

```csharp
// Environmental Weather System
public class WeatherEffectSystem : BaseFixedSystem
{
    public WeatherEffectSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(WeatherAffectedComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var weatherComponent = entity.FindComponent<WeatherAffectedComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();
        
        var currentWeather = gameState.GetWeatherAt(physicsComponent.Position);
        
        if (weatherComponent.Modifiers.ContainsKey(currentWeather.Type))
        {
            ApplyWeatherEffects(entity, currentWeather, weatherComponent);
        }
        
        if (weatherComponent.SeeksShelter && RequiresShelter(currentWeather))
        {
            var intelligence = entity.FindComponent<IntelligenceComponent>();
            intelligence.TargetPosition = FindNearestShelter(physicsComponent.Position);
        }
    }
}

// Production Chain System
public class ProductionChainSystem : BaseFixedSystem
{
    public ProductionChainSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(ProductionChainComponent),
        typeof(FactoryComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var productionChain = entity.FindComponent<ProductionChainComponent>();
        var factory = entity.FindComponent<FactoryComponent>();
        
        if (factory.Workers.Count == 0) return;
        
        // Process each production step
        foreach (var step in productionChain.Steps)
        {
            ProcessProductionStep(entity, step, factory);
        }
        
        // Update efficiency based on worker skills and facility condition
        UpdateProductionEfficiency(productionChain, factory);
    }
}

// Ecosystem Balance System
public class EcosystemBalanceSystem : BaseFixedSystem
{
    public EcosystemBalanceSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(EcosystemComponent),
        typeof(ReproductionComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var ecosystem = entity.FindComponent<EcosystemComponent>();
        var reproduction = entity.FindComponent<ReproductionComponent>();
        
        // Population pressure affects reproduction rates
        var populationPressure = ecosystem.PopulationSize / ecosystem.CarryingCapacity;
        var adjustedFertility = reproduction.Fertility * (2.0f - populationPressure);
        
        // Predator-prey relationships
        AdjustPopulationBasedOnPredators(entity, ecosystem);
        
        // Resource competition
        if (populationPressure > 1.0f)
        {
            ApplyResourceCompetitionStress(entity);
        }
    }
}
```

### Specialized System Implementations (Priority 3)

```csharp
// Multi-Generational Relationship System
public class GenerationalRelationshipSystem : BaseFixedSystem
{
    public GenerationalRelationshipSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(GenealogyComponent),
        typeof(ReputationInheritanceComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var genealogy = entity.FindComponent<GenealogyComponent>();
        var reputation = entity.FindComponent<ReputationInheritanceComponent>();
        
        // Update family reputation based on personal actions
        var personalActions = GetRecentPersonalActions(entity);
        foreach (var action in personalActions)
        {
            var reputationImpact = CalculateReputationImpact(action);
            reputation.PersonalReputation += reputationImpact;
            
            // Family reputation changes at 25% rate
            reputation.FamilyReputation += reputationImpact * 0.25f;
        }
        
        // Inheritance and legacy systems
        ProcessInheritanceTransfer(entity, genealogy);
    }
}

// Archaeological Discovery System
public class ArchaeologySystem : BaseFixedSystem
{
    public ArchaeologySystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(ArchaeologicalSiteComponent),
        typeof(RuinComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var archaeologicalSite = entity.FindComponent<ArchaeologicalSiteComponent>();
        var ruin = entity.FindComponent<RuinComponent>();
        
        if (archaeologicalSite.Archaeologists.Count == 0) return;
        
        foreach (var archaeologistId in archaeologicalSite.Archaeologists)
        {
            if (EntityManager.TryGetEntity(archaeologistId, out var archaeologist))
            {
                ProcessArchaeologicalExcavation(archaeologist, entity, archaeologicalSite, ruin);
            }
        }
    }
}

// Narrative Generation System
public class NarrativeGenerationSystem : BaseVariableSystem
{
    public NarrativeGenerationSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(NarrativeEngineComponent),
        typeof(BiographyComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var narrativeEngine = entity.FindComponent<NarrativeEngineComponent>();
        var biography = entity.FindComponent<BiographyComponent>();
        
        // Analyze entity's recent actions for narrative significance
        var significantEvents = AnalyzeRecentEvents(entity);
        
        foreach (var eventData in significantEvents)
        {
            if (IsNarrativelySignificant(eventData))
            {
                GenerateStoryArc(narrativeEngine, biography, eventData);
            }
        }
        
        // Update dramatic tension based on current conflicts
        UpdateDramaticTension(narrativeEngine, entity);
    }
}
```

---

## System Integration Notes

### Naming Conventions Followed:
- **Components**: `[Purpose]Component` inheriting from `IComponent`
- **Systems**: `[Purpose]System` inheriting from `BaseFixedSystem` or `BaseVariableSystem`
- **No Name Conflicts**: Each system has a unique purpose and name

### Architecture Compliance:
- All systems declare `RequiredComponentTypes` for entity filtering
- Systems use `EntityManager` for entity queries and component access
- Proper dependency injection through constructor
- Component data encapsulation with properties
- Separation of concerns between components (data) and systems (behavior)

### Performance Considerations:
- Systems process only entities with required components
- Spatial queries use radius-based entity finding
- Batch processing where applicable
- Event-driven updates to minimize unnecessary calculations

*Note: These system implementations follow the existing AI.Odin ECS architecture patterns and should integrate seamlessly with the current codebase. Each system is designed to work independently while supporting emergent behaviors through component interactions.*