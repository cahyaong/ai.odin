// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 22, 2025 5:22:07 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections;
using System.Collections.Immutable;
using System.Reflection;
using ComponentCreatorFunction = System.Func<ComponentBlueprint, IComponent>;

public class ComponentFactory : IComponentFactory
{
    private static readonly ImmutableDictionary<string, ComponentCreatorFunction> ComponentCreatorByBlueprintIdLookup;

    static ComponentFactory()
    {
        ComponentFactory.ComponentCreatorByBlueprintIdLookup = typeof(ComponentFactory)
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Select(static methodInfo => new
            {
                MethodInfo = methodInfo,
                ComponentCreatorAttribute = methodInfo.GetCustomAttribute<ComponentCreatorAttribute>()
            })
            .Where(anon => anon.ComponentCreatorAttribute is not null)
            .Select(static anon => new
            {
                BlueprintId = anon.ComponentCreatorAttribute!.CreatedType.Name.Replace("Component", string.Empty),
                ComponentCreatorFunction = (ComponentCreatorFunction)Delegate.CreateDelegate(
                    typeof(ComponentCreatorFunction),
                    anon.MethodInfo)
            })
            .ToImmutableDictionary(anon => anon.BlueprintId, anon => anon.ComponentCreatorFunction);
    }

    public IComponent CreateComponent(ComponentBlueprint componentBlueprint)
    {
        var hasComponentCreator = ComponentFactory.ComponentCreatorByBlueprintIdLookup.TryGetValue(
            componentBlueprint.Id,
            out var createComponent);

        return hasComponentCreator
            ? createComponent!(componentBlueprint)
            : Component.Unknown;
    }

    [ComponentCreator(CreatedType = typeof(TraitComponent))]
    private static TraitComponent CreateTraitComponent(ComponentBlueprint componentBlueprint)
    {
        return new TraitComponent
        {
            EnergyConsumptionRateByEntityStateLookup = componentBlueprint
                .Parameter
                .FindValue<IDictionary>(nameof(TraitComponent.EnergyConsumptionRateByEntityStateLookup))
                .Cast<KeyValuePair<string, string>>()
                .Select(pair => new
                {
                    EntityState = Enum.Parse<MotionState>(pair.Key, true),
                    EnergyConsumptionRate = float.Parse(pair.Value)
                })
                .ToImmutableDictionary(anon => anon.EntityState, anon => anon.EnergyConsumptionRate)
        };
    }

    [ComponentCreator(CreatedType = typeof(VitalityComponent))]
    private static VitalityComponent CreateVitalityComponent(ComponentBlueprint _)
    {
        return new VitalityComponent
        {
            IsDead = false,
            Energy = 100.0f
        };
    }

    [ComponentCreator(CreatedType = typeof(IntelligenceComponent))]
    private static IntelligenceComponent CreateIntelligenceComponent(ComponentBlueprint _)
    {
        return new IntelligenceComponent
        {
            RemainingTick = 3
        };
    }

    [ComponentCreator(CreatedType = typeof(PhysicsComponent))]
    private static PhysicsComponent CreatePhysicsComponent(ComponentBlueprint _)
    {
        return new PhysicsComponent
        {
            MotionState = MotionState.Idling
        };
    }

    [ComponentCreator(CreatedType = typeof(HarvestableComponent))]
    private static HarvestableComponent CreateHarvestableComponent(ComponentBlueprint componentBlueprint)
    {
        return new HarvestableComponent
        {
            ResourceBlueprintId = componentBlueprint
                .Parameter
                .FindValue(nameof(HarvestableComponent.ResourceBlueprintId)),
            AmountMax = ushort.Parse(componentBlueprint
                .Parameter
                .FindValue(nameof(HarvestableComponent.AmountMax))),
            Amount = 0,
            RemainingTick = ComponentConstant.Harvestable.UpdatingTickRate
        };
    }
}