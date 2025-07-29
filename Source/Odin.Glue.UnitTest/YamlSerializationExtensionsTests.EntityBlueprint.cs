// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YamlSerializationExtensionsTests.EntityBlueprint.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, April 9, 2025 7:06:28 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue.UnitTest;

using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using nGratis.AI.Odin.Engine;
using Xunit;
using YamlDotNet.Serialization;

public class YamlSerializationExtensionsTests_EntityBlueprint
{
    public class SerializeToYamlMethod
    {
        [Fact]
        public void WhenGettingValidEntityBlueprint_ShouldSerializeImportantFields()
        {
            // Arrange.

            var entityBlueprint = new EntityBlueprint
            {
                Id = "[_MOCK_ENTITY_ID_]",
                ComponentBlueprints =
                [
                    new ComponentBlueprint
                    {
                        Id = "[_MOCK_COMPONENT_ID_01_]"
                    },
                    new ComponentBlueprint
                    {
                        Id = "[_MOCK_COMPONENT_ID_02_]",
                        Parameter = Parameter.None
                    },
                    new ComponentBlueprint
                    {
                        Id = "[_MOCK_COMPONENT_ID_03_]",
                        Parameter = new Parameter(new Dictionary<string, object>
                        {
                            ["[_MOCK_PARAMETER_KEY_31_]"] = "[_MOCK_PARAMETER_VALUE_31_]",
                            ["[_MOCK_PARAMETER_KEY_32_]"] = new Size { Width = 8, Height = 16 }
                        })
                    }
                ]
            };

            // Act.

            var blob = entityBlueprint.SerializeToYaml();

            // Assert.

            blob
                .Should().Be(Yaml.EntityBlueprint.Trim(), "because YAML should contain entity blueprint content");
        }
    }

    public class DeserializeFromYamlMethod
    {
        [Fact]
        public void WhenGettingValidYamlContent_ShouldDeserializeImportantFields()
        {
            // Arrange.

            // Act.

            var entityBlueprint = Yaml.EntityBlueprint.DeserializeFromYaml<EntityBlueprint>();

            // Assert.

            entityBlueprint
                .Should().NotBeNull();

            entityBlueprint
                .Id
                .Should().Be("[_MOCK_ENTITY_ID_]", "because entity blueprint should have ID");

            entityBlueprint
                .ComponentBlueprints
                .Should()
                .NotBeNull().And
                .HaveCount(3, "because entity blueprint should have component blueprints");

            var componentBlueprintByIdLookup = entityBlueprint
                .ComponentBlueprints
                .ToImmutableDictionary(componentBlueprint => componentBlueprint.Id);

            componentBlueprintByIdLookup
                .Keys
                .Should()
                .HaveCount(3, "because component blueprints should have unique ID").And
                .BeEquivalentTo("[_MOCK_COMPONENT_ID_01_]", "[_MOCK_COMPONENT_ID_02_]", "[_MOCK_COMPONENT_ID_03_]");

            componentBlueprintByIdLookup
                .Values
                .Should().NotContainNulls("because component blueprint should be initialized");

            using (new AssertionScope())
            {
                componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_01_]"]
                    .Parameter
                    .Should().Be(Parameter.None, "because first component blueprint has no parameter");

                componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_02_]"]
                    .Parameter
                    .Should().Be(Parameter.None, "because second component blueprint has no parameter");

                componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_03_]"]
                    .Parameter
                    .Should().NotBeNull("because third component blueprint has parameter");
            }

            componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_03_]"]
                .Parameter.Keys
                .Should()
                .HaveCount(2, "because third component parameters should have unique key").And
                .BeEquivalentTo("[_MOCK_PARAMETER_KEY_31_]", "[_MOCK_PARAMETER_KEY_32_]");

            using (new AssertionScope())
            {
                componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_03_]"]
                    .Parameter.FindValue<string>("[_MOCK_PARAMETER_KEY_31_]")
                    .Should().Be(
                        "[_MOCK_PARAMETER_VALUE_31_]",
                        "because third component blueprint should have parameter with valid string");

                componentBlueprintByIdLookup["[_MOCK_COMPONENT_ID_03_]"]
                    .Parameter.FindValue<Size>("[_MOCK_PARAMETER_KEY_32_]")
                    .Should().Be(
                        new Size { Width = 8, Height = 16 },
                        "because third component blueprint should have parameter with valid size");
            }
        }
    }

    internal static class Yaml
    {
        public static string EntityBlueprint => @"
id: '[_MOCK_ENTITY_ID_]'
component-blueprints:
  - id: '[_MOCK_COMPONENT_ID_01_]'
    parameter: None
  - id: '[_MOCK_COMPONENT_ID_02_]'
    parameter: None
  - id: '[_MOCK_COMPONENT_ID_03_]'
    parameter:
      '[_MOCK_PARAMETER_KEY_31_]': '[_MOCK_PARAMETER_VALUE_31_]'
      '[_MOCK_PARAMETER_KEY_32_]': (W:8, H:16)";
    }
}