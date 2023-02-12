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
    internal class Level21 : SampleLevel
    {
        private readonly Cursor cursor;
        private readonly GlitchBlockCollection WallLeft;
        private readonly GlitchBlockCollection WallRight;
        private readonly GlitchBlockCollection WallButtom;
        private readonly GlitchBlockCollection Block;
        private readonly GlitchBlockCollection Block2;
        private readonly LockButton button;
        private readonly HoldButton UnLockbutton;
        private float GT;
        private bool MoveLeft = false;
        private bool MoveUp = false;
        public Level21(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 21 - this again? cmon!";

            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            WallLeft = new GlitchBlockCollection(new Vector2(-512, -512), new Vector2(420, 1024));
            WallRight = new GlitchBlockCollection(new Vector2(96, -512), new Vector2(420, 1024));
            WallButtom = new GlitchBlockCollection(new Vector2(-512, 96), new Vector2(1024, 1024));
            
            Block = new GlitchBlockCollection(new Vector2(-256, 32), new Vector2(64, 64));
            Block2 = new GlitchBlockCollection(new Vector2(-32, 96), new Vector2(64, 64));
            WallRight.EnterEventHandler += Fail;
            WallLeft.EnterEventHandler += Fail;
            WallButtom.EnterEventHandler += Fail;
            Block.EnterEventHandler += Fail;
            Block2.EnterEventHandler += Fail;
            button = new LockWinButton(new Vector2(-32, -128), new Vector2(64, 32), true);
            button.ClickEventHandler += Finish;
            UnLockbutton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32)) {
                EndHoldTime = 5000
            };
            UnLockbutton.ClickEventHandler += UnlockBtn;
        }

        private void UnlockBtn(object sender) {
            button.Unlock();
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
            cursor.Position = mousePosition - cursor.Canvas / 2;
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
