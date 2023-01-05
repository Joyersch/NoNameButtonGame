using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raigy.Obj;
using Raigy.Camera;
using NoNameButtonGame.LevelSystem.LevelContainer;
using Raigy.Input;

namespace NoNameButtonGame.LevelSystem
{
    class LevelManager : MonoObject
    {
        SampleLevel CurrentLevel;
        StartScreen startScreen;
        SettingsScreen settings;
        public delegate void Changewindowname(string name);
        public Changewindowname ChangeWindowName;

        public CameraClass GetCurrentCamera() {
            return state switch {
                MState.Settings => settings.Camera,
                MState.Startmenu => startScreen.Camera,
                MState.BetweenLevel => new CameraClass(Screen),
                _ => CurrentLevel.Camera,
            };
        }
            
        int DHeight;
        int DWidth;
        Random rand;
        Vector2 Screen;
        bool CanOverallSelect = true;
        bool RedoCall = false;
        MState state;
        SettingsScreen.ApplySettings changesettings;
        int LastLevel = 0;
        public void ChangeScreen(Vector2 Screen) {
            this.Screen = Screen;
            if (CurrentLevel != null)
                CurrentLevel.SetScreen(Screen);
        }
        public LevelManager(int Height, int Width, Vector2 Screen, SettingsScreen.ApplySettings changesettings) {
            this.changesettings = changesettings;
            DHeight = Height;
            DWidth = Width;
            this.Screen = Screen;
            
            string[] args = Environment.GetCommandLineArgs();
            rand = new Random();
            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "-seed") {
                    if (args.Length > i + 1) {
                        if (int.TryParse(args[i + 1], out int res))
                            rand = new Random(res);
                        
                    }
                }
            }
            state = MState.Startmenu;
            LastLevel = Globals.Storage.GameData.MaxLevel;
            startScreen = new StartScreen(Width, Height, Screen, rand);
            startScreen.Finish += ExitStartScreen;
            settings = new SettingsScreen(Width, Height, Screen, rand, changesettings);
        }
        enum MState
        {
            Settings,
            Startmenu,
            Level,
            BetweenLevel,
            LevelSelect,
        }
        public override void Draw(SpriteBatch sp) {
            switch (state) {
                case MState.Settings:
                    settings.Draw(sp);
                    break;
                case MState.Startmenu:
                    startScreen.Draw(sp);
                    break;
                case MState.Level:
                    CurrentLevel.Draw(sp);
                    break;
                case MState.BetweenLevel:
                    break;
                case MState.LevelSelect:
                    CurrentLevel.Draw(sp);
                    break;
            }
        }
        public override void Update(GameTime gt) {
            if (InputReaderKeyboard.CheckKey(Microsoft.Xna.Framework.Input.Keys.Escape, true)) {
                MState save = state;
                switch (state) {
                case MState.Settings:
                    case MState.Level:
                    case MState.LevelSelect:
                        state = MState.Startmenu;
                        startScreen = new StartScreen(DWidth, DHeight, Screen, rand);
                        startScreen.Finish += ExitStartScreen;
                        break;
                }
            }
            ChangeWindowName((CurrentLevel ?? new SampleLevel(DWidth, DHeight, Screen, rand) { Name = "NoNameButtonGame" }).Name);
            switch (state) {
                case MState.Settings:
                    settings.Update(gt);
                    ChangeWindowName(settings.Name);
                    break;
                case MState.Startmenu:
                    startScreen.Update(gt);
                    ChangeWindowName(startScreen.Name);
                    break;
                case MState.Level:
                    CurrentLevel.Update(gt);
                    break;
                case MState.LevelSelect:
                    CurrentLevel.Update(gt);
                    break;
                case MState.BetweenLevel:
                    InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true);
                    if (CanOverallSelect && !RedoCall) {
                        CurrentLevel = new LevelSelect(DWidth, DHeight, Screen, rand);
                        CurrentLevel.Finish += LevelSelected;
                        state = MState.LevelSelect;
                    } else {
                        SelectLevel(LastLevel);
                        state = MState.Level;
                    }
                    break;
            }


        }
        private void SelectLevel(int LL) {
            CurrentLevel = LL switch {
                0 => new Level1(DWidth, DHeight, Screen, rand),
                1 => new Level2(DWidth, DHeight, Screen, rand),
                2 => new Level3(DWidth, DHeight, Screen, rand),
                3 => new Level4(DWidth, DHeight, Screen, rand),
                4 => new Level5(DWidth, DHeight, Screen, rand),
                5 => new Level6(DWidth, DHeight, Screen, rand),
                6 => new Level7(DWidth, DHeight, Screen, rand),
                7 => new Level8(DWidth, DHeight, Screen, rand),
                8 => new Level9(DWidth, DHeight, Screen, rand),
                9 => new Level10(DWidth, DHeight, Screen, rand),
                10 => new Level11(DWidth, DHeight, Screen, rand),
                11 => new Level12(DWidth, DHeight, Screen, rand),
                12 => new Level13(DWidth, DHeight, Screen, rand),
                13 => new Level14(DWidth, DHeight, Screen, rand),
                14 => new Level15(DWidth, DHeight, Screen, rand),
                15 => new Level16(DWidth, DHeight, Screen, rand),
                16 => new Level17(DWidth, DHeight, Screen, rand),
                17 => new Level18(DWidth, DHeight, Screen, rand),
                18 => new Level19(DWidth, DHeight, Screen, rand),
                19 => new Level20(DWidth, DHeight, Screen, rand),
                20 => new Level21(DWidth, DHeight, Screen, rand),
                21 => new Level22(DWidth, DHeight, Screen, rand),
                22 => new Level23(DWidth, DHeight, Screen, rand),
                23 => new Level24(DWidth, DHeight, Screen, rand),
                24 => new Level25(DWidth, DHeight, Screen, rand),
                25 => new Level26(DWidth, DHeight, Screen, rand),
                26 => new Level27(DWidth, DHeight, Screen, rand),
                27 => new Level28(DWidth, DHeight, Screen, rand),
                28 => new Level29(DWidth, DHeight, Screen, rand),
                29 => new Level30(DWidth, DHeight, Screen, rand),
                30 => new Level31(DWidth, DHeight, Screen, rand),
                31 => new Level32(DWidth, DHeight, Screen, rand),
                32 => new Level33(DWidth, DHeight, Screen, rand),
                33 => new Level34(DWidth, DHeight, Screen, rand),
                34 => new Level35(DWidth, DHeight, Screen, rand),
                35 => new Level36(DWidth, DHeight, Screen, rand),
                36 => new Level37(DWidth, DHeight, Screen, rand),
                37 => new Level38(DWidth, DHeight, Screen, rand),
                38 => new Level39(DWidth, DHeight, Screen, rand),
                39 => new Level40(DWidth, DHeight, Screen, rand),
                40 => new Level41(DWidth, DHeight, Screen, rand),
                41 => new Level42(DWidth, DHeight, Screen, rand),
                42 => new Level43(DWidth, DHeight, Screen, rand),
                43 => new Level44(DWidth, DHeight, Screen, rand),
                44 => new Level45(DWidth, DHeight, Screen, rand),
                45 => new Level46(DWidth, DHeight, Screen, rand),
                46 => new Level47(DWidth, DHeight, Screen, rand),
                47 => new Level48(DWidth, DHeight, Screen, rand),
                48 => new Level49(DWidth, DHeight, Screen, rand),
                49 => new Level50(DWidth, DHeight, Screen, rand),
                _ => new LevelNULL(DWidth, DHeight, Screen, rand),
            };
            CurrentLevel.Finish += LevelFinish;
            CurrentLevel.Fail += LevelFail;
            CurrentLevel.Reset += LevelReset;
        }
        private void ExitStartScreen(object sender, EventArgs e) {
            StartScreen.ButtonPressed action = (sender as StartScreen).pressedAction;
            switch (action) {
                case StartScreen.ButtonPressed.Start:
                    CanOverallSelect = false;
                    state = MState.BetweenLevel;
                    RedoCall = true;
                    LastLevel = Globals.Storage.GameData.MaxLevel;
                    break;
                case StartScreen.ButtonPressed.LevelSelect:
                    state = MState.BetweenLevel;
                    CanOverallSelect = true;
                    RedoCall = false;
                    break;
                case StartScreen.ButtonPressed.Settings:
                    state = MState.Settings;
                    settings = new SettingsScreen(DWidth, DHeight, Screen, rand, changesettings);
                    break;
                case StartScreen.ButtonPressed.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
       
        private void LevelSelected(object sender, EventArgs e) {
            LastLevel = int.Parse((sender as GameObjects.TextButton).Name) - 1;
            SelectLevel(LastLevel);
            state = MState.Level;
        }
        private void LevelFinish(object sender, EventArgs e) {

            state = MState.BetweenLevel;
            if (!CanOverallSelect) {
                LastLevel++;
                if (Globals.Storage.GameData.MaxLevel < LastLevel) {
                    Globals.Storage.GameData.MaxLevel = LastLevel;
                    changesettings.Invoke(Screen, Globals.Storage.Settings.IsFixedStep, Globals.Storage.Settings.IsFullscreen);
                }
            }
            RedoCall = false;

        }
        private void LevelFail(object sender, EventArgs e) {
            state = MState.BetweenLevel;
            RedoCall = true;
        }
        private void LevelReset(object sender, EventArgs e) {
            state = MState.BetweenLevel;
            RedoCall = true;
        }
    }
}
