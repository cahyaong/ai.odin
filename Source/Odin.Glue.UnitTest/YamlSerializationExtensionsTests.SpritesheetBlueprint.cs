// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YamlSerializationExtensionsTests.SpriteSheetBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, July 26, 2025 5:16:52 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue.UnitTest;

using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using nGratis.AI.Odin.Engine;
using Xunit;
using YamlDotNet.Serialization;

public class YamlSerializationExtensionsTests_SpriteSheetBlueprint
{
    public class SerializeToYamlMethod
    {
        [Fact]
        public void WhenGettingValidSpriteSheetBlueprint_ShouldSerializeImportantFields()
        {
            // Arrange.

            var spriteSheetBlueprint = new SpriteSheetBlueprint
            {
                Id = "[_MOCK_BLUEPRINT_ID_]",
                SpriteSize = new Size { Width = 16, Height = 32 },
                AnimationBlueprints =
                [
                    new AnimationBlueprint
                    {
                        Name = "[_MOCK_ANIMATION_ID_01_]",
                        StartingCell = new Cell { Row = 1, Column = 3 },
                        EndingCell = new Cell { Row = 5, Column = 7 }
                    },
                    new AnimationBlueprint
                    {
                        Name = "[_MOCK_ANIMATION_ID_02_]",
                        StartingCell = new Cell { Row = 2, Column = 4 },
                        EndingCell = new Cell { Row = 6, Column = 8 }
                    }
                ]
            };

            // Act.

            var blob = spriteSheetBlueprint.SerializeToYaml();

            // Assert.

            blob
                .Should().Be(Yaml.SpriteSheetBlueprint.Trim(), "because YAML should contain entity blueprint content");
        }
    }

    public class DeserializeFromYamlMethod
    {
        [Fact]
        public void WhenGettingValidSpriteSheetBlueprint_ShouldDeserializeImportantFields()
        {
            // Arrange.

            // Act.

            var spriteSheetBlueprint = Yaml.SpriteSheetBlueprint.DeserializeFromYaml<SpriteSheetBlueprint>();

            // Assert.

            spriteSheetBlueprint
                .Should().NotBeNull();

            spriteSheetBlueprint
                .Id
                .Should().Be("[_MOCK_BLUEPRINT_ID_]", "because spritesheet blueprint should have ID");

            spriteSheetBlueprint
                .SpriteSize
                .Should().Be(
                    new Size { Width = 16, Height = 32 }, "because spritesheet blueprint should have sprite size");

            spriteSheetBlueprint
                .AnimationBlueprints
                .Should()
                .NotBeNull().And
                .HaveCount(2, "because spritesheet blueprint should have animation blueprints");

            var animationBlueprintByIdLookup = spriteSheetBlueprint
                .AnimationBlueprints
                .ToImmutableDictionary(animationBlueprint => animationBlueprint.Name);

            animationBlueprintByIdLookup
                .Keys
                .Should()
                .HaveCount(2, "because animation blueprints should have unique ID").And
                .BeEquivalentTo("[_MOCK_ANIMATION_ID_01_]", "[_MOCK_ANIMATION_ID_02_]");

            animationBlueprintByIdLookup
                .Values
                .Should().NotContainNulls("because animation blueprint should be initialized");

            using (new AssertionScope())
            {
                animationBlueprintByIdLookup["[_MOCK_ANIMATION_ID_01_]"]
                    .StartingCell
                    .Should().Be(
                        new Cell { Row = 1, Column = 3 },
                        "because first animation blueprint should have starting coordinate");

                animationBlueprintByIdLookup["[_MOCK_ANIMATION_ID_01_]"]
                    .EndingCell
                    .Should().Be(
                        new Cell { Row = 5, Column = 7 },
                        "because first animation blueprint should have ending coordinate");
            }

            using (new AssertionScope())
            {
                animationBlueprintByIdLookup["[_MOCK_ANIMATION_ID_02_]"]
                    .StartingCell
                    .Should().Be(
                        new Cell { Row = 2, Column = 4 },
                        "because second animation blueprint should have starting coordinate");

                animationBlueprintByIdLookup["[_MOCK_ANIMATION_ID_02_]"]
                    .EndingCell
                    .Should().Be(
                        new Cell { Row = 6, Column = 8 },
                        "because second animation blueprint should have ending coordinate");
            }
        }
    }

    internal static class Yaml
    {
        public static string SpriteSheetBlueprint => @"
id: '[_MOCK_BLUEPRINT_ID_]'
sprite-size: <Size> (W:16, H:32)
animation-blueprints:
  - name: '[_MOCK_ANIMATION_ID_01_]'
    starting-cell: <Cell> (R:1, C:3)
    ending-cell: <Cell> (R:5, C:7)
  - name: '[_MOCK_ANIMATION_ID_02_]'
    starting-cell: <Cell> (R:2, C:4)
    ending-cell: <Cell> (R:6, C:8)";
    }
}