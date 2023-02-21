using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.LevelSystem.LevelContainer;
using NoNameButtonGame.Input;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Linker;

namespace NoNameButtonGame.LevelSystem;

internal class LevelManager
{
    private SampleLevel _currentLevel;
    private StartScreen _startMenu;
    private SettingsScreen _settings;
    private LevelSelect _levelSelect;

    private Display.Display _display;
    private Storage.Storage _storage;

    private Random _random;
    private MenuState _state;
    private int currentSelectLevel = 0;
    private bool fromSelect = false;

    private string currentMusicName;
    private MusicSettingsLinker _musicSettingsLinker;
    private SoundEffectInstance currentMusic;
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
        currentSelectLevel = storage.GameData.MaxLevel;
        _musicSettingsLinker = new MusicSettingsLinker(_storage.Settings);

        _startMenu = new StartScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
            _storage.Settings.Resolution.ToVertor2(), _random);
        _startMenu.StartEventHandler += StartMenuOnStartEventHandler;
        _startMenu.SelectEventHandler += StartMenuOnSelectEventHandler;
        _startMenu.SettingsEventHandler += StartMenuOnSettingsEventHandler;
        _startMenu.ExitEventHandler += StartMenuOnExitEventHandler;
        _startMenu.CurrentMusicEventHandler += ALevelOnCurrentMusic;

        _settings = new SettingsScreen((int) _display.DefaultWidth, (int) _display.DefaultHeight,
            _storage.Settings.Resolution.ToVertor2(), _random, storage);
        _settings.ExitEventHandler += SettingsOnExitEventHandler;
        _settings.WindowsResizeEventHandler += SettingsOnWindowsResizeEventHandler;
        _settings.CurrentMusicEventHandler += ALevelOnCurrentMusic;
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

        if (InputReaderKeyboard.CheckKey(Microsoft.Xna.Framework.Input.Keys.Escape, true))
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
        var width = (int) _display.DefaultWidth;
        var height = (int) _display.DefaultHeight;
        var screen = _storage.Settings.Resolution.ToVertor2();
        // ToDo: LevelFactory
        _currentLevel = level switch
        {
            1 => new Level1(width, height, screen, _random),
            2 => new Level2(width, height, screen, _random),
            3 => new Level3(width, height, screen, _random),
            4 => new Level4(width, height, screen, _random),
            _ => new LevelNull(width, height, screen, _random),
        };
        _currentLevel.FinishEventHandler += LevelFinish;
        _currentLevel.FailEventHandler += LevelFail;
        _currentLevel.ExitEventHandler += LevelExitEventHandler;
        _currentLevel.CurrentMusicEventHandler += ALevelOnCurrentMusic;
        ChangeWindowName?.Invoke(_currentLevel.Name);
    }

    private void ALevelOnCurrentMusic(string newMusic)
    {
        // updates music volume on settings change!


        if (currentMusicName == newMusic)
            return;
        
        if (currentMusic is not null)
        {
            currentMusic.Stop();
            currentMusic.Dispose();
            currentMusic = null;
        }
        
        currentMusicName = newMusic;
        
        if (newMusic == String.Empty)
            return;

        currentMusic = Globals.SoundEffects.GetInstance(newMusic);
        _musicSettingsLinker.AddSettingsLink(currentMusic);
        currentMusic.IsLooped = true;
        currentMusic.Play();
    }


    private void LevelSelected(int level)
    {
        currentSelectLevel = level;
        fromSelect = true;
        SelectLevel(currentSelectLevel);
        _state = MenuState.Level;
    }

    private void LevelFinish()
    {
        if (fromSelect)
        {
            fromSelect = false;
            _state = MenuState.LevelSelect;
            return;
        }

        currentSelectLevel++;
        _storage.GameData.MaxLevel = currentSelectLevel;
        _storage.Save();

        SelectLevel(currentSelectLevel + 1);
    }

    private void LevelFail()
        => SelectLevel(currentSelectLevel);

    private void LevelExitEventHandler()
    {
        if (fromSelect)
        {
            fromSelect = false;
            _state = MenuState.LevelSelect;
            ChangeWindowName?.Invoke(_levelSelect.Name);
            return;
        }

        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_startMenu.Name);
    }

    private void StartMenuOnExitEventHandler(object sender)
        => StartMenuOnExitEventHandler();

    private void StartMenuOnExitEventHandler()
        => CloseGameEventHandler?.Invoke();

    private void StartMenuOnSettingsEventHandler(object sender)
        => StartMenuOnSettingsEventHandler();

    private void StartMenuOnSettingsEventHandler()
        => _state = MenuState.Settings;

    private void StartMenuOnSelectEventHandler(object sender)
        => StartMenuOnSelectEventHandler();

    private void StartMenuOnSelectEventHandler()
    {
        InitializeLevelSelect();
        _state = MenuState.LevelSelect;
    }

    private void StartMenuOnStartEventHandler(object sender)
        => StartMenuOnStartEventHandler();

    private void StartMenuOnStartEventHandler()
    {
        SelectLevel(_storage.GameData.MaxLevel + 1);
        _state = MenuState.Level;
    }

    private void SettingsOnExitEventHandler()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_settings.Name);
    }

    private void LevelSelectOnExitEventHandler()
    {
        _state = MenuState.StartMenu;
        ChangeWindowName?.Invoke(_levelSelect.Name);
    }

    private void SettingsOnWindowsResizeEventHandler(Vector2 newSize)
        => _startMenu.Window = newSize;

    private void InitializeLevelSelect()
    {
        _levelSelect = new LevelSelect((int) _display.DefaultWidth, (int) _display.DefaultHeight,
            _storage.Settings.Resolution.ToVertor2(), _random, _storage);
        _levelSelect.ExitEventHandler += LevelSelectOnExitEventHandler;
        _levelSelect.LevelSelectedEventHandler += LevelSelected;
        _levelSelect.CurrentMusicEventHandler += ALevelOnCurrentMusic;
        ChangeWindowName?.Invoke(_levelSelect.Name);
    }
}