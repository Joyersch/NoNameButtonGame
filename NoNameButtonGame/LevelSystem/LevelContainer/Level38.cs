using System;
using System.Collections.Generic;
using System.Text;

using Joyersch.Obj;
using Joyersch.Input;
using Joyersch.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level38 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly Laserwall wallup;
        readonly Laserwall walldown;
        readonly float Multiplier = 200;
        float GT;
        public Level38(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 38 - Swap time. again idk";
            button = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"));
            button.Click += CallFinish;
            cursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            wallup = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40), new Vector2(base.defaultWidth, defaultHeight), Globals.Content.GetHitboxMapping("zonenew"));
            walldown = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), 40), new Vector2(base.defaultWidth, defaultHeight), Globals.Content.GetHitboxMapping("zonenew"));
            wallup.Enter += CallFail;
            walldown.Enter += CallFail;
        }

        public override void Draw(SpriteBatch sp) {
            button.Draw(sp);
            wallup.Draw(sp);
            walldown.Draw(sp);
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            GT += (float) gt.ElapsedGameTime.TotalMilliseconds * 10;
            double angle = (GT % 1000  / 1000F * Math.PI * 2);
            cursor.Position = new Vector2(Multiplier * (float)Math.Cos(angle), Multiplier * (float)Math.Sin(angle));
            button.Position = mousePosition - button.Size / 2;
            wallup.Update(gt, button.rec);
            walldown.Update(gt, button.rec);
            button.Update(gt, cursor.Hitbox[0]);

        }
    }
}
