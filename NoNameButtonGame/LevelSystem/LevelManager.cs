using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Joyersch.Camera;
using NoNameButtonGame.LevelSystem.LevelContainer;
using Joyersch.Input;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.LevelSystem
{
    class LevelManager : MonoObject
    {
        private SampleLevel CurrentLevel;
        private StartScreen startScreen;
        private SettingsScreen settings;
        private Display.Display _display;
        private Storage _storage;
        private Random _random;

        bool CanOverallSelect = true;
        bool RedoCall = false;

        int LastLevel = 0;
        MenuState _state;

        enum MenuState
        {
            Settings,
            Startmenu,
            Level,
            BetweenLevel,
            LevelSelect
        }

        public CameraClass CurrentCamera =>
            _state switch
            {
                MenuState.Settings => settings.Camera,
                MenuState.Startmenu => startScreen.Camera,
                MenuState.BetweenLevel => new CameraClass(_storage.Settings.Resolution.ToVertor2()),
                _ => CurrentLevel.Camera,
            };

        public event Action<string> ChangeWindowName;

        public LevelManager(Display.Display display, Storage storage, int? seed = null)
        {
            _display = display;
            _storage = storage;
            _random = new Random(seed ?? DateTime.Now.Millisecond);
            _state = MenuState.Startmenu;
            LastLevel = storage.GameData.MaxLevel;
            startScreen = new StartScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                _storage.Settings.Resolution.ToVertor2(), _random);
            startScreen.Finish += ExitStartScreen;
            settings = new SettingsScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                _storage.Settings.Resolution.ToVertor2(), _random, storage);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (_state)
            {
                case MenuState.Settings:
                    settings.Draw(spriteBatch);
                    break;
                case MenuState.Startmenu:
                    startScreen.Draw(spriteBatch);
                    break;
                case MenuState.Level:
                    CurrentLevel.Draw(spriteBatch);
                    break;
                case MenuState.BetweenLevel:
                    break;
                case MenuState.LevelSelect:
                    CurrentLevel.Draw(spriteBatch);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputReaderKeyboard.CheckKey(Microsoft.Xna.Framework.Input.Keys.Escape, true))
            {
                MenuState save = _state;
                switch (_state)
                {
                    case MenuState.Settings:
                    case MenuState.Level:
                    case MenuState.LevelSelect:
                        _state = MenuState.Startmenu;
                        startScreen = new StartScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                            _storage.Settings.Resolution.ToVertor2(), _random);
                        startScreen.Finish += ExitStartScreen;
                        break;
                }
            }

            ChangeWindowName((CurrentLevel ?? new SampleLevel((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                    _storage.Settings.Resolution.ToVertor2(), _random)
                {Name = "NoNameButtonGame"}).Name);
            switch (_state)
            {
                case MenuState.Settings:
                    settings.Update(gameTime);
                    ChangeWindowName(settings.Name);
                    break;
                case MenuState.Startmenu:
                    startScreen.Update(gameTime);
                    ChangeWindowName(startScreen.Name);
                    break;
                case MenuState.Level:
                    CurrentLevel.Update(gameTime);
                    break;
                case MenuState.LevelSelect:
                    CurrentLevel.Update(gameTime);
                    break;
                case MenuState.BetweenLevel:
                    InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true);
                    if (CanOverallSelect && !RedoCall)
                    {
                        CurrentLevel = new LevelSelect((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                            _storage.Settings.Resolution.ToVertor2(), _random, _storage);
                        CurrentLevel.Finish += LevelSelected;
                        _state = MenuState.LevelSelect;
                    }
                    else
                    {
                        SelectLevel(LastLevel);
                        _state = MenuState.Level;
                    }

                    break;
            }
        }

        private void SelectLevel(int level)
        {
            var width = (int) _display.DefaultWidth;
            var height = (int) _display.DefaultHeight;
            var screen = _storage.Settings.Resolution.ToVertor2();
            CurrentLevel = level switch
            {
                0 => new Level1(width, height, screen, _random),
                1 => new Level2(width, height, screen, _random),
                2 => new Level3(width, height, screen, _random),
                3 => new Level4(width, height, screen, _random),
                4 => new Level5(width, height, screen, _random),
                5 => new Level6(width, height, screen, _random),
                6 => new Level7(width, height, screen, _random),
                7 => new Level8(width, height, screen, _random),
                8 => new Level9(width, height, screen, _random),
                9 => new Level10(width, height, screen, _random),
                10 => new Level11(width, height, screen, _random),
                11 => new Level12(width, height, screen, _random),
                12 => new Level13(width, height, screen, _random),
                13 => new Level14(width, height, screen, _random),
                14 => new Level15(width, height, screen, _random),
                15 => new Level16(width, height, screen, _random),
                16 => new Level17(width, height, screen, _random),
                17 => new Level18(width, height, screen, _random),
                18 => new Level19(width, height, screen, _random),
                19 => new Level20(width, height, screen, _random),
                20 => new Level21(width, height, screen, _random),
                21 => new Level22(width, height, screen, _random),
                22 => new Level23(width, height, screen, _random),
                23 => new Level24(width, height, screen, _random),
                24 => new Level25(width, height, screen, _random),
                25 => new Level26(width, height, screen, _random),
                26 => new Level27(width, height, screen, _random),
                27 => new Level28(width, height, screen, _random),
                28 => new Level29(width, height, screen, _random),
                29 => new Level30(width, height, screen, _random),
                30 => new Level31(width, height, screen, _random),
                31 => new Level32(width, height, screen, _random),
                32 => new Level33(width, height, screen, _random),
                33 => new Level34(width, height, screen, _random),
                34 => new Level35(width, height, screen, _random),
                35 => new Level36(width, height, screen, _random),
                36 => new Level37(width, height, screen, _random),
                37 => new Level38(width, height, screen, _random),
                38 => new Level39(width, height, screen, _random),
                39 => new Level40(width, height, screen, _random),
                40 => new Level41(width, height, screen, _random),
                41 => new Level42(width, height, screen, _random),
                42 => new Level43(width, height, screen, _random),
                43 => new Level44(width, height, screen, _random),
                44 => new Level45(width, height, screen, _random),
                45 => new Level46(width, height, screen, _random),
                46 => new Level47(width, height, screen, _random),
                47 => new Level48(width, height, screen, _random),
                48 => new Level49(width, height, screen, _random),
                49 => new Level50(width, height, screen, _random),
                _ => new LevelNULL(width, height, screen, _random),
            };
            CurrentLevel.Finish += LevelFinish;
            CurrentLevel.Fail += LevelFailOrReset;
            CurrentLevel.Reset += LevelFailOrReset;
        }

        private void ExitStartScreen(object sender, EventArgs e)
        {
            StartScreen.ButtonPressed action = (sender as StartScreen).pressedAction;
            switch (action)
            {
                case StartScreen.ButtonPressed.Start:
                    CanOverallSelect = false;
                    _state = MenuState.BetweenLevel;
                    RedoCall = true;
                    LastLevel = _storage.GameData.MaxLevel;
                    break;
                case StartScreen.ButtonPressed.LevelSelect:
                    _state = MenuState.BetweenLevel;
                    CanOverallSelect = true;
                    RedoCall = false;
                    break;
                case StartScreen.ButtonPressed.Settings:
                    _state = MenuState.Settings;
                    settings = new SettingsScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
                        _storage.Settings.Resolution.ToVertor2(), _random, _storage);
                    break;
                case StartScreen.ButtonPressed.Exit:
                    Environment.Exit(0);
                    break;
            }
        }

        private void LevelSelected(object sender, EventArgs e)
        {
            LastLevel = int.Parse((sender as GameObjects.TextButton).Name) - 1;
            SelectLevel(LastLevel);
            _state = MenuState.Level;
        }

        private void LevelFinish(object sender, EventArgs e)
        {
            _state = MenuState.BetweenLevel;
            if (CanOverallSelect)
                return;

            LastLevel++;
            
            if (_storage.GameData.MaxLevel >= LastLevel)
                return;
            
            _storage.GameData.MaxLevel = LastLevel;
            _storage.Save();


            RedoCall = false;
        }

        private void LevelFailOrReset(object sender, EventArgs e)
        {
            _state = MenuState.BetweenLevel;
            RedoCall = true;
        }
    }
}