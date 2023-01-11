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
    class Level50 : SampleLevel
    {

        readonly StateButton button;
        readonly Cursor cursor;
        readonly TextBuilder Info;
        public Level50(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            button = new StateButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"), 1337) {
                DrawColor = Color.White,
            };
            button.Click += BtnEvent;
            Name = "Level 50 - It's finaly over";
            cursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            Info = new TextBuilder("NO HELP THIS TIME", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
        }



        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            Info.Draw(spriteBatch);
            button.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime) {
            cursor.Update(gameTime);
            base.Update(gameTime);
            Info.ChangePosition(-Info.rec.Size.ToVector2() / 2 + new Vector2(0, -64));
            
            cursor.Position = mousePosition - cursor.Size / 2;
            button.Update(gameTime, cursor.Hitbox[0]);
            Info.Update(gameTime);
        }
    }
}
