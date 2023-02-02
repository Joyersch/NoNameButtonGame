﻿using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

class Level8 : SampleLevel
{
    readonly LockButton button;
    readonly Cursor cursor;
    readonly TextBuilder Info;
    readonly Laserwall wall;
    readonly TextButton ButtonStartTimer;
    readonly TextBuilder Timer;
    bool TimerStarted;
    float GT;
    float TGT;

    public Level8(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        Name = "Level 8 - RUN FOREST, RUN!!!";
        button = new LockWinButton(new Vector2(-256, -128), new Vector2(128, 64), true);
        button.ClickEventHandler += Finish;
        cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Info = new TextBuilder("RUN! IT FOLLOWs you!", new Vector2(-64, -132), new Vector2(16, 16), null, 0);

        wall = new Laserwall(new Vector2(-32, -200), new Vector2(64, 64));
        wall.EnterEventHandler += Fail;
        ButtonStartTimer = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "TimerStart", "Start Timer",
            new Vector2(8, 8));
        ButtonStartTimer.ClickEventHandler += StartTimer;
        Timer = new TextBuilder("0.0S", new Vector2(-16, 64), new Vector2(16, 16), null, 0);
    }

    private void StartTimer(object sender)
        => TimerStarted = true;

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        if (TimerStarted)
        {
            Info.Draw(spriteBatch);
            wall.Draw(spriteBatch);
            Timer.Draw(spriteBatch);
        }

        if (!TimerStarted && button.Locked)
            ButtonStartTimer.Draw(spriteBatch);
        cursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        cursor.Position = mousePosition - cursor.Size / 2;
        cursor.Update(gameTime);
        if (TimerStarted)
        {
            Info.Update(gameTime);
            GT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            TGT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            while (GT > 32)
            {
                GT -= 32;
                Vector2 Dir = cursor.Hitbox[0].Center.ToVector2() - wall.rectangle.Center.ToVector2();

                wall.Move(Dir / Dir.Length() * (TGT / 1000));
            }

            wall.Update(gameTime, cursor.Hitbox[0]);
            float TL = (20000 - TGT) / 1000;
            if (TL <= 0)
            {
                TimerStarted = false;
                button.Locked = false;
            }

            Timer.ChangeText(TL.ToString("0.0").Replace(',', '.') + "S");
            Timer.Update(gameTime);
        }

        if (!TimerStarted && button.Locked)
            ButtonStartTimer.Update(gameTime, cursor.Hitbox[0]);
        button.Update(gameTime, cursor.Hitbox[0]);
    }
}