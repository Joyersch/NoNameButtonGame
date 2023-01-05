using System;
using System.Collections.Generic;
using System.Text;

using Raigy.Obj;
using Raigy.Input;
using Raigy.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.GameObjects;
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

            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024), Globals.Content.GetTHBox("zonenew"));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024), Globals.Content.GetTHBox("zonenew"));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024), Globals.Content.GetTHBox("zonenew"));
            
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew"));
            Block2 = new Laserwall(new Vector2(-32, 96), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew"));
            WallRight.Enter += CallFail;
            WallLeft.Enter += CallFail;
            WallButtom.Enter += CallFail;
            Block.Enter += CallFail;
            Block2.Enter += CallFail;
            button = new LockButton(new Vector2(-32, -128), new Vector2(64, 32), Globals.Content.GetTHBox("awesomebutton"), true);
            button.Click += CallFinish;
            UnLockbutton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32), Globals.Content.GetTHBox("emptybutton")) {
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
            Block2.Draw(sp);
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            cursor.Position = mousePosition - cursor.Size / 2;
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;

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
            UnLockbutton.Update(gt, cursor.Hitbox[0]);
            button.Update(gt, cursor.Hitbox[0]);
            WallLeft.Update(gt, cursor.Hitbox[0]);
            WallRight.Update(gt, cursor.Hitbox[0]);
            WallButtom.Update(gt, cursor.Hitbox[0]);
            Block.Update(gt, cursor.Hitbox[0]);
            Block2.Update(gt, cursor.Hitbox[0]);
        }
    }
}
