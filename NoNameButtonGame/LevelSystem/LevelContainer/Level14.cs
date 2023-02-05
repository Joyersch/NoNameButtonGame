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
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    internal class Level14 : SampleLevel
    {
        private readonly TextButton[] awnserButton;
        private readonly Cursor mouseCursor;
        private readonly TextBuilder Questions;
        private int Awnsered;
        private readonly int[] RightAwnsers = new int[4] { -1, -1, -1 ,-1};
        private double totalGameTime;
        public Level14(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 14 - QnA Time! AGAIN!!!!";
            awnserButton = new TextButton[3];
            Questions = new TextBuilder("IS \"HAN SOLO\" STILL ALIVE", new Vector2(-64, -128), new Vector2(8, 8), null, 0);
            awnserButton[0] = new TextButton(new Vector2(-64, -96), new Vector2(128, 64), "0", "YES", new Vector2(8, 8));
            awnserButton[1] = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "1", "YES", new Vector2(8, 8));
            awnserButton[2] = new TextButton(new Vector2(-64, 32), new Vector2(128, 64), "2", "it depends", new Vector2(8, 8));
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].ClickEventHandler += BtnEvent;
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        }

        private void BtnEvent(object sender) {

            if (RightAwnsers[Awnsered] != 0 && RightAwnsers[Awnsered] != -1) {
                Fail();
            } else {
                Awnsered++;
                if (Awnsered == RightAwnsers.Length)
                    Finish();
                else {
                    switch (Awnsered) {
                        case 1:
                            Questions.ChangeText("IS \"2\" bigger than \"1\" if you count backwards?");
                            awnserButton[0].Text.ChangeText("NO");
                            awnserButton[1].Text.ChangeText("NO");
                            awnserButton[2].Text.ChangeText("PERHAPS");
                            break;
                        case 2:
                            Questions.ChangeText("DO PIGS EAT WITH THERE ASS?");
                            awnserButton[0].Text.ChangeText("NO");
                            awnserButton[1].Text.ChangeText("I THINK NOT");
                            awnserButton[2].Text.ChangeText("NEVER SEEn IT");
                            break;
                        case 3:
                            Questions.ChangeText("WHAT DID YOU DO " + totalGameTime +" SECONDS AGO?");
                            awnserButton[0].Text.ChangeText("WASTE MY LIFE");
                            awnserButton[1].Text.ChangeText("BE AN EEDIOT");
                            awnserButton[2].Text.ChangeText("(:->)");
                            break;
                    }

                }
            }

        }
        
        public override void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].Draw(spriteBatch);
            }
            Questions.Draw(spriteBatch);
            mouseCursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            totalGameTime = gameTime.TotalGameTime.TotalSeconds;
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            Questions.ChangePosition(new Vector2(0, -128) - Questions.rectangle.Size.ToVector2() / 2);
            Questions.Update(gameTime);
            for (int i = 0; i < awnserButton.Length; i++) {
                awnserButton[i].Update(gameTime, mouseCursor.Hitbox[0]);
            }
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
