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
    class Level11 : SampleLevel
    {
        readonly TextBuilder[] displayText;
        readonly Cursor mouseCursor;
        readonly EmptyButton finishButton;
        bool Loaded = false;

        public Level11(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 11 - ?";
            displayText = new TextBuilder[15];
            finishButton = new EmptyButton(new Vector2(-1, 8), new Vector2(8, 4));
            finishButton.ClickEventHandler += Finish;
            displayText[0] = new TextBuilder("So this is Level 11. I though there would be more. the creator must has", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[1] = new TextBuilder("run out of Ideas other wise he would have put some effort", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[2] = new TextBuilder("into finding an insteresting concept for this level", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[3] = new TextBuilder("but now there only text here. What Am i supposed to do", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[4] = new TextBuilder("with this! unless there is a clue hidden among the", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[5] = new TextBuilder("messages. maybe it was to do with the colors.", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[6] = new TextBuilder("maybe i need to increase the gamma.", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[7] = new TextBuilder("maybe i need to look though the game files", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[8] = new TextBuilder("maybe i need to go on some shady website", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[9] = new TextBuilder("maybe i need to look at the audio with a spectrometer", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[10] = new TextBuilder("or i might just need to wait. what a waste of time", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[11] = new TextBuilder("i usualy like meta games but i though this one was not", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[12] = new TextBuilder("one of them. well i must have guessed wrong.", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[13] = new TextBuilder(" Or this text was just a distraction so you could not", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            displayText[14] = new TextBuilder("see the button hidden behind the  >on< . Oh wow i feel dumb", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            for (int i = 0; i < displayText.Length; i++) {
                displayText[i].ChangePosition(new Vector2(0,-128 + i * 16) - displayText[i].Size / 2);
                Color[] c = new Color[displayText[i].Text.Length];
                for (int b = 0; b < c.Length; b++) {
                    if (rand.Next(0, 10) == 0) {
                        switch (rand.Next(0, 5)) {
                            case 0:
                                c[b] = Color.Red;
                                break;
                            case 1:
                                c[b] = Color.Blue;
                                break;
                            case 2:
                                c[b] = Color.Yellow;
                                break;
                            case 3:
                                c[b] = Color.Green;
                                break;
                            case 4:
                                c[b] = Color.Purple;
                                break;
                        }
                    } else
                        c[b] = Color.White;
                }
                displayText[i].ChangeColor(c);
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            
        }

        public override void Draw(SpriteBatch spriteBatch) {
            finishButton.Draw(spriteBatch);
            for (int i = 0; i < displayText.Length; i++) {
                displayText[i].Draw(spriteBatch);
            }
            mouseCursor.Draw(spriteBatch);
        }
       
        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            finishButton.Update(gameTime, mouseCursor.Hitbox[0]);
            if (!Loaded) {
                for (int i = 0; i < displayText.Length; i++) {
                    displayText[i].Update(gameTime);
                    displayText[i].ChangePosition(new Vector2(24, -120 + i * 16) - displayText[i].rectangle.Size.ToVector2() / 2);

                }
                Loaded = true;
            } else
                for (int i = 0; i < displayText.Length; i++)
                    displayText[i].Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
