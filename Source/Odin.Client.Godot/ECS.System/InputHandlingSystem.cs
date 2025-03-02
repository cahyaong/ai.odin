// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputHandlingSystem.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, February 28, 2025 3:44:39 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;

[SystemMetadata(OrderingIndex = int.MinValue)]
public class InputHandlingSystem : BaseSystem
{
    private readonly ICamera _camera;

    public InputHandlingSystem(ICamera camera)
    {
        this._camera = camera;
    }

    public override void ProcessVariableDuration(double _, IGameState __)
    {
        if (Input.IsActionJustPressed("UI.ZoomingIn"))
        {
            this._camera.ZoomIn();
        }
        else if (Input.IsActionJustPressed("UI.ZoomingOut"))
        {
            this._camera.ZoomOut();
        }

        if (Input.IsActionPressed("UI.PanningLeft"))
        {
            this._camera.PanLeft();
        }
        else if (Input.IsActionPressed("UI.PanningRight"))
        {
            this._camera.PanRight();
        }

        if (Input.IsActionPressed("UI.PanningUp"))
        {
            this._camera.PanUp();
        }
        else if (Input.IsActionPressed("UI.PanningDown"))
        {
            this._camera.PanDown();
        }
    }
}