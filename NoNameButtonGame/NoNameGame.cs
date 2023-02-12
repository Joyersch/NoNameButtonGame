﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Empty;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Groups;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.Input;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LogicObjects;
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
        Globals.SoundSettingsLinker = new SoundSettingsLinker(_storage.Settings);

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
        
        Globals.Textures.AddMappingToCache(typeof(Cursor), Content.GetHitboxMapping("cursor"));
        Globals.Textures.AddMappingToCache(typeof(MousePointer), Content.GetHitboxMapping("mousepoint"));
        Globals.Textures.AddMappingToCache(typeof(EmptyButton), Content.GetHitboxMapping("emptybutton"));
        Globals.Textures.AddMappingToCache(typeof(StartButton), Content.GetHitboxMapping("startbutton"));
        Globals.Textures.AddMappingToCache(typeof(SelectButton), Content.GetHitboxMapping("selectbutton"));
        Globals.Textures.AddMappingToCache(typeof(SettingsButton), Content.GetHitboxMapping("settingsbutton"));
        Globals.Textures.AddMappingToCache(typeof(ExitButton), Content.GetHitboxMapping("exitbutton"));
        Globals.Textures.AddMappingToCache(typeof(FailButton), Content.GetHitboxMapping("failbutton"));
        Globals.Textures.AddMappingToCache(typeof(WinButton), Content.GetHitboxMapping("awesomebutton"));
        Globals.Textures.AddMappingToCache(typeof(MiniTextButton), Content.GetHitboxMapping("minibutton"));
        Globals.Textures.AddMappingToCache(typeof(Letter), Content.GetHitboxMapping("font"));
        Globals.Textures.AddMappingToCache(typeof(GlitchBlock), Content.GetHitboxMapping("zonenew"));
        Globals.Textures.AddMappingToCache(typeof(LockButtonAddon), Content.GetHitboxMapping("placeholder"));
        Globals.Textures.AddMappingToCache(typeof(CounterButtonAddon), Content.GetHitboxMapping("placeholder"));
        Globals.Textures.AddMappingToCache(typeof(MiniButton), Content.GetHitboxMapping("minibutton"));
        Globals.Textures.AddMappingToCache(typeof(SquareButton), Content.GetHitboxMapping("squarebutton"));
        Globals.Textures.AddMappingToCache(typeof(SquareTextButton), Content.GetHitboxMapping("squarebutton"));
        Globals.Textures.AddMappingToCache(typeof(ValueSelection), Content.GetHitboxMapping("placeholder"));
        Globals.SoundEffects.AddMappingToCache("TitleMusic", Content.GetMusic("NoNameTitleMusic"));
        Globals.SoundEffects.AddMappingToCache("ButtonSound", Content.GetSFX("NoNameButtonSound"));
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