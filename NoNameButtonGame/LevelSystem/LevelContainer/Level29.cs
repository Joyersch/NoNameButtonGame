using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level29 : SampleLevel
    {

        readonly LockButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        readonly Laserwall wall;
        readonly TextButton ButtonStartTimer;
        readonly TextBuilder Timer;
        bool TimerStarted;
        float GT;
        float TGT;
        public Level29(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 29 - Get duck";
            button = new LockButton(new Vector2(-256, -128), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"), true);
            button.Click += BtnEvent;
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Info = new TextBuilder("RUN! IT FOLLOWs you!", new Vector2(-64, -132), new Vector2(16, 16), null, 0);

            wall = new Laserwall(new Vector2(-32, -200), new Vector2(64, 64), Globals.Content.GetHitboxMapping("zonenew"));
            wall.Enter += WallEvent;
            ButtonStartTimer = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "TimerStart", "Start Timer", new Vector2(8, 8));
            ButtonStartTimer.Click += StartTimer;
            Timer = new TextBuilder("0.0S", new Vector2(-16, 64), new Vector2(16, 16), null, 0);

        }


        private void StartTimer(object s, EventArgs e) {
            TimerStarted = true;
        }
        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        private void WallEvent(object sender, EventArgs e) {
            CallExit(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            if (TimerStarted) {
                Info.Draw(spriteBatch);
                wall.Draw(spriteBatch);
                Timer.Draw(spriteBatch);
            }
            if (!TimerStarted && button.Locked)
                ButtonStartTimer.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            cursor.Position = mousePosition - cursor.Size / 2;
            cursor.Update(gameTime);
            if (TimerStarted) {
                Info.Update(gameTime);
                GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                TGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > 32) {
                    GT -= 32;
                    Vector2 Dir = cursor.Hitbox[0].Center.ToVector2() - wall.rectangle.Center.ToVector2();

                    wall.Move(Dir / Dir.Length() * (TGT / 1000));
                }
                wall.Update(gameTime, cursor.Hitbox[0]);
                float TL = (37444 - TGT) / 1000;
                if (TL <= 0) {
                    TimerStarted = false;
                    button.Locked = false;
                }
                Timer.ChangeText(TL.ToString("0.0").Replace(',', '.') + "S");
                Timer.Update(gameTime);
            }
            if (!TimerStarted && button.Locked)
                ButtonStartTimer.Update(gameTime, cursor.Hitbox[0]);
            button.Update(gameTime, cursor.Hitbox[0]);

        }
    }
}
