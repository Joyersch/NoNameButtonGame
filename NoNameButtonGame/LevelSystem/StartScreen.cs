using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

class StartScreen : SampleLevel
{
    readonly AwesomeButton startButton;
    readonly AwesomeButton settingsButton;
    readonly AwesomeButton selectLevelButton;
    readonly AwesomeButton exitButton;
    readonly Cursor mouseCursor;

    public event Action StartEventHandler;
    public event Action SelectEventHandler;
    public event Action SettingsEventHandler;
    public event Action ExitEventHandler;

    public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Start Menu";
        int Startpos = -(64 * 2);
        mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        startButton = new AwesomeButton(new Vector2(-64, Startpos), new Vector2(160, 64),
            Globals.Content.GetHitboxMapping("startbutton"));
        startButton.Click += StartButtonPressed;
        selectLevelButton = new AwesomeButton(new Vector2(-92, Startpos + 64), new Vector2(216, 64),
            Globals.Content.GetHitboxMapping("selectbutton"));
        selectLevelButton.Click += SelectButtonPressed;
        settingsButton = new AwesomeButton(new Vector2(-130, Startpos + 64 * 2), new Vector2(292, 64),
            Globals.Content.GetHitboxMapping("settingsbutton"));
        settingsButton.Click += SettingsButtonPressed;
        exitButton = new AwesomeButton(new Vector2(-52, Startpos + 64 * 3), new Vector2(136, 64),
            Globals.Content.GetHitboxMapping("exitbutton"));
        exitButton.Click += ExitButtonPressed;
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

    private void StartButtonPressed(object sender, EventArgs e)
        => StartEventHandler?.Invoke();

    private void SelectButtonPressed(object sender, EventArgs e)
        => SelectEventHandler?.Invoke();

    private void SettingsButtonPressed(object sender, EventArgs e)
        => SettingsEventHandler?.Invoke();

    private void ExitButtonPressed(object sender, EventArgs e)
        => ExitEventHandler?.Invoke();
}