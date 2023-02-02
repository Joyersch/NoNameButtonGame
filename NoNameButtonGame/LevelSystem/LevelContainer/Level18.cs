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
    class Level18 : SampleLevel
    {

        readonly Cursor mouseCursor;
        readonly Laserwall WallLeft;
        readonly Laserwall WallRight;
        readonly Laserwall WallButtom;

        readonly Laserwall Block;
        readonly LockButton lockedButton;
        readonly HoldButton unlockButton;
        float gameTimeMoveBlock;
        bool MoveLeft = false;

        public Level18(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 18 - oh no";

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024));
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64));
            WallRight.EnterEventHandler += Fail;
            WallLeft.EnterEventHandler += Fail;
            WallButtom.EnterEventHandler += Fail;
            Block.EnterEventHandler += Fail;
            lockedButton = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32), true);
            lockedButton.ClickEventHandler += Finish;
            unlockButton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32)) {
                EndHoldTime = 10000
            };
            unlockButton.ClickEventHandler += UnlockBtn;
        }

        private void UnlockBtn(object sender) {
            lockedButton.Locked = false;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            lockedButton.Draw(spriteBatch);
            unlockButton.Draw(spriteBatch);
            Block.Draw(spriteBatch);
            WallLeft.Draw(spriteBatch);
            WallRight.Draw(spriteBatch);
            WallButtom.Draw(spriteBatch);

            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            gameTimeMoveBlock += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (gameTimeMoveBlock > 8) {
                gameTimeMoveBlock -= 8;
                if (MoveLeft) {
                    Block.Move(new Vector2(-3, 0));
                } else {
                    Block.Move(new Vector2(3, 0));
                }
                if (Block.Position.X > 180)
                    MoveLeft = true;
                if (Block.Position.X < -180)
                    MoveLeft = false;
            }
            unlockButton.Update(gameTime, mouseCursor.Hitbox[0]);
            lockedButton.Update(gameTime, mouseCursor.Hitbox[0]);
            WallLeft.Update(gameTime, mouseCursor.Hitbox[0]);
            WallRight.Update(gameTime, mouseCursor.Hitbox[0]);
            WallButtom.Update(gameTime, mouseCursor.Hitbox[0]);
            Block.Update(gameTime, mouseCursor.Hitbox[0]);
        }
    }
}
