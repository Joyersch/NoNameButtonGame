using System;
using System.Collections.Generic;
using System.Text;

using Raigy.Obj;
using Raigy.Input;
using Raigy.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level39 : SampleLevel
    {

        readonly TextButton[] button;
        readonly Cursor cursor;
        readonly TextBuilder Questions;
        readonly TextBuilder Timer;
        int Awnsered;
        readonly int[] RightAwnsers = new int[6] { 0, -1, 1, 2, 1, 1 };
        float GT;
        readonly float GTMax = 15000;
        public Level39(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 39 - Questions!";
            button = new TextButton[3];
            Timer = new TextBuilder("", new Vector2(0, 0), new Vector2(16, 16), null, 0);
            Questions = new TextBuilder("q", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            button[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "0", "a", new Vector2(8, 8));
            button[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "1", "b", new Vector2(8, 8));
            button[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "2", "c", new Vector2(8, 8));
            for (int i = 0; i < button.Length; i++) {
                button[i].Click += BtnEvent;
            }
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
        }
        private void BtnEvent(object sender, EventArgs e) {

            if (RightAwnsers[Awnsered] != int.Parse((sender as TextButton).Name) && RightAwnsers[Awnsered] != -1) {
                CallFail(this, e);
            } else {
                Awnsered++;
                if (Awnsered == RightAwnsers.Length)
                    CallFinish(this, e);
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
        public override void Draw(SpriteBatch sp) {
            for (int i = 0; i < button.Length; i++) {
                button[i].Draw(sp);
            }
            Questions.Draw(sp);
            Timer.Draw(sp);
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            Timer.ChangeText(((GTMax - GT) / 1000).ToString("0.0").Replace(',', '.') + "s");
            Timer.ChangePosition(-Timer.rec.Size.ToVector2() / 2 + new Vector2(0, -160));
            Timer.Update(gt);
            if (Timer.Text.Contains("-"))
                CallReset();
            Questions.ChangePosition(new Vector2(0, -128) - Questions.rec.Size.ToVector2() / 2);
            Questions.Update(gt);
            for (int i = 0; i < button.Length; i++) {
                button[i].Update(gt, cursor.Hitbox[0]);
            }
            cursor.Position = mousePosition - cursor.Size / 2;
        }
    }
}
