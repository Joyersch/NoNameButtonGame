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
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level31 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly TextBuilder[] Infos;
        readonly Laserwall wall;
        public Level31(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 31 - THICC";
            button = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"));
            button.Click += BtnEvent;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Infos = new TextBuilder[2];
            Infos[0] = new TextBuilder("Thin can be penetrated!", new Vector2(80, -132), new Vector2(8, 8), null, 0);
            Infos[1] = new TextBuilder("But this one is not thin its thicc!", new Vector2(80, -100), new Vector2(8, 8), null, 0);
            wall = new Laserwall(new Vector2(-120, -300), new Vector2(100, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            wall.Enter += WallEvent;
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Draw(spriteBatch);
            }
            wall.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Update(gameTime);
            }

            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            wall.Update(gameTime, cursor.Hitbox[0]);
        }
    }
}
