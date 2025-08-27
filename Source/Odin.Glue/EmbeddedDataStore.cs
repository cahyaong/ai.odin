// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedDataStore.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, May 28, 2025 3:05:12 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using System.Reflection;
using nGratis.AI.Odin.Engine;
using YamlDotNet.Serialization;

public class EmbeddedDataStore : IDataStore
{
    public IEnumerable<EntityBlueprint> LoadEntityBlueprints()
    {
        return EmbeddedDataStore.LoadBlueprints<EntityBlueprint>("entity");
    }

    public IEnumerable<SpriteSheetBlueprint> LoadSpriteSheetBlueprints()
    {
        return EmbeddedDataStore.LoadBlueprints<SpriteSheetBlueprint>("spritesheet");
    }

    private static IEnumerable<T> LoadBlueprints<T>(string discriminator)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var blueprintNames = assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(OdinMime.Blueprint.FileExtension, StringComparison.OrdinalIgnoreCase))
            .Where(name => name.Contains($"{discriminator}-"));

        var deserializedBlueprints = new List<T>();

        foreach (var blueprintName in blueprintNames)
        {
            using var blueprintStream = assembly.GetManifestResourceStream(blueprintName);

            if (blueprintStream is null)
            {
                continue;
            }

            using var blueprintReader = new StreamReader(blueprintStream);

            deserializedBlueprints.AddRange(blueprintReader
                .ReadToEnd()
                .DeserializeFromYaml<IEnumerable<T>>());
        }

        return deserializedBlueprints;
    }
}