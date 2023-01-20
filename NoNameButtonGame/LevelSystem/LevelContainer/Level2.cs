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
using NoNameButtonGame.GameObjects.Buttons.Level;
using NoNameButtonGame.GameObjects.Buttons.StartMenu;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level2 : SampleLevel
    {

        readonly EmptyButton[] buttonGrid;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        public Level2(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 2 - WHAAT?!? There is more to this Game?!";
            buttonGrid = new EmptyButton[16];
            int randI64 = rand.Next(0, 16);
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            for (int i = 0; i < buttonGrid.Length; i++) {
                if (i == randI64) {
                    buttonGrid[i] = new WinButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                        DrawColor = Color.White,
                    };
                    buttonGrid[i].ClickEventHandler += BtnWinEvent;
                } else {
                    buttonGrid[i] = new FailButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64)) {
                        DrawColor = Color.White,
                    };
                    buttonGrid[i].ClickEventHandler += BtnFailEvent;
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
            for (int i = 0; i < buttonGrid.Length; i++) {
                buttonGrid[i].Draw(spriteBatch);
            }
            Info.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            for (int i = 0; i < buttonGrid.Length; i++) {
                buttonGrid[i].Update(gameTime, mouseCursor.Hitbox[0]);
            }
            Info.Update(gameTime);
        }
    }
}
