using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Input;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.Text;
using Display = NoNameButtonGame.Display;

namespace NoNameButtonGame;

public class NoNameGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Display.Display _display;
    private Storage _storage;
    private LevelManager levelManager;

    private bool ShowActualMousePos = false;
    private MousePointer _mousePointer;

    public NoNameGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _storage = new Storage(Globals.SaveDirectory);

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Read argument(s)
        ShowActualMousePos = Environment.GetCommandLineArgs().Any(a => a == "-mp");

        // Check Save directory
        if (!Directory.Exists(Globals.SaveDirectory))
            Directory.CreateDirectory(Globals.SaveDirectory);

        // Load or generate settings file (as well as save)
        try
        {
            _storage.Load();
        }
        catch
        {
            // fallback default settings
            _storage.Settings.Resolution.Width = 1280;
            _storage.Settings.Resolution.Height = 720;
            _storage.Settings.IsFullscreen = false;
            _storage.Settings.IsFixedStep = true;
            _storage.Save();
        }
        finally
        {
            SettingsChanged(_storage.Settings, EventArgs.Empty);
            _storage.Settings.HasChanged += SettingsChanged;
        }

        _display = new(GraphicsDevice);
        _mousePointer = new MousePointer(Vector2.Zero, Vector2.Zero, ShowActualMousePos);

        levelManager = new LevelManager(_display, _storage);
        levelManager.ChangeWindowName += ChangeTitle;
        levelManager.CloseGameEventHandler += Exit;
    }

    private void ChangeTitle(string newName)
    {
        Window.Title = newName;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        Mapping.AddMappingToCache(typeof(Cursor), Content.GetHitboxMapping("cursor"));
        Mapping.AddMappingToCache(typeof(MousePointer), Content.GetHitboxMapping("mousepoint"));
        Mapping.AddMappingToCache(typeof(EmptyButton), Content.GetHitboxMapping("emptybutton"));
        Mapping.AddMappingToCache(typeof(StartButton), Content.GetHitboxMapping("startbutton"));
        Mapping.AddMappingToCache(typeof(SelectButton), Content.GetHitboxMapping("selectbutton"));
        Mapping.AddMappingToCache(typeof(SettingsButton), Content.GetHitboxMapping("settingsbutton"));
        Mapping.AddMappingToCache(typeof(ExitButton), Content.GetHitboxMapping("exitbutton"));
        Mapping.AddMappingToCache(typeof(FailButton), Content.GetHitboxMapping("failbutton"));
        Mapping.AddMappingToCache(typeof(WinButton), Content.GetHitboxMapping("awesomebutton"));
        Mapping.AddMappingToCache(typeof(MiniTextButton), Content.GetHitboxMapping("minibutton"));
        Mapping.AddMappingToCache(typeof(Letter), Content.GetHitboxMapping("font"));
        Mapping.AddMappingToCache(typeof(GlitchBlock), Content.GetHitboxMapping("zonenew"));
        Mapping.AddMappingToCache(typeof(ButtonLock), Content.GetHitboxMapping("placeholder"));
        Mapping.AddMappingToCache(typeof(ButtonStateAddon), Content.GetHitboxMapping("placeholder"));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Console.SetCursorPosition(0,1);
        MouseState mouse = Mouse.GetState();
        _mousePointer.Update(gameTime, mouse.Position.ToVector2());

        _display.Update(gameTime);

        levelManager.Update(gameTime);
        // This will store the last key states
        InputReaderMouse.UpdateLast();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_display.Target);
        GraphicsDevice.Clear(new Color(50, 50, 50));

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: levelManager.CurrentCamera.CameraMatrix);

        levelManager.Draw(_spriteBatch);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        GraphicsDevice.Clear(Color.HotPink);

        _spriteBatch.Draw(_display.Target, _display.Window, null, Color.White);

        if (ShowActualMousePos)
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