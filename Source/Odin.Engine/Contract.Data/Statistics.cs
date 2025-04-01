// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistics.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, April 1, 2025 2:41:40 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using System.Collections.Immutable;
using nGratis.Cop.Olympus.Contract;

public class Statistics : IStatistics
{
    private readonly IDictionary<string, string> _metricValueByMetricKeyLookup;

    public Statistics()
    {
        this._metricValueByMetricKeyLookup = new Dictionary<string, string>();
    }

    public IReadOnlyCollection<string> MetricKeys => this
        ._metricValueByMetricKeyLookup.Keys
        .ToImmutableList();

    public IStatistics AddMetric(string key, string value)
    {
        this._metricValueByMetricKeyLookup[key] = value;

        return this;
    }

    public IStatistics RemoveMetric(string key)
    {
        this._metricValueByMetricKeyLookup.Remove(key);

        return this;
    }

    public string FindMetric(string key)
    {
        return this._metricValueByMetricKeyLookup.TryGetValue(key, out var value)
            ? value
            : DefinedText.Unknown;
    }
}