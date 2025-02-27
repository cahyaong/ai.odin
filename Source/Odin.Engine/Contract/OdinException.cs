// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OdinException.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, February 25, 2025 7:31:38 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Engine;

using nGratis.Cop.Olympus.Contract;

public class OdinException : OlympusException
{
    public OdinException(string message, params (string Key, object Value)[] details)
        : base(message, details)
    {
    }
}