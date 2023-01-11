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
    class Level49 : SampleLevel
    {

        readonly Cursor mouseCursor;
        readonly Laserwall leftWall;
        readonly Laserwall rightWall;
        readonly Laserwall buttomWall;
        readonly Laserwall firstBlock;
        readonly Laserwall secondBlock;
        readonly LockButton winButton;
        readonly HoldButton UnlockButton;
        float GT;
        bool leftMoveLeft = false;
        bool upMove = false;
        public Level49(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 49 - so close, you can do it!";

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            leftWall = new Laserwall(new Vector2(-512, -512), new Vector2(420, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            rightWall = new Laserwall(new Vector2(96, -512), new Vector2(420, 1024), Globals.Content.GetHitboxMapping("zonenew"));
            buttomWall = new Laserwall(new Vector2(-512, 96), new Vector2(1024, 1024), Globals.Content.GetHitboxMapping("zonenew"));

            firstBlock = new Laserwall(new Vector2(-256, 32), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew"));
            secondBlock = new Laserwall(new Vector2(-32, 96), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew"));
            rightWall.Enter += CallFail;
            leftWall.Enter += CallFail;
            buttomWall.Enter += CallFail;
            firstBlock.Enter += CallFail;
            secondBlock.Enter += CallFail;
            winButton = new LockButton(new Vector2(-32, -128), new Vector2(64, 32), Globals.Content.GetHitboxMapping("awesomebutton"), true);
            winButton.Click += CallFinish;
            UnlockButton = new HoldButton(new Vector2(-32, 48), new Vector2(64, 32), Globals.Content.GetHitboxMapping("emptybutton")) {
                EndHoldTime = 2500
            };
            UnlockButton.Click += UnlockBtn;
        }

        private void UnlockBtn(object sender, EventArgs e) {
            winButton.Locked = false;
        }

        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch sp) {
            winButton.Draw(sp);
            UnlockButton.Draw(sp);
            firstBlock.Draw(sp);
            leftWall.Draw(sp);
            rightWall.Draw(sp);
            buttomWall.Draw(sp);
            secondBlock.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            GT += (float)gt.ElapsedGameTime.TotalMilliseconds;

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
            UnlockButton.Update(gt, mouseCursor.Hitbox[0]);
            winButton.Update(gt, mouseCursor.Hitbox[0]);
            leftWall.Update(gt, mouseCursor.Hitbox[0]);
            rightWall.Update(gt, mouseCursor.Hitbox[0]);
            buttomWall.Update(gt, mouseCursor.Hitbox[0]);
            firstBlock.Update(gt, mouseCursor.Hitbox[0]);
            secondBlock.Update(gt, mouseCursor.Hitbox[0]);
        }
    }
}
