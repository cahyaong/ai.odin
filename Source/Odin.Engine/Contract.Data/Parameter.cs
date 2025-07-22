// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parameter.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, April 9, 2025 2:38:20 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public record Parameter : IParameter
{
    private readonly IReadOnlyDictionary<string, object> _valueByKeyLookup;

    public Parameter(IReadOnlyDictionary<string, object> valueByKeyLookup)
    {
        this._valueByKeyLookup = valueByKeyLookup;
    }

    public static Parameter None { get; } = new(new Dictionary<string, object>());

    public IEnumerable<string> Keys => this._valueByKeyLookup.Keys;

    public T FindValue<T>(string key)
    {
        if (!this._valueByKeyLookup.TryGetValue(key, out var value))
        {
            throw new OdinException(
                "Parameter entry is not defined!",
                ("Expected Key", key),
                ("Actual Keys", this.Keys.ToPrettifiedText()));
        }

        return (T)value;
    }
}