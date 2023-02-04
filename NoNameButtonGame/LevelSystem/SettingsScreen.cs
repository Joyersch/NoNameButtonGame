using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

class SettingsScreen : SampleLevel
{
    readonly TextBuilder Resolution;
    readonly TextBuilder fixedStep;
    readonly TextBuilder Fullscreen;
    readonly Cursor mouseCursor;
    readonly TextButton[] resolutionButton;
    readonly TextButton fixedStepButton;
    readonly TextButton fullscreenButton;
    private Storage storage;

    private readonly string crossout = Letter.ReverseParse(Letter.Character.Crossout).ToString();
    private readonly string checkmark = Letter.ReverseParse(Letter.Character.Checkmark).ToString();
    private readonly string left = Letter.ReverseParse(Letter.Character.Left).ToString();
    private readonly string right = Letter.ReverseParse(Letter.Character.Right).ToString();
    Vector2 vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage storage) : base(
        defaultWidth, defaultHeight, window, rand)
    {
        this.storage = storage;
        string settingOne = crossout, settingTwo = crossout;
        if (storage.Settings.IsFixedStep)
            settingOne = checkmark;
        if (storage.Settings.IsFullscreen)
            settingTwo = checkmark;
        Name = "Start Menu";
        mouseCursor = new Cursor(Vector2.Zero, new Vector2(7, 10));
        fixedStep = new TextBuilder("FPS-Limit", new Vector2(-60, -0));
        Resolution = new TextBuilder(window.X + "x" + window.Y, new Vector2(-60, -64));
        Fullscreen = new TextBuilder("Fullscreen", new Vector2(-60, 64));
        resolutionButton = new TextButton[2];
        resolutionButton[0] = new MiniTextButton(new Vector2(50, -72), new Vector2(40, 32), right, right, MiniTextButton.DefaultTextSize);
        resolutionButton[1] = new MiniTextButton(new Vector2(-108, -72), new Vector2(40, 32),
             left, left, MiniTextButton.DefaultTextSize);
        resolutionButton[0].ClickEventHandler += ChangeResolution;
        resolutionButton[1].ClickEventHandler += ChangeResolution;
        fixedStepButton = new MiniTextButton(new Vector2(-108, -8), new Vector2(40, 32),
             "IsFixedStep", settingOne, MiniTextButton.DefaultTextSize);
        fixedStepButton.Text.ChangeColor(new Color[1] {settingOne == crossout ? Color.Red : Color.Green});
        fixedStepButton.ClickEventHandler += ChangePressState;
        fullscreenButton = new MiniTextButton(new Vector2(-108, 56), new Vector2(40, 32),
             "Fullscreen", settingTwo, MiniTextButton.DefaultTextSize);
        fullscreenButton.Text.ChangeColor(new Color[1] {settingTwo == crossout ? Color.Red : Color.Green});
        fullscreenButton.ClickEventHandler += ChangePressState;
        vectorResolution = window;
    }

    
    private void ChangeResolution(object sender)
        => ChangeResolution((TextButton)sender);
    
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

        Resolution.ChangeText(vectorResolution.X + "x" + vectorResolution.Y);
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

        storage.Settings.IsFixedStep = fixedStepButton.Text.Text == checkmark;
        storage.Settings.IsFullscreen = fullscreenButton.Text.Text == checkmark;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        mouseCursor.Position = mousePosition;
        mouseCursor.Update(gameTime);
        fixedStep.Update(gameTime);
        Resolution.Update(gameTime);
        Fullscreen.Update(gameTime);

        for (int i = 0; i < resolutionButton.Length; i++)
        {
            resolutionButton[i].Update(gameTime, mouseCursor.Hitbox[0]);
        }

        fixedStepButton.Update(gameTime, mouseCursor.Hitbox[0]);
        fullscreenButton.Update(gameTime, mouseCursor.Hitbox[0]);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        fixedStep.Draw(spriteBatch);
        Resolution.Draw(spriteBatch);
        Fullscreen.Draw(spriteBatch);

        for (int i = 0; i < resolutionButton.Length; i++)
        {
            resolutionButton[i].Draw(spriteBatch);
        }

        fixedStepButton.Draw(spriteBatch);
        fullscreenButton.Draw(spriteBatch);
        mouseCursor.Draw(spriteBatch);
    }
}