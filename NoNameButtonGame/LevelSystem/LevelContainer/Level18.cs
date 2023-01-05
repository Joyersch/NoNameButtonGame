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

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            WallLeft = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024), Globals.Content.GetTHBox("zonenew"));
            WallRight = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024), Globals.Content.GetTHBox("zonenew"));
            WallButtom = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024), Globals.Content.GetTHBox("zonenew"));
            Block = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew"));
            WallRight.Enter += CallFail;
            WallLeft.Enter += CallFail;
            WallButtom.Enter += CallFail;
            Block.Enter += CallFail;
            lockedButton = new LockButton(new Vector2(-32, -128), new Vector2(64, 32), Globals.Content.GetTHBox("awesomebutton"), true);
            lockedButton.Click += CallFinish;
            unlockButton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32), Globals.Content.GetTHBox("emptybutton")) {
                EndHoldTime = 10000
            };
            unlockButton.Click += UnlockBtn;
        }

        private void UnlockBtn(object sender, EventArgs e) {
            lockedButton.Locked = false;
        }

        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            lockedButton.Draw(sp);
            unlockButton.Draw(sp);
            Block.Draw(sp);
            WallLeft.Draw(sp);
            WallRight.Draw(sp);
            WallButtom.Draw(sp);

            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            gameTimeMoveBlock += (float)gt.ElapsedGameTime.TotalMilliseconds;

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
            unlockButton.Update(gt, mouseCursor.Hitbox[0]);
            lockedButton.Update(gt, mouseCursor.Hitbox[0]);
            WallLeft.Update(gt, mouseCursor.Hitbox[0]);
            WallRight.Update(gt, mouseCursor.Hitbox[0]);
            WallButtom.Update(gt, mouseCursor.Hitbox[0]);
            Block.Update(gt, mouseCursor.Hitbox[0]);
        }
    }
}
