﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.BeforeMaths;
namespace NoNameButtonGame.LevelSystem.LevelContainer
{
    class SettingsScreen : SampleLevel
    {
        readonly TextBuilder Resolution;
        readonly TextBuilder fixedStep;
        readonly TextBuilder Fullscreen;
        readonly Cursor mouseCursor;
        readonly TextButton[] resolutionButton;
        readonly TextButton fixedStepButton;
        readonly TextButton fullscreenButton;
        readonly ApplySettings applySettings;
        Vector2 vectorResolution;
        public delegate void ApplySettings(Vector2 Rec, bool Step, bool Full);
        public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, ApplySettings applySettings) : base(defaultWidth, defaultHeight, window, rand) {
            this.applySettings = applySettings;
            string s1 ="❌", s2 = "❌";
            if (Globals.IsFix)
                s1 = "✔";
            if (Globals.FullScreen)
                s2 = "✔";
            Name = "Start Menu";
            mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10), Globals.Content.GetTHBox("cursor"));
            fixedStep = new TextBuilder("FixedStep", new Vector2(-64, -0), new Vector2(16, 16), null, 0);
            Resolution = new TextBuilder(window.X + "x" + window.Y, new Vector2(-64, -64), new Vector2(16, 16), null, 0);
            Fullscreen = new TextBuilder("Fullscreen", new Vector2(-64, 64), new Vector2(16, 16), null, 0);
            resolutionButton = new TextButton[2];
            resolutionButton[0] = new TextButton(new Vector2(64, -72), new Vector2(40, 32), Globals.Content.GetTHBox("minibutton"), ">", ">",new Vector2(16,16));
            resolutionButton[1] = new TextButton(new Vector2(-108, -72), new Vector2(40, 32), Globals.Content.GetTHBox("minibutton"), "<", "<", new Vector2(16, 16));
            resolutionButton[0].Click += ChangeRes;
            resolutionButton[1].Click += ChangeRes;
            fixedStepButton = new TextButton(new Vector2(-108, -8), new Vector2(40, 32), Globals.Content.GetTHBox("minibutton"), "IsFixedStep", s1, new Vector2(16, 16));
            fixedStepButton.Text.ChangeColor(new Color[1] { s1 == "❌" ? Color.Red : Color.Green});
            fixedStepButton.Click += ChangePressState;
            fullscreenButton = new TextButton(new Vector2(-108, 56), new Vector2(40, 32), Globals.Content.GetTHBox("minibutton"), "Fullscreen", s2, new Vector2(16, 16));
            fullscreenButton.Text.ChangeColor(new Color[1] { s2 == "❌" ? Color.Red : Color.Green });
            fullscreenButton.Click += ChangePressState;
            vectorResolution = window;

        }
        private void ChangeRes(object sender, EventArgs e) {
            
            if ((sender as TextButton).Name == ">") {
                switch (vectorResolution.X + "x" + vectorResolution.Y) {
                    case "1280x720":
                        vectorResolution = new Vector2(1920, 1080);
                        break;
                    case "1920x1080":
                        vectorResolution = new Vector2(2560, 1440);
                        break;
                    case "2560x1440":
                        vectorResolution = new Vector2(3840, 2160);
                        break;
                    case "3840x2160":
                        vectorResolution = new Vector2(1280, 720);
                        break;
                    
                }
            }

            if ((sender as TextButton).Name == "<") {
                switch (vectorResolution.X + "x" + vectorResolution.Y) {
                    case "1280x720":
                        vectorResolution = new Vector2(3840, 2160);
                        break;
                    case "1920x1080":
                        vectorResolution = new Vector2(1280, 720);
                        break;
                    case "2560x1440":
                        vectorResolution = new Vector2(1920, 1080);
                        break;
                    case "3840x2160":
                        vectorResolution = new Vector2(2560, 1440);
                        
                        break;
                }
            }
            Resolution.ChangeText(vectorResolution.X + "x" + vectorResolution.Y);
            applySettings(vectorResolution, fixedStepButton.Text.Text == "✔", fullscreenButton.Text.Text == "✔");
        }
        
        private void ChangePressState(object sender, EventArgs e) {
            string s = (sender as TextButton).Text.Text;
            switch (s){
                case "❌":
                    (sender as TextButton).Text.ChangeText("✔", new Color[1] { Color.Green });
                    break;
                case "✔":
                    (sender as TextButton).Text.ChangeText("❌", new Color[1] { Color.Red });
                    break;
            }
            applySettings(vectorResolution, fixedStepButton.Text.Text == "✔", fullscreenButton.Text.Text == "✔");
        }
        public override void Draw(SpriteBatch sp) {
            
            fixedStep.Draw(sp);
            Resolution.Draw(sp);
            Fullscreen.Draw(sp);
            for (int i = 0; i < resolutionButton.Length; i++) {
                resolutionButton[i].Draw(sp);
            }
            fixedStepButton.Draw(sp);
            fullscreenButton.Draw(sp);
            mouseCursor.Draw(sp);
        }
        public override void Update(GameTime gt) {
            base.Update(gt);
            mouseCursor.Position = mousePosition;
            mouseCursor.Update(gt);
            fixedStep.Update(gt);
            Resolution.Update(gt);
            Fullscreen.Update(gt);
            for (int i = 0; i < resolutionButton.Length; i++) {
                resolutionButton[i].Update(gt,mouseCursor.Hitbox[0]);
            }
            fixedStepButton.Update(gt, mouseCursor.Hitbox[0]);
            fullscreenButton.Update(gt, mouseCursor.Hitbox[0]);
        }
    }
}
