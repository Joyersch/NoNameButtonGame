using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Text;
using NoNameButtonGame.GameObjects.Groups;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem;

internal class SettingsScreen : SampleLevel
{
    private readonly TextBuilder resolutionInfo;
    private readonly ValueSelection resolution;

    private readonly TextBuilder fixedStepText;
    private readonly SquareTextButton fixedStepButton;

    private readonly TextBuilder fullscreenText;
    private readonly SquareTextButton fullscreenButton;

    private readonly Cursor mouseCursor;

    private readonly TextBuilder musicInfo;
    private readonly ValueSelection musicVolume;
    
    private readonly TextBuilder sfxInfo;
    private readonly ValueSelection sfxVolume;


    private readonly Storage.Storage storage;

    private readonly string crossout = Letter.ReverseParse(Letter.Character.Crossout).ToString();
    private readonly string checkmark = Letter.ReverseParse(Letter.Character.Checkmark).ToString();
    private Vector2 vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage.Storage storage) : base(
        defaultWidth, defaultHeight, window, rand)
    {
        this.storage = storage;
        string settingOne = crossout, settingTwo = crossout;
        if (storage.Settings.IsFixedStep)
            settingOne = checkmark;
        if (storage.Settings.IsFullscreen)
            settingTwo = checkmark;

        var leftAnchor = new Vector2(-196, -64);
        var rightAnchor = new Vector2(16, -64);

        Name = "Start Menu";
        mouseCursor = new Cursor(Vector2.Zero);

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

        resolutionInfo = new TextBuilder("Resolution", leftAnchor);

        leftAnchor += new Vector2(0, resolutionInfo.Rectangle.Height + 4);

        resolution = new ValueSelection(leftAnchor, 1, resolutions, index);
        resolution.ValueChanged += ChangeResolution;

        leftAnchor += new Vector2(0, resolution.Rectangle.Height + 4);

        fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo);
        fullscreenButton.Text.ChangeColor(new[] {settingTwo == crossout ? Color.Red : Color.Green});
        fullscreenButton.ClickEventHandler += ChangePressState;
        fullscreenText = new TextBuilder("Fullscreen", Vector2.Zero);
        fullscreenText.Move(leftAnchor + new Vector2(fullscreenButton.Rectangle.Width + 4,
            fullscreenButton.Rectangle.Height / 2 - fullscreenText.Rectangle.Height / 2));

        leftAnchor += new Vector2(0, fullscreenButton.Rectangle.Height + 4);

        fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne);
        fixedStepButton.Text.ChangeColor(new[] {settingOne == crossout ? Color.Red : Color.Green});
        fixedStepButton.ClickEventHandler += ChangePressState;
        fixedStepText = new TextBuilder("FPS-Limit", Vector2.Zero);
        fixedStepText.Move(leftAnchor + new Vector2(fixedStepButton.Rectangle.Width + 4,
            fixedStepButton.Rectangle.Height / 2 - fixedStepText.Rectangle.Height / 2));


        List<string> volumeValues = new List<string>();
        for (int i = 0; i <= 10; i++)
            volumeValues.Add(i.ToString());

        musicInfo = new TextBuilder("Music Volume", rightAnchor);

        rightAnchor += new Vector2(0, musicInfo.Rectangle.Height + 4);

        musicVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.MusicVolume);
        musicVolume.ValueChanged += MusicVolumeChanged;
        
        rightAnchor += new Vector2(0, musicVolume.Rectangle.Height + 4);
        
        sfxInfo = new TextBuilder("SFX Volume", rightAnchor);

        rightAnchor += new Vector2(0, sfxInfo.Rectangle.Height + 4);

        sfxVolume = new ValueSelection(rightAnchor, 1, volumeValues, storage.Settings.SfxVolume);
        sfxVolume.ValueChanged += SFXVolumeOnValueChanged;

        vectorResolution = window;
    }

    private void SFXVolumeOnValueChanged(string obj)
        => storage.Settings.SfxVolume = int.Parse(obj);

    private void MusicVolumeChanged(string obj)
        => storage.Settings.MusicVolume = int.Parse(obj);

    private void ChangeResolution(string newValue)
    {
        var split = newValue.Split('x');
        var x = int.Parse(split[0]);
        var y = int.Parse(split[1]);
        vectorResolution = new Vector2(x, y);
        storage.Settings.Resolution.Width = x;
        storage.Settings.Resolution.Height = y;
        Window.X = vectorResolution.X;
        Window.Y = vectorResolution.Y;
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
                button.Text.ChangeText(checkmark);
                button.Text.ChangeColor(new[] {Color.Green});
                break;
            case Letter.Character.Checkmark:
                button.Text.ChangeText(crossout);
                button.Text.ChangeColor(new[] {Color.Red});
                break;
        }

        storage.Settings.IsFixedStep = fixedStepButton.Text.Text == checkmark;
        storage.Settings.IsFullscreen = fullscreenButton.Text.Text == checkmark;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CurrentMusic("TitleMusic");
        mouseCursor.Position = mousePosition;
        mouseCursor.Update(gameTime);

        resolutionInfo.Update(gameTime);
        resolution.Update(gameTime, mouseCursor.Hitbox[0]);

        fixedStepText.Update(gameTime);
        fixedStepButton.Update(gameTime, mouseCursor.Hitbox[0]);

        fullscreenText.Update(gameTime);
        fullscreenButton.Update(gameTime, mouseCursor.Hitbox[0]);

        musicInfo.Update(gameTime);
        musicVolume.Update(gameTime, mouseCursor.Hitbox[0]);
        
        sfxInfo.Update(gameTime);
        sfxVolume.Update(gameTime, mouseCursor.Hitbox[0]);

        //_volumeMusic.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        fixedStepText.Draw(spriteBatch);
        fixedStepButton.Draw(spriteBatch);

        fullscreenText.Draw(spriteBatch);
        fullscreenButton.Draw(spriteBatch);

        resolutionInfo.Draw(spriteBatch);
        resolution.Draw(spriteBatch);

        musicInfo.Draw(spriteBatch);
        musicVolume.Draw(spriteBatch);
        
        sfxInfo.Draw(spriteBatch);
        sfxVolume.Draw(spriteBatch);

        mouseCursor.Draw(spriteBatch);
    }
}