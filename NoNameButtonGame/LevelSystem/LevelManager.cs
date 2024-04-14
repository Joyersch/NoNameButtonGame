using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using NoNameButtonGame.LevelSystem.Endless;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private readonly SettingsAndSaveManager _settingsAndSaveManager;
    private readonly NoNameGame _game;
    private readonly Settings.VideoSettings _videoSettings;
    private readonly Progress _progress;

    private LevelFactory _levelFactory;

    private SampleLevel _currentLevel;

    private FinishScreen.Level _finishScreen;

    private bool _onFinishScreen;
    private int _levelId;
    private LevelState _levelState;

    public event Action CloseGame;

    public event Action<string> ChangeTitle;

    private int _currentDifficulty = 1;
    private int _currentEndlessLevelId = -1;
    private bool _starting = true;

    private enum LevelState
    {
        Menu,
        Settings,
        Credits,
        Select,
        SelectLevel,
        Level,
        Endless,
        EndlessLevel
    }

    public LevelManager(Display display, GameWindow gameWindow, SettingsAndSaveManager settingsAndSaveManager,
        NoNameGame game, EffectsRegistry effectsRegistry,
        int? seed = null)
    {
        _settingsAndSaveManager = settingsAndSaveManager;
        _game = game;
        _videoSettings = _settingsAndSaveManager.GetSetting<Settings.VideoSettings>();
        _progress = _settingsAndSaveManager.GetSave<Progress>();
        _levelId = _progress.MaxLevel + 1;
        var random = new Random(seed ?? DateTime.Now.Millisecond);
        _levelFactory = new LevelFactory(display, _videoSettings.Resolution.ToVector2(), random,
            settingsAndSaveManager, game, _progress, effectsRegistry);
        _finishScreen = _levelFactory.GetFinishScreen();
        _finishScreen.OnFinish += FinishScreenDisplayed;
        _levelState = LevelState.Menu;
        if (_progress.MaxLevel == 0)
        {
            _levelState = LevelState.Level;
            ChangeLevel(_progress.MaxLevel + 1);
            _starting = false;
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

    public void ChangeLevel(int level)
    {
        _currentLevel = _levelFactory.GetLevel(level);
        _levelId = level;
        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
    }

    private void ChangeLevel()
    {
        switch (_levelState)
        {
            case LevelState.Menu:
                _currentLevel = _levelFactory.GetStartLevel(_starting);
                _starting = false;
                break;
            case LevelState.Settings:
                _currentLevel = _levelFactory.GetSettingsLevel();
                break;
            case LevelState.Select:
                _currentLevel = _levelFactory.GetSelectLevel();
                break;
            case LevelState.SelectLevel:
            case LevelState.Level:
                ChangeLevel(_levelId);
                return;
            case LevelState.Endless:
                _currentLevel = _levelFactory.GetEndless();
                _currentEndlessLevelId = -1;
                _currentDifficulty = 1;
                break;
            case LevelState.EndlessLevel:
                if (_currentEndlessLevelId == -1)
                    _currentEndlessLevelId = _levelFactory.GetRandomDifficultyLevelId();
                _currentLevel = _levelFactory.GetLevel(_currentEndlessLevelId, _currentDifficulty);
                break;
            case LevelState.Credits:
                _currentLevel = _levelFactory.GetCredits();
                break;
        }

        ChangeTitle?.Invoke(_currentLevel.Name);
        RegisterEvents();
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
                ChangeLevel(_progress.MaxLevel + 1);
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

            mainMenu.EndlessClicked += delegate
            {
                _levelState = LevelState.Endless;
                ChangeLevel();
            };
        }
        else if (_currentLevel is Selection.Level selectLevel)
        {
            selectLevel.OnExit += ExitLevel;
            selectLevel.OnLevelSelect += delegate(int level)
            {
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
                var videoSettings = _settingsAndSaveManager.GetSetting<Settings.VideoSettings>();
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
        else if (_currentLevel is Endless.Level endlessLevel)
        {
            endlessLevel.OnExit += ExitLevel;
            endlessLevel.Selected += delegate
            {
                _levelState = LevelState.EndlessLevel;
                ChangeLevel();
            };
        }
        else
        {
            _currentLevel.OnExit += ExitLevel;
        }
    }

    private void LevelFinishes()
    {
        Log.WriteInformation("On finish screen");
        if (_levelState != LevelState.EndlessLevel)
        {
            _onFinishScreen = true;
            return;
        }

        FinishScreenDisplayed();
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

                    _settingsAndSaveManager.SaveSave();
                    Log.WriteInformation("Saved progress!");
                }

                if (_levelId == _levelFactory.MaxLevel())
                {
                    _levelState = LevelState.Menu;
                }

                _levelId++;
                Log.WriteInformation($"Increased level id to {_levelId}");
                break;

            case LevelState.SelectLevel:
                Log.WriteInformation($"Changing level to select screen.");
                _levelState = LevelState.Select;
                break;
            case LevelState.EndlessLevel:
                _currentEndlessLevelId = -1;
                if (_currentDifficulty == 1)
                    _currentDifficulty = 0;
                _currentDifficulty += 5;
                var endlessProgress = _settingsAndSaveManager.GetSave<EndlessProgress>();
                var progress = _currentDifficulty / 5;
                if (endlessProgress.HighestLevel < progress)
                {
                    endlessProgress.HighestLevel = progress;
                    _settingsAndSaveManager.SaveSave();
                }
                break;
        }

        ChangeLevel();
    }

    private void ExitLevel()
    {
        _levelState = _levelState switch
        {
            LevelState.SelectLevel => LevelState.Select,
            LevelState.EndlessLevel => LevelState.Endless,
            _ => LevelState.Menu
        };

        ChangeLevel();
    }

    private void FailLevel()
    {
        Log.WriteInformation($"Level failed. Current level: {_levelId}");
        ChangeLevel();
    }

    public int GetCurrentLevelId()
        => _levelId;

    public SampleLevel GetCurrentLevel()
        => _currentLevel;
}