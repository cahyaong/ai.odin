// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HarvestableComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, August 21, 2025 5:41:53 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public class HarvestableComponent : IComponent
{
    public HarvestableComponent()
    {
        this.ResourceBlueprintId = DefinedText.Unknown;
        this.AmountMax = 0;
        this.Amount = 0;
        this.RemainingTick = ushort.MaxValue;
    }

    public required string ResourceBlueprintId { get; init; }

    public required ushort AmountMax { get; init; }

    public ushort Amount { get; set; }

    public ushort RemainingTick { get; set; }

    public bool IsFull => this.Amount >= this.AmountMax;
}