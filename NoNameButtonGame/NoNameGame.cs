using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Groups;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LogicObjects.Linker;
using NoNameButtonGame.Storage;
using NoNameButtonGame.Text;

namespace NoNameButtonGame;

public class NoNameGame : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Display.Display display;
    private readonly Storage.Storage storage;
    private LevelManager levelManager;

    private bool showActualMousePos;
    private MousePointer mousePointer;

    public NoNameGame()
    {
        graphics = new GraphicsDeviceManager(this);
        storage = new Storage.Storage(Globals.SaveDirectory);

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // This will also call LoadContent
        base.Initialize();

        // Read argument(s)
        showActualMousePos = Environment.GetCommandLineArgs().Any(a => a == "-mp");

        // Check Save directory
        if (!Directory.Exists(Globals.SaveDirectory))
            Directory.CreateDirectory(Globals.SaveDirectory);

        // Load or generate settings file (as well as save)
        try
        {
            storage.Load();
        }
        catch
        {
            // fallback default settings
            storage.Settings.Resolution.Width = 1280;
            storage.Settings.Resolution.Height = 720;
            storage.Settings.IsFullscreen = false;
            storage.Settings.IsFixedStep = true;
            storage.Save();
        }
        finally
        {
            SettingsChanged(storage.Settings, EventArgs.Empty);
            storage.Settings.HasChanged += SettingsChanged;
        }
        Globals.SoundSettingsLinker = new SoundSettingsLinker(storage.Settings);

        display = new(GraphicsDevice);
        mousePointer = new MousePointer(Vector2.Zero, Vector2.Zero, showActualMousePos);

        levelManager = new LevelManager(display, storage);
        levelManager.ChangeWindowName += ChangeTitle;
        levelManager.CloseGameEventHandler += Exit;
    }

    private void ChangeTitle(string newName)
    {
        Window.Title = newName;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        GameObject.DefaultTexture = Content.GetTexture("placeholder");
        Cursor.DefaultTexture = Content.GetTexture("cursor");
        MousePointer.DefaultTexture = Content.GetTexture("mousepoint");
        EmptyButton.DefaultTexture = Content.GetTexture("emptybutton");
        MiniTextButton.DefaultTexture = Content.GetTexture("minibutton");
        SquareTextButton.DefaultTexture = Content.GetTexture("squarebutton");
        GlitchBlock.DefaultTexture = Content.GetTexture("glitch");
        Letter.DefaultTexture = Content.GetTexture("font");
        
        Globals.SoundEffects.AddMappingToCache("TitleMusic", Content.GetMusic("NoNameTitleMusic"));
        Globals.SoundEffects.AddMappingToCache("ButtonSound", Content.GetSfx("NoNameButtonSound"));
        Globals.SoundEffects.AddMappingToCache("Talking", Content.GetSfx("NoNameButtonSound"));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Console.SetCursorPosition(0,1);
        MouseState mouse = Mouse.GetState();
        mousePointer.Update(gameTime, mouse.Position.ToVector2());

        display.Update(gameTime);

        levelManager.Update(gameTime);
        // This will store the last key states
        InputReaderMouse.UpdateLast();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(display.Target);
        GraphicsDevice.Clear(new Color(50, 50, 50));

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
            transformMatrix: levelManager.CurrentCamera.CameraMatrix);

        levelManager.Draw(spriteBatch);

        spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        GraphicsDevice.Clear(Color.HotPink);

        spriteBatch.Draw(display.Target, display.Window, null, Color.White);

        if (showActualMousePos)
            mousePointer.Draw(spriteBatch);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void SettingsChanged(object obj, EventArgs e)
    {
        if (obj is not Settings settings)
            return;

        IsFixedTimeStep = settings.IsFixedStep;

        if ((!graphics.IsFullScreen && settings.IsFullscreen) ||
            (!settings.IsFullscreen && graphics.IsFullScreen))
            graphics.ToggleFullScreen();

        graphics.PreferredBackBufferWidth = settings.Resolution.Width;
        graphics.PreferredBackBufferHeight = settings.Resolution.Height;
        graphics.ApplyChanges();

        // Update level screen
        storage.Save();
    }
}