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
        typeof(VitalityComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState gameState, IEntity entity)
    {
        var vitalityComponent = entity.FindComponent<VitalityComponent>();
        var isValid = vitalityComponent.EntityState is not EntityState.Unknown and not EntityState.Dead;

        if (!isValid)
        {
            return;
        }

        var traitComponent = entity.FindComponent<TraitComponent>();
        vitalityComponent.Energy -= traitComponent.FindEnergyConsumptionRate(vitalityComponent.EntityState);

        if (vitalityComponent.Energy <= 0)
        {
            vitalityComponent.EntityState = EntityState.Dead;
        }
    }
}