// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScalarYamlConverter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, July 26, 2025 5:40:49 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class ScalarYamlConverter : IYamlTypeConverter
{
    private ScalarYamlConverter()
    {
    }

    public static ScalarYamlConverter Instance { get; } = new();

    public bool Accepts(Type type) => ScalarExtensions
        .ScalarHandlerByTargetTypeLookup
        .ContainsKey(type);

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (parser.Current is not Scalar)
        {
            throw new OdinException(
                "Current token must be scalar!",
                ("Expected Type", typeof(Scalar).FullName ?? DefinedText.Unknown),
                ("Actual Type", parser.Current?.GetType().FullName ?? DefinedText.Unknown));
        }

        return parser
            .Consume<Scalar>()
            .AsInferredObject();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (value == null)
        {
            return;
        }

        var scalar = value.AsScalar();
        emitter.Emit(scalar);
    }
}