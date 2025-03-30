// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntelligenceComponent.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Thursday, March 27, 2025 4:41:19 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

public class IntelligenceComponent : IComponent
{
    public IntelligenceComponent()
    {
        this.TargetPosition = Point.Unknown;
    }

    public Point TargetPosition { get; set; }
}