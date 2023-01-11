using System;
using System.Collections.Generic;
using System.Text;

using Joyersch.Input;
using Joyersch.Camera;

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
    class Level17 : SampleLevel
    {

        readonly AwesomeButton finishButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        readonly Rainbow rainbowColorTransition;
        readonly Laserwall[] laserWalls;
        float gameTimeMoveWalls;
        bool Left;
        public Level17(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 17 - Tutorial time?";



            Vector2 clustPos = new Vector2(-250, -150);
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));

            Info = new TextBuilder("this is still bad! ->", new Vector2(-296, -96), new Vector2(16, 16), null, 0);
            rainbowColorTransition = new Rainbow {
                Increment = 32,
                Speed = 32,
                Offset = 256
            };
            laserWalls = new Laserwall[4];
            finishButton = new AwesomeButton(new Vector2(-64, 96), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton")) {
                DrawColor = Color.White,
            };
            finishButton.Click += CallFinish;
            laserWalls[0] = new Laserwall(new Vector2(-320, -256), new Vector2(576, 224), Globals.Content.GetHitboxMapping("zonenew"));
            laserWalls[1] = new Laserwall(new Vector2(-320, -256), new Vector2(224, 576), Globals.Content.GetHitboxMapping("zonenew"));
            laserWalls[2] = new Laserwall(new Vector2(96, -256), new Vector2(224, 576), Globals.Content.GetHitboxMapping("zonenew"));
            laserWalls[3] = new Laserwall(new Vector2(-128, 64), new Vector2(200, 48), Globals.Content.GetHitboxMapping("zonenew"));
            for (int i = 0; i < laserWalls.Length; i++) {
                laserWalls[i].Enter += LaserEvent;
            }

        }

        private void LaserEvent(object sender, EventArgs e) {
            CallFail(sender, e);
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            finishButton.Draw(spriteBatch);
            for (int i = 0; i < laserWalls.Length; i++) {
                laserWalls[i].Draw(spriteBatch);
            }
            Info.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);

        }
        public override void Update(GameTime gameTime) {
            gameTimeMoveWalls += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (gameTimeMoveWalls > 80) {
                gameTimeMoveWalls -= 80;
                if (Left)
                    laserWalls[3].Move(new Vector2(-10, 0));
                else
                    laserWalls[3].Move(new Vector2(10, 0));
                if (laserWalls[3].Position.X >= -80)
                    Left = true;
                if (laserWalls[3].Position.X <= -128)
                    Left = false;
            }
            mouseCursor.Update(gameTime);
            rainbowColorTransition.Update(gameTime);
            Info.ChangeColor(rainbowColorTransition.GetColor(Info.Text.Length));
            Info.Update(gameTime);
            base.Update(gameTime);
            for (int i = 0; i < laserWalls.Length; i++) {
                laserWalls[i].Update(gameTime, mouseCursor.Hitbox[0]);
            }

            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            finishButton.Update(gameTime, mouseCursor.Hitbox[0]);
        }
    }
}
