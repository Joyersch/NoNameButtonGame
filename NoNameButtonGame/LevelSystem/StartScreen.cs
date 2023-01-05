using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.BeforeMaths;

namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class StartScreen : SampleLevel
    {
        readonly AwesomeButton startButton;
        readonly AwesomeButton settingsButton;
        readonly AwesomeButton selectLevelButton;
        readonly AwesomeButton exitButton;
        readonly Cursor mouseCursor;
        public ButtonPressed pressedAction;
        public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight, window, rand) {
            Name = "Start Menu";
            int Startpos = -(64 * 2);
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            startButton = new AwesomeButton(new Vector2(-64, Startpos), new Vector2(160, 64), Globals.Content.GetTHBox("startbutton"));
            startButton.Click += BnEvStart;
            selectLevelButton = new AwesomeButton(new Vector2(-92, Startpos + 64), new Vector2(216, 64), Globals.Content.GetTHBox("selectbutton"));
            selectLevelButton.Click += BnEvSelect;
            settingsButton = new AwesomeButton(new Vector2(-130, Startpos + 64 * 2), new Vector2(292, 64), Globals.Content.GetTHBox("settingsbutton"));
            settingsButton.Click += BnEvSettings;
            exitButton = new AwesomeButton(new Vector2(-52, Startpos + 64 * 3), new Vector2(136, 64), Globals.Content.GetTHBox("exitbutton"));
            exitButton.Click += BnEvExit;
        }
        
        
        public enum ButtonPressed
        {
            Start,
            LevelSelect,
            Settings,
            Exit
        }
        private void BnEvStart(object sender, EventArgs e) {
            pressedAction = ButtonPressed.Start;
            CallFinish(this, e);
        }
        private void BnEvSelect(object sender, EventArgs e) {
            pressedAction = ButtonPressed.LevelSelect;
            CallFinish(this, e);
        }
        private void BnEvSettings(object sender, EventArgs e) {
            pressedAction = ButtonPressed.Settings;
            CallFinish(this, e);
        }
        private void BnEvExit(object sender, EventArgs e) {
            pressedAction = ButtonPressed.Exit;
            CallFinish(this, e);
        }
        public override void Draw(SpriteBatch sp) {

            startButton.Draw(sp);
            settingsButton.Draw(sp);
            selectLevelButton.Draw(sp);
            exitButton.Draw(sp);
            mouseCursor.Draw(sp);
        }
        public override void Update(GameTime gt) {
            base.Update(gt);
            startButton.Update(gt, mouseCursor.Hitbox[0]);
            settingsButton.Update(gt, mouseCursor.Hitbox[0]);
            selectLevelButton.Update(gt, mouseCursor.Hitbox[0]);
            exitButton.Update(gt, mouseCursor.Hitbox[0]);
            mouseCursor.Position = mousePosition;
            mouseCursor.Update(gt);
        }
    }
}
