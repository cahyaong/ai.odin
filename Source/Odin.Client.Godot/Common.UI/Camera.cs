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
    private static readonly IReadOnlyDictionary<int, float> ZoomValueByZoomLevelLookup;

    static Camera()
    {
        Camera.ZoomValueByZoomLevelLookup = new Dictionary<int, float>
        {
            [0] = 0.5f,
            [1] = 1,
            [2] = 2,
            [3] = 4,
        };
    }

    public Camera()
    {
        this.MinZoomLevel = Camera
            .ZoomValueByZoomLevelLookup.Keys
            .Min();

        this.MaxZoomLevel = Camera
            .ZoomValueByZoomLevelLookup.Keys
            .Max();

        this.ZoomIn();
    }

    public int ZoomLevel { get; private set; }

    public int MinZoomLevel { get; init; }

    public int MaxZoomLevel { get; init; }

    public void ZoomIn()
    {
        if (this.ZoomLevel >= this.MaxZoomLevel)
        {
            return;
        }

        this.ZoomLevel++;

        var zoomValue = Camera.ZoomValueByZoomLevelLookup[this.ZoomLevel];
        this.Zoom = new Vector2(zoomValue, zoomValue);
    }

    public void ZoomOut()
    {
        if (this.ZoomLevel <= this.MinZoomLevel)
        {
            return;
        }

        this.ZoomLevel--;

        var zoomValue = Camera.ZoomValueByZoomLevelLookup[this.ZoomLevel];
        this.Zoom = new Vector2(zoomValue, zoomValue);
    }
}