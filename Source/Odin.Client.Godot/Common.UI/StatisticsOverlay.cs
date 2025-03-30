// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticsOverlay.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:31:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using nGratis.AI.Odin.Engine;

public partial class StatisticsOverlay : CanvasLayer, IStatisticsOverlay
{
    private static class Default
    {
        public const string MetricLabel = "<SO.METRIC>";
    }

    private readonly IDictionary<string, string> _metricValueByMetricKeyLookup;

    private RichTextLabel _metricLabel;

    public StatisticsOverlay()
    {
        this._metricValueByMetricKeyLookup = new Dictionary<string, string>();
    }

    public override void _Ready()
    {
        this._metricLabel = this.GetNode<RichTextLabel>("MetricLabel");
    }

    public override void _Process(double _)
    {
        var formattedStatisticChunks = this
            ._metricValueByMetricKeyLookup
            .Select(pair => $"[b]{pair.Key}:[/b] {pair.Value}")
            .ToImmutableArray();

        this._metricLabel.Text = formattedStatisticChunks.Any()
            ? string.Join(System.Environment.NewLine, formattedStatisticChunks)
            : Default.MetricLabel;
    }

    public void UpdateMetric(string key, string value)
    {
        this._metricValueByMetricKeyLookup[key] = value;
    }
}