// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderableEntity.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, April 4, 2025 2:09:08 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

public partial class RenderableEntity : Node2D
{
    private readonly AnimatedSprite2D _animatedSprite;

    public RenderableEntity()
    {
        this._animatedSprite = new AnimatedSprite2D();

        this.AddChild(this._animatedSprite);
    }

    public void UpdateSpritesheet(SpriteFrames spriteFrames)
    {
        this._animatedSprite.SpriteFrames = spriteFrames;
    }

    public void UpdateAnimationState(EntityState entityState)
    {
        Guard
            .Require(entityState, nameof(entityState))
            .Is.Not.Default();

        this._animatedSprite.Play(entityState.ToString());
    }
}