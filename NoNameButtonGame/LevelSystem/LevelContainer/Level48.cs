﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level48 : SampleLevel
    {

        readonly LockButton lockedButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        readonly Laserwall Walls;
        readonly TextButton ButtonStartTimer;
        readonly TextBuilder Timer;
        bool TimerStarted;
        float GT;
        float TGT;
        public Level48(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 48 - THERE IS NO ESCAPE!!";
            lockedButton = new LockButton(new Vector2(-256, -128), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"), true);
            lockedButton.Click += BtnEvent;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Info = new TextBuilder("RUN! IT FOLLOWs you!", new Vector2(-64, -132), new Vector2(16, 16), null, 0);

            Walls = new Laserwall(new Vector2(-32, -200), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew"));
            Walls.Enter += WallEvent;
            ButtonStartTimer = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "TimerStart", "Start Timer", new Vector2(8, 8));
            ButtonStartTimer.Click += StartTimer;
            Timer = new TextBuilder("0.0S", new Vector2(-16, 64), new Vector2(16, 16), null, 0);

        }
        private void StartTimer(object s, EventArgs e) {
            TimerStarted = true;
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }

        private void WallEvent(object sender, EventArgs e) {
            CallExit(sender, e);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            lockedButton.Draw(spriteBatch);
            if (TimerStarted) {
                Info.Draw(spriteBatch);
                Walls.Draw(spriteBatch);
                Timer.Draw(spriteBatch);
            }
            if (!TimerStarted && lockedButton.Locked)
                ButtonStartTimer.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            mouseCursor.Update(gameTime);
            if (TimerStarted) {
                Info.Update(gameTime);
                GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                TGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > 32) {
                    GT -= 32;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - Walls.rectangle.Center.ToVector2();

                    Walls.Move(Dir / Dir.Length() * (TGT / 1000));
                }
                Walls.Update(gameTime, mouseCursor.Hitbox[0]);
                float TL = (50000 - TGT) / 1000;
                if (TL <= 0) {
                    TimerStarted = false;
                    lockedButton.Locked = false;
                }
                Timer.ChangeText(TL.ToString("0.0").Replace(',', '.') + "S");
                Timer.Update(gameTime);
            }
            if (!TimerStarted && lockedButton.Locked)
                ButtonStartTimer.Update(gameTime, mouseCursor.Hitbox[0]);
            lockedButton.Update(gameTime, mouseCursor.Hitbox[0]);

        }
    }
}
