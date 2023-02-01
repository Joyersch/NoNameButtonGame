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
    class Level1 : SampleLevel
    {

        readonly StartButton startButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        public Level1(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            startButton = new StartButton(new Vector2(-64, -32), new Vector2(160, 64)) {
                DrawColor = Color.White,
            };
            startButton.ClickEventHandler += BtnEvent;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Name = "Click the Button!";
            Info = new TextBuilder("How hard can it be?", new Vector2(-128, -64), new Vector2(16, 16), null, 0);
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            Info.Draw(spriteBatch);
            startButton.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            startButton.Update(gameTime, mouseCursor.Hitbox[0]);
            Info.Update(gameTime);
        }
    }
}
