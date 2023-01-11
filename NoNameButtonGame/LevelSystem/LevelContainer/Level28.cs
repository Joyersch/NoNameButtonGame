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
    class Level28 : SampleLevel
    {


        readonly Cursor cursor;
        readonly TextBuilder GUN;
        readonly TextBuilder Timer;
        readonly List<Tuple<Laserwall, Vector2>> shots;
        float GT;
        float MGT;
        readonly float ShotTime = 420;
        readonly float TravelSpeed = 4;
        float UpdateSpeed = 2;
        readonly float MaxUpdateSpeed = 56;
        readonly float MinUpdateSpeed = 4;
        Vector2 OldMPos;
        readonly List<int> removeItem = new List<int>();


        readonly float TimerMax = 42000;
        float TimerC = 0;
        public Level28(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 28 - A HOT!";
            Timer = new TextBuilder("", new Vector2(0 - 128), new Vector2(16, 16), null, 0);
            GUN = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            shots = new List<Tuple<Laserwall, Vector2>>();
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }
        public override void Draw(SpriteBatch sp) {

            GUN.Draw(sp);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(sp);
            }
            Timer.Draw(sp);
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);
            GUN.Update(gt);

            MGT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            TimerC += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while (MGT > UpdateSpeed) {
                MGT -= UpdateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * TravelSpeed);
                }
                GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (GT > ShotTime) {
                    GT -= ShotTime;
                    Vector2 Dir = cursor.Hitbox[0].Center.ToVector2() - GUN.rec.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(GUN.Position, new Vector2(16, 8), Globals.Content.GetHitboxMapping("zonenew")), Dir / Dir.Length()));
                    shots[^1].Item1.Enter += CallFail;
                }
            }
            removeItem.Clear();
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Update(gt, cursor.Hitbox[0]);
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
                UpdateSpeed -= Vector2.Distance(mousePosition, OldMPos) * 10;
                if (UpdateSpeed < MinUpdateSpeed)
                    UpdateSpeed = MinUpdateSpeed;
            } else {
                UpdateSpeed = MaxUpdateSpeed;
            }
            if (TimerC >= TimerMax)
                CallFinish(this, new EventArgs());
            Timer.ChangeText(((TimerMax - TimerC) / 1000).ToString("0.0") + "S");
            Timer.Update(gt);
            cursor.Position = mousePosition - cursor.Size / 2;
            OldMPos = mousePosition;
        }
    }
}
