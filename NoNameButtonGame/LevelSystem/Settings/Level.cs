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
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly NoNameGame _game;
    private readonly VideoSettings _videoSettings;
    private readonly AudioSettings _audioSettings;
    private readonly LanguageSettings _languageSettings;
    private readonly AdvancedSettings _advancedSettings;

    private readonly GameObject _anchorLeft;
    private readonly GameObject _anchorMiddle;
    private readonly GameObject _anchorRight;

    private Cursor _cursor;

    public event Action<Vector2> OnWindowResize;

    public event Action OnSave;
    public event Action OnDiscard;

    public event Action OnNameChange;

    private ManagmentCollection _advancedCollection;
    private ManagmentCollection _videoCollection;
    private ManagmentCollection _audioCollection;
    private ManagmentCollection _languageCollection;

    private TextButton _advancedButton;
    private TextButton _videoButton;
    private TextButton _audioButton;
    private TextButton _languageButton;

    private Text _consoleEnabledLabel;
    private Text _resolutionInfo;
    private Text _fixedStepLabel;
    private Text _elapsedTimeLabel;
    private Text _musicVolumeLabel;
    private Text _soundEffectVolumeLabel;
    private Text _saveChangesLabel;

    private TextComponent _textComponent;
    private Checkbox _consoleEnabled;
    private Checkbox _showElapsedTime;
    private Checkbox _fixedStep;
    private Checkbox _fullscreen;
    private ValueSelection _resolution;
    private Text _fullscreenLabel;

    private ValueSelection _musicVolume;
    private ValueSelection _soundEffectVolume;

    private MenuState _menuState;
    private bool _saveDialog;

    private TextButton _saveButton;
    private TextButton _discardButton;
    private Dot _highlight;

    private int _deleteButtonClicked = 0;
    private TextButton _deleteSave;
    private PulsatingRed _deleteColor;

    private enum MenuState
    {
        Video,
        Audio,
        Language,
        Advanced
    }

    public Level(Display display, Vector2 window, Random random, SettingsManager settings, NoNameGame game) : base(
        display,
        window, random)
    {
        _game = game;
        _advancedSettings = settings.GetSetting<AdvancedSettings>();
        _videoSettings = settings.GetSetting<VideoSettings>();
        _languageSettings = settings.GetSetting<LanguageSettings>();
        _audioSettings = settings.GetSetting<AudioSettings>();

        OnExit += delegate
        {
            if (_saveDialog)
            {
                OnDiscard?.Invoke();
            }
            else
                _saveDialog = true;
        };

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

        _videoCollection = new ManagmentCollection();
        _audioCollection = new ManagmentCollection();
        _languageCollection = new ManagmentCollection();
        _advancedCollection = new ManagmentCollection();

        _videoButton = new TextButton(string.Empty);
        _videoButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.2F)
            .Centered()
            .Move();
        _videoButton.Click += _ => _menuState = MenuState.Video;

        _audioButton = new TextButton(string.Empty);
        _audioButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.4F)
            .Centered()
            .Move();
        _audioButton.Click += _ => _menuState = MenuState.Audio;

        _languageButton = new TextButton(string.Empty);
        _languageButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.6F)
            .Centered()
            .Move();
        _languageButton.Click += _ => _menuState = MenuState.Language;

        _advancedButton = new TextButton(string.Empty);
        _advancedButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.8F)
            .Centered()
            .Move();
        _advancedButton.Click += _ => _menuState = MenuState.Advanced;

        #region Video

        _resolutionInfo = new Text(string.Empty);

        _videoCollection.Add(_resolutionInfo);

        List<object> resolutions = new List<object>()
        {
            new Resolution(1280, 720),
            new Resolution(1920, 1080),
            new Resolution(2560, 1440),
            new Resolution(3840, 2160),
        };

        var index = resolutions.IndexOf(resolutions.First(r =>
            ((Resolution)r).Width == _videoSettings.Resolution.Width));

        _resolution = new ValueSelection(Vector2.Zero, 1, resolutions, index);

        _resolution.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.4F)
            .Centered()
            .Move();

        _resolution.ValueChanged += delegate(object o)
        {
            var resolution = (Resolution)o;
            _videoSettings.Resolution = resolution;
            SetScreen(resolution.ToVector2());
            Log.WriteInformation($"Changed resolution to: {resolution}");
            OnWindowResize?.Invoke(Window);
            _game.ApplyResolution(resolution);
        };

        _videoCollection.Add(_resolution);

        _fixedStep = new Checkbox(_videoSettings.IsFixedStep);
        _fixedStep.GetAnchor(_resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(Checkbox.DefaultSize.Y / 4)
            .Move();

        _fixedStep.ValueChanged += delegate(bool value)
        {
            _videoSettings.IsFixedStep = value;
            _game.ApplyFixedStep(value);
        };

        _videoCollection.Add(_fixedStep);

        _fixedStepLabel = new Text(string.Empty);

        _videoCollection.Add(_fixedStepLabel);

        _fullscreen = new Checkbox(_videoSettings.IsFullscreen);
        _fullscreen.GetAnchor(_fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(Checkbox.DefaultSize.Y / 4)
            .Move();

        _fullscreen.ValueChanged += delegate(bool value)
        {
            _videoSettings.IsFullscreen = value;
            _game.ApplyFullscreen(value);
        };

        _videoCollection.Add(_fullscreen);

        _fullscreenLabel = new Text(string.Empty);

        _videoCollection.Add(_fullscreenLabel);

        #endregion // Video

        #region Audio

        List<object> volumeValues = new List<object>()
        {
            new Volume(0),
            new Volume(0.1F),
            new Volume(0.2F),
            new Volume(0.3F),
            new Volume(0.4F),
            new Volume(0.5F),
            new Volume(0.6F),
            new Volume(0.7F),
            new Volume(0.8F),
            new Volume(0.9F),
            new Volume(1F),
        };

        _musicVolume = new ValueSelection(Vector2.Zero, 1F, volumeValues, (int)(_audioSettings.MusicVolume * 10F));
        _musicVolume.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.4F)
            .Centered()
            .Move();
        _musicVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            _audioSettings.MusicVolume = v.Value;
        };
        _audioCollection.Add(_musicVolume);

        _musicVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_musicVolumeLabel);

        _soundEffectVolume = new ValueSelection(Vector2.Zero, 1F, volumeValues, (int)(_audioSettings.SoundEffectVolume * 10F));
        _soundEffectVolume.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Move();
        _soundEffectVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            _audioSettings.SoundEffectVolume = v.Value;
        };
        _audioCollection.Add(_soundEffectVolume);

        _soundEffectVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_soundEffectVolumeLabel);

        #endregion // Audio

        #region Language

        Flag flag = new Flag(TextProvider.Language.en_GB, 2.5F);
        flag.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.33F)
            .Centered()
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        flag = new Flag(TextProvider.Language.de_DE, 2.5F);
        flag.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.66F)
            .Centered()
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        #endregion // Language

        #region Advanced

        _consoleEnabled = new Checkbox(_advancedSettings.ConsoleEnabled);
        _consoleEnabled.ValueChanged += delegate(bool value)
        {
            _advancedSettings.ConsoleEnabled = value;
            _game.ApplyConsole(value);
        };
        _consoleEnabled.GetAnchor(_anchorLeft)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            //.SetDistanceY(4F)
            .Move();

        _advancedCollection.Add(_consoleEnabled);

        _consoleEnabledLabel = new Text(string.Empty);

        _advancedCollection.Add(_consoleEnabledLabel);


        _showElapsedTime = new Checkbox(_advancedSettings.ShowElapsedTime);
        _showElapsedTime.ValueChanged += (c) =>
        {
            _advancedSettings.ShowElapsedTime = c;
            _game.ShowElapsedTime(c);
        };
        _showElapsedTime.GetAnchor(_consoleEnabled)
            .SetMainAnchor(AnchorCalculator.Anchor.Bottom)
            .SetSubAnchor(AnchorCalculator.Anchor.Top)
            .SetDistanceY(8F)
            .Move();

        _advancedCollection.Add(_showElapsedTime);

        _elapsedTimeLabel = new Text(string.Empty);

        _advancedCollection.Add(_elapsedTimeLabel);

        _deleteSave = new TextButton(string.Empty);
        _deleteSave.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.7F)
            .Centered()
            .Move();

        var deleteSaveHold = new HoldButtonAddon(new ButtonAddonAdapter(_deleteSave), 5000F);
        deleteSaveHold.Callback += (o, state) =>
        {
            if (state != IButtonAddon.CallState.Click)
                return;

            settings.DeleteSave();
            _game.Exit();
        };

        _advancedCollection.Add(deleteSaveHold);

        _deleteColor = new PulsatingRed
        {
            GameTimeStepInterval = 14F,
            Increment = 1,
            NoGradient = true
        };

        AutoManaged.Add(_deleteColor);
        ColorListener.Add(_deleteColor, _deleteSave);

        #endregion // Advanced

        #region SaveSettings

        _saveChangesLabel = new Text(string.Empty);

        _saveButton = new TextButton(string.Empty);
        _saveButton.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.36F)
            .Centered()
            .Move();

        _saveButton.Click += delegate { OnSave?.Invoke(); };

        _discardButton = new TextButton(string.Empty);
        _discardButton.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.64F)
            .Centered()
            .Move();

        _discardButton.Click += delegate { OnDiscard?.Invoke(); };

        _highlight = new Dot(Camera.Rectangle.Location.ToVector2(), Camera.Rectangle.Size.ToVector2())
        {
            DrawColor = new Color(32, 32, 32, 240)
        };

        #endregion

        foreach (var f in _languageCollection.OfType<Flag>())
        {
            if (f.Language == _languageSettings.Localization)
                OnFlagClick(f); // Calls ApplyText
        }

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!_saveDialog)
        {
            _videoButton.UpdateInteraction(gameTime, _cursor);
            _videoButton.Update(gameTime);

            _audioButton.UpdateInteraction(gameTime, _cursor);
            _audioButton.Update(gameTime);

            _languageButton.UpdateInteraction(gameTime, _cursor);
            _languageButton.Update(gameTime);

            _advancedButton.UpdateInteraction(gameTime, _cursor);
            _advancedButton.Update(gameTime);

            _cursor.Update(gameTime);

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.MouseUp, true))
            {
                if (_menuState == MenuState.Advanced)
                    _menuState = MenuState.Video;
                else
                    _menuState++;
            }


            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.MouseDown, true))
            {
                if (_menuState == MenuState.Video)
                    _menuState = MenuState.Advanced;
                else
                    _menuState--;
            }

            // Does not need to be run every frame, only when a menu button is clicked
            UpdateButtonSelection();

            switch (_menuState)
            {
                case MenuState.Video:
                    _videoCollection.UpdateInteraction(gameTime, _cursor);
                    _videoCollection.Update(gameTime);
                    break;
                case MenuState.Audio:
                    _audioCollection.UpdateInteraction(gameTime, _cursor);
                    _audioCollection.Update(gameTime);
                    break;
                case MenuState.Language:
                    _languageCollection.UpdateInteraction(gameTime, _cursor);
                    _languageCollection.Update(gameTime);
                    break;
                case MenuState.Advanced:
                    _advancedCollection.UpdateInteraction(gameTime, _cursor);
                    _advancedCollection.Update(gameTime);
                    break;
            }

            return;
        }

        _cursor.Update(gameTime);
        _highlight.Update(gameTime);
        _saveChangesLabel.Update(gameTime);
        _saveButton.UpdateInteraction(gameTime, _cursor);
        _saveButton.Update(gameTime);
        _discardButton.UpdateInteraction(gameTime, _cursor);
        _discardButton.Update(gameTime);
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        _videoButton.Draw(spriteBatch);
        _audioButton.Draw(spriteBatch);
        _languageButton.Draw(spriteBatch);
        _advancedButton.Draw(spriteBatch);

        switch (_menuState)
        {
            case MenuState.Video:
                _videoCollection.Draw(spriteBatch);
                break;
            case MenuState.Audio:
                _audioCollection.Draw(spriteBatch);
                break;
            case MenuState.Language:
                _languageCollection.Draw(spriteBatch);
                break;
            case MenuState.Advanced:
                _advancedCollection.Draw(spriteBatch);
                break;
        }

        if (!_saveDialog)
        {
            _cursor.Draw(spriteBatch);
            return;
        }

        _highlight.Draw(spriteBatch);
        _saveButton.Draw(spriteBatch);
        _discardButton.Draw(spriteBatch);
        _saveChangesLabel.Draw(spriteBatch);
        _cursor.Draw(spriteBatch);
    }

    private void OnFlagClick(object sender)
    {
        float multiplier = 0.5F;
        foreach (Flag f in _languageCollection.OfType<Flag>())
            f.DrawColor = new Color(multiplier, multiplier, multiplier);

        Flag flag = (Flag)sender;

        flag.DrawColor = Color.White;
        _languageSettings.Localization = flag.Language;
        TextProvider.Localization = flag.Language;
        ApplyText();
    }

    private void UpdateButtonSelection()
    {
        _advancedButton.Text.ChangeColor(Color.Gray);
        _videoButton.Text.ChangeColor(Color.Gray);
        _audioButton.Text.ChangeColor(Color.Gray);
        _languageButton.Text.ChangeColor(Color.Gray);

        switch (_menuState)
        {
            case MenuState.Video:
                _videoButton.Text.ChangeColor(Color.White);
                break;
            case MenuState.Audio:
                _audioButton.Text.ChangeColor(Color.White);
                break;
            case MenuState.Language:
                _languageButton.Text.ChangeColor(Color.White);
                break;
            case MenuState.Advanced:
                _advancedButton.Text.ChangeColor(Color.White);
                break;
        }
    }

    private void ApplyText()
    {
        _textComponent = TextProvider.GetText("Levels.Settings");
        Name = _textComponent.GetValue("Name");
        OnNameChange?.Invoke();

        _videoButton.Text.ChangeText(_textComponent.GetValue("Video"));
        _audioButton.Text.ChangeText(_textComponent.GetValue("Audio"));
        _languageButton.Text.ChangeText(_textComponent.GetValue("Language"));
        _advancedButton.Text.ChangeText(_textComponent.GetValue("Advanced"));

        UpdateButtonSelection();

        _consoleEnabledLabel.ChangeText(_textComponent.GetValue("DevConsoleEnabled"));
        _consoleEnabledLabel.GetAnchor(_consoleEnabled)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .Move();

        _resolutionInfo.ChangeText(_textComponent.GetValue("Resolution"));
        _resolutionInfo.GetAnchor(_resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistanceY(4F)
            .Move();

        _fixedStepLabel.ChangeText(_textComponent.GetValue("FPSLimit"));
        _fixedStepLabel.GetAnchor(_fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(Checkbox.DefaultSize.Y / 2)
            .Move();

        _fullscreenLabel.ChangeText(_textComponent.GetValue("Fullscreen"));
        _fullscreenLabel.GetAnchor(_fullscreen)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(Checkbox.DefaultSize.Y / 2)
            .Move();

        _saveButton.Text.ChangeText(_textComponent.GetValue("Save"));
        _saveButton.Text.ChangeColor(Color.Green);
        _discardButton.Text.ChangeText(_textComponent.GetValue("Discard"));
        _discardButton.Text.ChangeColor(Color.Red);

        _elapsedTimeLabel.ChangeText(_textComponent.GetValue("ShowTotalGameTime"));
        _elapsedTimeLabel.GetAnchor(_showElapsedTime)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .Move();

        _deleteSave.Text.ChangeText(_textComponent.GetValue("DeleteSave"));

        _musicVolumeLabel.ChangeText(_textComponent.GetValue("MusicVolume"));
        _musicVolumeLabel.GetAnchor(_musicVolume)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistance(4F)
            .Move();

        _soundEffectVolumeLabel.ChangeText(_textComponent.GetValue("SoundEffectVolume"));
        _soundEffectVolumeLabel.GetAnchor(_soundEffectVolume)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistance(4F)
            .Move();

        _saveChangesLabel.ChangeText(_textComponent.GetValue("SaveChanges"));
        _saveChangesLabel.ChangeColor(Color.DeepSkyBlue);
        _saveChangesLabel.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.33F)
            .Centered()
            .Move();
    }
}