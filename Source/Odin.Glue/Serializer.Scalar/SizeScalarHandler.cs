// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SizeScalarHandler.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, May 9, 2025 3:18:41 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using System.Text.RegularExpressions;
using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

internal class SizeScalarHandler : IScalarHandler
{
    public Type TargetType => typeof(Size);

    public bool CanRead(string text) => Pattern.Size.IsMatch(text);

    public object Read(string text)
    {
        var match = Pattern.Size.Match(text);

        if (!match.Success)
        {
            throw new OdinException(
                "Text does not match <Size> pattern!",
                ("Text", text),
                ("Pattern", Pattern.Size));
        }

        return new Size
        {
            Width = int.Parse(match.Groups["width"].Value),
            Height = int.Parse(match.Groups["height"].Value)
        };
    }

    public string Write(object value)
    {
        if (value is not Size size)
        {
            throw new OdinException(
                "Value is not a <Size>!",
                ("Expected Type", this.TargetType),
                ("Actual Type", value.GetType().FullName ?? DefinedText.Unknown));
        }

        return $"(W:{size.Width}, H:{size.Height})";
    }

    private static class Pattern
    {
        public static readonly Regex Size = new(@"^\(W:(?<width>\d+), H:(?<height>\d+)\)$");
    }
}