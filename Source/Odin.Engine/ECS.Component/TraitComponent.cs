// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TraitComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, August 3, 2025 6:34:46 PM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class TraitComponent : IComponent
{
    public IReadOnlyDictionary<EntityState, float> EnergyConsumptionRateByEntityStateLookup { private get; init; }

    public float FindEnergyConsumptionRate(EntityState entityState) => this
        .EnergyConsumptionRateByEntityStateLookup
        .GetValueOrDefault(entityState, 0.0f);
}