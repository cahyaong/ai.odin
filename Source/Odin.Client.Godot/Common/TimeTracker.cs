// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeTracker.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:31:48 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using System;
using JetBrains.Annotations;
using nGratis.AI.Odin.Engine;

public partial class TimeTracker : Node, ITimeTracker
{
    private bool _isPaused;
    private uint _tick;
    private double _accumulatedDelta;

    public TimeTracker()
    {
        this._isPaused = true;
        this._tick = 0;
        this._accumulatedDelta = 0.0;
    }

    [CanBeNull]
    public event EventHandler<DeltaChangedEventArgs> DeltaChanged;

    [CanBeNull]
    public event EventHandler<TickChangedEventArgs> TickChanged;

    public void Start()
    {
        this._tick = 0;
        this._accumulatedDelta = 0.0;

        this._isPaused = false;
    }

    public void End()
    {
        this._isPaused = true;
    }

    public override void _Process(double delta)
    {
        if (this._isPaused)
        {
            return;
        }

        this.PublishDeltaChangedEvent(delta);

        this._accumulatedDelta += delta;

        while (this._accumulatedDelta >= 1.0)
        {
            this._accumulatedDelta--;
            this._tick++;
            this.PublishTickChangedEvent(this._tick);
        }
    }

    private void PublishDeltaChangedEvent(double delta)
    {
        if (this.DeltaChanged == null)
        {
            return;
        }

        var args = new DeltaChangedEventArgs
        {
            Value = delta
        };

        this.DeltaChanged.Invoke(this, args);
    }

    private void PublishTickChangedEvent(uint tick)
    {
        if (this.TickChanged == null)
        {
            return;
        }

        var args = new TickChangedEventArgs
        {
            Value = tick
        };

        this.TickChanged.Invoke(this, args);
    }
}