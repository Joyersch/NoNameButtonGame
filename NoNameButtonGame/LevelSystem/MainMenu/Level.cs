using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem.MainMenu;

public class Level : SampleLevel
{
    public event Action<object> StartClicked;
    public event Action<object> SelectClicked;
    public event Action<object> SettingsClicked;

    public event Action<object> CreditsClicked;
    public event Action<object> ExitClicked;

    public Level(Display.Display display, Vector2 window, Random rand) : base(display, window, rand)
    {
        Name = "Start Menu";

        const int startPositionY = -(64 * 2 + 32);
        int x = -304;

        var startButton = new TextButton(new Vector2(x, startPositionY), "Start");
        startButton.Click += StartButtonPressed;
        AutoManaged.Add(startButton);

        var selectLevelButton = new TextButton(new Vector2(x, startPositionY + 64), "Select Level");
        selectLevelButton.Click += SelectButtonPressed;
        AutoManaged.Add(selectLevelButton);

        var settingsButton = new TextButton(new Vector2(x, startPositionY + 64 * 2), "Settings");
        settingsButton.Click += SettingsButtonPressed;
        AutoManaged.Add(settingsButton);

        var creditButton = new TextButton(new Vector2(x, startPositionY + 64 * 3), "Credits");
        creditButton.Click += CreditButtonPressed;
        AutoManaged.Add(creditButton);

        var exitButton = new TextButton(new Vector2(x, startPositionY + 64 * 4), "Exit");
        exitButton.Click += ExitButtonPressed;
        AutoManaged.Add(exitButton);

        var header = new Text("NoNameButtonGame", Vector2.Zero, 2.5F, 1);
        header.Move(-header.Rectangle.Size.ToVector2() / 2 +
                               new Vector2(TextButton.DefaultSize.X, -TextButton.DefaultSize.Y) / 2);
        AutoManaged.Add(header);

        var mouseCursor = new Cursor();
        Actuator = mouseCursor;
        PositionListener.Add(_mouse, mouseCursor);
        AutoManaged.Add(mouseCursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        base.CurrentMusic("TitleMusic");
    }

    private void StartButtonPressed(object sender)
        => StartClicked?.Invoke(sender);

    private void SelectButtonPressed(object sender)
        => SelectClicked?.Invoke(sender);

    private void SettingsButtonPressed(object sender)
        => SettingsClicked?.Invoke(sender);

    private void CreditButtonPressed(object sender)
        => CreditsClicked?.Invoke(sender);

    private void ExitButtonPressed(object sender)
        => ExitClicked?.Invoke(sender);
}