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
using ComponentCreatorFunction = System.Func<Engine.ComponentBlueprint, Engine.IComponent>;

public class ComponentFactory : IComponentFactory
{
    private readonly IReadOnlyDictionary<string, ComponentCreatorFunction> _componentCreatorByIdLookup;

    public ComponentFactory()
    {
        this._componentCreatorByIdLookup = new Dictionary<string, ComponentCreatorFunction>
        {
            { "Trait", this.CreateTraitComponent },
            { "Vitality", this.CreateVitalityComponent },
            { "Physics" , this.CreatePhysicsComponent },
            { "Intelligence", this.CreateIntelligenceComponent }
        };
    }

    private IComponent CreateTraitComponent(ComponentBlueprint componentBlueprint)
    {
        return new TraitComponent
        {
            EnergyConsumptionRateByEntityStateLookup = componentBlueprint
                .Parameter
                .FindValue<IDictionary>("EnergyConsumptionRateByEntityState")
                .Cast<KeyValuePair<string, string>>()
                .Select(pair => new
                {
                    EntityState = Enum.Parse<EntityState>(pair.Key, true),
                    EnergyConsumptionRate = float.Parse(pair.Value)
                })
                .ToImmutableDictionary(anon => anon.EntityState, anon => anon.EnergyConsumptionRate)
        };
    }

    private IComponent CreateVitalityComponent(ComponentBlueprint _)
    {
        return new VitalityComponent
        {
            Energy = 100.0f
        };
    }

    public IComponent CreateComponent(ComponentBlueprint componentBlueprint)
    {
        return this._componentCreatorByIdLookup.TryGetValue(componentBlueprint.Id, out var createComponent)
            ? createComponent(componentBlueprint)
            : Component.Unknown;
    }

    private IntelligenceComponent CreateIntelligenceComponent(ComponentBlueprint _)
    {
        return new IntelligenceComponent
        {
            EntityState = EntityState.Idle,
            RemainingTickCount = 3
        };
    }

    private PhysicsComponent CreatePhysicsComponent(ComponentBlueprint _)
    {
        return new PhysicsComponent();
    }
}