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
using NoNameButtonGame.Colors;
namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level3 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        readonly Rainbow raincolor;
        readonly Laserwall[] laserwall;
        float GT;
        public Level3(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 3 - Tutorial time!";
            

            
            Vector2 clustPos = new Vector2(-250, -150);
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            
            Info = new TextBuilder("this is bad! ->", new Vector2(-296, -96), new Vector2(16, 16), null, 0);
            raincolor = new Rainbow {
                Increment = 32,
                Speed = 32,
                Offset = 256
            };
            laserwall = new Laserwall[4];
            button = new AwesomeButton(new Vector2(-64,96), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton")) {
                DrawColor = Color.White,
            };
            button.Click += CallFinish;
            laserwall[0] = new Laserwall(new Vector2(-320, -256), new Vector2(576, 224), Globals.Content.GetHitboxMapping("zonenew"));
            laserwall[1] = new Laserwall(new Vector2(-320, -256), new Vector2(224, 576), Globals.Content.GetHitboxMapping("zonenew"));
            laserwall[2] = new Laserwall(new Vector2(96, -256), new Vector2(224, 576), Globals.Content.GetHitboxMapping("zonenew"));
            laserwall[3] = new Laserwall(new Vector2(-128, 64), new Vector2(200, 24), Globals.Content.GetHitboxMapping("zonenew"));
            for (int i = 0; i < laserwall.Length; i++) {
                laserwall[i].Enter += LaserEvent;
            }
            
        }

        private void LaserEvent(object sender, EventArgs e) {
            CallFail(sender, e);
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender,e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            for (int i = 0; i < laserwall.Length; i++) {
                laserwall[i].Draw(spriteBatch);
            }
            Info.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
           
        }

        public override void Update(GameTime gameTime) {
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while(GT > 125) {
                GT -= 125;
                //laserwall.Move(new Vector2(1, 0));
            }
            cursor.Update(gameTime);
            raincolor.Update(gameTime);
            Info.ChangeColor(raincolor.GetColor(Info.Text.Length));
            Info.Update(gameTime);
            base.Update(gameTime);
            for (int i = 0; i < laserwall.Length; i++) {
                laserwall[i].Update(gameTime, cursor.Hitbox[0]);
            }
            
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
        }
    }
}
