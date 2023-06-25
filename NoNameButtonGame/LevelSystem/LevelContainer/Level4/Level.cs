using System;
using Microsoft.Xna.Framework;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level4;

public class Level : SampleLevel
{
    protected Level(Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 4 - RPG";
    }
}