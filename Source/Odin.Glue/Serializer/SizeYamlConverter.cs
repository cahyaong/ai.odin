// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SizeYamlConverter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, April 9, 2025 6:45:31 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using nGratis.AI.Odin.Engine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class SizeYamlConverter : IYamlTypeConverter
{
    private SizeYamlConverter()
    {
    }

    public static SizeYamlConverter Instance { get; } = new();

    public bool Accepts(Type type) => type.IsAssignableTo(typeof(Size));

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value is not Size size)
        {
            throw new OdinException(
                "Value must be supported YAML converter!",
                ("Expected Type", typeof(Size)),
                ("Actual Type", type));
        }

        emitter.Emit(new Scalar($"(W:{size.Width}, H:{size.Height})"));
    }
}