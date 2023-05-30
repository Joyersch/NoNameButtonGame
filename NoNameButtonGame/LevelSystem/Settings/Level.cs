using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoUtils.Logic.Objects.Buttons;
using MonoUtils.Objects;
using MonoUtils.Objects.Buttons;
using MonoUtils.Objects.TextSystem;
using MonoUtils.Ui;
using MonoUtils.Ui.TextSystem;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Level : SampleLevel
{
    private readonly Storage.Storage _storage;

    private Vector2 _vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public Level(Display display, Vector2 window, Random rand, Storage.Storage storage) : base(display,
        window, rand)
    {
        _storage = storage;
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
        AutoManaged.Add(resolutionInfo);

        leftAnchor += new Vector2(0, resolutionInfo.Rectangle.Height + 4);

        var resolution = new ValueSelection(leftAnchor, 1, resolutions, index);
        resolution.ValueChanged += ChangeResolution;
        AutoManaged.Add(resolution);

        leftAnchor += new Vector2(0, resolution.Rectangle.Height + 4);

        var fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo);
        fullscreenButton.Text.ChangeColor(new[] {settingTwo == Letter.Crossout.ToString() ? Color.Red : Color.Green});
        fullscreenButton.Click += ChangePressState;
        AutoManaged.Add(fullscreenButton);

        var fullscreenText = new Text("Fullscreen", Vector2.Zero);
        fullscreenText.Move(leftAnchor + new Vector2(fullscreenButton.Rectangle.Width + 4,
            fullscreenButton.Rectangle.Height / 2 - fullscreenText.Rectangle.Height / 2));
        AutoManaged.Add(fullscreenText);


        leftAnchor += new Vector2(0, fullscreenButton.Rectangle.Height + 4);

        var fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne);
        fixedStepButton.Text.ChangeColor(new[] {settingOne == Letter.Crossout.ToString() ? Color.Red : Color.Green});
        fixedStepButton.Click += ChangePressState;
        AutoManaged.Add(fixedStepButton);

        var fixedStepText = new Text("FPS-Limit", Vector2.Zero);
        fixedStepText.Move(leftAnchor + new Vector2(fixedStepButton.Rectangle.Width + 4,
            fixedStepButton.Rectangle.Height / 2 - fixedStepText.Rectangle.Height / 2));
        AutoManaged.Add(fixedStepText);

        List<string> volumeValues = new List<string>();
        for (int i = 0; i <= 10; i++)
            volumeValues.Add(i.ToString());

        var musicInfo = new Text("Music Volume", rightAnchor);
        AutoManaged.Add(musicInfo);

        rightAnchor += new Vector2(0, musicInfo.Rectangle.Height + 4);

        var musicVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.MusicVolume);
        musicVolume.ValueChanged += MusicVolumeChanged;
        AutoManaged.Add(musicVolume);

        rightAnchor += new Vector2(0, musicVolume.Rectangle.Height + 4);

        var sfxInfo = new Text("SFX Volume", rightAnchor);
        AutoManaged.Add(sfxInfo);

        rightAnchor += new Vector2(0, sfxInfo.Rectangle.Height + 4);

        var sfxVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.SfxVolume);
        sfxVolume.ValueChanged += SFXVolumeOnValueChanged;
        AutoManaged.Add(sfxVolume);

        _vectorResolution = window;
        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(Mouse, cursor);
        AutoManaged.Add(cursor);
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
        Window.X = _vectorResolution.X;
        Window.Y = _vectorResolution.Y;
        WindowsResizeEventHandler?.Invoke(Window);
    }

    private void ChangePressState(object sender)
        => ChangePressState((TextButton) sender);

    private void ChangePressState(TextButton button)
    {
        string text = button.Text.Value;
        switch (Letter.Parse(text[0]))
        {
            case Letter.Character.Crossout:
                button.Text.ChangeText(Letter.Checkmark.ToString());
                button.Text.ChangeColor(new[] {Color.Green});
                break;
            case Letter.Character.Checkmark:
                button.Text.ChangeText(Letter.Crossout.ToString());
                button.Text.ChangeColor(new[] {Color.Red});
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
        CurrentMusic("TitleMusic");
    }
}