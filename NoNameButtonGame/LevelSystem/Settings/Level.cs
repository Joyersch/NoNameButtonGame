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
using MonoUtils.Sound;
using MonoUtils.Ui;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly NoNameGame _game;
    private readonly VideoSettings _videoSettings;
    private readonly AudioSettings _audioSettings;
    private readonly LanguageSettings _languageSettings;
    private readonly AdvancedSettings _advancedSettings;

    private readonly SampleObject _anchorLeft;
    private readonly SampleObject _anchorMiddle;
    private readonly SampleObject _anchorRight;

    public event Action<Vector2> OnWindowResize;

    public event Action OnSave;
    public event Action OnDiscard;

    public event Action OnNameChange;

    private ManagementCollection _advancedCollection;
    private ManagementCollection _videoCollection;
    private ManagementCollection _audioCollection;
    private ManagementCollection _languageCollection;

    private Button _advancedButton;
    private Button _videoButton;
    private Button _audioButton;
    private Button _languageButton;

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
    private ValueSelection<Resolution> _resolution;
    private Text _fullscreenLabel;

    private ValueSelection<Volume> _musicVolume;
    private ValueSelection<Volume> _soundEffectVolume;

    private MenuState _menuState;
    private bool _saveDialog;

    private Button _saveButton;
    private Button _discardButton;
    private Dot _highlight;

    private int _deleteButtonClicked = 0;
    private Button _deleteSave;
    private PulsatingRed _deleteColor;

    private enum MenuState
    {
        Video,
        Audio,
        Language,
        Advanced
    }

    public Level(Display display, Vector2 window, Random random, SettingsAndSaveManager settingsAndSave,
        NoNameGame game, EffectsRegistry effectsRegistry) : base(
        display,
        window, random, effectsRegistry)
    {
        _game = game;
        _advancedSettings = settingsAndSave.GetSetting<AdvancedSettings>();
        _videoSettings = settingsAndSave.GetSetting<VideoSettings>();
        _languageSettings = settingsAndSave.GetSetting<LanguageSettings>();
        _audioSettings = settingsAndSave.GetSetting<AudioSettings>();

        OnExit += delegate
        {
            if (_saveDialog)
            {
                OnDiscard?.Invoke();
            }
            else
                _saveDialog = true;
        };

        _anchorLeft = new SampleObject(Vector2.Zero, Vector2.One);
        _anchorLeft.GetCalculator(Camera.Rectangle)
            .OnX(1, 4)
            .OnY(0.3F)
            .Move();

        _anchorMiddle = new SampleObject(Vector2.Zero, Vector2.One);
        _anchorMiddle.GetCalculator(Camera.Rectangle)
            .OnX(2, 4)
            .OnY(0.3F)
            .Move();

        _anchorRight = new SampleObject(Vector2.Zero, Vector2.One);
        _anchorRight.GetCalculator(Camera.Rectangle)
            .OnX(3, 4)
            .OnY(0.3F)
            .Move();

        _videoCollection = new ManagementCollection();
        _audioCollection = new ManagementCollection();
        _languageCollection = new ManagementCollection();
        _advancedCollection = new ManagementCollection();

        _videoButton = new Button(string.Empty);
        _videoButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.2F)
            .Centered()
            .Move();
        _videoButton.Click += _ => _menuState = MenuState.Video;

        _audioButton = new Button(string.Empty);
        _audioButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.4F)
            .Centered()
            .Move();
        _audioButton.Click += _ => _menuState = MenuState.Audio;

        _languageButton = new Button(string.Empty);
        _languageButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.6F)
            .Centered()
            .Move();
        _languageButton.Click += _ => _menuState = MenuState.Language;

        _advancedButton = new Button(string.Empty);
        _advancedButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.8F)
            .Centered()
            .Move();
        _advancedButton.Click += _ => _menuState = MenuState.Advanced;

        #region Video

        _resolutionInfo = new Text(string.Empty);

        _videoCollection.Add(_resolutionInfo);

        List<Resolution> resolutions = new List<Resolution>()
        {
            new Resolution(1280, 720),
            new Resolution(1920, 1080),
            new Resolution(2560, 1440),
            new Resolution(3840, 2160),
        };

        var index = resolutions.IndexOf(resolutions.First(r =>
            ((Resolution)r).Width == _videoSettings.Resolution.Width));

        _resolution = new ValueSelection<Resolution>(Vector2.Zero, 1, resolutions, index);

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
            .SetDistanceY(8)
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
            .SetDistanceY(8F)
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

        List<Volume> volumeValues = new List<Volume>()
        {
            new Volume(0),
            new Volume(0.01F),
            new Volume(0.02F),
            new Volume(0.03F),
            new Volume(0.04F),
            new Volume(0.05F),
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

        Volume currentVolume = volumeValues.First(i => i.Value == _audioSettings.MusicVolume);
        _musicVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
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

        currentVolume = volumeValues.First(i => i.Value == _audioSettings.SoundEffectVolume);
        _soundEffectVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
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
            .SetDistanceY(8)
            .Move();

        _advancedCollection.Add(_showElapsedTime);

        _elapsedTimeLabel = new Text(string.Empty);

        _advancedCollection.Add(_elapsedTimeLabel);

        _deleteSave = new Button(string.Empty);
        _deleteSave.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.7F)
            .Centered()
            .Move();


        var deleteSaveHold = new HoldButtonAddon(_deleteSave, 5000F);
        deleteSaveHold.Click += delegate
        {
            settingsAndSave.DeleteSave();
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
        ColorListener.Add(_deleteColor, _deleteSave.Text);

        #endregion // Advanced

        #region SaveSettings

        _saveChangesLabel = new Text(string.Empty);

        _saveButton = new Button(string.Empty);
        _saveButton.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.36F)
            .Centered()
            .Move();

        _saveButton.Click += delegate { OnSave?.Invoke(); };

        _discardButton = new Button(string.Empty);
        _discardButton.GetCalculator(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.64F)
            .Centered()
            .Move();

        _discardButton.Click += delegate { OnDiscard?.Invoke(); };

        _highlight = new Dot(Camera.Rectangle.Location.ToVector2(), Camera.Rectangle.Size.ToVector2())
        {
            //DrawColor = new Color(32, 32, 32, 240)
        };

        #endregion

        foreach (var f in _languageCollection.OfType<Flag>())
        {
            if (f.Language == _languageSettings.Localization)
                OnFlagClick(f); // Calls ApplyText
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!_saveDialog)
        {
            _videoButton.UpdateInteraction(gameTime, Cursor);
            _videoButton.Update(gameTime);

            _audioButton.UpdateInteraction(gameTime, Cursor);
            _audioButton.Update(gameTime);

            _languageButton.UpdateInteraction(gameTime, Cursor);
            _languageButton.Update(gameTime);

            _advancedButton.UpdateInteraction(gameTime, Cursor);
            _advancedButton.Update(gameTime);

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
                    _videoCollection.UpdateInteraction(gameTime, Cursor);
                    _videoCollection.Update(gameTime);
                    break;
                case MenuState.Audio:
                    _audioCollection.UpdateInteraction(gameTime, Cursor);
                    _audioCollection.Update(gameTime);
                    break;
                case MenuState.Language:
                    _languageCollection.UpdateInteraction(gameTime, Cursor);
                    _languageCollection.Update(gameTime);
                    break;
                case MenuState.Advanced:
                    _advancedCollection.UpdateInteraction(gameTime, Cursor);
                    _advancedCollection.Update(gameTime);
                    break;
            }

            return;
        }

        _highlight.Update(gameTime);
        _saveChangesLabel.Update(gameTime);
        _saveButton.UpdateInteraction(gameTime, Cursor);
        _saveButton.Update(gameTime);
        _discardButton.UpdateInteraction(gameTime, Cursor);
        _discardButton.Update(gameTime);
    }

    protected override void Draw(SpriteBatch spriteBatch)
    {
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

        if (_saveDialog)
        {
            _highlight.Draw(spriteBatch);
            _saveButton.Draw(spriteBatch);
            _discardButton.Draw(spriteBatch);
            _saveChangesLabel.Draw(spriteBatch);
        }

        base.Draw(spriteBatch);
    }

    private void OnFlagClick(object sender)
    {
        float multiplier = 0.5F;
        foreach (Flag f in _languageCollection.OfType<Flag>())
            f.ChangeColor(new[] { new Color(multiplier, multiplier, multiplier) });

        Flag flag = (Flag)sender;

        flag.ChangeColor(new[] { Color.White });
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
            .SetDistanceX(4F)
            .Move();

        _fullscreenLabel.ChangeText(_textComponent.GetValue("Fullscreen"));
        _fullscreenLabel.GetAnchor(_fullscreen)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
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