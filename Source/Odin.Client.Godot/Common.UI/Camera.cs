// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, February 28, 2025 3:51:11 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System.Collections.Generic;
using System.Linq;
using nGratis.AI.Odin.Engine;

public partial class Camera : Camera2D, ICamera
{
    private static readonly Vector2 HorizontalPanningOffset = new(Constant.PixelPerUnit, 0);
    private static readonly Vector2 VerticalPanningOffset = new(0, Constant.PixelPerUnit);

    private static readonly IReadOnlyDictionary<int, Vector2> ZoomingValueByZoomingLevelLookup;

    static Camera()
    {
        Camera.ZoomingValueByZoomingLevelLookup = new Dictionary<int, Vector2>
        {
            [0] = new(0.5f, 0.5f),
            [1] = new(1, 1),
            [2] = new(2, 2),
            [3] = new(4, 4),
        };
    }

    public Camera()
    {
        this.MinZoomingLevel = Camera
            .ZoomingValueByZoomingLevelLookup.Keys
            .Min();

        this.MaxZoomingLevel = Camera
            .ZoomingValueByZoomingLevelLookup.Keys
            .Max();

        this.ZoomIn();
    }

    public int ZoomingLevel { get; private set; }

    public int MinZoomingLevel { get; init; }

    public int MaxZoomingLevel { get; init; }

    public void ZoomIn()
    {
        if (this.ZoomingLevel >= this.MaxZoomingLevel)
        {
            return;
        }

        this.ZoomingLevel++;

        this.Zoom = Camera.ZoomingValueByZoomingLevelLookup[this.ZoomingLevel];
    }

    public void ZoomOut()
    {
        if (this.ZoomingLevel <= this.MinZoomingLevel)
        {
            return;
        }

        this.ZoomingLevel--;

        this.Zoom = Camera.ZoomingValueByZoomingLevelLookup[this.ZoomingLevel];
    }

    public void PanLeft()
    {
        this.Position -= Camera.HorizontalPanningOffset;
    }

    public void PanRight()
    {
        this.Position += Camera.HorizontalPanningOffset;
    }

    public void PanUp()
    {
        this.Position -= Camera.VerticalPanningOffset;
    }

    public void PanDown()
    {
        this.Position += Camera.VerticalPanningOffset;
    }
}