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

            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew"));
            WallRight.Enter += CallFail;
            WallLeft.Enter += CallFail;
            WallButtom.Enter += CallFail;
            Block.Enter += CallFail;
            button = new LockButton(new Vector2(-32, -128), new Vector2(64, 32), Globals.Content.GetHitboxMapping("awesomebutton"),true);
            button.Click += CallFinish;
            UnLockbutton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32), Globals.Content.GetHitboxMapping("emptybutton")) {
                EndHoldTime = 5000
            };
            UnLockbutton.Click += UnlockBtn;
        }

        private void UnlockBtn(object sender, EventArgs e) {
            button.Locked = false;
        }

        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            button.Draw(sp);
            UnLockbutton.Draw(sp);
            Block.Draw(sp);
            WallLeft.Draw(sp);
            WallRight.Draw(sp);
            WallButtom.Draw(sp);
           
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            cursor.Position = mousePosition - cursor.Size / 2;
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            
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
            UnLockbutton.Update(gt, cursor.Hitbox[0]);
            button.Update(gt, cursor.Hitbox[0]);
            WallLeft.Update(gt, cursor.Hitbox[0]);
            WallRight.Update(gt, cursor.Hitbox[0]);
            WallButtom.Update(gt, cursor.Hitbox[0]);
            Block.Update(gt, cursor.Hitbox[0]);
        }
    }
}
