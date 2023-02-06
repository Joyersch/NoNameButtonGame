﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level24 : SampleLevel
    {
        private readonly EmptyButton button;
        private readonly Cursor cursor;
        private readonly TextBuilder[] Infos;
        private readonly GlitchBlockCollection[] WallLeft;
        private readonly GlitchBlockCollection[] WallRight;
        private readonly GlitchBlockCollection[] Blocks;
        private readonly TextBuilder GUN;
        private readonly int WallLength = 15;
        private readonly List<Tuple<GlitchBlockCollection, Vector2>> shots;
        private float GT;

        private float GT2;
        private float MGT;
        private readonly float ShotTime = 700;
        private readonly float TravelSpeed = 5;
        private float UpdateSpeed = 2;
        private readonly float MaxUpdateSpeed = 80;
        private readonly float MinUpdateSpeed = 80;
        private Vector2 OldMPos;
        private readonly List<int> removeItem = new List<int>();
        public Level24(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 24 - now with a gun";
            button = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
            button.ClickEventHandler += Finish;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Infos = new TextBuilder[2];
            Infos[0] = new TextBuilder("wow you did it!", new Vector2(120, -132), new Vector2(8, 8), null, 0);
            Infos[1] = new TextBuilder("gg!", new Vector2(120, -100), new Vector2(8, 8), null, 0);
            WallLeft = new GlitchBlockCollection[WallLength];
            WallRight = new GlitchBlockCollection[WallLength];
            List<GlitchBlockCollection> Walls = new List<GlitchBlockCollection>();
            for (int i = 0; i < WallLength; i++) {
                WallLeft[i] = new GlitchBlockCollection(new Vector2(96, 512 * i), new Vector2(416, 512));
                WallRight[i] = new GlitchBlockCollection(new Vector2(-512, 512 * i), new Vector2(416, 512));
                WallRight[i].EnterEventHandler += Fail;
                WallLeft[i].EnterEventHandler += Fail;
            }
            for (int i = 0; i < (WallLength - 1) * 8 / 2; i++) {
                int c = rand.Next(0, 3);
                if (c != 0)
                    Walls.Add(new GlitchBlockCollection(new Vector2(-96, 512 + 128 * i), new Vector2(64, 64)));
                if (c != 1)
                    Walls.Add(new GlitchBlockCollection(new Vector2(-32, 512 + 128 * i), new Vector2(64, 64)));
                if (c != 2)
                    Walls.Add(new GlitchBlockCollection(new Vector2(32, 512 + 128 * i), new Vector2(64, 64)));

                // Walls.Add(new Laserwall(new Vector2(-96 + rand.Next(0, 3) * 64, 512 + 128 * i), new Vector2(64, 64), Globals.Content.GetTHBox("zonenew")));

            }
            Blocks = Walls.ToArray();
            for (int i = 0; i < Blocks.Length; i++) {
                Blocks[i].EnterEventHandler += Fail;
            }
            shots = new List<Tuple<GlitchBlockCollection, Vector2>>();
            GUN = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
        }



        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Draw(spriteBatch);
            }
            for (int i = 0; i < WallLength; i++) {
                if (WallLeft[i].rectangle.Intersects(cameraRectangle))
                    WallLeft[i].Draw(spriteBatch);
                if (WallRight[i].rectangle.Intersects(cameraRectangle))
                    WallRight[i].Draw(spriteBatch);

            }
            for (int i = 0; i < Blocks.Length; i++) {
                if (Blocks[i].rectangle.Intersects(cameraRectangle))
                    Blocks[i].Draw(spriteBatch);
            }
            GUN.Draw(spriteBatch);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(spriteBatch);
            }
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            GUN.Update(gameTime);
            for (int i = 0; i < Infos.Length; i++) {
                Infos[i].Update(gameTime);
            }
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (GT > 8) {
                GT -= 8;
                for (int i = 0; i < WallLength; i++) {
                    WallLeft[i].Move(new Vector2(0, -2));
                    WallRight[i].Move(new Vector2(0, -2));
                }
                for (int i = 0; i < Blocks.Length; i++) {
                    Blocks[i].Move(new Vector2(0, -2));
                }
            }

            MGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (MGT > UpdateSpeed) {
                MGT -= UpdateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * TravelSpeed);
                }
                GT2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT2 > ShotTime) {
                    GT2 -= ShotTime;
                    Vector2 Dir = cursor.Hitbox[0].Center.ToVector2() - GUN.rectangle.Center.ToVector2();
                    var ls = new GlitchBlockCollection(GUN.Position, new Vector2(16, 8)) {
                        DrawColor = Color.Green
                    };
                    shots.Add(new Tuple<GlitchBlockCollection, Vector2>(ls, Dir / Dir.Length()));
                    shots[^1].Item1.EnterEventHandler += Fail;
                }
            }
            removeItem.Clear();
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Update(gameTime, cursor.Hitbox[0]);
                if (!shots[i].Item1.rectangle.Intersects(cameraRectangle)) {
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
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            for (int i = 0; i < WallLength; i++) {
                WallLeft[i].Update(gameTime, cursor.Hitbox[0]);
                WallRight[i].Update(gameTime, cursor.Hitbox[0]);
            }
            for (int i = 0; i < Blocks.Length; i++) {
                Blocks[i].Update(gameTime, cursor.Hitbox[0]);
            }
            OldMPos = mousePosition;
        }
    }
}
