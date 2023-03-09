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
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level5;

internal class Level : SampleLevel
{
    private bool _isTextFinished;
    private float savedGameTime;

    public Level(Display.Display display, Vector2 window, Random random) : base(display, window, random)
    {
        Name = "Level 4 - Tutorial 3 - Button Addon: Hold";


        var info = new DelayedText("Now that you know the basics. Lets actually start!")
        {
            StartAfter = 100
        };

        info.Move(-info.GetBaseCopy().Rectangle.Size.ToVector2() / 2);
        info.FinishedPlaying += DelayFinish;
        info.Start();
        
        AutoManaged.Add(info);
    }

    private void DelayFinish()
        => _isTextFinished = true;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!_isTextFinished)
            return;

        // finish level after 3 seconds after the delayedText has finished displaying
        savedGameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (savedGameTime >= 3000F)
            Finish();
    }
}