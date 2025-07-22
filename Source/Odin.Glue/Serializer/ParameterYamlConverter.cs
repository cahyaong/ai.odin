// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterYamlConverter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, April 9, 2025 3:02:12 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class ParameterYamlConverter : IYamlTypeConverter
{
    private ParameterYamlConverter()
    {
    }

    public static ParameterYamlConverter Instance { get; } = new();

    public bool Accepts(Type type) => type.IsAssignableTo(typeof(IParameter));

    public object ReadYaml(IParser parser, Type _, ObjectDeserializer __)
    {
        return parser.Current is Scalar
            ? ParameterYamlConverter.ReadScalarYaml(parser)
            : ParameterYamlConverter.ReadMappingYaml(parser);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer _)
    {
        if (value is not Parameter parameter)
        {
            throw new OdinException(
                "Value must be supported YAML converter!",
                ("Expected Type", typeof(Parameter).FullName ?? DefinedText.Unknown),
                ("Actual Type", type));
        }

        if (parameter == Parameter.None)
        {
            emitter.Emit(new Scalar(nameof(Parameter.None)));
        }
        else
        {
            emitter.Emit(new MappingStart());

            foreach (var key in parameter.Keys)
            {
                emitter.Emit(new Scalar(key));

                emitter.Emit(parameter
                    .FindValue<object>(key)
                    .AsScalar());
            }

            emitter.Emit(new MappingEnd());
        }
    }

    private static Parameter ReadScalarYaml(IParser parser)
    {
        var value = parser.Consume<Scalar>().Value;

        if (value == nameof(Parameter.None))
        {
            return Parameter.None;
        }

        throw new OdinException(
            "Content must contain supported scalar value!",
            ("Expected Value", nameof(Parameter.None)),
            ("Actual Value", value));
    }

    private static Parameter ReadMappingYaml(IParser parser)
    {
        if (parser.Current is not MappingStart)
        {
            throw new OdinException(
                "Content must contain supported token!",
                ("Expected Token", typeof(MappingStart).FullName ?? DefinedText.Unknown),
                ("Actual Token", parser.Current?.GetType().FullName ?? DefinedText.Unknown));
        }

        var valueByKeyLookup = new Dictionary<string, object>();

        parser.MoveNext();

        while (parser.Current is not MappingEnd)
        {
            var key = parser
                .Consume<Scalar>()
                .Value;

            var value = parser
                .Consume<Scalar>()
                .AsInferredObject();

            valueByKeyLookup[key] = value;
        }

        parser.MoveNext();

        return new Parameter(valueByKeyLookup);
    }
}