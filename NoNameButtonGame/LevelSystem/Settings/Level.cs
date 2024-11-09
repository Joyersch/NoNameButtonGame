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
    private Blank _highlight;

    private Button _deleteSave;

    private enum MenuState
    {
        Video,
        Audio,
        Mouse,
        Language,
        Advanced
    }

    public Level(Scene scene, Random random, SettingsAndSaveManager<string> settingsAndSave, NoNameGame game,
        EffectsRegistry effectsRegistry) : base(scene, random, effectsRegistry, settingsAndSave)
    {
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

        AnchorCalculator anchorCalculator = null;
        PositionCalculator positionCalculator = null;

        _videoCollection = new ManagementCollection();
        _audioCollection = new ManagementCollection();
        _mouseCollection = new ManagementCollection();
        _languageCollection = new ManagementCollection();
        _advancedCollection = new ManagementCollection();

        _videoButton = new Button(string.Empty);
        _videoButton.Click += _ => _menuState = MenuState.Video;
        DynamicScaler.Register(_videoButton);

        positionCalculator = _videoButton.InRectangle(Camera)
            .OnY(0.1F)
            .OnX(0.1F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _audioButton = new Button(string.Empty);
        _audioButton.Click += _ => _menuState = MenuState.Audio;
        DynamicScaler.Register(_audioButton);

        positionCalculator = _audioButton.InRectangle(Camera)
            .OnY(0.1F)
            .OnX(0.3F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _mouseButton = new Button(string.Empty);
        _mouseButton.Click += _ => _menuState = MenuState.Mouse;
        DynamicScaler.Register(_mouseButton);

        positionCalculator = _mouseButton.InRectangle(Camera)
            .OnY(0.1F)
            .OnX(0.5F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _languageButton = new Button(string.Empty);
        _languageButton.Click += _ => _menuState = MenuState.Language;
        DynamicScaler.Register(_languageButton);

        positionCalculator = _languageButton.InRectangle(Camera)
            .OnY(0.1F)
            .OnX(0.7F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _advancedButton = new Button(string.Empty);
        _advancedButton.Click += _ => _menuState = MenuState.Advanced;
        DynamicScaler.Register(_advancedButton);

        positionCalculator = _advancedButton.InRectangle(Camera)
            .OnY(0.1F)
            .OnX(0.9F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        #region Video

        var index = VideoSettings.Resolutions.IndexOf(VideoSettings.Resolutions.First(r =>
            r.Width == videoSettings.Resolution.Width));

        _resolution = new ValueSelection<Resolution>(Vector2.Zero, 1, VideoSettings.Resolutions, index);
        _resolution.ValueChanged += delegate(object o)
        {
            var resolution = (Resolution)o;
            videoSettings.Resolution = resolution;
            SetScreen(resolution.ToVector2());
            Log.Information($"Changed resolution to: {resolution}");
            OnWindowResize?.Invoke(Window);
            game.ApplyResolution(resolution);
        };
        _videoCollection.Add(_resolution);
        DynamicScaler.Register(_resolution);

        positionCalculator = _resolution.InRectangle(Camera)
            .OnCenter()
            .OnY(0.4F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _resolutionInfo = new Text(string.Empty);
        DynamicScaler.Register(_resolutionInfo);
        _videoCollection.Add(_resolutionInfo);

        anchorCalculator = _resolutionInfo.GetAnchor(_resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistanceY(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _fixedStep = new Checkbox(videoSettings.IsFixedStep);
        _fixedStep.ValueChanged += delegate(bool value)
        {
            videoSettings.IsFixedStep = value;
            game.ApplyFixedStep(value);
        };
        DynamicScaler.Register(_fixedStep);
        _videoCollection.Add(_fixedStep);

        anchorCalculator = _fixedStep.GetAnchor(_resolution)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _fixedStepLabel = new Text(string.Empty);
        DynamicScaler.Register(_fixedStepLabel);
        _videoCollection.Add(_fixedStepLabel);
        anchorCalculator = _fixedStepLabel.GetAnchor(_fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _fullscreen = new Checkbox(videoSettings.IsFullscreen);
        _fullscreen.ValueChanged += delegate(bool value)
        {
            videoSettings.IsFullscreen = value;
            game.ApplyFullscreen(value);
        };
        DynamicScaler.Register(_fullscreen);
        _videoCollection.Add(_fullscreen);

        anchorCalculator = _fullscreen.GetAnchor(_fixedStep)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomLeft)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceY(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _fullscreenLabel = new Text(string.Empty);
        DynamicScaler.Register(_fullscreenLabel);
        _videoCollection.Add(_fullscreenLabel);

        anchorCalculator = _fullscreenLabel.GetAnchor(_fullscreen)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        #endregion // Video

        #region Audio

        List<Volume> volumeValues =
        [
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
            new Volume(1F)
        ];

        Volume currentVolume = volumeValues.First(i => i.Value == audioSettings.MusicVolume);
        _musicVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
        _musicVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            audioSettings.MusicVolume = v.Value;
        };
        DynamicScaler.Register(_musicVolume);
        _audioCollection.Add(_musicVolume);

        positionCalculator = _musicVolume.InRectangle(Camera)
            .OnCenter()
            .OnY(0.4F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _musicVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_musicVolumeLabel);
        DynamicScaler.Register(_musicVolumeLabel);

        anchorCalculator = _musicVolumeLabel.GetAnchor(_musicVolume)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistance(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        currentVolume = volumeValues.First(i => i.Value == audioSettings.SoundEffectVolume);
        _soundEffectVolume =
            new ValueSelection<Volume>(Vector2.Zero, 1F, volumeValues, volumeValues.IndexOf(currentVolume));
        _soundEffectVolume.ValueChanged += delegate(object o)
        {
            Volume v = (Volume)o;
            audioSettings.SoundEffectVolume = v.Value;
        };
        _audioCollection.Add(_soundEffectVolume);
        DynamicScaler.Register(_soundEffectVolume);

        positionCalculator = _soundEffectVolume.InRectangle(Camera)
            .OnCenter()
            .OnY(0.6F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _soundEffectVolumeLabel = new Text(string.Empty);
        _audioCollection.Add(_soundEffectVolumeLabel);
        DynamicScaler.Register(_soundEffectVolumeLabel);

        anchorCalculator = _soundEffectVolumeLabel.GetAnchor(_soundEffectVolume)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistance(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        #endregion // Audio

        #region Mouse

        List<float> values =
        [
            0.1F,
            0.2F,
            0.3F,
            0.4F,
            0.5F,
            0.6F,
            0.7F,
            0.8F,
            0.9F,
            1F
        ];
        _sens = new ValueSelection<float>(Vector2.Zero, 1F, values, values.IndexOf(mouseSettings.Sensitivity));
        _sens.ValueChanged += delegate(object o) { mouseSettings.Sensitivity = (float)o; };
        _mouseCollection.Add(_sens);
        DynamicScaler.Register(_sens);

        positionCalculator = _sens.InRectangle(Camera)
            .OnCenter()
            .OnY(0.4F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _sensLabel = new Text(string.Empty);
        _mouseCollection.Add(_sensLabel);
        DynamicScaler.Register(_sensLabel);

        anchorCalculator = _sensLabel.GetAnchor(_sens)
            .SetMainAnchor(AnchorCalculator.Anchor.Top)
            .SetSubAnchor(AnchorCalculator.Anchor.Bottom)
            .SetDistanceY(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        #endregion // Mouse

        #region Language

        Flag flag = new Flag(TextProvider.Language.en_GB, 5F);
        flag.Click += OnFlagClick;
        _languageCollection.Add(flag);
        DynamicScaler.Register(flag);

        positionCalculator = flag.InRectangle(Camera)
            .OnY(0.55F)
            .OnX(0.33F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        flag = new Flag(TextProvider.Language.de_DE, 5F);
        flag.Click += OnFlagClick;
        _languageCollection.Add(flag);
        DynamicScaler.Register(flag);

        positionCalculator = flag.InRectangle(Camera)
            .OnY(0.55F)
            .OnX(0.66F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        #endregion // Language

        #region Advanced

        _consoleEnabled = new Checkbox(advancedSettings.ConsoleEnabled);
        _consoleEnabled.ValueChanged += delegate(bool value)
        {
            advancedSettings.ConsoleEnabled = value;
            game.ApplyConsole(value);
        };
        _advancedCollection.Add(_consoleEnabled);
        DynamicScaler.Register(_consoleEnabled);

        positionCalculator = _consoleEnabled.InRectangle(Camera)
            .OnX(1, 4)
            .OnY(0.3F);
        CalculatorCollection.Register(positionCalculator);

        _consoleEnabledLabel = new Text(string.Empty);
        _advancedCollection.Add(_consoleEnabledLabel);
        DynamicScaler.Register(_consoleEnabledLabel);
        anchorCalculator = _consoleEnabledLabel.GetAnchor(_consoleEnabled)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(4F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _showElapsedTime = new Checkbox(advancedSettings.ShowElapsedTime);
        _showElapsedTime.ValueChanged += (c) =>
        {
            advancedSettings.ShowElapsedTime = c;
            game.ShowElapsedTime(c);
        };
        _advancedCollection.Add(_showElapsedTime);
        DynamicScaler.Register(_showElapsedTime);

        anchorCalculator = _showElapsedTime.GetAnchor(_consoleEnabled)
            .SetMainAnchor(AnchorCalculator.Anchor.Bottom)
            .SetSubAnchor(AnchorCalculator.Anchor.Top)
            .SetDistanceY(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _elapsedTimeLabel = new Text(string.Empty);
        _advancedCollection.Add(_elapsedTimeLabel);
        DynamicScaler.Register(_elapsedTimeLabel);

        anchorCalculator = _elapsedTimeLabel.GetAnchor(_showElapsedTime)
            .SetMainAnchor(AnchorCalculator.Anchor.Right)
            .SetSubAnchor(AnchorCalculator.Anchor.Left)
            .SetDistanceX(8F)
            .SetDistanceScale(Display);
        CalculatorCollection.Register(anchorCalculator);

        _deleteSave = new Button(string.Empty);
        var deleteSaveHold = new HoldButtonAddon(_deleteSave, 5000F);
        deleteSaveHold.Click += delegate
        {
            settingsAndSave.DeleteSave();
            game.Exit();
        };
        _advancedCollection.Add(deleteSaveHold);
        DynamicScaler.Register(deleteSaveHold);

        positionCalculator = deleteSaveHold.InRectangle(Camera)
            .OnCenter()
            .OnY(0.7F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

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
        DynamicScaler.Register(_saveChangesLabel);

        positionCalculator = _saveChangesLabel.InRectangle(Camera)
            .OnCenter()
            .OnY(0.33F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _saveButton = new Button(string.Empty);
        _saveButton.Click += delegate { OnSave?.Invoke(); };
        DynamicScaler.Register(_saveButton);

        positionCalculator = _saveButton.InRectangle(Camera)
            .OnY(0.55F)
            .OnX(0.36F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _discardButton = new Button(string.Empty);
        _discardButton.Click += delegate { OnDiscard?.Invoke(); };
        DynamicScaler.Register(_discardButton);
        positionCalculator = _discardButton.InRectangle(Camera)
            .OnY(0.55F)
            .OnX(0.64F)
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        _highlight = new Blank(Camera.Rectangle.Location.ToVector2(), Camera.Rectangle.Size.ToVector2())
        {
            //DrawColor = new Color(32, 32, 32, 240)
        };
        DynamicScaler.Register(_highlight);

        positionCalculator = _highlight.InRectangle(Camera)
            .OnCenter()
            .Centered();
        CalculatorCollection.Register(positionCalculator);

        #endregion

        foreach (var f in _languageCollection.OfType<Flag>())
        {
            if (f.Language == _languageSettings.Localization)
                OnFlagClick(f); // Calls ApplyText
        }

        DynamicScaler.Apply(Display.Scale);
        CalculatorCollection.Apply();
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

        _resolutionInfo.ChangeText(_textComponent.GetValue("Resolution"));
        _fixedStepLabel.ChangeText(_textComponent.GetValue("FPSLimit"));
        _fullscreenLabel.ChangeText(_textComponent.GetValue("Fullscreen"));

        _musicVolumeLabel.ChangeText(_textComponent.GetValue("MusicVolume"));
        _soundEffectVolumeLabel.ChangeText(_textComponent.GetValue("SoundEffectVolume"));

        _sensLabel.ChangeText(_textComponent.GetValue("Sens"));

        _consoleEnabledLabel.ChangeText(_textComponent.GetValue("DevConsoleEnabled"));
        _elapsedTimeLabel.ChangeText(_textComponent.GetValue("ShowTotalGameTime"));
        _deleteSave.Text.ChangeText(_textComponent.GetValue("DeleteSave"));

        _saveChangesLabel.ChangeText(_textComponent.GetValue("SaveChanges"));
        _saveChangesLabel.ChangeColor(Color.DeepSkyBlue);
        _saveButton.Text.ChangeText(_textComponent.GetValue("Save"));
        _saveButton.Text.ChangeColor(Color.Green);
        _discardButton.Text.ChangeText(_textComponent.GetValue("Discard"));
        _discardButton.Text.ChangeColor(Color.Red);

        CalculatorCollection.Apply();
    }
}