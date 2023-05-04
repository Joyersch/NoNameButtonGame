using System;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level7;

public class Level : SampleLevel
{
    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        var x = new Quiz();
    }
}