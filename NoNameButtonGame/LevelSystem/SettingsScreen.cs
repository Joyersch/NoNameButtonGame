using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons;
using NoNameButtonGame.GameObjects.Groups;
using NoNameButtonGame.GameObjects.TextSystem;

namespace NoNameButtonGame.LevelSystem;

public class SettingsScreen : SampleLevel
{
    private readonly Text _resolutionInfo;
    private readonly ValueSelection _resolution;

    private readonly Text _fixedStepText;
    private readonly SquareTextButton _fixedStepButton;

    private readonly Text _fullscreenText;
    private readonly SquareTextButton _fullscreenButton;

    private readonly Cursor _mouseCursor;

    private readonly Text _musicInfo;
    private readonly ValueSelection _musicVolume;
    
    private readonly Text _sfxInfo;
    private readonly ValueSelection _sfxVolume;


    private readonly Storage.Storage _storage;

    private readonly string _crossout = Letter.ReverseParse(Letter.Character.Crossout).ToString();
    private readonly string _checkmark = Letter.ReverseParse(Letter.Character.Checkmark).ToString();
    private Vector2 _vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage.Storage storage) : base(
        defaultWidth, defaultHeight, window, rand)
    {
        this._storage = storage;
        string settingOne = _crossout, settingTwo = _crossout;
        if (storage.Settings.IsFixedStep)
            settingOne = _checkmark;
        if (storage.Settings.IsFullscreen)
            settingTwo = _checkmark;

        var leftAnchor = new Vector2(-196, -64);
        var rightAnchor = new Vector2(16, -64);

        Name = "Start Menu";
        _mouseCursor = new Cursor(Vector2.Zero);

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

        _resolutionInfo = new Text("Resolution", leftAnchor);

        leftAnchor += new Vector2(0, _resolutionInfo.Rectangle.Height + 4);

        _resolution = new ValueSelection(leftAnchor, 1, resolutions, index);
        _resolution.ValueChanged += ChangeResolution;

        leftAnchor += new Vector2(0, _resolution.Rectangle.Height + 4);

        _fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo);
        _fullscreenButton.Text.ChangeColor(new[] {settingTwo == _crossout ? Color.Red : Color.Green});
        _fullscreenButton.Click += ChangePressState;
        _fullscreenText = new Text("Fullscreen", Vector2.Zero);
        _fullscreenText.Move(leftAnchor + new Vector2(_fullscreenButton.Rectangle.Width + 4,
            _fullscreenButton.Rectangle.Height / 2 - _fullscreenText.Rectangle.Height / 2));

        leftAnchor += new Vector2(0, _fullscreenButton.Rectangle.Height + 4);

        _fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne);
        _fixedStepButton.Text.ChangeColor(new[] {settingOne == _crossout ? Color.Red : Color.Green});
        _fixedStepButton.Click += ChangePressState;
        _fixedStepText = new Text("FPS-Limit", Vector2.Zero);
        _fixedStepText.Move(leftAnchor + new Vector2(_fixedStepButton.Rectangle.Width + 4,
            _fixedStepButton.Rectangle.Height / 2 - _fixedStepText.Rectangle.Height / 2));


        List<string> volumeValues = new List<string>();
        for (int i = 0; i <= 10; i++)
            volumeValues.Add(i.ToString());

        _musicInfo = new Text("Music Volume", rightAnchor);

        rightAnchor += new Vector2(0, _musicInfo.Rectangle.Height + 4);

        _musicVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.MusicVolume);
        _musicVolume.ValueChanged += MusicVolumeChanged;
        
        rightAnchor += new Vector2(0, _musicVolume.Rectangle.Height + 4);
        
        _sfxInfo = new Text("SFX Volume", rightAnchor);

        rightAnchor += new Vector2(0, _sfxInfo.Rectangle.Height + 4);

        _sfxVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.SfxVolume);
        _sfxVolume.ValueChanged += SFXVolumeOnValueChanged;

        _vectorResolution = window;
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
        string text = button.Text.ToString();
        switch (Letter.Parse(text[0]))
        {
            case Letter.Character.Crossout:
                button.Text.ChangeText(_checkmark);
                button.Text.ChangeColor(new[] {Color.Green});
                break;
            case Letter.Character.Checkmark:
                button.Text.ChangeText(_crossout);
                button.Text.ChangeColor(new[] {Color.Red});
                break;
        }

        _storage.Settings.IsFixedStep = _fixedStepButton.Text.Value == _checkmark;
        _storage.Settings.IsFullscreen = _fullscreenButton.Text.Value == _checkmark;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CurrentMusic("TitleMusic");
        _mouseCursor.Position = MousePosition;
        _mouseCursor.Update(gameTime);

        _resolutionInfo.Update(gameTime);
        _resolution.Update(gameTime, _mouseCursor.Hitbox[0]);

        _fixedStepText.Update(gameTime);
        _fixedStepButton.Update(gameTime, _mouseCursor.Hitbox[0]);

        _fullscreenText.Update(gameTime);
        _fullscreenButton.Update(gameTime, _mouseCursor.Hitbox[0]);

        _musicInfo.Update(gameTime);
        _musicVolume.Update(gameTime, _mouseCursor.Hitbox[0]);
        
        _sfxInfo.Update(gameTime);
        _sfxVolume.Update(gameTime, _mouseCursor.Hitbox[0]);

        //_volumeMusic.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _fixedStepText.Draw(spriteBatch);
        _fixedStepButton.Draw(spriteBatch);

        _fullscreenText.Draw(spriteBatch);
        _fullscreenButton.Draw(spriteBatch);

        _resolutionInfo.Draw(spriteBatch);
        _resolution.Draw(spriteBatch);

        _musicInfo.Draw(spriteBatch);
        _musicVolume.Draw(spriteBatch);
        
        _sfxInfo.Draw(spriteBatch);
        _sfxVolume.Draw(spriteBatch);

        _mouseCursor.Draw(spriteBatch);
    }
}