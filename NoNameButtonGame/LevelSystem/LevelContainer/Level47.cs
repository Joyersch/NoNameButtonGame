using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level47 : SampleLevel
    {
        private readonly Cursor mouseCursor;
        private readonly TextBuilder Timer;
        private readonly TextBuilder Gun;
        private readonly List<Tuple<GlitchBlockCollection, Vector2>> Shots;
        private readonly float shotTime = 100;
        private readonly float travelSpeed = 10;
        private readonly float maxUpdateSpeed = 64;
        private readonly float minUpdateSpeed = 4;
        private float updateSpeed = 2;
        private Vector2 oldMousePosition;
        private float GT;
        private float MGT;
        private readonly List<int> removeItem = new List<int>();


        private readonly float timerMax = 30000;
        private float timerCurrent = 0;
        public Level47(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 47 - MEGA GUN!";
            Timer = new TextBuilder("", new Vector2(0 - 128), new Vector2(16, 16), null, 0);

            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Gun = new TextBuilder("AGUN", new Vector2(-256, 0), new Vector2(16, 16), null, 0);
            Shots = new List<Tuple<GlitchBlockCollection, Vector2>>();
        }
        public override void Draw(SpriteBatch spriteBatch) {

            Gun.Draw(spriteBatch);
            for (int i = 0; i < Shots.Count; i++) {
                Shots[i].Item1.Draw(spriteBatch);
            }
            Timer.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }
        
        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            Gun.Update(gameTime);

            MGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timerCurrent += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (MGT > updateSpeed) {
                MGT -= updateSpeed;
                for (int i = 0; i < Shots.Count; i++) {
                    Shots[i].Item1.Move(Shots[i].Item2 * travelSpeed);
                }
                GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > shotTime) {
                    GT -= shotTime;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - Gun.Rectangle.Center.ToVector2();
                    Shots.Add(new Tuple<GlitchBlockCollection, Vector2>(new GlitchBlockCollection(Gun.Position, new Vector2(16, 8)), Dir / Dir.Length()));
                    Shots[^1].Item1.EnterEventHandler += Fail;
                }
            }
            removeItem.Clear();
            for (int i = 0; i < Shots.Count; i++) {
                Shots[i].Item1.Update(gameTime, mouseCursor.Hitbox[0]);
                if (!Shots[i].Item1.rectangle.Intersects(cameraRectangle)) {
                    removeItem.Add(i);
                }
            }
            for (int i = 0; i < removeItem.Count; i++) {
                try {
                    Shots.RemoveAt(removeItem[i]);
                } catch { }
            }
            if (mousePosition != oldMousePosition) {
                updateSpeed -= Vector2.Distance(mousePosition, oldMousePosition) * 10;
                if (updateSpeed < minUpdateSpeed)
                    updateSpeed = minUpdateSpeed;
            } else {
                updateSpeed = maxUpdateSpeed;
            }
            if (timerCurrent >= timerMax)
                Finish();
            Timer.Update(gameTime);
            Timer.ChangeText(((timerMax - timerCurrent) / 1000).ToString("0.0") + "S");
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            oldMousePosition = mousePosition;
        }
    }
}
