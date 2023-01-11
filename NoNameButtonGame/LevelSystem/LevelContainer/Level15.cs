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
    class Level15 : SampleLevel
    {

        readonly StateButton stateButton;
        readonly Cursor mouseCursor;
        readonly TextBuilder infoText;
        public Level15(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {

            stateButton = new StateButton(new Vector2(-64, -32), new Vector2(128, 64), Globals.Content.GetHitboxMapping("awesomebutton"),100) {
                DrawColor = Color.White,
            };
            stateButton.Click += BtnEvent;
            Name = "Level 15 - Back to the roots!";
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetHitboxMapping("cursor"));
            infoText = new TextBuilder("THiS AGAIN!", new Vector2(-128, -0), new Vector2(16, 16), null, 0);
        }

        private void BtnEvent(object sender, EventArgs e) {
            CallFinish();
        }

        public override void Draw(SpriteBatch sp) {
            infoText.Draw(sp);
            stateButton.Draw(sp);
            mouseCursor.Draw(sp);
        }

        public override void Update(GameTime gt) {
            mouseCursor.Update(gt);
            base.Update(gt);
            infoText.ChangePosition(- infoText.rec.Size.ToVector2() / 2 + new Vector2(0,-64));
            mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
            stateButton.Update(gt, mouseCursor.Hitbox[0]);
            infoText.Update(gt);
        }
    }
}
