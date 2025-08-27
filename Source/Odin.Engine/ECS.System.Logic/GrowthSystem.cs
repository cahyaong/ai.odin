// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GrowthSystem.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, August 24, 2025 7:03:36 PM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class GrowthSystem : BaseFixedSystem
{
    public GrowthSystem(IEntityManager entityManager)
        : base(entityManager)
    {
    }

    protected override IReadOnlyCollection<Type> RequiredComponentTypes { get; } =
    [
        typeof(HarvestableComponent)
    ];

    protected override void ProcessEntity(uint tick, IGameState _, IEntity entity)
    {
        var harvestableComponent = entity.FindComponent<HarvestableComponent>();

        if (harvestableComponent.IsFull)
        {
            harvestableComponent.RemainingTick = 0;
            return;
        }

        harvestableComponent.RemainingTick--;

        if (harvestableComponent.RemainingTick > 0)
        {
            return;
        }

        harvestableComponent.RemainingTick = ComponentConstant.Harvestable.UpdatingTickRate;
        harvestableComponent.Amount++;
    }
}