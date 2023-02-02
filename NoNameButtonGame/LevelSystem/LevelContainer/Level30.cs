﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level30 : SampleLevel
    {

        readonly TextButton[] button;
        readonly Cursor cursor;
        readonly TextBuilder Questions;
        readonly TextBuilder Timer;
        
        readonly int[] RightAwnsers = new int[7] { -1, 0, 0, 0, 2, 1,2 };
        readonly float GTMax = 20000;

        int Awnsered;
        float GT;
        
        public Level30(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 30 - You now the drill. now it will just get harder(ish)";
            button = new TextButton[3];
            Timer = new TextBuilder("", new Vector2(0, 0), new Vector2(16, 16), null, 0);
            Questions = new TextBuilder("0(n)", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            button[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), "0", "?", new Vector2(8, 8));
            button[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "1", "?", new Vector2(8, 8));
            button[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), "2", "?", new Vector2(8, 8));
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
                            Questions.ChangeText("sin (PI * 2) =");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("1");
                            button[2].Text.ChangeText("0.707");
                            break;
                        case 2:
                            Questions.ChangeText("sin(PI) = ");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("1");
                            button[2].Text.ChangeText("0.707");
                            break;
                        case 3:
                            Questions.ChangeText("sin(0) = ");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("1");
                            button[2].Text.ChangeText("0.707");
                            break;
                        case 4:
                            Questions.ChangeText("sin(PI/4)");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("1");
                            button[2].Text.ChangeText("0.707");
                            break;
                        case 5:
                            Questions.ChangeText("sin(PI/2)");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("1");
                            button[2].Text.ChangeText("0.707");
                            break;
                        case 6:
                            Questions.ChangeText("sin(PI/2 + PI)");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("-1");
                            button[2].Text.ChangeText("-0.707");
                            break;
                        case 7:
                            Questions.ChangeText("sin(PI/4 + PI)");
                            button[0].Text.ChangeText("0");
                            button[1].Text.ChangeText("-1");
                            button[2].Text.ChangeText("-0.707");
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
            Timer.ChangePosition(-Timer.rectangle.Size.ToVector2() / 2 + new Vector2(0, -160));
            Timer.Update(gameTime);
            if (Timer.Text.Contains("-"))
                Exit();
            Questions.ChangePosition(new Vector2(0, -128) - Questions.rectangle.Size.ToVector2() / 2);
            Questions.Update(gameTime);
            for (int i = 0; i < button.Length; i++) {
                button[i].Update(gameTime, cursor.Hitbox[0]);
            }
            cursor.Position = mousePosition - cursor.Size / 2;
        }
    }
}
