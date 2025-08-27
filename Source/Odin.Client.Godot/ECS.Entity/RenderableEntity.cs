// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderableEntity.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, April 4, 2025 2:09:08 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.Cop.Olympus.Contract;

public partial class RenderableEntity : Node2D
{
    // TODO (SHOULD): Implement fallback sprite if requested name is missing!

    private readonly AnimatedSprite2D _animatedSprite;

    private string _activeName;

    public RenderableEntity()
    {
        this._animatedSprite = new AnimatedSprite2D();

        this.AddChild(this._animatedSprite);
    }

    public void UpdateSpritesheet(SpriteFrames spriteFrames)
    {
        this._animatedSprite.SpriteFrames = spriteFrames;
    }

    public void UpdateAnimationState(string name)
    {
        Guard
            .Require(name, nameof(name))
            .Is.Not.Empty();
        
        if (this._activeName == name)
        {
            return;
        }

        this._animatedSprite.Play(name);
        this._activeName = name;
    }
}