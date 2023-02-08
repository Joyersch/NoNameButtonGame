﻿using System;
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
    internal class Level15 : SampleLevel
    {
        private readonly StateButton stateButton;
        private readonly Cursor mouseCursor;
        private readonly TextBuilder infoText;
        public Level15(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            stateButton = new StateButton(new Vector2(-64, -32), new Vector2(128, 64),100) {
                DrawColor = Color.White,
            };
            stateButton.ClickEventHandler += Finish;
            Name = "Level 15 - Back to the roots!";
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            infoText = new TextBuilder("THiS AGAIN!", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            infoText.Draw(spriteBatch);
            stateButton.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            infoText.ChangePosition(- infoText.Rectangle.Size.ToVector2() / 2 + new Vector2(0,-64));
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            stateButton.Update(gameTime, mouseCursor.Hitbox[0]);
            infoText.Update(gameTime);
        }
    }
}
