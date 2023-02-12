using System;
using System.Collections.Generic;
using System.Text;

using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level49 : SampleLevel
    {
        private readonly Cursor mouseCursor;
        private readonly GlitchBlockCollection leftWall;
        private readonly GlitchBlockCollection rightWall;
        private readonly GlitchBlockCollection buttomWall;
        private readonly GlitchBlockCollection firstBlock;
        private readonly GlitchBlockCollection secondBlock;
        private readonly LockButton winButton;
        private readonly HoldButton UnlockButton;
        private float GT;
        private bool leftMoveLeft = false;
        private bool upMove = false;
        public Level49(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 49 - so close, you can do it!";

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            leftWall = new GlitchBlockCollection(new Vector2(-512, -512), new Vector2(420, 1024));
            rightWall = new GlitchBlockCollection(new Vector2(96, -512), new Vector2(420, 1024));
            buttomWall = new GlitchBlockCollection(new Vector2(-512, 96), new Vector2(1024, 1024));

            firstBlock = new GlitchBlockCollection(new Vector2(-256, 32), new Vector2(64, 64));
            secondBlock = new GlitchBlockCollection(new Vector2(-32, 96), new Vector2(64, 64));
            rightWall.EnterEventHandler += Fail;
            leftWall.EnterEventHandler += Fail;
            buttomWall.EnterEventHandler += Fail;
            firstBlock.EnterEventHandler += Fail;
            secondBlock.EnterEventHandler += Fail;
            winButton = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32), true);
            winButton.ClickEventHandler += Finish;
            UnlockButton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32)) {
                EndHoldTime = 2500
            };
            UnlockButton.ClickEventHandler += UnlockBtn;
        }

        private void UnlockBtn(object sender) {
            winButton.Unlock();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            winButton.Draw(spriteBatch);
            UnlockButton.Draw(spriteBatch);
            firstBlock.Draw(spriteBatch);
            leftWall.Draw(spriteBatch);
            rightWall.Draw(spriteBatch);
            buttomWall.Draw(spriteBatch);
            secondBlock.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Canvas / 2;
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (GT > 20) {
                GT -= 20;
                if (leftMoveLeft) {
                    firstBlock.Move(new Vector2(-5, 0));
                } else {
                    firstBlock.Move(new Vector2(5, 0));
                }
                if (firstBlock.Position.X > 180-64)
                    leftMoveLeft = true;
                if (firstBlock.Position.X < -180)
                    leftMoveLeft = false;

                if (upMove) {
                    secondBlock.Move(new Vector2(0, -4));
                } else {
                    secondBlock.Move(new Vector2(0, 4));
                }
                if (secondBlock.Position.Y > 96)
                    upMove = true;
                if (secondBlock.Position.Y < 58)
                    upMove = false;
            }
            UnlockButton.Update(gameTime, mouseCursor.Hitbox[0]);
            winButton.Update(gameTime, mouseCursor.Hitbox[0]);
            leftWall.Update(gameTime, mouseCursor.Hitbox[0]);
            rightWall.Update(gameTime, mouseCursor.Hitbox[0]);
            buttomWall.Update(gameTime, mouseCursor.Hitbox[0]);
            firstBlock.Update(gameTime, mouseCursor.Hitbox[0]);
            secondBlock.Update(gameTime, mouseCursor.Hitbox[0]);
        }
    }
}
