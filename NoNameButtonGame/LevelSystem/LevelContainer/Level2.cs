using System;
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
    class Level2 : SampleLevel
    {

        readonly AwesomeButton[] buttonGrid;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        public Level2(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 2 - WHAAT?!? There is more to this Game?!";
            buttonGrid = new AwesomeButton[16];
            int randI64 = rand.Next(0, 16);
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            for (int i = 0; i < buttonGrid.Length; i++) {
                if (i == randI64) {
                    buttonGrid[i] = new AwesomeButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64), Globals.Content.GetTHBox("awesomebutton")) {
                        DrawColor = Color.White,
                    };
                    buttonGrid[i].Click += BtnWinEvent;
                } else {
                    buttonGrid[i] = new AwesomeButton(new Vector2(130 * (i % 4) - 256, (i / 4) * 68 - 128), new Vector2(128, 64), Globals.Content.GetTHBox("failbutton")) {
                        DrawColor = Color.White,
                    };
                    buttonGrid[i].Click += BtnFailEvent;
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
        public override void Draw(SpriteBatch sp) {
            for (int i = 0; i < buttonGrid.Length; i++) {
                buttonGrid[i].Draw(sp);
            }
            Info.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            for (int i = 0; i < buttonGrid.Length; i++) {
                buttonGrid[i].Update(gt, mouseCursor.Hitbox[0]);
            }
            Info.Update(gt);
        }
    }
}
