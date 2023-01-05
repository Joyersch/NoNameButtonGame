﻿using System;
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
    class Level19 : SampleLevel
    {

        readonly HoldButton holdButton;
        readonly Cursor mouseCursor;
        readonly LockButton lockButton;
        readonly TextBuilder Gun;
        readonly List<Tuple<Laserwall, Vector2>> shots;
        readonly List<int> removeItem = new List<int>();

        float gameTimeShotTime;
        float gameTimeUpdate;
        readonly float ShotTime = 350;
        readonly float TravelSpeed = 5;
        readonly float MaxUpdateSpeed = 10;
        readonly float MinUpdateSpeed = 10;

        float UpdateSpeed = 2;
        Vector2 OldMPos;
        
        public Level19(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 19 - Hold Click Repeat!";
            Gun = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            holdButton = new HoldButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("emptybutton")) {
                EndHoldTime = 25000
            };
            holdButton.Click += EmptyBtnEvent;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            
            lockButton = new LockButton(new Vector2(-192, -32), new Vector2(128, 64), Globals.Content.GetTHBox("awesomebutton"), true);
            lockButton.Click += BtnEvent;
            shots = new List<Tuple<Laserwall, Vector2>>();
            OldMPos = new Vector2(0, 0);
        }


        private void EmptyBtnEvent(object sender, EventArgs e) {
            lockButton.Locked = !lockButton.Locked;
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }

        private void WallEvent(object sender, EventArgs e) {
            CallReset(sender, e);
        }

        public override void Draw(SpriteBatch sp) {
            holdButton.Draw(sp);
            
            lockButton.Draw(sp);
            mouseCursor.Draw(sp);
            Gun.Draw(sp);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(sp);
            }
        }

        public override void Update(GameTime gt) {
            
            mouseCursor.Update(gt);
            base.Update(gt);
            Gun.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            holdButton.Update(gt, mouseCursor.Hitbox[0]);
            lockButton.Update(gt, mouseCursor.Hitbox[0]);
            gameTimeUpdate += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while (gameTimeUpdate > UpdateSpeed) {
                gameTimeUpdate -= UpdateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * TravelSpeed);
                }
                gameTimeShotTime += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (gameTimeShotTime > ShotTime) {
                    gameTimeShotTime -= ShotTime;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - Gun.rec.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(Gun.Position, new Vector2(16, 8), Globals.Content.GetTHBox("zonenew")), Dir / Dir.Length()));
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
                UpdateSpeed -= Vector2.Distance(mousePosition, OldMPos) * 10;
                if (UpdateSpeed < MinUpdateSpeed)
                    UpdateSpeed = MinUpdateSpeed;
            } else {
                UpdateSpeed = MaxUpdateSpeed;
            }
        }
    }
}
