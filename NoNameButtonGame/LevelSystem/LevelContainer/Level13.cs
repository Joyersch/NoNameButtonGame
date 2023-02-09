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
    internal class Level13 : SampleLevel
    {
        private readonly EmptyButton userButton;
        private readonly Cursor movingCursor;
        private readonly GlitchBlockCollection wallUp;
        private readonly GlitchBlockCollection wallDown;
        private readonly float Multiplier = 100;
        public Level13(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 13 - Swap time.";
            userButton = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
            userButton.ClickEventHandler += Finish;
            movingCursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10));
            wallUp = new GlitchBlockCollection(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40),new Vector2(base.defaultWidth, defaultHeight));
            wallDown = new GlitchBlockCollection(new Vector2(-(defaultWidth / Camera.Zoom), 40), new Vector2(base.defaultWidth, defaultHeight));
            wallUp.EnterEventHandler += Fail;
            wallDown.EnterEventHandler += Fail;
        }
        
        public override void Draw(SpriteBatch spriteBatch) {
            userButton.Draw(spriteBatch);
            wallUp.Draw(spriteBatch);
            wallDown.Draw(spriteBatch);
            movingCursor.Draw(spriteBatch);
        }

        
        public override void Update(GameTime gameTime) {
            movingCursor.Update(gameTime);
            base.Update(gameTime);
            double angle = gameTime.TotalGameTime.Milliseconds / 1000F * Math.PI * 2;
            movingCursor.Position = new Vector2(Multiplier * (float)Math.Sin(angle), Multiplier * (float)Math.Cos(angle));
            userButton.Position = mousePosition - userButton.Size / 2;
            wallUp.Update(gameTime, userButton.Rectangle);
            wallDown.Update(gameTime, userButton.Rectangle);
            userButton.Update(gameTime, movingCursor.Hitbox[0]);

        }
    }
}
