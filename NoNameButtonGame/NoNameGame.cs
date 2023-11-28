using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Console;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level12;
using NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;
using NoNameButtonGame.LevelSystem.Settings;

namespace NoNameButtonGame;

public class NoNameGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Display _display;
    private SettingsManager _settingsManager;
    private LevelManager _levelManager;

    private AdvancedSettings advancedSettings;

    private DevConsole _console;
    private bool _isConsoleActive;
    private bool _isConsoleEnabled;

    private Dictionary<string, string> UtilsMapping = new()
    {
        { nameof(GameObject), "placeholder" },
        { nameof(Cursor), "cursor" }
    };


    public NoNameGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // This will also call LoadContent()
        base.Initialize();

        _display = new Display(GraphicsDevice);
        Window.TextInput += OnTextInput;

        Global.CommandProcessor.Initialize();

        // Check Save directory
        if (!Directory.Exists(Globals.SaveDirectory))
            Directory.CreateDirectory(Globals.SaveDirectory);

        _console = new DevConsole(Global.CommandProcessor, Vector2.Zero, _display.SimpleScale,
            _console);
        Log.Out = new LogAdapter(_console);

        _settingsManager = new SettingsManager(Globals.SaveDirectory, 0);
        if (!_settingsManager.Load())
            _settingsManager.Save();

        ApplySettings();

        TextProvider.Initialize();

        // register soundSettingsListener to change sound volume if
        //Global.SoundSettingsListener = new SoundSettingsListener(_settingsManager.Settings);

        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(_display, Window, _settingsManager, this);
        _levelManager.ChangeTitle += ChangeTitle;
        _levelManager.CloseGame += Exit;
        _levelManager.SettingsChanged += ApplySettings;

        // register context for console commands
        _console.Context.RegisterContext(nameof(LevelManager), _levelManager);
        _console.Context.RegisterContext(nameof(_settingsManager), _settingsManager);
    }

    private void ChangeTitle(string newName)
        => Window.Title = newName;

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Initialize the Textures of objects from MonoUtils
        Global.Initialize(Content);

        // Set all Textures for object.
        // As all kind of objects have the same texture it is saved static in the object.
        // The Texture are being forwarded through the constructor unless otherwise specified.
        MousePointer.DefaultTexture = Content.GetTexture("mousepoint");
        GlitchBlock.DefaultTexture = Content.GetTexture("glitch");
        Nbg.DefaultTexture = Content.GetTexture("NBG");

        // Settings
        Flag.DefaultTexture = Content.GetTexture("Flags");
        Dot.DefaultTexture = Content.GetTexture("Dot");

        // Level 12
        SmallTree.DefaultTexture = Content.GetTexture("OverworldTileSmallTree");
        BigTree.DefaultTexture = Content.GetTexture("OverworldTileBigTree");
        House.DefaultTexture = Content.GetTexture("OverworldTileHouse");
        Human.DefaultTexture = Content.GetTexture("OverworldTileHuman");
        Castle.DefaultTexture = Content.GetTexture("OverworldTileCastle");
        UserInterface.DefaultTexture = Content.GetTexture("LocationInterface");
        Resource.DefaultTexture = Content.GetTexture("Resources");
        Forest.DefaultTexture = Content.GetTexture("Forest");

        // Cache for sound effects as only one SoundEffect object is required.
        // Sound is played over SoundEffectInstance's which are created from the SoundEffect object.
        Global.SoundEffects.AddMusicToCache("TitleMusic", Content.GetMusic("NoNameTitleMusic"));
        Global.SoundEffects.AddSfxToCache("ButtonSound", Content.GetSfx("NoNameButtonSound"));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _display.Update();

        if (IsActive)
            _levelManager.Update(gameTime);

        if (InputReaderKeyboard.CheckKey(Keys.F10, true))
            _isConsoleActive = !_isConsoleActive;

        if (_isConsoleActive && _isConsoleEnabled)
            _console.Update(gameTime);

        // This will store the last key states
        InputReaderMouse.StoreButtonStates();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _levelManager.Draw(GraphicsDevice, _spriteBatch, spriteBatch =>
        {
            if (_isConsoleActive && _isConsoleEnabled)
                _console.Draw(spriteBatch);
        });
    }

    public void ApplySettings()
    {
        var settings = _settingsManager.GetSetting<VideoSettings>();
        var languageSettings = _settingsManager.GetSetting<LanguageSettings>();
        var advancedSettings = _settingsManager.GetSetting<AdvancedSettings>();

        ApplySettings(settings);

        TextProvider.Localization = languageSettings.Localization;
        _isConsoleEnabled = advancedSettings.ConsoleEnabled;
    }

    public void ApplySettings(VideoSettings settings)
    {
        ApplyResolution(settings.Resolution);
        ApplyFullscreen(settings.IsFullscreen);
        ApplyFixedStep(settings.IsFixedStep);
    }

    public void ApplyResolution(Resolution resolution)
    {
        _graphics.PreferredBackBufferWidth = resolution.Width;
        _graphics.PreferredBackBufferHeight = resolution.Height;
        _graphics.ApplyChanges();

        if (_console != null)
        {
            _console = new DevConsole(Global.CommandProcessor, _console.Position, _display.SimpleScale,
                _console);
            Log.Out.UpdateReference(_console);
        }

        _display?.Update();
    }

    public void ApplyFullscreen(bool fullscreen)
    {
        if (_graphics.IsFullScreen != fullscreen)
            _graphics.ToggleFullScreen();
    }

    public void ApplyFixedStep(bool fixedStep)
    {
        IsFixedTimeStep = fixedStep;
    }

    public void ApplyConsole(bool isEnabled)
        => _isConsoleEnabled = isEnabled;

    private void OnTextInput(object sender, TextInputEventArgs e)
    {
        if (_console is null)
            return;

        if (!_isConsoleActive)
            return;

        _console.TextInput(sender, e);
    }
}