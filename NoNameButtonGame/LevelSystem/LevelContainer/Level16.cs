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
    class Level16 : SampleLevel
    {

        readonly TextBuilder[] longText;
        readonly Cursor mouseCursor;
        readonly EmptyButton finishButton;
        bool Loaded = false;
        public Level16(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 16 - ???";
            longText = new TextBuilder[15];
            finishButton = new EmptyButton(new Vector2(-(defaultWidth / Camera.Zoom / 2), -(defaultHeight / Camera.Zoom / 2)), new Vector2(4, 2));
            finishButton.ClickEventHandler += Finish;
            longText[0] = new TextBuilder("So this again.... what happened... did the creator go even more", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[1] = new TextBuilder("lazy? now he is just repeating levels. i bet he just used some", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[2] = new TextBuilder("sort of randomizer like random.org to mix the levels and makes them", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[3] = new TextBuilder("more difficult or something of the sort. But who can realy tell?", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[4] = new TextBuilder("the only way would be to look at the source code but it is not", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[5] = new TextBuilder("like the code is open source or anything to the sort so people", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[6] = new TextBuilder("could check on how bad the coding actualy is!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[7] = new TextBuilder("Enough with the time wasting I need to find the stupid button", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[8] = new TextBuilder("there is nothing else here anyway other than this text!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[9] = new TextBuilder("or maybe the creator added something else in here but i bet", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[10] = new TextBuilder("he was to lazy to do something as simple as that as well.", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[11] = new TextBuilder("so the last button was already hard to find where could he", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[12] = new TextBuilder("have been hiding this one. . . oh wow i realy cannot find it", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[13] = new TextBuilder("this time... he must have hide it realy well this time or", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            longText[14] = new TextBuilder("just made through it in a corner or something stupid like that", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            for (int i = 0; i < longText.Length; i++) {
                longText[i].ChangePosition(new Vector2(0, -128 + i * 16) - longText[i].Size / 2);
                Color[] c = new Color[longText[i].Text.Length];
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
                longText[i].ChangeColor(c);
            }
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));

        }
        
        public override void Draw(SpriteBatch spriteBatch) {
            finishButton.Draw(spriteBatch);
            for (int i = 0; i < longText.Length; i++) {
                longText[i].Draw(spriteBatch);
            }
            mouseCursor.Draw(spriteBatch);
        }
        
        public override void Update(GameTime gameTime) {
            mouseCursor.Update(gameTime);
            base.Update(gameTime);
            finishButton.Update(gameTime, mouseCursor.Hitbox[0]);
            if (!Loaded) {
                for (int i = 0; i < longText.Length; i++) {
                    longText[i].Update(gameTime);
                    longText[i].ChangePosition(new Vector2(24, -120 + i * 16) - longText[i].rectangle.Size.ToVector2() / 2);

                }
                Loaded = true;
            } else
                for (int i = 0; i < longText.Length; i++)
                    longText[i].Update(gameTime);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        }
    }
}
