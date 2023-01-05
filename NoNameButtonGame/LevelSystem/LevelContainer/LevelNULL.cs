using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.BeforeMaths;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class LevelNULL : SampleLevel
    {

        readonly AwesomeButton failButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder Info;
        public LevelNULL(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            failButton = new AwesomeButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetTHBox("failbutton")) {
                DrawColor = Color.White,
            };
            failButton.Click += BtnEvent;
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            Name = "Level ??? End";
            Info = new TextBuilder("This is the end!", new Vector2(-116, -64), new Vector2(16, 16), null, 0);
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFail();
        }
        public override void Draw(SpriteBatch sp) {
            Info.Draw(sp);
            failButton.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            failButton.Update(gt, mouseCursor.Hitbox[0]);
            Info.Update(gt);
        }
    }
}
