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
    class Level6 : SampleLevel
    {

        readonly Cursor cursor;
        readonly Laserwall WallLeft;
        readonly Laserwall WallRight;
        readonly Laserwall WallButtom;

        readonly Laserwall Block;
        readonly LockButton button;
        readonly HoldButton UnLockbutton;
        float GT;
        bool MoveLeft = false;
        public Level6(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 6 - Now what?!";

            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024));
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64));
            WallRight.EnterEventHandler += CallFail;
            WallLeft.EnterEventHandler += CallFail;
            WallButtom.EnterEventHandler += CallFail;
            Block.EnterEventHandler += CallFail;
            button = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32),true);
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
           
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            cursor.Position = mousePosition - cursor.Size / 2;
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            while (GT > 8) {
                GT -= 8;
                if (MoveLeft) {
                    Block.Move(new Vector2(-2, 0));
                } else {
                    Block.Move(new Vector2(2, 0));
                }
                if (Block.Position.X > 180)
                    MoveLeft = true;
                if (Block.Position.X < -180)
                    MoveLeft = false;
            }
            UnLockbutton.Update(gameTime, cursor.Hitbox[0]);
            button.Update(gameTime, cursor.Hitbox[0]);
            WallLeft.Update(gameTime, cursor.Hitbox[0]);
            WallRight.Update(gameTime, cursor.Hitbox[0]);
            WallButtom.Update(gameTime, cursor.Hitbox[0]);
            Block.Update(gameTime, cursor.Hitbox[0]);
        }
    }
}
