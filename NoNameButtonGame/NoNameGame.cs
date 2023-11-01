using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Listener;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Console;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4;
using NoNameButtonGame.LevelSystem.LevelContainer.Level4.Overworld;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame;

public class NoNameGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Display _display;
    private Storage.Storage _storage;
    private LevelManager _levelManager;

    private DevConsole _console;

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

        _console = new DevConsole(Global.CommandProcessor, Window, Vector2.Zero, _display.SimpleScale);

        Global.CommandProcessor.Initialize();
        Log.Out = new LogAdapter(_console);

        // Check Save directory
        if (!Directory.Exists(Globals.SaveDirectory))
            Directory.CreateDirectory(Globals.SaveDirectory);

        SettingsManager.SetBasePath(Globals.SaveDirectory);
        SettingsManager.Add(new GameData());
        SettingsManager.Load();
        _storage = new Storage.Storage();
        _storage.Save();

        // apply settings and register Change Event for reapplying
        SettingsChanged(_storage.Settings, EventArgs.Empty);
        _storage.Settings.HasChanged += SettingsChanged;
        _storage.GameData.HasChanged += ProgressMade;

        // settings update windows size, so console will need to be recreated.
        // However loading settings could fail requiring an output for logging beforehand
        _display.Update();
        _console = new DevConsole(Global.CommandProcessor, Window, _console.Position, _display.SimpleScale,
            _console);
        Log.Out.UpdateReference(_console);
        TextProvider.Initialize();

        // register soundSettingsListener to change sound volume if
        Global.SoundSettingsListener = new SoundSettingsListener(_storage.Settings);

        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(_display, Window, _storage);
        _levelManager.ChangeWindowName += ChangeTitle;
        _levelManager.CloseGameEventHandler += Exit;

        _console.Context.RegisterContext(nameof(LevelManager), _levelManager);
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

        // Level 4
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

        _console.Update(gameTime);

        // This will store the last key states
        InputReaderMouse.StoreButtonStates();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _levelManager.Draw(GraphicsDevice, _spriteBatch, spriteBatch => { _console.Draw(spriteBatch); });
    }

    private void SettingsChanged(object obj, EventArgs e)
    {
        if (obj is not GeneralSettings settings)
            return;

        IsFixedTimeStep = settings.IsFixedStep;

        if ((!_graphics.IsFullScreen && settings.IsFullscreen) ||
            (!settings.IsFullscreen && _graphics.IsFullScreen))
            _graphics.ToggleFullScreen();

        _graphics.PreferredBackBufferWidth = settings.Resolution.Width;
        _graphics.PreferredBackBufferHeight = settings.Resolution.Height;

        _display?.Update();

        if (_console != null)
        {
            _console = new DevConsole(Global.CommandProcessor, Window, _console.Position, _display.SimpleScale,
                _console);
            Log.Out.UpdateReference(_console);
        }

        _graphics.ApplyChanges();

        // Update level screen
        _storage.Save();
    }

    private void ProgressMade(object sender, EventArgs e)
        => _storage.Save();
}