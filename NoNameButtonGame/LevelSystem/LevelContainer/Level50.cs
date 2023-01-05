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
    class Level50 : SampleLevel
    {

        readonly StateButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        public Level50(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            button = new StateButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("awesomebutton"), 1337) {
                DrawColor = Color.White,
            };
            button.Click += BtnEvent;
            Name = "Level 50 - It's finaly over";
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            Info = new TextBuilder("NO HELP THIS TIME", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch sp) {
            Info.Draw(sp);
            button.Draw(sp);
            cursor.Draw(sp);
        }
        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            Info.ChangePosition(-Info.rec.Size.ToVector2() / 2 + new Vector2(0, -64));
            
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gt, cursor.Hitbox[0]);
            Info.Update(gt);
        }
    }
}
