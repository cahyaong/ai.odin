// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpriteSheetFactory.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 29, 2025 1:29:11 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using nGratis.AI.Odin.Engine;

public partial class SpriteSheetFactory : Node
{
    private readonly IDictionary<string, SpriteFrames> _spriteFramesBySpriteSheetIdLookup;
    private readonly IDictionary<string, Texture2D> _textureByNameLookup;

    public SpriteSheetFactory()
    {
        this._spriteFramesBySpriteSheetIdLookup = new ConcurrentDictionary<string, SpriteFrames>();
        this._textureByNameLookup = new ConcurrentDictionary<string, Texture2D>();
    }

    public SpriteFrames CreateSpriteFrames(SpriteSheetBlueprint spriteSheetBlueprint, string textureName)
    {
        if (this._spriteFramesBySpriteSheetIdLookup.TryGetValue(spriteSheetBlueprint.Id, out var spriteFrames))
        {
            return spriteFrames;
        }

        if (!this._textureByNameLookup.TryGetValue(textureName, out var texture))
        {
            texture = ResourceLoader.Load<Texture2D>($"res://Common.Art/{textureName}.png");

            if (texture == null)
            {
                throw new OdinException(
                    "Texture must be embedded under 'res://Common.Art'!",
                    ("Spritesheet Blueprint ID", spriteSheetBlueprint.Id),
                    ("Texture Name", textureName));
            }

            this._textureByNameLookup.Add(textureName, texture);
        }

        spriteFrames = new SpriteFrames();
        this._spriteFramesBySpriteSheetIdLookup.Add(spriteSheetBlueprint.Id, spriteFrames);

        foreach (var animationBlueprint in spriteSheetBlueprint.AnimationBlueprints)
        {
            spriteFrames.AddAnimation(animationBlueprint.Id);
            spriteFrames.SetAnimationSpeed(animationBlueprint.Id, 8.0);
            spriteFrames.SetAnimationLoop(animationBlueprint.Id, true);

            SpriteSheetFactory
                .CreateFrameRegions(animationBlueprint, spriteSheetBlueprint.SpriteSize)
                .Select(frameRegion => new AtlasTexture
                {
                    Atlas = texture,
                    Region = frameRegion
                })
                .ForEach(atlasTexture => spriteFrames.AddFrame(animationBlueprint.Id, atlasTexture));
        }

        return spriteFrames;
    }

    private static IEnumerable<Rect2I> CreateFrameRegions(AnimationBlueprint animationBlueprint, Size spriteSize)
    {
        var spriteWidth = (int)spriteSize.Width;
        var spriteHeight = (int)spriteSize.Height;

        var startingRow = animationBlueprint.StartingCell.Row;
        var endingRow = animationBlueprint.EndingCell.Row;

        for (var row = startingRow; row <= endingRow; row++)
        {
            var startingColumn = animationBlueprint.StartingCell.Column;
            var endingColumn = animationBlueprint.EndingCell.Column;

            for (var column = startingColumn; column <= endingColumn; column++)
            {
                yield return new Rect2I(column * spriteWidth, row * spriteHeight, spriteWidth, spriteHeight);
            }
        }
    }
}