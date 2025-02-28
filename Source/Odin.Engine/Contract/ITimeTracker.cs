// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeTracker.cs" company="nGratis">
//  The MIT License � Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong � cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, February 23, 2025 6:26:26 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface ITimeTracker
{
    event EventHandler<DeltaChangedEventArgs> DeltaChanged;

    event EventHandler<TickChangedEventArgs> TickChanged;

    void Start();

    void End();
}

public class DeltaChangedEventArgs : EventArgs
{
    public double Value { get; init; }
}

public class TickChangedEventArgs : EventArgs
{
    public uint Value { get; init; }
}