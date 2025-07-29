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
        var assembly = Assembly.GetExecutingAssembly();

        var blueprintNames = assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(OdinMime.Blueprint.FileExtension, StringComparison.OrdinalIgnoreCase))
            .Where(name => name.Contains("entity-"));

        foreach (var blueprintName in blueprintNames)
        {
            using var blueprintStream = assembly.GetManifestResourceStream(blueprintName);

            if (blueprintStream is not null)
            {
                using var blueprintReader = new StreamReader(blueprintStream);

                yield return blueprintReader
                    .ReadToEnd()
                    .DeserializeFromYaml<EntityBlueprint>();
            }
        }
    }

    public IEnumerable<SpriteSheetBlueprint> LoadSpriteSheetBlueprints()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var blueprintNames = assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(OdinMime.Blueprint.FileExtension, StringComparison.OrdinalIgnoreCase))
            .Where(name => name.Contains("spritesheet-"));

        foreach (var blueprintName in blueprintNames)
        {
            using var blueprintStream = assembly.GetManifestResourceStream(blueprintName);

            if (blueprintStream is not null)
            {
                using var blueprintReader = new StreamReader(blueprintStream);

                yield return blueprintReader
                    .ReadToEnd()
                    .DeserializeFromYaml<SpriteSheetBlueprint>();
            }
        }
    }
}