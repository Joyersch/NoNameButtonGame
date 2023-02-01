using System;
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

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level38 : SampleLevel
    {

        readonly EmptyButton button;
        readonly Cursor cursor;
        readonly Laserwall wallup;
        readonly Laserwall walldown;
        readonly float Multiplier = 200;
        float GT;
        public Level38(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 38 - Swap time. again idk";
            button = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
            button.ClickEventHandler += CallFinish;
            cursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10));
            wallup = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40), new Vector2(base.defaultWidth, defaultHeight));
            walldown = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), 40), new Vector2(base.defaultWidth, defaultHeight));
            wallup.EnterEventHandler += CallFail;
            walldown.EnterEventHandler += CallFail;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            wallup.Draw(spriteBatch);
            walldown.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            GT += (float) gameTime.ElapsedGameTime.TotalMilliseconds * 10;
            double angle = (GT % 1000  / 1000F * Math.PI * 2);
            cursor.Position = new Vector2(Multiplier * (float)Math.Cos(angle), Multiplier * (float)Math.Sin(angle));
            button.Position = mousePosition - button.Size / 2;
            wallup.Update(gameTime, button.rectangle);
            walldown.Update(gameTime, button.rectangle);
            button.Update(gameTime, cursor.Hitbox[0]);

        }
    }
}
