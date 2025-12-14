using System;
using System.IO;
using System.Linq;
using Joyersch.Monogame.Logging;
using Joyersch.Monogame.Storage;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame;
using NoNameButtonGame.Console;
using NoNameButtonGame.Ui;
using NoNameButtonGame.Ui.Buttons;
using NoNameButtonGame.Ui.Text;

namespace Joyersch.Monogame;

public class ExtendedGame : Game
{
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch;

    protected Scene Scene;
    protected SettingsAndSaveManager<string> SettingsAndSaveManager;
    protected IFileFormatHandler? saveFormatHandler = null;
    protected IFileFormatHandler? settingsHandler = null;

    protected DevConsole Console;
    protected bool IsConsoleActive;
    protected bool IsConsoleEnabled;
    
    public static readonly CommandProcessor CommandProcessor = new();

    protected Vector2 ScreenSize = new(1280F, 720F);

    protected string SaveDirectory = "saves";
    protected string SaveFile = string.Empty;
    protected string SavePrefix = "save";
    protected string SaveType = "json";

    protected float CameraZoom = 1F;

    protected bool Debug;

    protected string[] Args;

    protected bool ConsoleVisible => IsConsoleActive && IsConsoleEnabled;

    public ExtendedGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        // This will also call LoadContent()
        base.Initialize();

        Args = Environment.GetCommandLineArgs();

        if (Args.Contains("--debug"))
            Debug = true;

        Scene = new Scene(GraphicsDevice, ScreenSize, CameraZoom);
        Window.TextInput += OnTextInput;

        CommandProcessor.Initialize();

        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        Console = new DevConsole(CommandProcessor, Scene, Console);
        Log.Out = new LogAdapter(Console);

        SettingsAndSaveManager = new SettingsAndSaveManager<string>(SaveDirectory, SaveFile, saveFormatHandler, settingsHandler);
        SettingsAndSaveManager.SetSaveFile(SaveFile);
        SettingsAndSaveManager.SaveFilePrefix = SavePrefix;
        SettingsAndSaveManager.SaveFileType = SaveType;

        if (!SettingsAndSaveManager.LoadSettings())
            SettingsAndSaveManager.SaveSettings();

        if (SaveFile is not null && !SettingsAndSaveManager.LoadSaves())
            SettingsAndSaveManager.SaveSave();
        SettingsAndSaveManager.Load();

        TextProvider.Initialize();
    }

    /// <summary>
    /// Please remember to call Scene.Update(gameTime)!
    /// </summary>
    /// <param name="gameTime"></param>
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsConsoleActive && IsConsoleEnabled)
            Console.Update(gameTime);

        MouseActionsMat.ResetState();
    }

    protected void DrawConsole()
        => DrawConsole(SpriteBatch);

    protected void DrawConsole(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        if (ConsoleVisible)
            Console.Draw(SpriteBatch);
        spriteBatch.End();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        // Initialize the Textures of objects from MonoUtils
        Global.Initialize(Content);
        
        SampleObject.Texture = Content.GetTexture("placeholder");
        Cursor.Texture = Content.GetTexture("cursor");

        DefaultLetters.Texture = Content.GetTexture("Font/DefaultLetters");
        ActionSymbols.Texture = Content.GetTexture("Font/ActionSymbols");
        ButtonAddonIcons.Texture = Content.GetTexture("Font/ButtonAddons");
        SampleButton.Texture = Content.GetTexture("Button/empty");
        SquareButton.Texture = Content.GetTexture("Button/square");
        RatioBox.Texture = Content.GetTexture("Button/ratio");
        Blank.Texture = Content.GetTexture("Dot");
        MousePointer.Texture = Content.GetTexture("mousepoint");

        Letter.Initialize();
    }

    protected void ToggleConsole()
    {
        if (IsConsoleEnabled)
            IsConsoleActive = !IsConsoleActive;
    }

    public void ApplyFullscreen(bool fullscreen)
    {
        // Workaround for wayland on linux
        Window.IsBorderless = fullscreen;
        Graphics.IsFullScreen = fullscreen;
        Graphics.ApplyChanges();
    }

    public void ApplyFixedStep(bool fixedStep)
    {
        IsFixedTimeStep = fixedStep;
    }

    public void ApplyConsole(bool isEnabled)
        => IsConsoleEnabled = isEnabled;

    private void OnTextInput(object sender, TextInputEventArgs e)
    {
        if (!IsConsoleEnabled)
            return;

        if (Console is null)
            return;

        if (!IsConsoleActive)
            return;

        Console.TextInput(sender, e);
    }
}