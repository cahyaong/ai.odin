// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScalarOrSequenceYamlConverter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, August 21, 2025 5:59:01 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class ScalarOrSequenceYamlConverter<T> : IYamlTypeConverter
{
    public bool Accepts(Type type) => type.IsAssignableTo(typeof(IEnumerable<T>));

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        var entries = new List<T>();

        if (parser.TryConsume<SequenceStart>(out _))
        {
            while (!parser.TryConsume<SequenceEnd>(out _))
            {
                entries.Add(parser.Deserialize<T>());
            }
        }
        else
        {
            entries.Add(parser.Deserialize<T>());
        }

        return entries;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        throw new NotSupportedException("Writing YAML using Scalar or Sequence converter is not supported!");
    }
}