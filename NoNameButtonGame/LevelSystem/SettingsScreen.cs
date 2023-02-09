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
    private readonly TextBuilder _resolution;
    private readonly SquareTextButton _decreaseResolutionButton;
    private readonly SquareTextButton _increaseResolutionButton;
    
    private readonly TextBuilder _volumeMusic;
    private readonly SquareTextButton _decreaseVolumeMusicButton;
    private readonly SquareTextButton _increaseVolumeMusicButton;
    
    private readonly TextBuilder _volumeSFX;
    private readonly SquareTextButton _decreaseVolumeSFXButton;
    private readonly SquareTextButton _increaseVolumeSFXButton;

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

        _decreaseResolutionButton = new SquareTextButton(leftAnchor, left, left);
        _decreaseResolutionButton.ClickEventHandler += ChangeResolution;
        _resolution = new TextBuilder(window.X + "x" + window.Y, Vector2.One);
        
        _increaseResolutionButton = new SquareTextButton(Vector2.Zero, right, right);
        _increaseResolutionButton.ClickEventHandler += ChangeResolution;
        
        CalculateResolutionPositions();
        
        leftAnchor += new Vector2(0, _decreaseResolutionButton.Rectangle.Height + 4);

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
        _musicVolume = new ValueSelection(rightAnchor, Vector2.Zero, volumeValues, 9);
        

        vectorResolution = window;
    }

    private void CalculateResolutionPositions()
    {
        _resolution.Move(_decreaseResolutionButton.Position + new Vector2(_decreaseResolutionButton.Rectangle.Width + 4,
            _decreaseResolutionButton.Rectangle.Height / 2 - _resolution.Rectangle.Height / 2));
        
        _increaseResolutionButton.Move(_resolution.Position + new Vector2(_resolution.Rectangle.Width + 4,
            _resolution.Rectangle.Height / 2 - _increaseResolutionButton.Rectangle.Height / 2));
    }

    private void ChangeResolution(object sender)
        => ChangeResolution((TextButton) sender);

    private void ChangeResolution(TextButton button)
    {
        if (button.Name == right)
        {
            vectorResolution = (vectorResolution.X + "x" + vectorResolution.Y) switch
            {
                "1280x720" => new Vector2(1920, 1080),
                "1920x1080" => new Vector2(2560, 1440),
                "2560x1440" => new Vector2(3840, 2160),
                "3840x2160" => new Vector2(1280, 720),
                _ => vectorResolution
            };
        }

        if (button.Name == left)
        {
            vectorResolution = (vectorResolution.X + "x" + vectorResolution.Y) switch
            {
                "1280x720" => new Vector2(3840, 2160),
                "1920x1080" => new Vector2(1280, 720),
                "2560x1440" => new Vector2(1920, 1080),
                "3840x2160" => new Vector2(2560, 1440),
                _ => vectorResolution
            };
        }

        _resolution.ChangeText(vectorResolution.X + "x" + vectorResolution.Y);
        CalculateResolutionPositions();
        storage.Settings.Resolution.Width = (int) vectorResolution.X;
        storage.Settings.Resolution.Height = (int) vectorResolution.Y;
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
        
        _resolution.Update(gameTime);
        _decreaseResolutionButton.Update(gameTime, mouseCursor.Hitbox[0]);
        _increaseResolutionButton.Update(gameTime, mouseCursor.Hitbox[0]);
        
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
        _decreaseResolutionButton.Draw(spriteBatch);
        _increaseResolutionButton.Draw(spriteBatch);
        
        //_volumeMusic.Draw(spriteBatch);
        
        _musicVolume.Draw(spriteBatch);

        mouseCursor.Draw(spriteBatch);
    }
}