// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointScalarHandler.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Sunday, December 7, 2025 4:55:17 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

namespace nGratis.AI.Odin.Glue;

internal class PointScalarHandler : IScalarHandler
{
    public Type TargetType => typeof(Point);

    public bool CanRead(string text) => Pattern.Point.IsMatch(text);

    public object Read(string text)
    {
        var match = Pattern.Point.Match(text);

        if (!match.Success)
        {
            throw new OdinException(
                "Text does not match <Point> pattern!",
                ("Text", text),
                ("Pattern", Pattern.Point));
        }

        return new Point
        {
            X = int.Parse(match.Groups["x"].Value),
            Y = int.Parse(match.Groups["y"].Value)
        };
    }

    public string Write(object value)
    {
        if (value is not Point point)
        {
            throw new OdinException(
                "Value is not a <Point>!",
                ("Expected Type", this.TargetType),
                ("Actual Type", value.GetType().FullName ?? DefinedText.Unknown));
        }

        return $"<Point> (X:{point.X}, Y:{point.Y})";
    }

    private static class Pattern
    {
        public static readonly Regex Point = new(@"^\<Point\>\s*\(X:(?<x>\d+),\s*Y:(?<y>\d+)\)$");
    }
}