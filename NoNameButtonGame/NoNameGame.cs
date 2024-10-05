using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Console;
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel;
using NoNameButtonGame.LevelSystem.Selection;
using CookieClickerFont = NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel.Font;
using NoNameButtonGame.LevelSystem.Settings;
using NoNameButtonGame.Music;
using Font = NoNameButtonGame.LevelSystem.MainMenu.Font;
using GeneralFont = NoNameButtonGame.LevelSystem.Font;

namespace NoNameButtonGame;

public sealed class NoNameGame : ExtentedGame
{
    private LevelManager _levelManager;
    private bool _showElapsedTime;
    private Text _elapsedTime;

    private readonly LoopStation _loopStation;
    private readonly EffectsRegistry _effectsRegistry;
    private bool _keyWasPressed;

    public NoNameGame()
    {
        IsMouseVisible = false;
        SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                        "/Joyersch/NoNameButtonGame/";
        SaveFile = string.Empty;
        _loopStation = new LoopStation();
        _effectsRegistry = new EffectsRegistry();
    }

    protected override void Initialize()
    {
        Text.DefaultLetterScale = 4F;
        DelayedText.DefaultScale = 4F;
        SquareTextButton.DefaultScale = 8F;
        SquareButton.DefaultScale = 8F;
        Checkbox.DefaultScale = 8F;
        SampleButton.DefaultScale = 8F;
        base.Initialize();

        ApplySettings();

        // This is to apply new settings to fix some bugs while upgrading to a newer version of the game
        var savedVersion = SettingsAndSaveManager.GetSetting<VersionSettings>();
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        if (savedVersion.Version < version)
        {
            savedVersion.Version = version;
            SettingsAndSaveManager.SaveSettings();
        }

        _elapsedTime = new Text(string.Empty, 1.5F * Text.DefaultLetterScale * Scene.Display.Scale / 2);

        // get seed from arguments if it is given
        var seedText = Args.FirstOrDefault(s => s.StartsWith("--seed="), "--seed=NULL")[7..];
        if (!int.TryParse(seedText, out var seed))
            seed = Guid.NewGuid().GetHashCode();

        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(Scene, SettingsAndSaveManager, this, _effectsRegistry, seed);
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

        // Fonts
        CookieClickerFont.Texture = Content.GetTexture("Font/CookieClicker");
        GeneralFont.Texture = Content.GetTexture("Font/General");
        Font.Texture = Content.GetTexture("Font/Progress");

        // Objects
        MousePointer.Texture = Content.GetTexture("mousepoint");
        GlitchBlock.Texture = Content.GetTexture("glitch");
        Nbg.Texture = Content.GetTexture("NBG");

        // Settings
        Flag.Texture = Content.GetTexture("Flags");
        Dot.Texture = Content.GetTexture("Dot");

        // Select
        SelectButton.Texture = Content.GetTexture("minibutton");
        Showcase.Texture[0] = Content.GetTexture("Showcases/Level0");
        Showcase.Texture[1] = Content.GetTexture("Showcases/Level1");
        Showcase.Texture[2] = Content.GetTexture("Showcases/Level2");
        Showcase.Texture[3] = Content.GetTexture("Showcases/Level3");
        Showcase.Texture[4] = Content.GetTexture("Showcases/Level4");
        Showcase.Texture[5] = Content.GetTexture("Showcases/Level5");
        Showcase.Texture[6] = Content.GetTexture("Showcases/Level6");
        Showcase.Texture[7] = Content.GetTexture("Showcases/Level7");
        Showcase.Texture[8] = Content.GetTexture("Showcases/Level8");
        Showcase.Texture[9] = Content.GetTexture("Showcases/Level9");
        Showcase.Texture[10] = Content.GetTexture("Showcases/Level10");

        // Sound effects
        RegisterEffect(Statics.Sfx.Notes.C);
        RegisterEffect(Statics.Sfx.Notes.D);
        RegisterEffect(Statics.Sfx.Notes.E);
        RegisterEffect(Statics.Sfx.Notes.F);
        RegisterEffect(Statics.Sfx.Notes.G);
        RegisterEffect(Statics.Sfx.Wall);

        // Music
        RegisterToStation(Statics.Music.Melody.Main);
        RegisterToStation(Statics.Music.Melody.Main2);
        RegisterToStation(Statics.Music.Melody.LoFiMain);
        RegisterToStation(Statics.Music.Melody.LoFiMain2);
        RegisterToStation(Statics.Music.Melody.LoFiMainMuffled);
        RegisterToStation(Statics.Music.Melody.LoFiMain2Muffled);
        RegisterToStation(Statics.Music.Ride.Memphis);
        RegisterToStation(Statics.Music.Percussion.Memphis);
        RegisterToStation(Statics.Music.Lead.Trance);
        RegisterToStation(Statics.Music.Lead.DnB);
        RegisterToStation(Statics.Music.Kickdrum.Trance);
        RegisterToStation(Statics.Music.Drums.Trap);
        RegisterToStation(Statics.Music.Drums.Trance);
        RegisterToStation(Statics.Music.Drums.Synthwave);
        RegisterToStation(Statics.Music.Drums.Memphis);
        RegisterToStation(Statics.Music.Drums.DnB);
        RegisterToStation(Statics.Music.Drums.LoFi);
        RegisterToStation(Statics.Music.Drums.LoFiMuffled);
        RegisterToStation(Statics.Music.Bass.Trap);
        RegisterToStation(Statics.Music.Bass.Trance);
        RegisterToStation(Statics.Music.Bass.SynthwaveDnB);
        RegisterToStation(Statics.Music.Bass.Memphis);
        RegisterToStation(Statics.Music.Bass.LoFi);
        RegisterToStation(Statics.Music.Bass.LoFiMuffled);

        Lofi.Initialize(_loopStation);
        LofiMuffled.Initialize(_loopStation);
        Memphis.Initialize(_loopStation);
        Default.Initialize(_loopStation);
        Default2.Initialize(_loopStation);
        Default3.Initialize(_loopStation);
        DnB.Initialize(_loopStation);
        DnB2.Initialize(_loopStation);
        DnB3.Initialize(_loopStation);
        DnB4.Initialize(_loopStation);
        Synthwave.Initialize(_loopStation);
        Trap.Initialize(_loopStation);
        Trap2.Initialize(_loopStation);
        Trance.Initialize(_loopStation);
        None.Initialize(_loopStation);

        _loopStation.Initialize(250F);
    }

    private void RegisterToStation(string name)
        => _loopStation?.Register(Content.GetMusic(name), name);
    
    private void RegisterEffect(string name)
        => _effectsRegistry?.Register(Content.GetSfx(name), name);

    protected override void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            Scene.Update(gameTime);
            _levelManager.Update(gameTime);
            if (_showElapsedTime)
            {
                _elapsedTime.ChangeText(gameTime.TotalGameTime.ToString());
                _elapsedTime.InRectangle(Scene.Display)
                    .ByGridY(1)
                    .BySizeY(-1F)
                    .Apply();
            }

            var keyboardState = Keyboard.GetState();
            var keyIsPressed = keyboardState[Keys.F10] == KeyState.Down;
            if (!_keyWasPressed && keyIsPressed)
                ToggleConsole();

            _keyWasPressed = keyIsPressed;

            _elapsedTime.Update(gameTime);
        }

        var audio = SettingsAndSaveManager.GetSetting<AudioSettings>();
        _loopStation.SetMasterVolume(audio.MusicVolume);
        _effectsRegistry.SetMasterVolume(audio.SoundEffectVolume);
        _effectsRegistry.Update(gameTime);
        _loopStation.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        _levelManager.Draw(GraphicsDevice, SpriteBatch);
        DrawConsole();
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
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

        if (settings.Resolution is null)
        {
            settings.Resolution = VideoSettings.Resolutions
                .OrderBy(r => r.Width < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                .First();
            SettingsAndSaveManager.SaveSettings();
        }

        ApplySettings(settings);

        TextProvider.Localization = languageSettings.Localization;
        IsConsoleEnabled = advancedSettings.ConsoleEnabled;
        _showElapsedTime = advancedSettings.ShowElapsedTime;
    }

    private void ApplySettings(VideoSettings settings)
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

        Scene.Display.Update();

        Console = new DevConsole(Global.CommandProcessor, Console.GetPosition(), Scene.Display.Scale,
            Console);
        Log.Out.UpdateReference(Console);

        _elapsedTime = new Text(string.Empty, 1.5F * Text.DefaultLetterScale * Scene.Display.Scale / 2);
    }

    public void ShowElapsedTime(bool show)
    {
        _showElapsedTime = show;
    }
}