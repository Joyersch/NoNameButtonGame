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
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level37 : SampleLevel
    {

        readonly StateButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;

        readonly TextBuilder GUN;
        readonly List<Tuple<Laserwall, Vector2>> shots;
        float GT;
        float MGT;
        readonly float ShotTime = 333;
        readonly float TravelSpeed = 7;
        float UpdateSpeed = 2;
        readonly float MaxUpdateSpeed = 64;
        readonly float MinUpdateSpeed = 8;
        Vector2 OldMPos;
        readonly List<int> removeItem = new List<int>();

        public Level37(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            button = new StateButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"), 333) {
                DrawColor = Color.White,
            };
            button.Click += BtnEvent;
            Name = "Level 37 - roots with a gun";
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Info = new TextBuilder("THiS AGAIN!", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
            GUN = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            shots = new List<Tuple<Laserwall, Vector2>>();

        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            Info.Draw(spriteBatch);
            button.Draw(spriteBatch);
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
                    Vector2 Dir = cursor.Hitbox[0].Center.ToVector2() - GUN.rectangle.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(GUN.Position, new Vector2(16, 8), Globals.Content.GetHitboxMapping("zonenew")), Dir / Dir.Length()));
                    shots[^1].Item1.Enter += CallFail;
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


            Info.ChangePosition(-Info.rectangle.Size.ToVector2() / 2 + new Vector2(0, -64));
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            Info.Update(gameTime);
            OldMPos = mousePosition;
        }
    }
}
