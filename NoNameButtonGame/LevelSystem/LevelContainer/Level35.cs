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
    class Level35 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly TextBuilder[] Infos;
        readonly TextBuilder[] Scare;
        readonly Laserwall[] WallLeft;
        readonly Laserwall[] WallRight;
        readonly Laserwall[] Blocks;
        readonly int WallLength = 15;
        float GT;
        public Level35(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 35 - ITS FASTER AND THERE ARE GUNS!";
            button = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"));
            button.Click += BtnEvent;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Infos = new TextBuilder[2];
            Infos[0] = new TextBuilder("wow you did it!", new Vector2(120, -132), new Vector2(8, 8), null, 0);
            Infos[1] = new TextBuilder("gg!", new Vector2(120, -100), new Vector2(8, 8), null, 0);
            WallLeft = new Laserwall[WallLength];
            WallRight = new Laserwall[WallLength];
            List<Laserwall> Walls = new List<Laserwall>();
            for (int i = 0; i < WallLength; i++) {
                WallLeft[i] = new Laserwall(new Vector2(96, 512 * i), new Vector2(416, 512), Globals.Content.GetHitboxMapping("zonenew"));
                WallRight[i] = new Laserwall(new Vector2(-512, 512 * i), new Vector2(416, 512), Globals.Content.GetHitboxMapping("zonenew"));
                WallRight[i].Enter += CallFail;
                WallLeft[i].Enter += CallFail;
            }
            for (int i = 0; i < (WallLength - 1) * 8 / 2; i++) {
                int c = rand.Next(0, 3);
                if (c != 0)
                    Walls.Add(new Laserwall(new Vector2(-96, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew")));
                if (c != 1)
                    Walls.Add(new Laserwall(new Vector2(-32, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew")));
                if (c != 2)
                    Walls.Add(new Laserwall(new Vector2(32, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew")));

                // Walls.Add(new Laserwall(new Vector2(-96 + rand.Next(0, 3) * 64, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew")));

            }
            Blocks = Walls.ToArray();
            for (int i = 0; i < Blocks.Length; i++) {
                Blocks[i].Enter += CallFail;
            }
            Scare = new TextBuilder[16];
            for (int i = 0; i < Scare.Length; i++) {
                Scare[i] = new TextBuilder("AGUN", new Vector2(200, -128 + i * 16), new Vector2(16, 16), null, 0);
            }
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            
            button.Draw(spriteBatch);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Draw(spriteBatch);
            }
            for (int i = 0; i < WallLength; i++) {
                if (WallLeft[i].rectangle.Intersects(cameraRectangle))
                    WallLeft[i].Draw(spriteBatch);
                if (WallRight[i].rectangle.Intersects(cameraRectangle))
                    WallRight[i].Draw(spriteBatch);

            }
            for (int i = 0; i < Blocks.Length; i++) {
                if (Blocks[i].rectangle.Intersects(cameraRectangle))
                    Blocks[i].Draw(spriteBatch);
            }
            for (int i = 0; i < Scare.Length; i++) {
                Scare[i].Draw(spriteBatch);
            }
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            for (int i = 0; i < Scare.Length; i++) {
                Scare[i].Update(gameTime);
            }
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Update(gameTime);
            }
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (GT > 8) {
                GT -= 8;
                for (int i = 0; i < WallLength; i++) {
                    if (WallLeft[i].rectangle.Y > -1000)
                        WallLeft[i].Move(new Vector2(0, -3));
                    if (WallRight[i].rectangle.Y > -1000)
                        WallRight[i].Move(new Vector2(0, -3));
                }
                for (int i = 0; i < Blocks.Length; i++) {
                    if (Blocks[i].rectangle.Y > -1000)
                        Blocks[i].Move(new Vector2(0, -3));
                }
            }
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            for (int i = 0; i < WallLength; i++) {
                if (WallLeft[i].rectangle.Y > -1000)
                    WallLeft[i].Update(gameTime, cursor.Hitbox[0]);
                if (WallRight[i].rectangle.Y > -1000)
                    WallRight[i].Update(gameTime, cursor.Hitbox[0]);
            }
            for (int i = 0; i < Blocks.Length; i++) {
                if (Blocks[i].rectangle.Y > -1000)
                Blocks[i].Update(gameTime, cursor.Hitbox[0]);
            }
        }
    }
}
