using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Text;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.Cache;
using NoNameButtonGame.LogicObjects;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class StartScreen : SampleLevel
{
    private readonly TextButton startButton;
    private readonly TextButton settingsButton;
    private readonly TextButton selectLevelButton;
    private readonly TextButton exitButton;
    private readonly TextButton creditButton;
    private readonly Cursor mouseCursor;
    private readonly MousePointer mousePointer;
    private readonly GameObjectLinker linker;
    private readonly TextBuilder header;

    public event Action<object> StartEventHandler;
    public event Action<object> SelectEventHandler;
    public event Action<object> SettingsEventHandler;

    public event Action<object> CreditsEventHandler;
    public event Action<object> ExitEventHandler;

    public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Start Menu";

        int StartPositionY = -(64 * 2 + 32);
        int x = -304;

        startButton = new TextButton(new Vector2(x, StartPositionY), string.Empty, "Start");
        startButton.ClickEventHandler += StartButtonPressed;

        selectLevelButton = new TextButton(new Vector2(x, StartPositionY + 64), string.Empty, "Select Level");
        selectLevelButton.ClickEventHandler += SelectButtonPressed;

        settingsButton = new TextButton(new Vector2(x, StartPositionY + 64 * 2), string.Empty, "Settings");
        settingsButton.ClickEventHandler += SettingsButtonPressed;

        creditButton = new TextButton(new Vector2(x, StartPositionY + 64 * 3), string.Empty, "Credits");
        creditButton.ClickEventHandler += CreditButtonPressed;

        exitButton = new TextButton(new Vector2(x, StartPositionY + 64 * 4), string.Empty, "Exit");
        exitButton.ClickEventHandler += ExitButtonPressed;

        header = new TextBuilder("NoNameButtonGame", Vector2.Zero, 2.5F, 1);
        header.ChangePosition(-header.Rectangle.Size.ToVector2() / 2 +
                              new Vector2(TextButton.DefaultSize.X, -TextButton.DefaultSize.Y) / 2);

        mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        mousePointer = new MousePointer();
        linker = new GameObjectLinker();
        linker.Add(mousePointer, mouseCursor);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        startButton.Draw(spriteBatch);
        settingsButton.Draw(spriteBatch);
        selectLevelButton.Draw(spriteBatch);
        creditButton.Draw(spriteBatch);
        exitButton.Draw(spriteBatch);

        header.Draw(spriteBatch);


        mouseCursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        base.CurrentMusic("TitleMusic");
        mousePointer.Update(gameTime, mousePosition);
        linker.Update(gameTime);

        mouseCursor.Update(gameTime);

        startButton.Update(gameTime, mouseCursor.Hitbox[0]);
        settingsButton.Update(gameTime, mouseCursor.Hitbox[0]);
        selectLevelButton.Update(gameTime, mouseCursor.Hitbox[0]);
        exitButton.Update(gameTime, mouseCursor.Hitbox[0]);
        creditButton.Update(gameTime, mouseCursor.Hitbox[0]);

        header.Update(gameTime);
    }

    private void StartButtonPressed(object sender)
        => StartEventHandler?.Invoke(sender);

    private void SelectButtonPressed(object sender)
        => SelectEventHandler?.Invoke(sender);

    private void SettingsButtonPressed(object sender)
        => SettingsEventHandler?.Invoke(sender);

    private void CreditButtonPressed(object sender)
        => CreditsEventHandler?.Invoke(sender);

    private void ExitButtonPressed(object sender)
        => ExitEventHandler?.Invoke(sender);
}