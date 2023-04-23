using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Transactions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{
    private bool _isTextFinished;

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