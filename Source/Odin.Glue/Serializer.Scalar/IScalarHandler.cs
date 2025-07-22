// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScalarHandler.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, May 9, 2025 3:16:24 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

public interface IScalarHandler
{
    public Type TargetType { get; }

    public bool CanRead(string text);

    public object Read(string text);

    public string Write(object value);
}