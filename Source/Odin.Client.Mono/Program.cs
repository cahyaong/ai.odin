// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="nGratis">
//  The MIT License (MIT)
//
//  Copyright (c) 2014 - 2025 Cahya Ong
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </copyright>
// <author>Cahya Ong - cahya.ong@gmail.com</author>
// <creation_timestamp>Saturday, April 9, 2022 5:34:26 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.AI.Odin.Client.Mono;

using SadConsole;
using SadConsole.Configuration;
using SadRogue.Primitives;

public static class Program
{
    private static void Main()
    {
        Settings.WindowTitle = "nGratis.AI.Odin.Client";

        Game.Create(new Builder()
            .UseDefaultConsole()
            .SetScreenSize(80, 40)
            .OnStart(OnStarted)
            .IsStartingScreenFocused(true)
            .ConfigureFonts(true));

        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void OnStarted(object _, GameHost __)
    {
        if (Game.Instance.StartingConsole == null)
        {
            return;
        }

        Game.Instance.StartingConsole.FillWithRandomGarbage(Game.Instance.StartingConsole.Font);
        Game.Instance.StartingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
        Game.Instance.StartingConsole.Print(4, 4, "Hello from Odin!");
    }
}