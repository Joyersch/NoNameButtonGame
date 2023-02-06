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
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level33 : SampleLevel
    {
        private readonly EmptyButton button;
        private readonly Cursor cursor;
        private readonly TextBuilder Info;
        private readonly Rainbow raincolor;
        private readonly GlitchBlockCollection[] laserwall;
        private float GT;
        private bool Left;
        public Level33(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 33 - Tutorial time?";



            Vector2 clustPos = new Vector2(-250, -150);
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));

            Info = new TextBuilder("this is still bad! ->", new Vector2(-296, -96), new Vector2(16, 16), null, 0);
            raincolor = new Rainbow {
                Increment = 32,
                GameTimeStepInterval = 32,
                Offset = 256
            };
            laserwall = new GlitchBlockCollection[4];
            button = new WinButton(new Vector2(-64, 96), new Vector2(128, 64)) {
                DrawColor = Color.White,
            };
            button.ClickEventHandler += Finish;
            laserwall[0] = new GlitchBlockCollection(new Vector2(-320, -256), new Vector2(576, 224));
            laserwall[1] = new GlitchBlockCollection(new Vector2(-320, -256), new Vector2(224, 576));
            laserwall[2] = new GlitchBlockCollection(new Vector2(96, -256), new Vector2(224, 576));
            laserwall[3] = new GlitchBlockCollection(new Vector2(-128, 64), new Vector2(200, 56));
            for (int i = 0; i < laserwall.Length; i++) {
                laserwall[i].EnterEventHandler += Fail;
            }

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
            while (GT > 80) {
                GT -= 80;
                if (Left)
                    laserwall[3].Move(new Vector2(-13, 0));
                else
                    laserwall[3].Move(new Vector2(13, 0));
                if (laserwall[3].Position.X >= -80)
                    Left = true;
                if (laserwall[3].Position.X <= -128)
                    Left = false;
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
