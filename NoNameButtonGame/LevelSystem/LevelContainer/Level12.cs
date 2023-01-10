using System;
using System.Collections.Generic;
using System.Text;

using Joyersch.Obj;
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
    class Level12 : SampleLevel
    {
        readonly Cursor mouseCursor;
        readonly TextBuilder displayTimer;
        readonly TextBuilder displayGun;
        readonly List<Tuple<Laserwall, Vector2>> shots;
        float gameTimeUpdateShots;
        float gameTimeUpdateOverAll;
        readonly float shotTime = 200;
        readonly float travelSpeed = 2;
        float updateSpeed = 2;
        readonly float maxUpdateSpeed = 128;
        readonly float minUpdateSpeed = 4;
        Vector2 OldMPos;
        readonly List<int> removeItem = new List<int>();


        readonly float TimerMax = 30000;
        float TimerC = 0;

        public Level12(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 12 - Super GUN!";
            displayTimer = new TextBuilder("", new Vector2(0 - 128), new Vector2(16, 16), null, 0);

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            displayGun = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            shots = new List<Tuple<Laserwall, Vector2>>();
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }

        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }

        public override void Draw(SpriteBatch sp) {

            displayGun.Draw(sp);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(sp);
            }
            displayTimer.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            displayGun.Update(gt);

            gameTimeUpdateOverAll += (float)gt.ElapsedGameTime.TotalMilliseconds;
            TimerC += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while (gameTimeUpdateOverAll > updateSpeed) {
                gameTimeUpdateOverAll -= updateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * travelSpeed);
                }
                gameTimeUpdateShots += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (gameTimeUpdateShots > shotTime) {
                    gameTimeUpdateShots -= shotTime;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - displayGun.rec.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(displayGun.Position, new Vector2(16, 8), Globals.Content.GetHitboxMapping("zonenew")), Dir / Dir.Length()));
                    shots[^1].Item1.Enter += CallFail;
                }
            }
            removeItem.Clear();
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Update(gt, mouseCursor.Hitbox[0]);
                if (!shots[i].Item1.rec.Intersects(cameraRectangle)) {
                    removeItem.Add(i);
                }
            }
            for (int i = 0; i < removeItem.Count; i++) {
                try {
                    shots.RemoveAt(removeItem[i]);
                } catch { }
            }
            if (mousePosition != OldMPos) {
                updateSpeed -= Vector2.Distance(mousePosition, OldMPos) * 10;
                if (updateSpeed < minUpdateSpeed)
                    updateSpeed = minUpdateSpeed;
            } else {
                updateSpeed = maxUpdateSpeed;
            }
            if (TimerC >= TimerMax)
                CallFinish(this, new EventArgs());
            displayTimer.ChangeText(((TimerMax - TimerC) / 1000).ToString("0.0") + "S");
            displayTimer.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            OldMPos = mousePosition;
        }
    }
}
