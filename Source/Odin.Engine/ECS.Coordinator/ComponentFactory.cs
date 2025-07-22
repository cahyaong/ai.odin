// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentFactory.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 22, 2025 5:22:07 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using ComponentCreatorFunction = System.Func<Engine.ComponentBlueprint, Engine.IComponent>;

public class ComponentFactory : IComponentFactory
{
    private readonly IReadOnlyDictionary<string, ComponentCreatorFunction> _componentCreatorByIdLookup;

    public ComponentFactory()
    {
        this._componentCreatorByIdLookup = new Dictionary<string, ComponentCreatorFunction>
        {
            { "Intelligence", this.CreateIntelligenceComponent },
            { "Physics" , this.CreatePhysicsComponent }
        };
    }

    public IComponent CreateComponent(ComponentBlueprint blueprint)
    {
        return this._componentCreatorByIdLookup.TryGetValue(blueprint.Id, out var createComponent)
            ? createComponent(blueprint)
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