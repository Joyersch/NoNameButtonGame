﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private SampleLevel _currentLevel;
    private StartScreen _startMenu;
    private SettingsScreen _settings;
    private LevelSelect _levelSelect;
    private LevelFactory _levelFactory;

    private Display.Display _display;
    private Storage.Storage _storage;

    private Random _random;
    private MenuState _state;
    private int _currentSelectLevel;
    private bool _fromSelect;

    private string _currentMusicName;
    private SoundEffectInstance _currentMusic;
    public event Action CloseGameEventHandler;

    private enum MenuState
    {
        Settings,
        StartMenu,
        Level,
        LevelSelect
    }

    public CameraClass CurrentCamera =>
        _state switch
        {
            MenuState.Settings => _settings.Camera,
            MenuState.StartMenu => _startMenu.Camera,
            MenuState.LevelSelect => _levelSelect.Camera,
            _ => _currentLevel.Camera,
        };

    public event Action<string> ChangeWindowName;

    public LevelManager(Display.Display display, Storage.Storage storage, int? seed = null)
    {
        _display = display;
        _storage = storage;
        _random = new Random(seed ?? DateTime.Now.Millisecond);
        _state = MenuState.StartMenu;
        _currentSelectLevel = storage.GameData.MaxLevel;

        _levelFactory = new LevelFactory((int) _display.DefaultWidth, (int) _display.DefaultHeight,
            _storage.Settings.Resolution.ToVertor2(), _random);

        _startMenu = _levelFactory.GetStartLevel();
        _startMenu.StartClicked += StartMenuStartClicked;
        _startMenu.SelectClicked += StartMenuSelectClicked;
        _startMenu.SettingsClicked += StartMenuSettingsClicked;
        _startMenu.ExitClicked += StartMenuExitClicked;
        _startMenu.CurrentMusicEventHandler += CurrentMusic;

        _settings = _levelFactory.GetSettingsLevel(storage);
        _settings.ExitEventHandler += SettingsExitClicked;
        _settings.WindowsResizeEventHandler += SettingsWindowResize;
        _settings.CurrentMusicEventHandler += CurrentMusic;
        InitializeLevelSelect();
    }

    public void Update(GameTime gameTime)
    {
        SampleLevel level = _state switch
        {
            MenuState.LevelSelect => _levelSelect,
            MenuState.Level => _currentLevel,
            MenuState.Settings => _settings,
            _ => _startMenu,
        };

        if (InputReaderKeyboard.CheckKey(Keys.Escape, true))
        {
            level.Exit();
        }

        level.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SampleLevel level = _state switch
        {
            MenuState.LevelSelect => _levelSelect,
            MenuState.Level => _currentLevel,
            MenuState.Settings => _settings,
            _ => _startMenu,
        };
        level.Draw(spriteBatch);
    }

    private void SelectLevel(int level)
    {
        _currentLevel = _levelFactory.GetLevel(level);
        _currentLevel.FinishEventHandler += LevelFinish;
        _currentLevel.FailEventHandler += LevelFail;
        _currentLevel.ExitEventHandler += LevelExitEventHandler;
        _currentLevel.CurrentMusicEventHandler += CurrentMusic;
        ChangeWindowName?.Invoke(_currentLevel.Name);
    }

    private void CurrentMusic(string newMusic)
    {
        // updates music volume on settings change!

        if (_currentMusicName == newMusic)
            return;
        
        if (_currentMusic is not null)
        {
            _currentMusic.Stop();
            _currentMusic.Dispose();
            _currentMusic = null;
        }
        
        _currentMusicName = newMusic;
        
        if (newMusic == string.Empty)
            return;

        _currentMusic = Globals.SoundEffects.GetMusicInstance(newMusic);
        _currentMusic.IsLooped = true;
        _currentMusic.Play();
    }


    private void LevelSelected(int level)
    {
        _currentSelectLevel = level;
        _fromSelect = true;
        SelectLevel(_currentSelectLevel);
        _state = MenuState.Level;
    }

    private void LevelFinish()
    {
        if (_fromSelect)
        {
            _fromSelect = false;
            _state = MenuState.LevelSelect;
            return;
        }

        _currentSelectLevel++;
        _storage.GameData.MaxLevel = _currentSelectLevel;
        _storage.Save();

        SelectLevel(_currentSelectLevel + 1);
    }

    private void LevelFail()
        => SelectLevel(_currentSelectLevel + 1);

    private void LevelExitEventHandler()
    {
        if (_fromSelect)
        {
            _fromSelect = false;
            _state = MenuState.LevelSelect;
            ChangeWindowName?.Invoke(_levelSelect.Name);
            return;
        }

        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_startMenu.Name);
    }

    private void StartMenuExitClicked(object sender)
        => StartMenuOnExitEventHandler();

    private void StartMenuOnExitEventHandler()
        => CloseGameEventHandler?.Invoke();

    private void StartMenuSettingsClicked(object sender)
        => StartMenuOnSettingsEventHandler();

    private void StartMenuOnSettingsEventHandler()
        => _state = MenuState.Settings;

    private void StartMenuSelectClicked(object sender)
        => StartMenuOnSelectEventHandler();

    private void StartMenuOnSelectEventHandler()
    {
        InitializeLevelSelect();
        _state = MenuState.LevelSelect;
    }

    private void StartMenuStartClicked(object sender)
        => StartMenuOnStartEventHandler();

    private void StartMenuOnStartEventHandler()
    {
        SelectLevel(_storage.GameData.MaxLevel + 1);
        _state = MenuState.Level;
    }

    private void SettingsExitClicked()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_settings.Name);
    }

    private void LevelSelectOnExitEventHandler()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_levelSelect.Name);
    }

    private void SettingsWindowResize(Vector2 newSize)
        => _startMenu.Window = newSize;

    private void InitializeLevelSelect()
    {
        _levelSelect = _levelFactory.GetSelectLevel(_storage);
        _levelSelect.ExitEventHandler += LevelSelectOnExitEventHandler;
        _levelSelect.LevelSelectedEventHandler += LevelSelected;
        _levelSelect.CurrentMusicEventHandler += CurrentMusic;
        ChangeWindowName?.Invoke(_levelSelect.Name);
    }
}