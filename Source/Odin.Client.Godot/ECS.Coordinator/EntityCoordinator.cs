// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityCoordinator.cs" company="nGratis">
//  The MIT License -- Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong -- cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 29, 2025 3:30:34 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Godot;

using nGratis.AI.Odin.Engine;

public partial class EntityCoordinator : Node
{
    [Export]
    public AppBootstrapper AppBootstrapper { get; private set; }

    public IDataStore DataStore => this.AppBootstrapper.DataStore;
}
