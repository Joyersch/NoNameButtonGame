﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Sound;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Console;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Glitch;
using NoNameButtonGame.LevelSystem;
using CookieClickerFont = NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel.Font;
using NoNameButtonGame.LevelSystem.Settings;
using GeneralFont = NoNameButtonGame.LevelSystem.Font;

namespace NoNameButtonGame;

public class NoNameGame : SimpleGame
{
    private LevelManager _levelManager;
    private bool _showElapsedTime;
    private Text _elapsedTime;

    private LoopStation _loopStation;
    private EffectsRegistry _effectsRegistry;

    public NoNameGame()
    {
        IsMouseVisible = false;
        SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NoNameButtonGame/";
        SaveFile = 0;
        _loopStation = new LoopStation();
        _effectsRegistry = new EffectsRegistry();
    }

    protected override void Initialize()
    {
        base.Initialize();

        ApplySettings();

        // register soundSettingsListener to change sound volume if
        //Global.SoundSettingsListener = new SoundSettingsListener(SettingsManager.Settings);

        _elapsedTime = new Text(string.Empty, 3F);

        // get seed from arguments if it is given
        var seedText = Args.FirstOrDefault(s => s.StartsWith("--seed="), "--seed=NULL")[7..];
        if (!int.TryParse(seedText, out int seed))
            seed = Guid.NewGuid().GetHashCode();
        // contains start-menu, settings, credits and all other levels
        _levelManager = new LevelManager(Display, Window, SettingsAndSaveManager, this, _effectsRegistry, seed);
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

        CookieClickerFont.Texture = Content.GetTexture("Font/CookieClicker");
        GeneralFont.Texture = Content.GetTexture("Font/General");

        // Set all Textures for object.
        // As all kind of objects have the same texture it is saved static in the object.
        // The Texture are being forwarded through the constructor unless otherwise specified.
        MousePointer.Texture = Content.GetTexture("mousepoint");
        GlitchBlock.Texture = Content.GetTexture("glitch");
        Nbg.Texture = Content.GetTexture("NBG");

        // Settings
        Flag.Texture = Content.GetTexture("Flags");
        Dot.Texture = Content.GetTexture("Dot");

        // Select
        SelectButton.Texture = Content.GetTexture("minibutton");

        // Sound effects
        _effectsRegistry.Register(Content.GetSfx("wall"), "wall");

        // Music
        // _loopStation.Register(Content.GetMusic("name"), "name");
        _loopStation.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        var audio = SettingsAndSaveManager.GetSetting<AudioSettings>();
        _loopStation.SetMasterVolume(audio.MusicVolume);
        _effectsRegistry.SetMasterVolume(audio.SoundEffectVolume);
        _effectsRegistry.Update(gameTime);
        _loopStation.Update(gameTime);

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

        Console = new DevConsole(Global.CommandProcessor, Console.GetPosition(), Display.SimpleScale,
            Console);
        Log.Out.UpdateReference(Console);
    }

    public void ShowElapsedTime(bool show)
    {
        _showElapsedTime = show;
    }
}