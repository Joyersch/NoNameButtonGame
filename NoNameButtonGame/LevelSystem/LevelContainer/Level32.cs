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
    class Level32 : SampleLevel
    {

        readonly TextBuilder[] text;
        readonly Cursor cursor;
        readonly AwesomeButton button;
        bool Loaded = false;
        public Level32(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Level 32 - THIS ONE AGAIN?! )";
            text = new TextBuilder[15];
            button = new AwesomeButton(new Vector2(60, 86), new Vector2(5, 2.5F), Globals.Content.GetHitboxMapping("emptybutton"));
            button.Click += CallFinish;
            text[0] = new TextBuilder("at this point this seems like a lazy way of getting the level count to", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[1] = new TextBuilder("100. wait you want to tell me there are only 50? did the creator", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[2] = new TextBuilder("realize that he will not be able to create 100 levels based on the", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[3] = new TextBuilder("14 level \"types\" from the beginning! what an uncreative eediot", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[4] = new TextBuilder("well back to the level. somewere around here should be a button!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[5] = new TextBuilder("player can you do me a favor and stop reading and press that allready?", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[6] = new TextBuilder("this level type is just bad. end me! just ducking do it!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[7] = new TextBuilder("press the button! do it eediot! dont make me wait!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[8] = new TextBuilder(". . . . ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[9] = new TextBuilder(". . . . ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[10] = new TextBuilder(". . . . ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[11] = new TextBuilder(". . . . ", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[12] = new TextBuilder("what are you doing press it allready!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[13] = new TextBuilder("it is right here -->   <---", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            text[14] = new TextBuilder("why are you not pressing it!", new Vector2(0, 0), new Vector2(8, 8), null, 0);
            for (int i = 0; i < text.Length; i++) {
                text[i].ChangePosition(new Vector2(0, -128 + i * 16) - text[i].Size / 2);
                Color[] c = new Color[text[i].Text.Length];
                for (int b = 0; b < c.Length; b++) {
                    if (rand.Next(0, 10) == 0) {
                        switch (rand.Next(0, 5)) {
                            case 0:
                                c[b] = Color.Aqua;
                                break;
                            case 1:
                                c[b] = Color.Aquamarine;
                                break;
                            case 2:
                                c[b] = Color.LightBlue;
                                break;
                            case 3:
                                c[b] = Color.SkyBlue;
                                break;
                            case 4:
                                c[b] = Color.RoyalBlue;
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
            CallReset(sender, e);
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
                    text[i].ChangePosition(new Vector2(24, -120 + i * 16) - text[i].rec.Size.ToVector2() / 2);

                }
                Loaded = true;
            } else
                for (int i = 0; i < text.Length; i++)
                    text[i].Update(gameTime);
            cursor.Position = mousePosition - cursor.Size / 2;
        }
    }
}
