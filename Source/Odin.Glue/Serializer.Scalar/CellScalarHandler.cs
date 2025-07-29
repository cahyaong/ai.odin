// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellScalarHandler.cs" company="nGratis">
//  The MIT License — Copyright (c) Cahya Ong
//  See the LICENSE file in the project root for more information.
// </copyright>
// <author>Cahya Ong — cahya.ong@gmail.com</author>
// <creation_timestamp>Tuesday, July 29, 2025 2:19:42 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Glue;

using System.Text.RegularExpressions;
using nGratis.AI.Odin.Engine;
using nGratis.Cop.Olympus.Contract;

public class CellScalarHandler : IScalarHandler
{
    public Type TargetType => typeof(Cell);

    public bool CanRead(string text) => Pattern.Cell.IsMatch(text);

    public object Read(string text)
    {
        var match = Pattern.Cell.Match(text);

        if (!match.Success)
        {
            throw new OdinException(
                "Text does not match <Cell> pattern!",
                ("Text", text),
                ("Pattern", Pattern.Cell));
        }

        return new Cell
        {
            Row = int.Parse(match.Groups["row"].Value),
            Column = int.Parse(match.Groups["column"].Value)
        };
    }

    public string Write(object value)
    {
        if (value is not Cell cell)
        {
            throw new OdinException(
                "Value is not a <Cell>!",
                ("Expected Type", this.TargetType),
                ("Actual Type", value.GetType().FullName ?? DefinedText.Unknown));
        }

        return $"(R:{cell.Row}, C:{cell.Column})";
    }

    private static class Pattern
    {
        public static readonly Regex Cell = new(@"^\(R:(?<row>-?\d+), C:(?<column>-?\d+)\)$");
    }
}