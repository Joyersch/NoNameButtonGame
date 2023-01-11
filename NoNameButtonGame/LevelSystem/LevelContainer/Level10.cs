using System;
using System.Collections.Generic;
using System.Text;

using Joyersch.Input;
using Joyersch.Camera;

using NoNameButtonGame.Interfaces;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level10 : SampleLevel
    {

        readonly TextButton[] awnserButtons;
        readonly Cursor mouseCursor;
        readonly TextBuilder Questions;
        int ammountAwnsered;
        readonly int[] RightAwnsers = new int[3] { 0, 2, 1 };
        
        public Level10(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 10 - QnA Time!";
            awnserButtons = new TextButton[3];
            Questions = new TextBuilder("3 + 4 = 5 => 5 + 5 =?", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            awnserButtons[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "0", "7", new Vector2(8, 8));
            awnserButtons[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "1", "11", new Vector2(8, 8));
            awnserButtons[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("emptybutton"), "2", "5", new Vector2(8, 8));
            for (int i = 0; i < awnserButtons.Length; i++) {
                awnserButtons[i].Click += BtnEvent;
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
        }



        private void BtnEvent(object sender, EventArgs e) {

            if (RightAwnsers[ammountAwnsered] != int.Parse((sender as TextButton).Name) && RightAwnsers[ammountAwnsered] != -1) {
                CallFail(this, e);
            } else {
                ammountAwnsered++;
                if (ammountAwnsered == RightAwnsers.Length)
                    CallFinish(this, e);
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
            Questions.ChangePosition(new Vector2(0, -128) - Questions.rec.Size.ToVector2() / 2);
            Questions.Update(gameTime);
            for (int i = 0; i < awnserButtons.Length; i++) {
                awnserButtons[i].Update(gameTime, mouseCursor.Hitbox[0]);
            }
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
