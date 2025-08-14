// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookupScalarHandler.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, August 12, 2025 6:00:03 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using System.Collections;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

public class LookupScalarHandler : IScalarHandler
{
    public Type TargetType => typeof(IDictionary);

    public bool CanRead(string text) => Pattern.Lookup.IsMatch(text);

    public object Read(string text)
    {
        var lookupMatch = Pattern.Lookup.Match(text);

        if (!lookupMatch.Success)
        {
            throw new OdinException(
                "Text does not match <Lookup> pattern!",
                ("Text", text),
                ("Pattern", Pattern.Lookup));
        }

        return Pattern
            .Pair
            .Matches(lookupMatch.Groups["pairs"].Value)
            .Select(pairMatch => new
            {
                Key = pairMatch.Groups["key"].Value,
                Value = pairMatch.Groups["value"].Value
            })
            .ToImmutableDictionary(anon => anon.Key, anon => anon.Value);
    }

    public string Write(object value)
    {
        if (value is not IDictionary lookup)
        {
            throw new OdinException(
                "Value is not a <Lookup>!",
                ("Expected Type", this.TargetType),
                ("Actual Type", value.GetType().FullName ?? DefinedText.Unknown));
        }

        var pairBlobs = lookup
            .Keys
            .Cast<object>()
            .Select(key => $"{key}:{lookup[key]}");

        return new StringBuilder()
            .Append("<Lookup> (")
            .AppendJoin(", ", pairBlobs)
            .Append(")")
            .ToString();
    }

    private static class Pattern
    {
        public static readonly Regex Lookup = new(@"^\<Lookup\>\s*\((?<pairs>.+)\)$");
        public static readonly Regex Pair = new(@"(?<key>[\w\[\]_]+):(?<value>[\w\d\.\[\]_]+)");
    }
}