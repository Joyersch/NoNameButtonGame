using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Buttons.Level;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level27 : SampleLevel
    {

        readonly EmptyButton[] button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        readonly Random rand;
        float GT;
        public Level27(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 27 - I hope you have a good reaction time";
            button = new EmptyButton[16];
            this.rand = rand;
            int randI64 = rand.Next(0, 16);
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            for (int i = 0; i < button.Length; i++) {
                if (i == randI64) {
                    button[i] = new WinButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                        DrawColor = Color.White,
                    };
                    button[i].ClickEventHandler += BtnWinEvent;
                } else {
                    button[i] = new FailButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                        DrawColor = Color.White,
                    };
                    button[i].ClickEventHandler += BtnFailEvent;
                }
            }
            Info = new TextBuilder("Watch out. There Random!", new Vector2(-170, -(defaultHeight / Camera.Zoom / 2) + 32), new Vector2(16, 16), null, 0);

        }

        private void BtnFailEvent(object sender, EventArgs e) {
            CallFail();
        }

        private void BtnWinEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < button.Length; i++) {
                button[i].Draw(spriteBatch);
            }
            Info.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            GT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            while (GT > 333) {
                GT -= 333;
                int randI64 = rand.Next(0, 16);
                for (int i = 0; i < button.Length; i++) {
                    if (i == randI64) {
                        button[i] = new WinButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                            DrawColor = Color.White,
                        };
                        button[i].ClickEventHandler += BtnWinEvent;
                    } else {
                        button[i] = new FailButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                            DrawColor = Color.White,
                        };
                        button[i].ClickEventHandler += BtnFailEvent;
                    }
                }
            }
            cursor.Position = mousePosition - cursor.Size / 2;
            for (int i = 0; i < button.Length; i++) {
                button[i].Update(gameTime, cursor.Hitbox[0]);
            }
            Info.Update(gameTime);
        }
    }
}
