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
    class Level21 : SampleLevel
    {

        readonly Cursor cursor;
        readonly Laserwall WallLeft;
        readonly Laserwall WallRight;
        readonly Laserwall WallButtom;
        readonly Laserwall Block;
        readonly Laserwall Block2;
        readonly LockButton button;
        readonly HoldButton UnLockbutton;
        float GT;
        bool MoveLeft = false;
        bool MoveUp = false;
        public Level21(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 21 - this again? cmon!";

            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024));
            
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64));
            Block2 = new Laserwall(new Vector2(-32, 96), new Vector2(64, 64));
            WallRight.EnterEventHandler += CallFail;
            WallLeft.EnterEventHandler += CallFail;
            WallButtom.EnterEventHandler += CallFail;
            Block.EnterEventHandler += CallFail;
            Block2.EnterEventHandler += CallFail;
            button = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32), true);
            button.ClickEventHandler += CallFinish;
            UnLockbutton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32)) {
                EndHoldTime = 5000
            };
            UnLockbutton.ClickEventHandler += UnlockBtn;
        }

        private void UnlockBtn(object sender, EventArgs e) {
            button.Locked = false;
        }

        private void WallEvent(object sender, EventArgs e) {
            CallExit(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            UnLockbutton.Draw(spriteBatch);
            Block.Draw(spriteBatch);
            WallLeft.Draw(spriteBatch);
            WallRight.Draw(spriteBatch);
            WallButtom.Draw(spriteBatch);
            Block2.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            cursor.Position = mousePosition - cursor.Size / 2;
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (GT > 20) {
                GT -= 20;
                if (MoveLeft) {
                    Block.Move(new Vector2(-4, 0));
                } else {
                    Block.Move(new Vector2(4, 0));
                }
                if (Block.Position.X > 180)
                    MoveLeft = true;
                if (Block.Position.X < -180)
                    MoveLeft = false;

                if (MoveUp) {
                    Block2.Move(new Vector2(0, -1));
                } else {
                    Block2.Move(new Vector2(0, 1));
                }
                if (Block2.Position.Y > 96)
                    MoveUp = true;
                if (Block2.Position.Y < 50)
                    MoveUp = false;
            }
            UnLockbutton.Update(gameTime, cursor.Hitbox[0]);
            button.Update(gameTime, cursor.Hitbox[0]);
            WallLeft.Update(gameTime, cursor.Hitbox[0]);
            WallRight.Update(gameTime, cursor.Hitbox[0]);
            WallButtom.Update(gameTime, cursor.Hitbox[0]);
            Block.Update(gameTime, cursor.Hitbox[0]);
            Block2.Update(gameTime, cursor.Hitbox[0]);
        }
    }
}
