// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatistics.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, April 1, 2025 2:32:17 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IStatistics
{
    IReadOnlyCollection<string> MetricKeys { get; }

    IStatistics AddMetric(string key, string value);

    IStatistics AddMetric(string key, uint value) => this.AddMetric(key, value.ToString("N0"));

    IStatistics AddMetric(string key, double value) => this.AddMetric(key, value.ToString("N2"));

    IStatistics RemoveMetric(string key);

    string FindMetric(string key);
}