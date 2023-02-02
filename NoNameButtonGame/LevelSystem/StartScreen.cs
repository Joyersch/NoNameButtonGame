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

class StartScreen : SampleLevel
{
    readonly StartButton startButton;
    readonly SettingsButton settingsButton;
    readonly SelectButton selectLevelButton;
    readonly ExitButton exitButton;
    readonly Cursor mouseCursor;

    public event Action<object> StartEventHandler;
    public event Action<object> SelectEventHandler;
    public event Action<object> SettingsEventHandler;
    public event Action<object> ExitEventHandler;

    public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Start Menu";
        int Startpos = -(64 * 2);
        mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        startButton = new StartButton(new Vector2(-64, Startpos), new Vector2(160, 64));
        startButton.ClickEventHandler += StartButtonPressed;
        selectLevelButton = new SelectButton(new Vector2(-92, Startpos + 64), new Vector2(216, 64));
        selectLevelButton.ClickEventHandler += SelectButtonPressed;
        settingsButton = new SettingsButton(new Vector2(-130, Startpos + 64 * 2), new Vector2(292, 64));
        settingsButton.ClickEventHandler += SettingsButtonPressed;
        exitButton = new ExitButton(new Vector2(-52, Startpos + 64 * 3), new Vector2(136, 64));
        exitButton.ClickEventHandler += ExitButtonPressed;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        startButton.Draw(spriteBatch);
        settingsButton.Draw(spriteBatch);
        selectLevelButton.Draw(spriteBatch);
        exitButton.Draw(spriteBatch);
        mouseCursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        startButton.Update(gameTime, mouseCursor.Hitbox[0]);
        settingsButton.Update(gameTime, mouseCursor.Hitbox[0]);
        selectLevelButton.Update(gameTime, mouseCursor.Hitbox[0]);
        exitButton.Update(gameTime, mouseCursor.Hitbox[0]);
        mouseCursor.Position = mousePosition;
        mouseCursor.Update(gameTime);
    }

    private void StartButtonPressed(object sender)
        => StartEventHandler?.Invoke(sender);

    private void SelectButtonPressed(object sender)
        => SelectEventHandler?.Invoke(sender);

    private void SettingsButtonPressed(object sender)
        => SettingsEventHandler?.Invoke(sender);

    private void ExitButtonPressed(object sender)
        => ExitEventHandler?.Invoke(sender);
}