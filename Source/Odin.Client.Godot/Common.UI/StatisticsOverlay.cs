// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticsOverlay.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:31:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using nGratis.AI.Odin.Engine;

public partial class StatisticsOverlay : CanvasLayer, IStatisticsOverlay
{
    private static class Default
    {
        public const string MetricLabel = "<SO.METRIC>";
    }

    private RichTextLabel _metricLabel;

    [CanBeNull]
    private IStatistics _statistics;

    public override void _Ready()
    {
        this._metricLabel = this.GetNode<RichTextLabel>("MetricLabel");
    }

    public override void _Process(double _)
    {
        if (this._statistics == null)
        {
            return;
        }

        var maxMetricKeyLength = this
            ._statistics.MetricKeys
            .Max(metricKey => metricKey.Length);

        var formattedStatisticChunks = this
            ._statistics.MetricKeys
            .Select(metricKey => new
                {
                    MetricKey = metricKey,
                    MetricValue = this._statistics.FindMetric(metricKey)
                })
            .Select(anon => $"[b]{anon.MetricKey.PadLeft(maxMetricKeyLength)}:[/b] {anon.MetricValue}")
            .ToImmutableArray();

        this._metricLabel.Text = formattedStatisticChunks.Any()
            ? string.Join(System.Environment.NewLine, formattedStatisticChunks)
            : Default.MetricLabel;
    }

    public void Update(IStatistics statistics)
    {
        this._statistics = statistics;
    }
}