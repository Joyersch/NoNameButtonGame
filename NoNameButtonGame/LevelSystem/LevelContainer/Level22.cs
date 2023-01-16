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
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class Level22 : SampleLevel
    {

        readonly TextBuilder[] text;
        readonly Cursor cursor;
        readonly AwesomeButton button;
        bool Loaded = false;
        public Level22(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 22 - Random.org do be choosing the same levels over and over again! ( I created them based on a random result )";
            text = new TextBuilder[15];
            button = new AwesomeButton(new Vector2(190, 106), new Vector2(5, 2.5F), Globals.Content.GetHitboxMapping("emptybutton"));
            button.Click += CallFinish;
            text[0] = new TextBuilder("this again. why! there needs to be something more to this, it cannot", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[1] = new TextBuilder("be the only thing! the first time it was funny", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[2] = new TextBuilder("the seconds time it was only sort of  funny but this", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[3] = new TextBuilder("just needs to stop. \" its time to stop!\" ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[4] = new TextBuilder("well I know how this works now, this text is just a", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[5] = new TextBuilder("distraction and the button is hidden somewere!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[6] = new TextBuilder("i hope this time it wont be as hard because the levels", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[7] = new TextBuilder("before were allready way to hard for my liking and", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[8] = new TextBuilder("this game is suppose to have 50 levels! at this", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[9] = new TextBuilder("rate the level will become a mario maker type of impossible.", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[10] = new TextBuilder("but i should focus on my goals now, which is finding the button", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[11] = new TextBuilder("but this time i cannot find it. i hope i dont have to edit the", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[12] = new TextBuilder("save file located at \"documents\\NoNameButtonGame\\\"! but if ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[13] = new TextBuilder("I am not able to beat this level i might need to do that!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[14] = new TextBuilder("lets just hope i am able to find the button at the end!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            for (int i = 0; i < text.Length; i++) {
                text[i].ChangePosition(new Vector2(0, -128 + i * 16) - text[i].Size / 2);
                Color[] c = new Color[text[i].Text.Length];
                for (int b = 0; b < c.Length; b++) {
                    if (rand.Next(0, 10) == 0) {
                        switch (rand.Next(0, 5)) {
                            case 0:
                                c[b] = Color.Red;
                                break;
                            case 1:
                                c[b] = Color.Coral;
                                break;
                            case 2:
                                c[b] = Color.LightCoral;
                                break;
                            case 3:
                                c[b] = Color.IndianRed;
                                break;
                            case 4:
                                c[b] = Color.OrangeRed;
                                break;
                        }
                    } else
                        c[b] = Color.White;
                }
                text[i].ChangeColor(c);
            }
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));

        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish(sender, e);
        }
        private void WallEvent(object sender, EventArgs e) {
            CallExit(sender, e);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            button.Draw(spriteBatch);
            for (int i = 0; i < text.Length; i++) {
                text[i].Draw(spriteBatch);
            }
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            button.Update(gameTime, cursor.Hitbox[0]);
            if (!Loaded) {
                for (int i = 0; i < text.Length; i++) {
                    text[i].Update(gameTime);
                    text[i].ChangePosition(new Vector2(24, -120 + i * 16) - text[i].rectangle.Size.ToVector2() / 2);
                    
                }
                Loaded = true;
            }
            else 
                for (int i = 0; i < text.Length; i++)
                text[i].Update(gameTime);
            cursor.Position = mousePosition - cursor.Size / 2;
        }
    }
}
