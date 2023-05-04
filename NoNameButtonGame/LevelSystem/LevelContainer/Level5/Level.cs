using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{

    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 5";
        
        var info = new DelayedText("Now that you know the basics. Lets actually start!")
        {
            DisplayDelay = 56,
            StartAfter = 100
        };
        var overTimeInvoker = new OverTimeInvoker(1500D, false);
        overTimeInvoker.Trigger += Finish;
        AutoManaged.Add(overTimeInvoker);
        
        info.Move(-info.GetBaseCopy().Rectangle.Size.ToVector2() / 2);
        info.FinishedPlaying += overTimeInvoker.Start;
        info.Start();
        AutoManaged.Add(info);
    }
}