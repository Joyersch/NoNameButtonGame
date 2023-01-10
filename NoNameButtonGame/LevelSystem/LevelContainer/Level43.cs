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
    class Level43 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly TextBuilder[] Infos;
        readonly Laserwall[] WallLeft;
        readonly Laserwall[] WallRight;
        readonly Laserwall[] Blocks;
        readonly int WallLength = 5;
        float GT;
        public Level43(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 43 - text here c:";
            button = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"));
            button.Click += BtnEvent;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
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
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            button.Draw(sp);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Draw(sp);
            }
            for (int i = 0; i < WallLength; i++) {
                if (WallLeft[i].rec.Intersects(cameraRectangle))
                    WallLeft[i].Draw(sp);
                if (WallRight[i].rec.Intersects(cameraRectangle))
                    WallRight[i].Draw(sp);

            }
            for (int i = 0; i < Blocks.Length; i++) {
                if (Blocks[i].rec.Intersects(cameraRectangle))
                    Blocks[i].Draw(sp);
            }

            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Update(gt);
            }
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while (GT > 8) {
                GT -= 8;
                for (int i = 0; i < WallLength; i++) {
                    WallLeft[i].Move(new Vector2(0, -4));
                    WallRight[i].Move(new Vector2(0, -4));
                }
                for (int i = 0; i < Blocks.Length; i++) {
                    Blocks[i].Move(new Vector2(0, -4));
                }
            }
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gt, cursor.Hitbox[0]);
            for (int i = 0; i < WallLength; i++) {
                WallLeft[i].Update(gt, cursor.Hitbox[0]);
                WallRight[i].Update(gt, cursor.Hitbox[0]);
            }
            for (int i = 0; i < Blocks.Length; i++) {
                Blocks[i].Update(gt, cursor.Hitbox[0]);
            }
        }
    }
}
