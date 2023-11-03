using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Ui;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private SampleLevel _currentLevel;
    private MainMenu.Level _startMenu;
    private Settings.Level _settings;
    private Selection.Level _level;
    private LevelFactory _levelFactory;

    private SampleLevel _shownLevel
    {
        get
        {
            return _state switch
            {
                MenuState.LevelSelect => _level,
                MenuState.Level => _currentLevel,
                MenuState.Settings => _settings,
                _ => _startMenu,
            };
        }
    }

    public LevelFactory Factory => _levelFactory;

    private Display _display;
    private Storage.Storage _storage;

    private MenuState _state;
    private int _currentSelectLevel;
    private bool _fromSelect;

    private string _currentMusicName;
    private SoundEffectInstance _currentMusic;

    private readonly List<SampleLevel> _toDispose;
    private readonly OverTimeInvoker _disposer;
    public event Action CloseGameEventHandler;

    private enum MenuState
    {
        Settings,
        StartMenu,
        Level,
        LevelFinish,
        LevelSelect,
        Credits
    }

    public Camera CurrentCamera =>
        _state switch
        {
            MenuState.Settings => _settings.Camera,
            MenuState.StartMenu => _startMenu.Camera,
            MenuState.LevelSelect => _level.Camera,
            _ => _currentLevel.Camera,
        };

    public event Action<string> ChangeWindowName;

    public LevelManager(Display display, GameWindow gameWindow, Storage.Storage storage, int? seed = null)
    {
        _toDispose = new List<SampleLevel>();
        _disposer = new OverTimeInvoker(200);
        _disposer.Trigger += DisposerOnTrigger;
        _display = display;
        _storage = storage;
        var random = new Random(seed ?? DateTime.Now.Millisecond);
        _state = MenuState.StartMenu;
        _currentSelectLevel = storage.GameData.MaxLevel;

        _levelFactory = new LevelFactory(_display,
            _storage.Settings.Resolution.ToVector2(), random, gameWindow, storage);

        _startMenu = _levelFactory.GetStartLevel();
        _startMenu.StartClicked += StartMenuStartClicked;
        _startMenu.SelectClicked += StartMenuSelectClicked;
        _startMenu.SettingsClicked += StartMenuSettingsClicked;
        _startMenu.ExitClicked += StartMenuExitClicked;
        _startMenu.CurrentMusicEventHandler += CurrentMusic;

        _settings = _levelFactory.GetSettingsLevel();
        _settings.ExitEventHandler += SettingsExitClicked;
        _settings.WindowsResizeEventHandler += SettingsWindowResize;
        _settings.CurrentMusicEventHandler += CurrentMusic;
        InitializeLevelSelect();
    }

    private void DisposerOnTrigger()
        => _toDispose.ForEach(l => l.Dispose());

    private void InitializeLevelSelect()
    {
        _level = _levelFactory.GetSelectLevel();
        _level.ExitEventHandler += LevelOnExitEventHandler;
        _level.LevelSelectedEventHandler += LevelSelected;
        _level.CurrentMusicEventHandler += CurrentMusic;
        ChangeWindowName?.Invoke(_level.Name);
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

    public void Update(GameTime gameTime)
    {
        var level = _shownLevel;

        if (InputReaderKeyboard.CheckKey(Keys.Escape, true))
        {
            level.Exit();
        }

        level.Update(gameTime);
        if (_state is MenuState.StartMenu or MenuState.LevelSelect or MenuState.Settings)
            _disposer.Update(gameTime);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Action<SpriteBatch> drawOnStatic)
    {
        graphicsDevice.SetRenderTarget(_display.Target);
        graphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: CurrentCamera.CameraMatrix);

        SampleLevel level = _state switch
        {
            MenuState.LevelSelect => this._level,
            MenuState.Level => _currentLevel,
            MenuState.Settings => _settings,
            _ => _startMenu,
        };

        level.Draw(spriteBatch);

        spriteBatch.End();


        graphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        graphicsDevice.Clear(Color.HotPink);

        spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

        level.DrawStatic(spriteBatch);

        drawOnStatic?.Invoke(spriteBatch);

        spriteBatch.End();
    }

    private void CurrentMusic(string newMusic)
    {
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

        _currentMusic = Global.SoundEffects.GetMusicInstance(newMusic);
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

        _toDispose.Add(_currentLevel);
        SelectLevel(_currentSelectLevel + 1);
    }

    private void LevelFail()
    {
        _toDispose.Add(_currentLevel);
        SelectLevel(_currentSelectLevel + 1);
    }

    private void LevelExitEventHandler()
    {
        _storage.Save();
        _toDispose.Add(_currentLevel);

        if (_fromSelect)
        {
            _fromSelect = false;
            _state = MenuState.LevelSelect;
            ChangeWindowName?.Invoke(_level.Name);
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
        _currentSelectLevel = _storage.GameData.MaxLevel;
        SelectLevel(_storage.GameData.MaxLevel + 1);
        _state = MenuState.Level;
    }

    private void SettingsExitClicked()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_settings.Name);
    }

    private void LevelOnExitEventHandler()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_level.Name);
    }

    private void SettingsWindowResize(Vector2 newSize)
    {
        _levelFactory.ChangeScreenSize(newSize);
        _startMenu.SetScreen(newSize);
    }

    public bool ChangeLevel(string value)
    {
        if (int.TryParse(value, out int level))
        {
            SelectLevel(level);
            _state = MenuState.Level;
            return true;
        }

        var state = _state;
        _state = value switch
        {
            "menu" => MenuState.StartMenu,
            "settings" => MenuState.Settings,
            "credits" => MenuState.Credits,
            "select" => MenuState.LevelSelect,
            _ => _state
        };

        ChangeWindowName?.Invoke(_shownLevel.Name);

        return state != _state;
    }

    public void Exit()
        => StartMenuExitClicked(_startMenu);
}