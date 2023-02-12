using System;
using System.Collections.Generic;
using System.Text;

using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level10 : SampleLevel
    {
        private readonly TextButton[] awnserButtons;
        private readonly Cursor mouseCursor;
        private readonly TextBuilder Questions;
        private int ammountAwnsered;
        private readonly int[] RightAwnsers = new int[3] { 0, 2, 1 };
        
        public Level10(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 10 - QnA Time!";
            awnserButtons = new TextButton[3];
            Questions = new TextBuilder("3 + 4 = 5 => 5 + 5 =?", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            awnserButtons[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), "0", "7", new Vector2(8, 8));
            awnserButtons[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "1", "11", new Vector2(8, 8));
            awnserButtons[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), "2", "5", new Vector2(8, 8));
            for (int i = 0; i < awnserButtons.Length; i++) {
                awnserButtons[i].ClickEventHandler += BtnEvent;
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        }



        private void BtnEvent(object sender) {

            if (RightAwnsers[ammountAwnsered] != 0 && RightAwnsers[ammountAwnsered] != -1) {
                Fail();
            } else {
                ammountAwnsered++;
                if (ammountAwnsered == RightAwnsers.Length)
                    Finish();
                else {
                    switch (ammountAwnsered) {
                        case 1:
                            Questions.ChangeText("Level 2 has ? Buttons!");
                            awnserButtons[0].Text.ChangeText("25");
                            awnserButtons[1].Text.ChangeText("20");
                            awnserButtons[2].Text.ChangeText("16");
                            break;
                        case 2:
                            Questions.ChangeText("What previous Level had you hold a button");
                            awnserButtons[0].Text.ChangeText("3!");
                            awnserButtons[1].Text.ChangeText("6!");
                            awnserButtons[2].Text.ChangeText("4!");
                            break;
                    }
                    
                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < awnserButtons.Length; i++) {
                awnserButtons[i].Draw(spriteBatch);
            }
            Questions.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            Questions.ChangePosition(new Vector2(0, -128) - Questions.Rectangle.Size.ToVector2() / 2);
            Questions.Update(gameTime);
            for (int i = 0; i < awnserButtons.Length; i++) {
                awnserButtons[i].Update(gameTime, mouseCursor.Hitbox[0]);
            }
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
