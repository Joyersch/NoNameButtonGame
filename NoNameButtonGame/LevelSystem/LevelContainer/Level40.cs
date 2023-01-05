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
    class Level40 : SampleLevel
    {

        readonly AwesomeButton button;
        readonly Cursor cursor;
        readonly Laserwall wallup;
        readonly Laserwall walldown;

        readonly TextBuilder GUN;
        readonly List<Tuple<Laserwall, Vector2>> shots;

        readonly float ShotTime = 500;
        readonly float TravelSpeed = 5;
        readonly float MaxUpdateSpeed = 32;
        readonly float MinUpdateSpeed = 32;
        readonly float Multiplier = 185;

        readonly List<int> removeItem = new List<int>();
        float UpdateSpeed = 2;
        Vector2 OldMPos;

        float EGT;
        float GT;
        float MGT;

        public Level40(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 40 - 10 Left";
            button = new AwesomeButton(new Vector2(-256, -0), new Vector2(128, 64), Globals.Content.GetTHBox("awesomebutton"));
            button.Click += CallFinish;
            cursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            wallup = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40), new Vector2(base.defaultWidth, defaultHeight - 40), Globals.Content.GetTHBox("zonenew"));
            walldown = new Laserwall(new Vector2(-(defaultWidth / Camera.Zoom), 40 + 40), new Vector2(base.defaultWidth, defaultHeight), Globals.Content.GetTHBox("zonenew"));
            wallup.Enter += CallFail;
            walldown.Enter += CallFail;
            GUN = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            shots = new List<Tuple<Laserwall, Vector2>>();
        }

        public override void Draw(SpriteBatch sp) {
            button.Draw(sp);
            wallup.Draw(sp);
            walldown.Draw(sp);
            GUN.Draw(sp);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(sp);
            }
            cursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            cursor.Update(gt);
            base.Update(gt);

            GUN.Update(gt);

            MGT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            while (MGT > UpdateSpeed) {
                MGT -= UpdateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * TravelSpeed);
                }
                GT += (float)gt.ElapsedGameTime.TotalMilliseconds;
                while (GT > ShotTime) {
                    GT -= ShotTime;
                    Vector2 Dir = button.rec.Center.ToVector2() - GUN.rec.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(GUN.Position, new Vector2(16, 8), Globals.Content.GetTHBox("zonenew")), Dir / Dir.Length()));
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
            if (cursor.rec.Intersects(wallup.rec) || cursor.rec.Intersects(walldown.rec))
                EGT += (float)gt.ElapsedGameTime.TotalMilliseconds / 11;
            else
                EGT += (float)gt.ElapsedGameTime.TotalMilliseconds;
            double angle = (EGT % 1000 / 1000F * Math.PI * 2);
            cursor.Position = new Vector2(Multiplier * (float)Math.Sin(angle), Multiplier * (float)Math.Cos(angle));
            button.Position = mousePosition - button.Size / 2;
            wallup.Update(gt, button.rec);
            walldown.Update(gt, button.rec);
            button.Update(gt, cursor.Hitbox[0]);

            OldMPos = mousePosition;
        }
    }
}
