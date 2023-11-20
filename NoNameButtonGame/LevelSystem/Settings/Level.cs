using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Logic.Text;
using MonoUtils.Settings;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly GeneralSettings _generalSettings;
    private readonly VideoSettings _videoSettings;
    private readonly LanguageSettings _languageSettings;

    private readonly GameObject _anchorLeft;
    private readonly GameObject _anchorMiddle;
    private readonly GameObject _anchorRight;

    private Cursor _cursor;

    public event Action<Vector2> OnWindowResize;
    public event Action OnSettingsChange;

    private ManagmentCollection _generalCollection;
    private ManagmentCollection _videoCollection;
    private ManagmentCollection _audioCollection;
    private ManagmentCollection _languageCollection;
    private ManagmentCollection _keybindsCollection;

    private TextButton _generalButton;
    private TextButton _videoButton;
    private TextButton _audioButton;
    private TextButton _languageButton;
    private TextButton _keybindsButton;

    private MenuState _menuState;

    private enum MenuState
    {
        General,
        Video,
        Audio,
        Language,
        Keybinds
    }

    public Level(Display display, Vector2 window, Random random, SettingsManager settings) : base(display,
        window, random)
    {
        var textComponent = TextProvider.GetText("Levels.Settings");
        Name = textComponent.GetValue("Name");

        _generalSettings = settings.GetSetting<GeneralSettings>();
        _videoSettings = settings.GetSetting<VideoSettings>();
        _languageSettings = settings.GetSetting<LanguageSettings>();

        _anchorLeft = new GameObject(Vector2.Zero, Vector2.One);
        _anchorLeft.GetCalculator(Camera.Rectangle)
            .OnX(1, 4)
            .OnY(0.3F)
            .Move();

        _anchorMiddle = new GameObject(Vector2.Zero, Vector2.One);
        _anchorMiddle.GetCalculator(Camera.Rectangle)
            .OnX(2, 4)
            .OnY(0.3F)
            .Move();

        _anchorRight = new GameObject(Vector2.Zero, Vector2.One);
        _anchorRight.GetCalculator(Camera.Rectangle)
            .OnX(3, 4)
            .OnY(0.3F)
            .Move();


        _generalCollection = new ManagmentCollection();
        _videoCollection = new ManagmentCollection();
        _audioCollection = new ManagmentCollection();
        _languageCollection = new ManagmentCollection();
        _keybindsCollection = new ManagmentCollection();

        _generalButton = new TextButton(textComponent.GetValue("General"));
        _generalButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.1F)
            .Centered()
            .Move();
        _generalButton.Click += _ => _menuState = MenuState.General;
        AutoManaged.Add(_generalButton);

        _videoButton = new TextButton(textComponent.GetValue("Video"));
        _videoButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.3F)
            .Centered()
            .Move();
        _videoButton.Click += _ => _menuState = MenuState.Video;
        AutoManaged.Add(_videoButton);

        _audioButton = new TextButton(textComponent.GetValue("Audio"));
        _audioButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.5F)
            .Centered()
            .Move();
        _audioButton.Click += _ => _menuState = MenuState.Audio;
        AutoManaged.Add(_audioButton);

        _languageButton = new TextButton(textComponent.GetValue("Language"));
        _languageButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.7F)
            .Centered()
            .Move();
        _languageButton.Click += _ => _menuState = MenuState.Language;
        AutoManaged.Add(_languageButton);

        _keybindsButton = new TextButton(textComponent.GetValue("Keybinds"));
        _keybindsButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.9F)
            .Centered()
            .Move();
        _keybindsButton.Click += _ => _menuState = MenuState.Keybinds;
        AutoManaged.Add(_keybindsButton);

        #region General

        var consoleEnabled = new Checkbox(_generalSettings.ConsoleEnabled);
        consoleEnabled.ValueChanged += delegate(bool value) { _generalSettings.ConsoleEnabled = value; };
        consoleEnabled.GetAnchor(_anchorLeft)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            //.SetDistanceY(4F)
            .Move();

        _generalCollection.Add(consoleEnabled);

        var consoleEnabledLabel = new Text(textComponent.GetValue("DevConsoleEnabled"));
        consoleEnabledLabel.GetAnchor(consoleEnabled)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .Move();

        _generalCollection.Add(consoleEnabledLabel);

        #endregion // General

        #region Video

        var resolutionInfo = new Text(textComponent.GetValue("Resolution"));
        resolutionInfo.GetAnchor(_anchorLeft)
            .SetMainAnchor(AnchorCalculator.Anchor.Bottom)
            .SetSubAnchor(AnchorCalculator.Anchor.Top)
            .SetDistanceY(4F)
            .Move();

        _videoCollection.Add(resolutionInfo);

        List<object> resolutions = new List<object>()
        {
            new Resolution(1280, 720),
            new Resolution(1920, 1080),
            new Resolution(2560, 1440),
            new Resolution(3840, 2160),
        };

        var index = resolutions.IndexOf(resolutions.First(r =>
            ((Resolution)r).Width == _videoSettings.Resolution.Width));

        var resolution = new ValueSelection(Vector2.Zero, 1, resolutions, index);
        var forCalucation = new Checkbox();

        forCalucation.GetAnchor(resolutionInfo)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceY(4F + forCalucation.Size.Y / 2)
            .Move();

        resolution.Move(forCalucation.Position);

        resolution.ValueChanged += delegate(object o)
        {
            var resolution = (Resolution)o;
            _videoSettings.Resolution = resolution;
            SetScreen(resolution.ToVector2());
            Log.WriteInformation($"Changed resolution to: {resolution}");
            OnWindowResize?.Invoke(Window);
            OnSettingsChange?.Invoke();
        };

        _videoCollection.Add(resolution);

        var fixedStep = new Checkbox(_videoSettings.IsFixedStep);
        fixedStep.GetAnchor(resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceY(4F + fixedStep.Size.Y / 2)
            .Move();

        fixedStep.ValueChanged += delegate(bool value)
        {
            _videoSettings.IsFixedStep = !value;
            OnSettingsChange?.Invoke();
        };

        _videoCollection.Add(fixedStep);

        var fixedStepLabel = new Text(textComponent.GetValue("FPSLimit"));
        fixedStepLabel.GetAnchor(fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .Move();

        _videoCollection.Add(fixedStepLabel);

        var fullscreen = new Checkbox(_videoSettings.IsFullscreen);
        fullscreen.GetAnchor(fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceY(4F + fullscreen.Size.Y / 2)
            .Move();

        fullscreen.ValueChanged += delegate(bool value)
        {
            _videoSettings.IsFullscreen = value;
            OnSettingsChange?.Invoke();
        };

        _videoCollection.Add(fullscreen);

        var fullscreenLabel = new Text(textComponent.GetValue("Fullscreen"));
        fullscreenLabel.GetAnchor(fullscreen)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .Move();

        _videoCollection.Add(fullscreenLabel);

        #endregion // Video

        #region Language

        Flag flag = new Flag(TextProvider.Language.en_US);
        flag.DrawColor *= 0.7F;
        flag.GetAnchor(_anchorLeft)
            .SetMainAnchor(AnchorCalculator.Anchor.Bottom)
            .SetSubAnchor(AnchorCalculator.Anchor.Top)
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        flag = new Flag(TextProvider.Language.de_DE);
        flag.DrawColor *= 0.7F;
        flag.GetAnchor(_anchorMiddle)
            .SetMainAnchor(AnchorCalculator.Anchor.Bottom)
            .SetSubAnchor(AnchorCalculator.Anchor.Top)
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        foreach (var f in _languageCollection.OfType<Flag>())
        {
            if (f.Language == _languageSettings.Localization)
                f.DrawColor = Color.White;
        }

        #endregion // Language

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _cursor.Update(gameTime);
        _generalButton.Text.ChangeColor(Color.Gray);
        _videoButton.Text.ChangeColor(Color.Gray);
        _audioButton.Text.ChangeColor(Color.Gray);
        _languageButton.Text.ChangeColor(Color.Gray);
        _keybindsButton.Text.ChangeColor(Color.Gray);

        switch (_menuState)
        {
            case MenuState.General:
                _generalButton.Text.ChangeColor(Color.White);
                _generalCollection.UpdateInteraction(gameTime, _cursor);
                _generalCollection.Update(gameTime);
                break;
            case MenuState.Video:
                _videoButton.Text.ChangeColor(Color.White);
                _videoCollection.UpdateInteraction(gameTime, _cursor);
                _videoCollection.Update(gameTime);
                break;
            case MenuState.Audio:
                _audioButton.Text.ChangeColor(Color.White);
                _audioCollection.UpdateInteraction(gameTime, _cursor);
                _audioCollection.Update(gameTime);
                break;
            case MenuState.Language:
                _languageButton.Text.ChangeColor(Color.White);
                _languageCollection.UpdateInteraction(gameTime, _cursor);
                _languageCollection.Update(gameTime);
                break;
            case MenuState.Keybinds:
                _keybindsButton.Text.ChangeColor(Color.White);
                _keybindsCollection.UpdateInteraction(gameTime, _cursor);
                _keybindsCollection.Update(gameTime);
                break;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        switch (_menuState)
        {
            case MenuState.General:
                _generalCollection.Draw(spriteBatch);
                break;
            case MenuState.Video:
                _videoCollection.Draw(spriteBatch);
                break;
            case MenuState.Audio:
                _audioCollection.Draw(spriteBatch);
                break;
            case MenuState.Language:
                _languageCollection.Draw(spriteBatch);
                break;
            case MenuState.Keybinds:
                _keybindsCollection.Draw(spriteBatch);
                break;
        }

        _cursor.Draw(spriteBatch);
    }

    private void OnFlagClick(object sender)
    {
        foreach (Flag f in _languageCollection.OfType<Flag>())
            f.DrawColor = Color.White * 0.7F;

        Flag flag = (Flag)sender;

        flag.DrawColor = Color.White;
        _languageSettings.Localization = flag.Language;
    }
}