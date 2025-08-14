// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScalarExtensions.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, May 8, 2025 2:53:43 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace YamlDotNet.Core.Events;

using System.Reflection;
using nGratis.AI.Odin.Glue;
using nGratis.Cop.Olympus.Contract;

internal static class ScalarExtensions
{
    internal static readonly IReadOnlyDictionary<Type, IScalarHandler> ScalarHandlerByTargetTypeLookup;

    static ScalarExtensions()
    {
        ScalarExtensions.ScalarHandlerByTargetTypeLookup = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IScalarHandler)))
            .Where(type => !type.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IScalarHandler>()
            .ToDictionary(handler => handler.TargetType);
    }

    public static Scalar AsScalar(this object value)
    {
        if (!ScalarExtensions.ScalarHandlerByTargetTypeLookup.TryGetValue(value.GetType(), out var matchedHandler))
        {
            var valueType = value.GetType();

            matchedHandler = ScalarExtensions
                .ScalarHandlerByTargetTypeLookup.Values
                .FirstOrDefault(handler => valueType.IsAssignableTo(handler.TargetType));
        }

        var text = matchedHandler?.Write(value) ?? value.ToString();

        return new Scalar(text ?? DefinedText.Unknown);
    }

    public static object AsInferredObject(this Scalar scalar)
    {
        var text = scalar.Value;

        return ScalarExtensions
            .ScalarHandlerByTargetTypeLookup
            .Values
            .FirstOrDefault(handler => handler.CanRead(text))?
            .Read(text) ?? text;
    }
}