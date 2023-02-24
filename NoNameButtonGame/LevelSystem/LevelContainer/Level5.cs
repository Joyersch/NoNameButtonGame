﻿using System;
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

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level5 : SampleLevel
{
    private readonly DelayedText _info;
    private bool _isTextFinished;
    private float savedGameTime;

    public Level5(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight,
        window, random)
    {
        Name = "Level 4 - Tutorial 3 - Button Addon: Hold";


        _info = new DelayedText("Now that you know the basics. Lets actually start!")
        {
            StartAfter = 100
        };

        _info.Move(-_info.GetBaseCopy().Rectangle.Size.ToVector2() / 2);
        _info.FinishedPlaying += DelayFinish;
        _info.Start();
    }

    private void DelayFinish()
        => _isTextFinished = true;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _info.Update(gameTime);

        if (!_isTextFinished)
            return;

        // finish level after 3 seconds after the delayedText has finished displaying
        savedGameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (savedGameTime >= 3000F)
            Finish();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _info.Draw(spriteBatch);
    }

    private void FakeReset(object sender) // ToDo: for maze level
    {
        SetMousePositionToCenter();
        if (_info.IsPlaying || _info.HasPlayed)
            return;
    }
}