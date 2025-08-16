// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VitalityComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, August 3, 2025 6:03:23 PM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class VitalityComponent : IComponent
{
    public VitalityComponent()
    {
        this.EntityState = EntityState.Unknown;
        this.Energy = 0;
    }

    public EntityState EntityState { get; set; }

    public float Energy { get; set; }
}