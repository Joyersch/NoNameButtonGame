using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
using System.Net;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LogicObjects.Listener;
using NoNameButtonGame.Storage;

namespace NoNameButtonGame;

public class NoNameGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Display.Display _display;
    private readonly Storage.Storage _storage;
    private LevelManager _levelManager;

    private bool _showActualMousePos;
    private MousePointer _mousePointer;

    public NoNameGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _storage = new Storage.Storage(Globals.SaveDirectory);

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // This will also call LoadContent()
        base.Initialize();

        // Read argument(s)
        _showActualMousePos = Environment.GetCommandLineArgs().Any(a => a == "-mp");

        // Check Save directory
        if (!Directory.Exists(Globals.SaveDirectory))
            Directory.CreateDirectory(Globals.SaveDirectory);

        // Load or generate settings file (as well as save)
        if (!_storage.Load())
            _storage.SetDefaults(); // fallback default settings
        
        _storage.Save();

        // apply settings and register Change Event for reapplying
        SettingsChanged(_storage.Settings, EventArgs.Empty);
        _storage.Settings.HasChanged += SettingsChanged;

        // register soundSettingsListener to change sound volume if 
        Globals.SoundSettingsListener = new SoundSettingsListener(_storage.Settings);

        _display = new(GraphicsDevice);

        // as OS mouse-pointer is disabled this is to show said position 
        _mousePointer = new MousePointer(Vector2.Zero, Vector2.Zero, _showActualMousePos);

        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(_display, _storage);
        _levelManager.ChangeWindowName += ChangeTitle;
        _levelManager.CloseGameEventHandler += Exit;
    }

    private void ChangeTitle(string newName)
        => Window.Title = newName;

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Set all Textures for object.
        // As all kind of objects have the same texture it is saved static in the object.
        // The Texture are being forwarded through the constructor unless otherwise specified.
        GameObject.DefaultTexture = Content.GetTexture("placeholder");
        Cursor.DefaultTexture = Content.GetTexture("cursor");
        MousePointer.DefaultTexture = Content.GetTexture("mousepoint");
        EmptyButton.DefaultTexture = Content.GetTexture("emptybutton");
        MiniTextButton.DefaultTexture = Content.GetTexture("minibutton");
        SquareTextButton.DefaultTexture = Content.GetTexture("squarebutton");
        GlitchBlock.DefaultTexture = Content.GetTexture("glitch");
        Letter.DefaultTexture = Content.GetTexture("font");

        // Cache for sound effects as only one SoundEffect object is required.
        // Sound is played over SoundEffectInstance's which are created from the SoundEffect object.
        Globals.SoundEffects.AddMusicToCache("TitleMusic", Content.GetMusic("NoNameTitleMusic"));
        Globals.SoundEffects.AddSfxToCache("ButtonSound", Content.GetSfx("NoNameButtonSound"));
        Globals.SoundEffects.AddSfxToCache("Talking", Content.GetSfx("NoNameButtonSound"));
        Globals.SoundEffects.AddSfxToCache("effect_3", Content.GetSfx("effect_3"));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

#if DEBUG
        // For outputting info while debugging
        Console.SetCursorPosition(0, 1);
#endif

        MouseState mouse = Mouse.GetState();
        _mousePointer.Update(gameTime, mouse.Position.ToVector2());

        _display.Update(gameTime);

        _levelManager.Update(gameTime);

        // This will store the last key states
        InputReaderMouse.StoreButtonStates();
    }

    protected override void Draw(GameTime gameTime)
    {
        // Display.Display always has the same size. Therefor if be draw to coordinates
        GraphicsDevice.SetRenderTarget(_display.Target);
        GraphicsDevice.Clear(new Color(50, 50, 50));

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: _levelManager.CurrentCamera.CameraMatrix);

        _levelManager.Draw(_spriteBatch);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        GraphicsDevice.Clear(Color.HotPink);

        _spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

        if (_showActualMousePos)
            _mousePointer.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void SettingsChanged(object obj, EventArgs e)
    {
        if (obj is not Settings settings)
            return;

        IsFixedTimeStep = settings.IsFixedStep;

        if ((!_graphics.IsFullScreen && settings.IsFullscreen) ||
            (!settings.IsFullscreen && _graphics.IsFullScreen))
            _graphics.ToggleFullScreen();

        _graphics.PreferredBackBufferWidth = settings.Resolution.Width;
        _graphics.PreferredBackBufferHeight = settings.Resolution.Height;
        _graphics.ApplyChanges();

        // Update level screen
        _storage.Save();
    }
}