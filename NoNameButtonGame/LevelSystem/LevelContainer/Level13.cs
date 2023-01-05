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
    class Level13 : SampleLevel
    {

        readonly AwesomeButton userButton;
        readonly Cursor movingCursor;
        readonly Laserwall wallUp;
        readonly Laserwall wallDown;
        readonly float Multiplier = 100;
        public Level13(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 13 - Swap time.";
            userButton = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetTHBox("awesomebutton"));
            userButton.Click += CallFinish;
            movingCursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            wallUp = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40),new Vector2(base.defaultWidth, defaultHeight), Globals.Content.GetTHBox("zonenew"));
            wallDown = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), 40), new Vector2(base.defaultWidth, defaultHeight), Globals.Content.GetTHBox("zonenew"));
            wallUp.Enter += CallFail;
            wallDown.Enter += CallFail;
        }
        
        public override void Draw(SpriteBatch sp) {
            userButton.Draw(sp);
            wallUp.Draw(sp);
            wallDown.Draw(sp);
            movingCursor.Draw(sp);
        }

        
        public override void Update(GameTime gt) {
            movingCursor.Update(gt);
            base.Update(gt);
            double angle = gt.TotalGameTime.Milliseconds / 1000F * Math.PI * 2;
            movingCursor.Position = new Vector2(Multiplier * (float)Math.Sin(angle), Multiplier * (float)Math.Cos(angle));
            userButton.Position = mousePosition - userButton.Size / 2;
            wallUp.Update(gt, userButton.rec);
            wallDown.Update(gt, userButton.rec);
            userButton.Update(gt, movingCursor.Hitbox[0]);

        }
    }
}
