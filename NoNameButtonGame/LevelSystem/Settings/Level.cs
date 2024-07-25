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
using MonoUtils.Ui.Buttons;
using MonoUtils.Ui.Buttons.AddOn;
using MonoUtils.Ui.TextSystem;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Music;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly LanguageSettings _languageSettings;

    public event Action<Vector2> OnWindowResize;

    public event Action OnSave;
    public event Action OnDiscard;

    public event Action OnNameChange;

    private ManagementCollection _advancedCollection;
    private ManagementCollection _videoCollection;
    private ManagementCollection _audioCollection;
    private ManagementCollection _mouseCollection;
    private ManagementCollection _languageCollection;

    private Button _advancedButton;
    private Button _videoButton;
    private Button _audioButton;
    private Button _mouseButton;
    private Button _languageButton;

    private Text _consoleEnabledLabel;
    private Text _resolutionInfo;
    private Text _fixedStepLabel;
    private Text _elapsedTimeLabel;
    private Text _musicVolumeLabel;
    private Text _soundEffectVolumeLabel;
    private Text _saveChangesLabel;
    private Text _sensLabel;
    private Text _fullscreenLabel;

    private TextComponent _textComponent;
    private Checkbox _consoleEnabled;
    private Checkbox _showElapsedTime;
    private Checkbox _fixedStep;
    private Checkbox _fullscreen;
    private ValueSelection<Resolution> _resolution;

    private ValueSelection<Volume> _musicVolume;
    private ValueSelection<Volume> _soundEffectVolume;
    private ValueSelection<float> _sens;

    private MenuState _menuState;
    private bool _saveDialog;

    private Button _saveButton;
    private Button _discardButton;
    private Dot _highlight;

    private Button _deleteSave;

    private enum MenuState
    {
        Video,
        Audio,
        Mouse,
        Language,
        Advanced
    }

    public Level(Display display, Vector2 window, Random random, SettingsAndSaveManager settingsAndSave,
        NoNameGame game, EffectsRegistry effectsRegistry) : base(display, window, random, effectsRegistry,
        settingsAndSave)
    {
        var game1 = game;
        var advancedSettings = settingsAndSave.GetSetting<AdvancedSettings>();
        var videoSettings = settingsAndSave.GetSetting<VideoSettings>();
        _languageSettings = settingsAndSave.GetSetting<LanguageSettings>();
        var audioSettings = settingsAndSave.GetSetting<AudioSettings>();
        var mouseSettings = settingsAndSave.GetSetting<MouseSettings>();

        Default.Play();

        OnExit += delegate
        {
            if (_saveDialog)
            {
                OnDiscard?.Invoke();
            }
            else
                _saveDialog = true;
        };

        var anchorLeft = new SampleObject(Vector2.Zero, Vector2.One);
        anchorLeft.InRectangle(Camera.Rectangle)
            .OnX(1, 4)
            .OnY(0.3F)
            .Move();

        var anchorMiddle = new SampleObject(Vector2.Zero, Vector2.One);
        anchorMiddle.InRectangle(Camera.Rectangle)
            .OnX(2, 4)
            .OnY(0.3F)
            .Move();

        var anchorRight = new SampleObject(Vector2.Zero, Vector2.One);
        anchorRight.InRectangle(Camera.Rectangle)
            .OnX(3, 4)
            .OnY(0.3F)
            .Move();

        _videoCollection = new ManagementCollection();
        _audioCollection = new ManagementCollection();
        _mouseCollection = new ManagementCollection();
        _languageCollection = new ManagementCollection();
        _advancedCollection = new ManagementCollection();

        _videoButton = new Button(string.Empty);
        _videoButton.InRectangle(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.1F)
            .Centered()
            .Move();
        _videoButton.Click += _ => _menuState = MenuState.Video;

        _audioButton = new Button(string.Empty);
        _audioButton.InRectangle(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.3F)
            .Centered()
            .Move();
        _audioButton.Click += _ => _menuState = MenuState.Audio;

        _mouseButton = new Button(string.Empty);
        _mouseButton.InRectangle(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.5F)
            .Centered()
            .Move();
        _mouseButton.Click += _ => _menuState = MenuState.Mouse;

        _languageButton = new Button(string.Empty);
        _languageButton.InRectangle(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.7F)
            .Centered()
            .Move();
        _languageButton.Click += _ => _menuState = MenuState.Language;

        _advancedButton = new Button(string.Empty);
        _advancedButton.InRectangle(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.9F)
            .Centered()
            .Move();
        _advancedButton.Click += _ => _menuState = MenuState.Advanced;

        #region Video

        _resolutionInfo = new Text(string.Empty);

        _videoCollection.Add(_resolutionInfo);

        var index = VideoSettings.Resolutions.IndexOf(VideoSettings.Resolutions.First(r =>
            r.Width == videoSettings.Resolution.Width));

        _resolution = new ValueSelection<Resolution>(Vector2.Zero, 1, VideoSettings.Resolutions, index);

        _resolution.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.4F)
            .Centered()
            .Move();

        _resolution.ValueChanged += delegate(object o)
        {
            var resolution = (Resolution)o;
            videoSettings.Resolution = resolution;
            SetScreen(resolution.ToVector2());
            Log.Information($"Changed resolution to: {resolution}");
            OnWindowResize?.Invoke(Window);
            game1.ApplyResolution(resolution);
        };

        _videoCollection.Add(_resolution);

        _fixedStep = new Checkbox(videoSettings.IsFixedStep);
        _fixedStep.GetAnchor(_resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(8)
            .Move();

        _fixedStep.ValueChanged += delegate(bool value)
        {
            videoSettings.IsFixedStep = value;
            game1.ApplyFixedStep(value);
        };

        _videoCollection.Add(_fixedStep);

        _fixedStepLabel = new Text(string.Empty);

        _videoCollection.Add(_fixedStepLabel);

        _fullscreen = new Checkbox(videoSettings.IsFullscreen);
        _fullscreen.GetAnchor(_fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(8F)
            .Move();

        _fullscreen.ValueChanged += delegate(bool value)
        {
            videoSettings.IsFullscreen = value;
            game1.ApplyFullscreen(value);
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

        Volume currentVolume = volumeValues.First(i => i.Value == audioSettings.MusicVolume);
        _musicVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
        _musicVolume.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.4F)
            .Centered()
            .Move();
        _musicVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            audioSettings.MusicVolume = v.Value;
        };
        _audioCollection.Add(_musicVolume);

        _musicVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_musicVolumeLabel);

        currentVolume = volumeValues.First(i => i.Value == audioSettings.SoundEffectVolume);
        _soundEffectVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
        _soundEffectVolume.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.6F)
            .Centered()
            .Move();
        _soundEffectVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            audioSettings.SoundEffectVolume = v.Value;
        };
        _audioCollection.Add(_soundEffectVolume);

        _soundEffectVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_soundEffectVolumeLabel);

        #endregion // Audio

        #region Mouse

        List<float> values = new List<float>()
        {
            0.1F,
            0.2F,
            0.3F,
            0.4F,
            0.5F,
            0.6F,
            0.7F,
            0.8F,
            0.9F,
            1F,
        };
        _sens = new ValueSelection<float>(Vector2.Zero, 1F, values, values.IndexOf(mouseSettings.Sensitivity));
        _sens.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.4F)
            .Centered()
            .Move();
        _sens.ValueChanged += delegate(object o) { mouseSettings.Sensitivity = (float)o; };
        _mouseCollection.Add(_sens);

        _sensLabel = new Text(string.Empty);
        _sensLabel.GetAnchor(_sens)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistanceY(4F)
            .Move();
        _mouseCollection.Add(_sensLabel);

        #endregion // Mouse

        #region Language

        Flag flag = new Flag(TextProvider.Language.en_GB, 2.5F);
        flag.InRectangle(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.33F)
            .Centered()
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        flag = new Flag(TextProvider.Language.de_DE, 2.5F);
        flag.InRectangle(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.66F)
            .Centered()
            .Move();

        flag.Click += OnFlagClick;

        _languageCollection.Add(flag);

        #endregion // Language

        #region Advanced

        _consoleEnabled = new Checkbox(advancedSettings.ConsoleEnabled);
        _consoleEnabled.ValueChanged += delegate(bool value)
        {
            advancedSettings.ConsoleEnabled = value;
            game1.ApplyConsole(value);
        };
        _consoleEnabled.GetAnchor(anchorLeft)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            //.SetDistanceY(4F)
            .Move();

        _advancedCollection.Add(_consoleEnabled);

        _consoleEnabledLabel = new Text(string.Empty);

        _advancedCollection.Add(_consoleEnabledLabel);


        _showElapsedTime = new Checkbox(advancedSettings.ShowElapsedTime);
        _showElapsedTime.ValueChanged += (c) =>
        {
            advancedSettings.ShowElapsedTime = c;
            game1.ShowElapsedTime(c);
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
        _deleteSave.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.7F)
            .Centered()
            .Move();


        var deleteSaveHold = new HoldButtonAddon(_deleteSave, 5000F);
        deleteSaveHold.Click += delegate
        {
            settingsAndSave.DeleteSave();
            game1.Exit();
        };

        _advancedCollection.Add(deleteSaveHold);


        var deleteColor = new PulsatingRed
        {
            GameTimeStepInterval = 14F,
            Increment = 1,
            NoGradient = true
        };

        AutoManaged.Add(deleteColor);
        ColorListener.Add(deleteColor, _deleteSave);
        ColorListener.Add(deleteColor, _deleteSave.Text);

        #endregion // Advanced

        #region SaveSettings

        _saveChangesLabel = new Text(string.Empty);

        _saveButton = new Button(string.Empty);
        _saveButton.InRectangle(Camera.Rectangle)
            .OnY(0.55F)
            .OnX(0.36F)
            .Centered()
            .Move();

        _saveButton.Click += delegate { OnSave?.Invoke(); };

        _discardButton = new Button(string.Empty);
        _discardButton.InRectangle(Camera.Rectangle)
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

            _mouseButton.UpdateInteraction(gameTime, Cursor);
            _mouseButton.Update(gameTime);

            _languageButton.UpdateInteraction(gameTime, Cursor);
            _languageButton.Update(gameTime);

            _advancedButton.UpdateInteraction(gameTime, Cursor);
            _advancedButton.Update(gameTime);

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
                case MenuState.Mouse:
                    _mouseCollection.UpdateInteraction(gameTime, Cursor);
                    _mouseCollection.Update(gameTime);
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
        _mouseButton.Draw(spriteBatch);
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
            case MenuState.Mouse:
                _mouseCollection.Draw(spriteBatch);
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
        _mouseButton.Text.ChangeColor(Color.Gray);
        _languageButton.Text.ChangeColor(Color.Gray);

        switch (_menuState)
        {
            case MenuState.Video:
                _videoButton.Text.ChangeColor(Color.White);
                break;
            case MenuState.Audio:
                _audioButton.Text.ChangeColor(Color.White);
                break;
            case MenuState.Mouse:
                _mouseButton.Text.ChangeColor(Color.White);
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
        _mouseButton.Text.ChangeText(_textComponent.GetValue("Mouse"));
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
        _saveChangesLabel.InRectangle(Camera.Rectangle)
            .OnCenter()
            .OnY(0.33F)
            .Centered()
            .Move();

        _sensLabel.ChangeText(_textComponent.GetValue("Sens"));
        _sensLabel.GetAnchor(_sens)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistanceY(4F)
            .Move();
    }
}