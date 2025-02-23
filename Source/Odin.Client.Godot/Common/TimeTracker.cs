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
    private double _unprocessedDelta;

    public TimeTracker()
    {
        this._isPaused = true;
        this._tick = 0;
        this._unprocessedDelta = 0.0;
    }

    [CanBeNull]
    public event EventHandler<TimeChangedEventArgs> TimeChanged;

    public void Start()
    {
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

        this._unprocessedDelta += delta;

        while (this._unprocessedDelta >= 1.0)
        {
            this._unprocessedDelta--;
            this._tick++;
            this.PublishTimeChangedEvent();
        }
    }

    private void PublishTimeChangedEvent()
    {
        if (this.TimeChanged == null)
        {
            return;
        }

        var args = new TimeChangedEventArgs
        {
            Tick = this._tick
        };

        this.TimeChanged.Invoke(this, args);
    }
}