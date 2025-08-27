// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YamlSerializationExtensions.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, April 9, 2025 6:16:00 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace YamlDotNet.Serialization;

using nGratis.AI.Odin.Engine;
using nGratis.AI.Odin.Glue;
using nGratis.Cop.Olympus.Contract;
using YamlDotNet.Core;
using YamlDotNet.Serialization.NamingConventions;

public static class YamlSerializationExtensions
{
    private static readonly INamingConvention NamingConvention = HyphenatedNamingConvention.Instance;

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
        .WithIndentedSequences()
        .WithNamingConvention(YamlSerializationExtensions.NamingConvention)
        .WithTypeConverter(ParameterYamlConverter.Instance)
        .WithTypeConverter(ScalarYamlConverter.Instance)
        .DisableAliases()
        .Build();

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(YamlSerializationExtensions.NamingConvention)
        .WithTypeConverter(ParameterYamlConverter.Instance)
        .WithTypeConverter(ScalarYamlConverter.Instance)
        .WithTypeConverter(new ScalarOrSequenceYamlConverter<EntityBlueprint>())
        .WithTypeConverter(new ScalarOrSequenceYamlConverter<SpriteSheetBlueprint>())
        .Build();

    public static string SerializeToYaml<T>(this T value)
    {
        return YamlSerializationExtensions
            .Serializer
            .Serialize(value)
            .Trim();
    }

    public static T DeserializeFromYaml<T>(this string value)
    {
        Guard
            .Require(value, nameof(value))
            .Is.Not.Empty();

        return YamlSerializationExtensions
            .Deserializer
            .Deserialize<T>(value.Trim());
    }

    internal static T Deserialize<T>(this IParser parser)
    {
        return YamlSerializationExtensions.Deserializer.Deserialize<T>(parser);
    }
}