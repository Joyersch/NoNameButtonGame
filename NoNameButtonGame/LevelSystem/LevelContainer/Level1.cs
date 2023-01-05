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
    class Level1 : SampleLevel
    {

        readonly AwesomeButton startButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        public Level1(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            startButton = new AwesomeButton(new Vector2(-64, -32), new Vector2(160, 64), Globals.Content.GetTHBox("startbutton")) {
                DrawColor = Color.White,
            };
            startButton.Click += BtnEvent;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            Name = "Click the Button!";
            Info = new TextBuilder("How hard can it be?", new Vector2(-128, -64), new Vector2(16, 16), null, 0);
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch sp) {
            Info.Draw(sp);
            startButton.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            startButton.Update(gt, mouseCursor.Hitbox[0]);
            Info.Update(gt);
        }
    }
}
