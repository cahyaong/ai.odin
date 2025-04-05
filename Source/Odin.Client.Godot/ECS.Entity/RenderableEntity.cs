// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderableEntity.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, April 4, 2025 2:09:08 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

public partial class RenderableEntity : Node2D
{
    private static readonly IReadOnlyDictionary<EntityState, string> AnimationNameByEntityStateLookup;

    static RenderableEntity()
    {
        RenderableEntity.AnimationNameByEntityStateLookup = Enum
            .GetValues<EntityState>()
            .Where(entityState => entityState != EntityState.Unknown)
            .ToImmutableDictionary(
                entityState => entityState,
                entityState => entityState.ToString().ToLowerInvariant());
    }

    public AnimatedSprite2D AnimatedSprite { get; private set; }

    public override void _Ready()
    {
        this.AnimatedSprite = this.GetNode<AnimatedSprite2D>(nameof(RenderableEntity.AnimatedSprite));
    }

    public void UpdateAnimationState(EntityState entityState)
    {
        Guard
            .Require(entityState, nameof(entityState))
            .Is.Not.Default();

        this.AnimatedSprite.Play(RenderableEntity.AnimationNameByEntityStateLookup[entityState]);
    }
}