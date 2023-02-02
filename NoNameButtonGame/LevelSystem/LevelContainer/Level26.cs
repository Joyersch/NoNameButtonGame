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
    class Level26 : SampleLevel
    {

        readonly StateButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        int stat2 = 0;
        public Level26(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            button = new StateButton(new Vector2(-64, -32), new Vector2(128, 64), 100000) {
                DrawColor = Color.White,
            };
            button.ClickEventHandler += Finish; 
            Name = "Level 26 - I hope you have an autoclicker";
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
            Info = new TextBuilder("THiS AGAIN again!!1!", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Info.Draw(spriteBatch);
            button.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            Info.ChangePosition(-Info.rectangle.Size.ToVector2() / 2 + new Vector2(0, -64));
            if (button.CurrentStates < 99950 && stat2 == 0) {
                stat2++;
                Info.ChangeText("this is not a trick its real!");
            }
            if (button.CurrentStates < 99900 && stat2 == 1) {
                stat2++;
                Info.ChangeText("4 real!");
            }
            if (button.CurrentStates < 998750 && stat2 == 2) {
                stat2++;
                Info.ChangeText("wow this is going to take long!");
            }
            if (button.CurrentStates < 99500 && stat2 == 3) {
                stat2++;
                Info.ChangeText("ok let me help you");
                button.States = 100;
            }
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            Info.Update(gameTime);
        }
    }
}
