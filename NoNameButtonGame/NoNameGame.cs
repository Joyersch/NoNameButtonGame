using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Console;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.Level12;
using NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;
using NoNameButtonGame.LevelSystem.Settings;
using Level11 = NoNameButtonGame.LevelSystem.LevelContainer.Level11;

namespace NoNameButtonGame;

public class NoNameGame : SimpleGame
{
    private LevelManager _levelManager;
    private bool _showElapsedTime;
    private Text _elapsedTime;

    public NoNameGame()
    {
        IsMouseVisible = false;
        SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NoNameButtonGame/";
        SaveFile = 0;
    }

    protected override void Initialize()
    {
        base.Initialize();

        ApplySettings();

        // register soundSettingsListener to change sound volume if
        //Global.SoundSettingsListener = new SoundSettingsListener(SettingsManager.Settings);

        _elapsedTime = new Text(string.Empty, 3F);

        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(Display, Window, SettingsAndSaveManager, this);
        _levelManager.ChangeTitle += ChangeTitle;
        _levelManager.CloseGame += Exit;

        // register context for console commands
        Console.Context.RegisterContext(nameof(LevelManager), _levelManager);
        Console.Context.RegisterContext(nameof(SettingsAndSaveManager), SettingsAndSaveManager);
    }

    private void ChangeTitle(string newName)
        => Window.Title = newName;

    protected override void LoadContent()
    {
        base.LoadContent();

        Level11.Font.DefaultTexture = Content.GetTexture("Font/Level11");

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
        if (IsActive)
        {
            _levelManager.Update(gameTime);
            if (_showElapsedTime)
            {

                _elapsedTime.ChangeText(gameTime.TotalGameTime.ToString());
                _elapsedTime.GetCalculator(Display.Window)
                    .ByGridY(1)
                    .BySizeY(-1F)
                    .Move();
            }

            _elapsedTime.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _levelManager.Draw(GraphicsDevice, SpriteBatch);
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
        if (IsConsoleActive && IsConsoleEnabled)
            Console.Draw(SpriteBatch);
        if (_showElapsedTime)
        {
            _elapsedTime.Draw(SpriteBatch);
        }
        SpriteBatch.End();
    }

    public void ApplySettings()
    {
        var settings = SettingsAndSaveManager.GetSetting<VideoSettings>();
        var languageSettings = SettingsAndSaveManager.GetSetting<LanguageSettings>();
        var advancedSettings = SettingsAndSaveManager.GetSetting<AdvancedSettings>();

        ApplySettings(settings);

        TextProvider.Localization = languageSettings.Localization;
        IsConsoleEnabled = advancedSettings.ConsoleEnabled;
        _showElapsedTime = advancedSettings.ShowElapsedTime;
    }

    public void ApplySettings(VideoSettings settings)
    {
        ApplyResolution(settings.Resolution);
        ApplyFullscreen(settings.IsFullscreen);
        ApplyFixedStep(settings.IsFixedStep);
    }

    public void ApplyResolution(Resolution resolution)
    {
        Graphics.PreferredBackBufferWidth = resolution.Width;
        Graphics.PreferredBackBufferHeight = resolution.Height;
        Graphics.ApplyChanges();

        Display.Update();

        Console = new DevConsole(Global.CommandProcessor, Console.Position, Display.SimpleScale,
            Console);
        Log.Out.UpdateReference(Console);
    }

    public void ShowElapsedTime(bool show)
    {
        _showElapsedTime = show;
    }
}