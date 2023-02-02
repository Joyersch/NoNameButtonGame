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

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            displayGun = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            shots = new List<Tuple<Laserwall, Vector2>>();
        }

        public override void Draw(SpriteBatch spriteBatch) {

            displayGun.Draw(spriteBatch);
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Draw(spriteBatch);
            }
            displayTimer.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            displayGun.Update(gameTime);

            gameTimeUpdateOverAll += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            TimerC += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (gameTimeUpdateOverAll > updateSpeed) {
                gameTimeUpdateOverAll -= updateSpeed;
                for (int i = 0; i < shots.Count; i++) {
                    shots[i].Item1.Move(shots[i].Item2 * travelSpeed);
                }
                gameTimeUpdateShots += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (gameTimeUpdateShots > shotTime) {
                    gameTimeUpdateShots -= shotTime;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - displayGun.rectangle.Center.ToVector2();
                    shots.Add(new Tuple<Laserwall, Vector2>(new Laserwall(displayGun.Position, new Vector2(16, 8)), Dir / Dir.Length()));
                    shots[^1].Item1.EnterEventHandler += Fail;
                }
            }
            removeItem.Clear();
            for (int i = 0; i < shots.Count; i++) {
                shots[i].Item1.Update(gameTime, mouseCursor.Hitbox[0]);
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
                updateSpeed -= Vector2.Distance(mousePosition, OldMPos) * 10;
                if (updateSpeed < minUpdateSpeed)
                    updateSpeed = minUpdateSpeed;
            } else {
                updateSpeed = maxUpdateSpeed;
            }
            if (TimerC >= TimerMax)
                Finish();
            displayTimer.ChangeText(((TimerMax - TimerC) / 1000).ToString("0.0") + "S");
            displayTimer.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            OldMPos = mousePosition;
        }
    }
}
