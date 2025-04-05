// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiagnosticsOverlay.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, March 8, 2025 5:53:04 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;

public partial class DiagnosticsOverlay : Node, IDiagnosticOverlay
{
    private static readonly Color DebuggingColor = Color.Color8(215, 123, 186);

    private bool _hasMapBoundary;

    public void Update(IGameState gameState)
    {
        if (!this._hasMapBoundary)
        {
            var mapWidth = gameState.Universe.Width * Constant.PixelPerUnit;
            var mapHeight = gameState.Universe.Height * Constant.PixelPerUnit;

            var mapBoundary = new Line2D
            {
                Points =
                [
                    new Vector2(0, 0),
                    new Vector2(mapWidth, 0),
                    new Vector2(mapWidth, mapHeight),
                    new Vector2(0, mapHeight),
                    new Vector2(0, 0)
                ],
                Width = 2,
                BeginCapMode = Line2D.LineCapMode.Box,
                EndCapMode = Line2D.LineCapMode.Box,
                DefaultColor = DiagnosticsOverlay.DebuggingColor
            };

            this.AddChild(mapBoundary);
            this._hasMapBoundary = true;
        }
    }
}