using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Groups;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.LogicObjects;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class SettingsScreen : SampleLevel
{
    private readonly ValueSelection _resolution;

    private readonly TextBuilder _fixedStepText;
    private readonly SquareTextButton _fixedStepButton;

    private readonly TextBuilder _fullscreenText;
    private readonly SquareTextButton _fullscreenButton;

    private readonly Cursor mouseCursor;
    private readonly TextButton[] resolutionButton;

    private readonly ValueSelection _musicVolume;


    private Storage storage;

    private readonly string crossout = Letter.ReverseParse(Letter.Character.Crossout).ToString();
    private readonly string checkmark = Letter.ReverseParse(Letter.Character.Checkmark).ToString();
    private readonly string left = Letter.ReverseParse(Letter.Character.Left).ToString();
    private readonly string right = Letter.ReverseParse(Letter.Character.Right).ToString();
    private Vector2 vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage storage) : base(
        defaultWidth, defaultHeight, window, rand)
    {
        this.storage = storage;
        string settingOne = crossout, settingTwo = crossout;
        int volume = 0;
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
        
        var s = settingOne + "x" + settingTwo;
        var index = resolutions.IndexOf(s);
        
        if (index == -1)
            index++;
        
        _resolution = new ValueSelection(leftAnchor, 1, resolutions,index);
        _resolution.ValueChanged += ChangeResolution;

        leftAnchor += new Vector2(0, _resolution.Rectangle.Height + 4);

        _fullscreenButton = new SquareTextButton(leftAnchor, "Fullscreen", settingTwo);
        _fullscreenButton.Text.ChangeColor(new Color[1] {settingTwo == crossout ? Color.Red : Color.Green});
        _fullscreenButton.ClickEventHandler += ChangePressState;
        _fullscreenText = new TextBuilder("Fullscreen", Vector2.Zero);
        _fullscreenText.Move(leftAnchor + new Vector2(_fullscreenButton.Rectangle.Width + 4,
            _fullscreenButton.Rectangle.Height / 2 - _fullscreenText.Rectangle.Height / 2));

        leftAnchor += new Vector2(0, _fullscreenButton.Rectangle.Height + 4);

        _fixedStepButton = new SquareTextButton(leftAnchor, "IsFixedStep", settingOne);
        _fixedStepButton.Text.ChangeColor(new Color[1] {settingOne == crossout ? Color.Red : Color.Green});
        _fixedStepButton.ClickEventHandler += ChangePressState;
        _fixedStepText = new TextBuilder("FPS-Limit", Vector2.Zero);
        _fixedStepText.Move(leftAnchor + new Vector2(_fixedStepButton.Rectangle.Width + 4,
            _fixedStepButton.Rectangle.Height / 2 - _fixedStepText.Rectangle.Height / 2));


        List<string> volumeValues = new List<string>();
        for (int i = 0; i < 10; i++)
            volumeValues.Add(i.ToString());
        _musicVolume = new ValueSelection(rightAnchor,1, volumeValues, 9);

        vectorResolution = window;
    }

    private void ChangeResolution(object sender)
        => ChangeResolution((TextButton) sender);

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

        storage.Settings.IsFixedStep = _fixedStepButton.Text.Text == checkmark;
        storage.Settings.IsFullscreen = _fullscreenButton.Text.Text == checkmark;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        mouseCursor.Position = mousePosition;
        mouseCursor.Update(gameTime);

        _resolution.Update(gameTime, mouseCursor.Hitbox[0]);

        _fixedStepText.Update(gameTime);
        _fixedStepButton.Update(gameTime, mouseCursor.Hitbox[0]);

        _fullscreenText.Update(gameTime);
        _fullscreenButton.Update(gameTime, mouseCursor.Hitbox[0]);

        _musicVolume.Update(gameTime, mouseCursor.Hitbox[0]);

        //_volumeMusic.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _fixedStepText.Draw(spriteBatch);
        _fixedStepButton.Draw(spriteBatch);

        _fullscreenText.Draw(spriteBatch);
        _fullscreenButton.Draw(spriteBatch);

        _resolution.Draw(spriteBatch);

        //_volumeMusic.Draw(spriteBatch);

        _musicVolume.Draw(spriteBatch);

        mouseCursor.Draw(spriteBatch);
    }
}