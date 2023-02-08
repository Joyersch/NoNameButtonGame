using System;
using System.Collections.Generic;
using System.Text;

using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level40 : SampleLevel
    {
        private readonly EmptyButton button;
        private readonly Cursor cursor;
        private readonly GlitchBlockCollection wallup;
        private readonly GlitchBlockCollection walldown;

        private readonly TextBuilder GUN;
        private readonly List<Tuple<GlitchBlockCollection, Vector2>> shots;

        private readonly float ShotTime = 500;
        private readonly float TravelSpeed = 5;
        private readonly float MaxUpdateSpeed = 32;
        private readonly float MinUpdateSpeed = 32;
        private readonly float Multiplier = 185;

        private readonly List<int> removeItem = new List<int>();
        private float UpdateSpeed = 2;
        private Vector2 OldMPos;

        private float EGT;
        private float GT;
        private float MGT;

        public Level40(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 40 - 10 Left";
            button = new WinButton(new Vector2(-256, -0), new Vector2(128, 64));
            button.ClickEventHandler += Finish;
            cursor = new Cursor(new Vector2(0, 32), new Vector2(7, 10));
            wallup = new GlitchBlockCollection(new Vector2(-(defaultWidth / Camera.Zoom), -defaultHeight - 40), new Vector2(base.defaultWidth, defaultHeight - 40));
            walldown = new GlitchBlockCollection(new Vector2(-(defaultWidth / Camera.Zoom), 40 + 40), new Vector2(base.defaultWidth, defaultHeight));
            wallup.EnterEventHandler += Fail;
            walldown.EnterEventHandler += Fail;
            GUN = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            shots = new List<Tuple<GlitchBlockCollection, Vector2>>();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            wallup.Draw(spriteBatch);
            walldown.Draw(spriteBatch);
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

            MGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (MGT > UpdateSpeed) {
                MGT -= UpdateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * TravelSpeed);
                }
                GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > ShotTime) {
                    GT -= ShotTime;
                    Vector2 Dir = button.rectangle.Center.ToVector2() - GUN.Rectangle.Center.ToVector2();
                    shots.Add(new Tuple<GlitchBlockCollection, Vector2>(new GlitchBlockCollection(GUN.Position, new Vector2(16, 8)), Dir / Dir.Length()));
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
            if (cursor.rectangle.Intersects(wallup.rectangle) || cursor.rectangle.Intersects(walldown.rectangle))
                EGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 11;
            else
                EGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            double angle = (EGT % 1000 / 1000F * Math.PI * 2);
            cursor.Position = new Vector2(Multiplier * (float)Math.Sin(angle), Multiplier * (float)Math.Cos(angle));
            button.Position = mousePosition - button.Size / 2;
            wallup.Update(gameTime, button.rectangle);
            walldown.Update(gameTime, button.rectangle);
            button.Update(gameTime, cursor.Hitbox[0]);

            OldMPos = mousePosition;
        }
    }
}
