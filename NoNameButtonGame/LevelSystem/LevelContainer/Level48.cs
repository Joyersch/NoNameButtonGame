using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level48 : SampleLevel
    {
        private readonly LockButton lockedButton;
        private readonly Cursor mouseCursor;
        private readonly TextBuilder Info;
        private readonly GlitchBlockCollection Walls;
        private readonly TextButton ButtonStartTimer;
        private readonly TextBuilder Timer;
        private bool TimerStarted;
        private float GT;
        private float TGT;
        public Level48(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 48 - THERE IS NO ESCAPE!!";
            lockedButton = new LockWinButton(new Vector2(-256, -128), new Vector2(128, 64), true);
            lockedButton.ClickEventHandler += Finish;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Info = new TextBuilder("RUN! IT FOLLOWs you!", new Vector2(-64, -132), new Vector2(16, 16), null, 0);

            Walls = new GlitchBlockCollection(new Vector2(-32, -200), new Vector2(64, 64));
            Walls.EnterEventHandler += Fail;
            ButtonStartTimer = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "TimerStart", "Start Timer", new Vector2(8, 8));
            ButtonStartTimer.ClickEventHandler += StartTimer;
            Timer = new TextBuilder("0.0S", new Vector2(-16, 64), new Vector2(16, 16), null, 0);

        }
        private void StartTimer(object sender) {
            TimerStarted = true;
        }


        public override void Draw(SpriteBatch spriteBatch) {
            lockedButton.Draw(spriteBatch);
            if (TimerStarted) {
                Info.Draw(spriteBatch);
                Walls.Draw(spriteBatch);
                Timer.Draw(spriteBatch);
            }
            if (!TimerStarted && lockedButton.IsLocked)
                ButtonStartTimer.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Canvas / 2;
            mouseCursor.Update(gameTime);
            if (TimerStarted) {
                Info.Update(gameTime);
                GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                TGT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (GT > 32) {
                    GT -= 32;
                    Vector2 Dir = mouseCursor.Hitbox[0].Center.ToVector2() - Walls.Rectangle.Center.ToVector2();

                    Walls.Move(Dir / Dir.Length() * (TGT / 1000));
                }
                Walls.Update(gameTime, mouseCursor.Hitbox[0]);
                float TL = (50000 - TGT) / 1000;
                if (TL <= 0) {
                    TimerStarted = false;
                    lockedButton.Unlock();
                }
                Timer.ChangeText(TL.ToString("0.0").Replace(',', '.') + "S");
                Timer.Update(gameTime);
            }
            if (!TimerStarted && lockedButton.IsLocked)
                ButtonStartTimer.Update(gameTime, mouseCursor.Hitbox[0]);
            lockedButton.Update(gameTime, mouseCursor.Hitbox[0]);

        }
    }
}
