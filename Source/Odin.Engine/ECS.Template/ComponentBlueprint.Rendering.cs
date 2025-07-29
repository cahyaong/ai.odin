// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentBlueprint.Rendering.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 29, 2025 3:59:55 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public record RenderingComponentBlueprint
{
    private readonly ComponentBlueprint _componentBlueprint;

    public RenderingComponentBlueprint(ComponentBlueprint componentBlueprint)
    {
        Guard
            .Require(componentBlueprint.Id, nameof(componentBlueprint.Id))
            .Is.EqualTo("Rendering");

        this._componentBlueprint = componentBlueprint;
    }

    public string SpritesheetBlueprintId => this
        ._componentBlueprint.Parameter
        .FindValue<string>(nameof(this.SpritesheetBlueprintId));

    public string TextureName => this
        ._componentBlueprint.Parameter
        .FindValue<string>(nameof(this.TextureName));
}