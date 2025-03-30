// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUniverse.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Wednesday, February 26, 2025 7:00:57 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public interface IUniverse
{
    public Size Size { get; }

    public float Width => this.Size.Width;

    public float Height => this.Size.Height;
}