﻿using System;
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
    class Level14 : SampleLevel
    {

        readonly TextButton[] awnserButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Questions;
        int Awnsered;
        readonly int[] RightAwnsers = new int[4] { -1, -1, -1 ,-1};
        double totalGameTime;
        public Level14(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 14 - QnA Time! AGAIN!!!!";
            awnserButton = new TextButton[3];
            Questions = new TextBuilder("IS \"HAN SOLO\" STILL ALIVE", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            awnserButton[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "0", "YES", new Vector2(8, 8));
            awnserButton[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "1", "YES", new Vector2(8, 8));
            awnserButton[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton"), "2", "it depends", new Vector2(8, 8));
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].Click += BtnEvent;
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
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
                            Questions.ChangeText("IS \"2\" bigger than \"1\" if you count backwards?");
                            awnserButton[0].Text.ChangeText("NO");
                            awnserButton[1].Text.ChangeText("NO");
                            awnserButton[2].Text.ChangeText("PERHAPS");
                            break;
                        case 2:
                            Questions.ChangeText("DO PIGS EAT WITH THERE ASS?");
                            awnserButton[0].Text.ChangeText("NO");
                            awnserButton[1].Text.ChangeText("I THINK NOT");
                            awnserButton[2].Text.ChangeText("NEVER SEEn IT");
                            break;
                        case 3:
                            Questions.ChangeText("WHAT DID YOU DO " + totalGameTime +" SECONDS AGO?");
                            awnserButton[0].Text.ChangeText("WASTE MY LIFE");
                            awnserButton[1].Text.ChangeText("BE AN EEDIOT");
                            awnserButton[2].Text.ChangeText("(:->)");
                            break;
                    }

                }
            }

        }
        
        public override void Draw(SpriteBatch sp) {
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].Draw(sp);
            }
            Questions.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            totalGameTime = gt.TotalGameTime.TotalSeconds;
            mouseCursor.Update(gt);
            base.Update(gt);
            Questions.ChangePosition(new Vector2(0, -128) - Questions.rec.Size.ToVector2() / 2);
            Questions.Update(gt);
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].Update(gt, mouseCursor.Hitbox[0]);
            }
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
