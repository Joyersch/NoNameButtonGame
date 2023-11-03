using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    private Vector2 _vectorResolution;
    private OverTimeMover _overTimeMoverDown;
    private OverTimeMover _overTimeMoverUp;
    private Cursor _cursor;

    public event Action<Vector2> WindowsResizeEventHandler;

    private ManagmentCollection _generalCollection;
    private ManagmentCollection _videoCollection;
    private ManagmentCollection _audioCollection;
    private ManagmentCollection _languageCollection;
    private ManagmentCollection _keybindsCollection;


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

        var generalButton = new TextButton("General");
        generalButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.1F)
            .Centered()
            .Move();
        generalButton.Click += _ => _menuState = MenuState.General;
        AutoManaged.Add(generalButton);

        var videoButton = new TextButton("Video");
        videoButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.3F)
            .Centered()
            .Move();
        videoButton.Click += _ => _menuState = MenuState.Video;
        AutoManaged.Add(videoButton);

        var audioButton = new TextButton("Audio");
        audioButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.5F)
            .Centered()
            .Move();
        audioButton.Click += _ => _menuState = MenuState.Audio;
        AutoManaged.Add(audioButton);

        var languageButton = new TextButton("Language");
        languageButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.7F)
            .Centered()
            .Move();
        languageButton.Click += _ => _menuState = MenuState.Language;
        AutoManaged.Add(languageButton);

        var keybindsButton = new TextButton("Keybinds");
        keybindsButton.GetCalculator(Camera.Rectangle)
            .OnY(0.1F)
            .OnX(0.9F)
            .Centered()
            .Move();
        keybindsButton.Click += _ => _menuState = MenuState.Keybinds;
        AutoManaged.Add(keybindsButton);


        string settingOne = Letter.Crossout.ToString(), settingTwo = Letter.Crossout.ToString();
        if (storage.Settings.IsFixedStep)
            settingOne = Letter.Checkmark.ToString();
        if (storage.Settings.IsFullscreen)
            settingTwo = Letter.Checkmark.ToString();

        var leftAnchor = new Vector2(-196, -64);
        var rightAnchor = new Vector2(16, -64);

        Name = "Start Menu";

        List<string> resolutions = new List<string>()
        {
            "1280x720",
            "1920x1080",
            "2560x1440",
            "3840x2160"
        };

        var savedResolution = storage.Settings.Resolution;
        var saved = savedResolution.Width + "x" + savedResolution.Height;
        var index = resolutions.IndexOf(saved);

        if (index == -1)
            index++;

        var resolutionInfo = new Text("Resolution", leftAnchor);
        _generalCollection.Add(resolutionInfo);

        leftAnchor += new Vector2(0, resolutionInfo.Rectangle.Height + 4);

        var resolution = new ValueSelection(leftAnchor, 1, resolutions, index);
        resolution.ValueChanged += ChangeResolution;
        _generalCollection.Add(resolution);

        leftAnchor += new Vector2(0, resolution.Rectangle.Height + 4);

        var fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo);
        fullscreenButton.Text.ChangeColor(new[] { settingTwo == Letter.Crossout.ToString() ? Color.Red : Color.Green });
        fullscreenButton.Click += ChangePressState;
        _generalCollection.Add(fullscreenButton);

        var fullscreenText = new Text("Fullscreen", Vector2.Zero);
        fullscreenText.Move(leftAnchor + new Vector2(fullscreenButton.Rectangle.Width + 4,
            fullscreenButton.Rectangle.Height / 2 - fullscreenText.Rectangle.Height / 2));
        _generalCollection.Add(fullscreenText);


        leftAnchor += new Vector2(0, fullscreenButton.Rectangle.Height + 4);

        var fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne);
        fixedStepButton.Text.ChangeColor(new[] { settingOne == Letter.Crossout.ToString() ? Color.Red : Color.Green });
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

        var musicVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.MusicVolume);
        musicVolume.ValueChanged += MusicVolumeChanged;
        _generalCollection.Add(musicVolume);

        rightAnchor += new Vector2(0, musicVolume.Rectangle.Height + 4);

        var sfxInfo = new Text("SFX Volume", rightAnchor);
        _generalCollection.Add(sfxInfo);

        rightAnchor += new Vector2(0, sfxInfo.Rectangle.Height + 4);

        var sfxVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.SfxVolume);
        sfxVolume.ValueChanged += SFXVolumeOnValueChanged;
        _generalCollection.Add(sfxVolume);

        _vectorResolution = window;

        _cursor = new Cursor();
        Actuator = _cursor;
        PositionListener.Add(Mouse, _cursor);
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
        _vectorResolution = new Vector2(x, y);
        _storage.Settings.Resolution.Width = x;
        _storage.Settings.Resolution.Height = y;
        SetScreen(_vectorResolution);
        WindowsResizeEventHandler?.Invoke(Window);
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
        switch (_menuState)
        {
            case MenuState.General:
                _generalCollection.UpdateInteraction(gameTime, _cursor);
                _generalCollection.Update(gameTime);
                break;
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
            case MenuState.Keybinds:
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