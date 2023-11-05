using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logging;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.TextSystem;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly Storage.Storage _storage;

    private Cursor _cursor;

    public event Action<Vector2> OnWindowResize;

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

    public Level(Display display, Vector2 window, Random rand, Storage.Storage storage) : base(display,
        window, rand)
    {
        _storage = storage;

        _generalCollection = new ManagmentCollection();
        _videoCollection = new ManagmentCollection();
        _audioCollection = new ManagmentCollection();
        _languageCollection = new ManagmentCollection();
        _keybindsCollection = new ManagmentCollection();

        // ToDo: when more settings are available, remove this
        _menuState = MenuState.Video;

        _generalButton = new TextButton("General");
        _generalButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.1F)
            .Centered()
            .Move();
        _generalButton.Click += _ => _menuState = MenuState.General;
        AutoManaged.Add(_generalButton);

        _videoButton = new TextButton("Video");
        _videoButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.3F)
            .Centered()
            .Move();
        _videoButton.Click += _ => _menuState = MenuState.Video;
        AutoManaged.Add(_videoButton);

        _audioButton = new TextButton("Audio");
        _audioButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.5F)
            .Centered()
            .Move();
        _audioButton.Click += _ => _menuState = MenuState.Audio;
        AutoManaged.Add(_audioButton);

        _languageButton = new TextButton("Language");
        _languageButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.7F)
            .Centered()
            .Move();
        _languageButton.Click += _ => _menuState = MenuState.Language;
        AutoManaged.Add(_languageButton);

        _keybindsButton = new TextButton("Keybinds");
        _keybindsButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.9F)
            .Centered()
            .Move();
        _keybindsButton.Click += _ => _menuState = MenuState.Keybinds;
        AutoManaged.Add(_keybindsButton);

        #region Video

        List<object> resolutions = new List<object>()
        {
            new Resolution(1280, 720),
            new Resolution(1920, 1080),
            new Resolution(2560, 1440),
            new Resolution(3840, 2160),
        };

        var index = resolutions.IndexOf(resolutions.First(r =>
            ((Resolution)r).Width == _storage.Settings.Resolution.Width));

        var resolution = new ValueSelection(Vector2.Zero, 1, resolutions, index);
        resolution.GetCalculator(Camera.Rectangle)
            .OnCenter()
            .OnY(0.33F)
            .Centered()
            .Move();

        resolution.ValueChanged += delegate(object o)
        {
            var resolution = (Resolution)o;
            _storage.Settings.Resolution.Width = resolution.Width;
            _storage.Settings.Resolution.Height = resolution.Height;
            SetScreen(resolution.Size);
            OnWindowResize?.Invoke(Window);
            Log.WriteInformation(resolution.ToString());
        };

        _videoCollection.Add(resolution);

        var resolutionInfo = new Text("Resolution");
        resolutionInfo.Move(resolution.Position + new Vector2(0, -resolutionInfo.Size.Y));
        _videoCollection.Add(resolutionInfo);

        var fixedStep = new Checkbox(!_storage.Settings.IsFixedStep);
        fixedStep.Move(resolution.Position + new Vector2(0, resolution.Size.Y + Checkbox.DefaultSize.Y / 8));
        fixedStep.ValueChanged += delegate(bool value)
        {
            _storage.Settings.IsFixedStep = !value;
        };

        _videoCollection.Add(fixedStep);

        var fixedStepLabel = new Text("FPS-Limit");
        Rectangle toCompare = new Rectangle(fixedStep.Position.ToPoint(), resolution.Rectangle.Size);
        fixedStepLabel.GetCalculator(toCompare)
            .OnCenter()
            .Centered()
            .Move();

        _videoCollection.Add(fixedStepLabel);

        var fullscreen = new Checkbox(!_storage.Settings.IsFullscreen);
        fullscreen.Move(fixedStep.Position + new Vector2(0, resolution.Size.Y + Checkbox.DefaultSize.Y / 8));
        fullscreen.ValueChanged += delegate(bool value)
        {
            _storage.Settings.IsFullscreen = !value;
        };

        _videoCollection.Add(fullscreen);

        var fullscreenLabel = new Text("Fullscreen");
        toCompare = new Rectangle(fullscreen.Position.ToPoint(), resolution.Rectangle.Size);
        fullscreenLabel.GetCalculator(toCompare)
            .OnCenter()
            .Centered()
            .Move();

        _videoCollection.Add(fullscreenLabel);

        #endregion // Video

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);

        return;

        char settingOne = Letter.Crossout, settingTwo = Letter.Crossout;
        if (storage.Settings.IsFixedStep)
            settingOne = Letter.Checkmark;
        if (storage.Settings.IsFullscreen)
            settingTwo = Letter.Checkmark;

        Name = "Start Menu";


        var savedResolution = storage.Settings.Resolution;
        var saved = savedResolution.Width + "x" + savedResolution.Height;

        if (index == -1)
            index++;


        var leftAnchor = Vector2.Zero;
        var rightAnchor = Vector2.Zero;

        //var resolution = new ValueSelection(leftAnchor, 1, resolutions, index);
        //resolution.ValueChanged += ChangeResolution;
        _generalCollection.Add(resolution);

        leftAnchor += new Vector2(0, resolution.Rectangle.Height + 4);

        var fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo.ToString());
        fullscreenButton.Text.ChangeColor(new[] { settingTwo == Letter.Crossout ? Color.Red : Color.Green });
        fullscreenButton.Click += ChangePressState;
        _generalCollection.Add(fullscreenButton);

        var fullscreenText = new Text("Fullscreen", Vector2.Zero);
        fullscreenText.Move(leftAnchor + new Vector2(fullscreenButton.Rectangle.Width + 4,
            fullscreenButton.Rectangle.Height / 2 - fullscreenText.Rectangle.Height / 2));
        _generalCollection.Add(fullscreenText);


        leftAnchor += new Vector2(0, fullscreenButton.Rectangle.Height + 4);

        var fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne.ToString());
        fixedStepButton.Text.ChangeColor(new[] { settingOne == Letter.Crossout ? Color.Red : Color.Green });
        fixedStepButton.Click += ChangePressState;
        _generalCollection.Add(fixedStepButton);

        var fixedStepText = new Text("FPS-Limit", Vector2.Zero);
        fixedStepText.Move(leftAnchor + new Vector2(fixedStepButton.Rectangle.Width + 4,
            fixedStepButton.Rectangle.Height / 2 - fixedStepText.Rectangle.Height / 2));
        _generalCollection.Add(fixedStepText);

        List<string> volumeValues = new List<string>();
        for (int i = 0; i <= 10; i++)
            volumeValues.Add(i.ToString());

        var musicInfo = new Text("Music Volume", rightAnchor);
        _generalCollection.Add(musicInfo);

        rightAnchor += new Vector2(0, musicInfo.Rectangle.Height + 4);

        //var musicVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.MusicVolume);
        //musicVolume.ValueChanged += MusicVolumeChanged;
        //_generalCollection.Add(musicVolume);

        //rightAnchor += new Vector2(0, musicVolume.Rectangle.Height + 4);

        var sfxInfo = new Text("SFX Volume", rightAnchor);
        _generalCollection.Add(sfxInfo);

        rightAnchor += new Vector2(0, sfxInfo.Rectangle.Height + 4);

        //var sfxVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.SfxVolume);
        //sfxVolume.ValueChanged += SFXVolumeOnValueChanged;
        //_generalCollection.Add(sfxVolume);
    }

    private void SFXVolumeOnValueChanged(string obj)
        => _storage.Settings.SfxVolume = int.Parse(obj);

    private void MusicVolumeChanged(string obj)
        => _storage.Settings.MusicVolume = int.Parse(obj);

    private void ChangeResolution(string newValue)
    {
        var split = newValue.Split('x');
        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);
        //_vectorResolution = new Vector2(x, y);
        _storage.Settings.Resolution.Width = x;
        _storage.Settings.Resolution.Height = y;
        //SetScreen(_vectorResolution);
        OnWindowResize?.Invoke(Window);
    }

    private void ChangePressState(object sender)
        => ChangePressState((TextButton)sender);

    private void ChangePressState(TextButton button)
    {
        string text = button.Text.Value;
        switch (Letter.Parse(text[0]))
        {
            case Letter.Character.Crossout:
                button.Text.ChangeText(Letter.Checkmark.ToString());
                button.Text.ChangeColor(new[] { Color.Green });
                break;
            case Letter.Character.Checkmark:
                button.Text.ChangeText(Letter.Crossout.ToString());
                button.Text.ChangeColor(new[] { Color.Red });
                break;
        }

        if (button.Name == "IsFixedStep")
            _storage.Settings.IsFixedStep = button.Text.Value == Letter.Checkmark.ToString();
        else
            _storage.Settings.IsFullscreen = button.Text.Value == Letter.Checkmark.ToString();
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
}