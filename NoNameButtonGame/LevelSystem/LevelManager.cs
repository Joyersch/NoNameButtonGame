using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Settings;
using MonoUtils.Ui;
using NoNameButtonGame.Saves;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private readonly Display _display;
    private readonly SettingsManager _settingsManager;
    private readonly VideoSettings _videoSettings;
    private readonly Progress _progress;

    private LevelFactory _levelFactory;

    private SampleLevel _currentLevel;
    private MainMenu.Level _startMenu;
    private Settings.Level _settings;

    private int _levelId;
    private Selection.Level _level;
    private LevelState _levelState;

    public event Action CloseGame;
    public event Action SettingsChanged;

    public event Action<string> ChangeTitle;

    private enum LevelState
    {
        Menu,
        Settings,
        Credits,
        Changelog,
        Select,
        SelectLevel,
        Level
    }

    public LevelManager(Display display, GameWindow gameWindow, SettingsManager settingsManager, int? seed = null)
    {
        _display = display;
        _settingsManager = settingsManager;
        _videoSettings = _settingsManager.GetSetting<VideoSettings>();
        _progress = _settingsManager.GetSetting<Progress>();
        var random = new Random(seed ?? DateTime.Now.Millisecond);
        _levelFactory = new LevelFactory(display,
            _videoSettings.Resolution.ToVector2(), random, gameWindow, settingsManager);
        _levelState = LevelState.Menu;
        ChangeLevel();
    }

    public void Update(GameTime gameTime)
    {
        _currentLevel.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Action<SpriteBatch> drawOnStatic)
    {
        graphicsDevice.SetRenderTarget(_display.Target);
        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: _currentLevel.Camera.CameraMatrix);

        _currentLevel.Draw(spriteBatch);

        spriteBatch.End();

        graphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        graphicsDevice.Clear(Color.HotPink);

        spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

        _currentLevel.DrawStatic(spriteBatch);

        drawOnStatic?.Invoke(spriteBatch);

        spriteBatch.End();
    }

    public void Exit()
        => CloseGame?.Invoke();

    public void SetAsLevelSelect()
        => _levelState = LevelState.SelectLevel;

    public bool ChangeLevel(int level)
    {
        _currentLevel = _levelFactory.GetLevel(level);
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
                ChangeTitle?.Invoke(_currentLevel.Name);
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
                // ToDo: Credits
                Log.WriteCritical("Credits not defined!");
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
                ChangeTitle?.Invoke(_currentLevel.Name);
            };
        }
        else if (_currentLevel is Settings.Level settingsLevel)
        {
            settingsLevel.OnExit += ExitLevel;
            settingsLevel.OnWindowResize += delegate(Vector2 screen) { _levelFactory.ChangeScreenSize(screen); };
            settingsLevel.OnSettingsChange += Save;
        }
        else
        {
            _currentLevel.OnExit += ExitLevel;
        }
    }

    private void LevelFinishes()
    {
        Log.WriteInformation("Finished level");
        // ToDo: Finish Screen here
        FinishScreenDisplayed();
    }

    private void FinishScreenDisplayed()
    {
        switch (_levelState)
        {
            case LevelState.Level:
                int max = _progress.MaxLevel;
                if (_levelId > max)
                {
                    _progress.MaxLevel = _levelId;
                    Log.WriteInformation($"Updated max level value to {_levelId}");
                    Save();
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
        if (_levelState == LevelState.Settings)
            Save();

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

    private void Save()
    {
        _settingsManager.Save();
        Log.WriteInformation("Saved the game!");
        if (_levelState == LevelState.Settings)
            SettingsChanged?.Invoke();
    }
}