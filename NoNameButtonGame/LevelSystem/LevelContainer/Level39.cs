﻿using System;
using System.Collections.Generic;
using System.Text;

using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level39 : SampleLevel
    {
        private readonly TextButton[] button;
        private readonly Cursor cursor;
        private readonly TextBuilder Questions;
        private readonly TextBuilder Timer;
        private int Awnsered;
        private readonly int[] RightAwnsers = new int[6] { 0, -1, 1, 2, 1, 1 };
        private float GT;
        private readonly float GTMax = 15000;
        public Level39(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 39 - Questions!";
            button = new TextButton[3];
            Timer = new TextBuilder("", new Vector2(0, 0), new Vector2(16, 16), null, 0);
            Questions = new TextBuilder("q", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            button[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), "0", "a", new Vector2(8, 8));
            button[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "1", "b", new Vector2(8, 8));
            button[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), "2", "c", new Vector2(8, 8));
            for (int i = 0; i < button.Length; i++) {
                button[i].ClickEventHandler += BtnEvent;
            }
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        }
        private void BtnEvent(object sender) {

            if (RightAwnsers[Awnsered] != 0 && RightAwnsers[Awnsered] != -1) {
                Fail();
            } else {
                Awnsered++;
                if (Awnsered == RightAwnsers.Length)
                    Finish();
                else {
                    switch (Awnsered) {
                        case 1:
                            Questions.ChangeText("Are you still having fun?");
                            button[0].Text.ChangeText("yes");
                            button[1].Text.ChangeText("no");
                            button[2].Text.ChangeText("from time to time");
                            break;
                        case 2:
                            Questions.ChangeText("what was the last simon says level?");
                            button[0].Text.ChangeText("25");
                            button[1].Text.ChangeText("30");
                            button[2].Text.ChangeText("28");
                            break;
                        case 3:
                            Questions.ChangeText("what date is in a week from today?");
                            button[0].Text.ChangeText(new DateTime((DateTime.Now.Ticks + new TimeSpan(14 * 24, 0, 0).Ticks)).ToShortDateString());
                            button[1].Text.ChangeText(new DateTime((DateTime.Now.Ticks + new TimeSpan(21 * 24, 0, 0).Ticks)).ToShortDateString());
                            button[2].Text.ChangeText(new DateTime((DateTime.Now.Ticks + new TimeSpan(7 * 24, 0, 0).Ticks)).ToShortDateString());
                            break;
                        case 4:
                            Questions.ChangeText("Awnser 2");
                            button[0].Text.ChangeText("No this one");
                            button[1].Text.ChangeText("Yes");
                            button[2].Text.ChangeText("No this one");
                            break;
                        case 5:
                            Questions.ChangeText(" 12 + 7");
                            button[0].Text.ChangeText("18");
                            button[1].Text.ChangeText("19");
                            button[2].Text.ChangeText("21");
                            break;
                    }

                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < button.Length; i++) {
                button[i].Draw(spriteBatch);
            }
            Questions.Draw(spriteBatch);
            Timer.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Timer.ChangeText(((GTMax - GT) / 1000).ToString("0.0").Replace(',', '.') + "s");
            Timer.ChangePosition(-Timer.Rectangle.Size.ToVector2() / 2 + new Vector2(0, -160));
            Timer.Update(gameTime);
            if (Timer.Text.Contains("-"))
                Exit();
            Questions.ChangePosition(new Vector2(0, -128) - Questions.Rectangle.Size.ToVector2() / 2);
            Questions.Update(gameTime);
            for (int i = 0; i < button.Length; i++) {
                button[i].Update(gameTime, cursor.Hitbox[0]);
            }
            cursor.Position = mousePosition - cursor.Canvas / 2;
        }
    }
}
