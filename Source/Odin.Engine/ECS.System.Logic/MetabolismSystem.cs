// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetabolismSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, August 16, 2025 2:37:37 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class MetabolismSystem : BaseFixedSystem
{
    public MetabolismSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(TraitComponent),
        typeof(VitalityComponent),
        typeof(PhysicsComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();

        if (vitalityComponent.IsDead)
        {
            return;
        }

        var traitComponent = entity.FindComponent<TraitComponent>();
        var physicsComponent = entity.FindComponent<PhysicsComponent>();

        vitalityComponent.Energy -= traitComponent.FindEnergyConsumptionRate(physicsComponent.MotionState);

        if (vitalityComponent.Energy <= 0)
        {
            vitalityComponent.IsDead = true;
        }
    }
}