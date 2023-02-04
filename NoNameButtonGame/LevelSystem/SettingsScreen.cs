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
    Vector2 vectorResolution;
    public event Action<Vector2> WindowsResizeEventHandler;

    public SettingsScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand, Storage storage) : base(
        defaultWidth, defaultHeight, window, rand)
    {
        this.storage = storage;
        string s1 = "❌", s2 = "❌";
        if (storage.Settings.IsFixedStep)
            s1 = "✔";
        if (storage.Settings.IsFullscreen)
            s2 = "✔";
        Name = "Start Menu";
        mouseCursor = new Cursor(Vector2.Zero, new Vector2(7, 10));
        fixedStep = new TextBuilder("FPS-Limit", new Vector2(-64, -0));
        Resolution = new TextBuilder(window.X + "x" + window.Y, new Vector2(-64, -64));
        Fullscreen = new TextBuilder("Fullscreen", new Vector2(-64, 64));
        resolutionButton = new TextButton[2];
        resolutionButton[0] = new MiniTextButton(new Vector2(64, -72), new Vector2(40, 32), ">", ">", MiniTextButton.DefaultTextSize);
        resolutionButton[1] = new MiniTextButton(new Vector2(-108, -72), new Vector2(40, 32),
             "<", "<", MiniTextButton.DefaultTextSize);
        resolutionButton[0].ClickEventHandler += ChangeResolution;
        resolutionButton[1].ClickEventHandler += ChangeResolution;
        fixedStepButton = new MiniTextButton(new Vector2(-108, -8), new Vector2(40, 32),
             "IsFixedStep", s1, MiniTextButton.DefaultTextSize);
        fixedStepButton.Text.ChangeColor(new Color[1] {s1 == "❌" ? Color.Red : Color.Green});
        fixedStepButton.ClickEventHandler += ChangePressState;
        fullscreenButton = new MiniTextButton(new Vector2(-108, 56), new Vector2(40, 32),
             "Fullscreen", s2, MiniTextButton.DefaultTextSize);
        fullscreenButton.Text.ChangeColor(new Color[1] {s2 == "❌" ? Color.Red : Color.Green});
        fullscreenButton.ClickEventHandler += ChangePressState;
        vectorResolution = window;
    }

    
    private void ChangeResolution(object sender)
        => ChangeResolution((TextButton)sender);
    
    private void ChangeResolution(TextButton button)
    {
        if (button.Name == ">")
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

        if (button.Name == "<")
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
        switch (text)
        {
            case "❌":
                button.Text.ChangeText("✔");
                button.Text.ChangeColor(new[] {Color.Green});
                break;
            case "✔":
                button.Text.ChangeText("❌");
                button.Text.ChangeColor(new[] {Color.Red});
                break;
        }

        storage.Settings.IsFixedStep = fixedStepButton.Text.Text == "✔";
        storage.Settings.IsFullscreen = fullscreenButton.Text.Text == "✔";
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