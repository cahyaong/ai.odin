// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeadUpDisplay.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:31:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;

public partial class HeadUpDisplay : Control
{
    public IStatisticsOverlay StatisticsOverlay => this.GetNode<StatisticsOverlay>(nameof(this.StatisticsOverlay));
}