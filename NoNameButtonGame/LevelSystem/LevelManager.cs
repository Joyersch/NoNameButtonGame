﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Settings;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.Settings;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private readonly SettingsAndSaveManager _settingsAndSaveManager;
    private readonly NoNameGame _game;
    private readonly VideoSettings _videoSettings;
    private readonly Progress _progress;

    private LevelFactory _levelFactory;

    private SampleLevel _currentLevel;

    private FinishScreen.Level _finishScreen;

    private bool _onFinishScreen;
    private int _levelId;
    private LevelState _levelState;

    public event Action CloseGame;

    public event Action<string> ChangeTitle;

    private enum LevelState
    {
        Menu,
        Settings,
        Credits,
        Select,
        SelectLevel,
        Level
    }

    public LevelManager(Display display, GameWindow gameWindow, SettingsAndSaveManager settingsAndSaveManager,
        NoNameGame game,
        int? seed = null)
    {
        _settingsAndSaveManager = settingsAndSaveManager;
        _game = game;
        _videoSettings = _settingsAndSaveManager.GetSetting<VideoSettings>();
        _progress = _settingsAndSaveManager.GetSave<Progress>();
        _levelId = _progress.MaxLevel + 1;
        var random = new Random(seed ?? DateTime.Now.Millisecond);
        _levelFactory = new LevelFactory(display, _videoSettings.Resolution.ToVector2(), random, gameWindow,
            settingsAndSaveManager, game, _progress);
        _finishScreen = _levelFactory.GetFinishScreen();
        _finishScreen.OnFinish += FinishScreenDisplayed;
        _levelState = LevelState.Menu;
        if (_progress.MaxLevel == 0)
        {
            _levelState = LevelState.Level;
            _levelId = _progress.MaxLevel + 1;
            ChangeLevel(_levelId);
        }
        else
            ChangeLevel();
    }

    public void Update(GameTime gameTime)
    {
        if (!_onFinishScreen)
        {
            _currentLevel.Update(gameTime);
            return;
        }

        _finishScreen.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (!_onFinishScreen)
            _currentLevel.Draw(graphicsDevice, spriteBatch);
        else
            _finishScreen.Draw(graphicsDevice, spriteBatch);
    }

    public void Exit()
        => CloseGame?.Invoke();

    public void SetAsLevelSelect()
        => _levelState = LevelState.SelectLevel;

    public bool ChangeLevel(int level)
    {
        _currentLevel = _levelFactory.GetLevel(level);
        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
        return true;
    }

    private bool ChangeLevel()
    {
        switch (_levelState)
        {
            case LevelState.Menu:
                _currentLevel = _levelFactory.GetStartLevel();
                break;
            case LevelState.Settings:
                _currentLevel = _levelFactory.GetSettingsLevel();
                break;
            case LevelState.Select:
                _currentLevel = _levelFactory.GetSelectLevel();
                break;
            case LevelState.SelectLevel:
            case LevelState.Level:
                return ChangeLevel(_levelId);
            case LevelState.Credits:
                _currentLevel = _levelFactory.GetCredits();
                break;
        }

        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
        return true;
    }

    private void RegisterEvents()
    {
        _currentLevel.OnFinish += LevelFinishes;
        _currentLevel.OnFail += FailLevel;

        if (_currentLevel is MainMenu.Level mainMenu)
        {
            mainMenu.OnExit += Exit;
            mainMenu.StartClicked += delegate
            {
                Log.WriteInformation($"Starting level {_levelId}");
                _levelState = LevelState.Level;
                _levelId = _progress.MaxLevel + 1;
                ChangeLevel(_levelId);
            };

            mainMenu.SelectClicked += delegate
            {
                _levelState = LevelState.Select;
                ChangeLevel();
            };

            mainMenu.SettingsClicked += delegate
            {
                _levelState = LevelState.Settings;
                ChangeLevel();
            };

            mainMenu.CreditsClicked += delegate
            {
                _levelState = LevelState.Credits;
                ChangeLevel();
            };
        }
        else if (_currentLevel is Selection.Level selectLevel)
        {
            selectLevel.OnExit += ExitLevel;
            selectLevel.OnLevelSelect += delegate(int level)
            {
                _levelId = level;
                Log.WriteInformation($"Selecting level {level}");
                _levelState = LevelState.SelectLevel;
                ChangeLevel(level);
            };
        }
        else if (_currentLevel is Settings.Level settingsLevel)
        {
            settingsLevel.OnDiscard += delegate
            {
                _settingsAndSaveManager.LoadSettings();
                _game.ApplySettings();
                var videoSettings = _settingsAndSaveManager.GetSetting<VideoSettings>();
                _levelFactory.ChangeScreenSize(videoSettings.Resolution.ToVector2());
                _finishScreen = _levelFactory.GetFinishScreen();
                ExitLevel();
            };
            settingsLevel.OnSave += delegate
            {
                _settingsAndSaveManager.SaveSettings();
                _finishScreen = _levelFactory.GetFinishScreen();
                _finishScreen.OnFinish += FinishScreenDisplayed;
                ExitLevel();
            };
            settingsLevel.OnWindowResize += delegate(Vector2 screen) { _levelFactory.ChangeScreenSize(screen); };
            settingsLevel.OnNameChange += delegate { ChangeTitle?.Invoke(settingsLevel.Name); };
        }
        else
        {
            _currentLevel.OnExit += ExitLevel;
        }
    }

    private void LevelFinishes()
    {
        Log.WriteInformation("On finish screen");
        _onFinishScreen = true;
    }

    private void FinishScreenDisplayed()
    {
        _onFinishScreen = false;
        Log.WriteInformation("Finished level");
        switch (_levelState)
        {
            case LevelState.Level:
                int max = _progress.MaxLevel;
                if (_levelId > max)
                {
                    _progress.MaxLevel = _levelId;
                    Log.WriteInformation($"Updated max level value to {_levelId}");
                    if (_levelId == _levelFactory.MaxLevel())
                    {
                        _levelState = LevelState.Credits;
                        _currentLevel = _levelFactory.GetCredits();
                    }

                    _settingsAndSaveManager.SaveSave();
                    Log.WriteInformation("Saved progress!");
                }

                _levelId++;
                Log.WriteInformation($"Increased level id to {_levelId}");
                break;

            case LevelState.SelectLevel:
                Log.WriteInformation($"Changing level to select screen.");
                _levelState = LevelState.Select;
                break;
        }

        ChangeLevel();
    }

    private void ExitLevel()
    {
        _levelState = _levelState switch
        {
            LevelState.SelectLevel => LevelState.Select,
            _ => LevelState.Menu
        };

        ChangeLevel();
    }

    private void FailLevel()
    {
        Log.WriteInformation($"Level failed. Current level: {_levelId}");
        ChangeLevel();
    }
}