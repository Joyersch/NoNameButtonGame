using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.LogicObjects.Listener;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem;

internal class StartScreen : SampleLevel
{
    private readonly TextButton _startButton;
    private readonly TextButton _settingsButton;
    private readonly TextButton _selectLevelButton;
    private readonly TextButton _exitButton;
    private readonly TextButton _creditButton;
    private readonly Cursor _mouseCursor;
    private readonly MousePointer _mousePointer;
    private readonly PositionListener _linker;
    private readonly TextBuilder _header;

    public event Action<object> StartEventHandler;
    public event Action<object> SelectEventHandler;
    public event Action<object> SettingsEventHandler;

    public event Action<object> CreditsEventHandler;
    public new event Action<object> ExitEventHandler;

    public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Start Menu";

        const int startPositionY = -(64 * 2 + 32);
        int x = -304;

        _startButton = new TextButton(new Vector2(x, startPositionY), string.Empty, "Start");
        _startButton.Click += StartButtonPressed;

        _selectLevelButton = new TextButton(new Vector2(x, startPositionY + 64), string.Empty, "Select Level");
        _selectLevelButton.Click += SelectButtonPressed;

        _settingsButton = new TextButton(new Vector2(x, startPositionY + 64 * 2), string.Empty, "Settings");
        _settingsButton.Click += SettingsButtonPressed;

        _creditButton = new TextButton(new Vector2(x, startPositionY + 64 * 3), string.Empty, "Credits");
        _creditButton.Click += CreditButtonPressed;

        _exitButton = new TextButton(new Vector2(x, startPositionY + 64 * 4), string.Empty, "Exit");
        _exitButton.Click += ExitButtonPressed;

        _header = new TextBuilder("NoNameButtonGame", Vector2.Zero, 2.5F, 1);
        _header.ChangePosition(-_header.Rectangle.Size.ToVector2() / 2 +
                              new Vector2(TextButton.DefaultSize.X, -TextButton.DefaultSize.Y) / 2);

        _mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        _mousePointer = new MousePointer();
        _linker = new PositionListener();
        _linker.Add(_mousePointer, _mouseCursor);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _startButton.Draw(spriteBatch);
        _settingsButton.Draw(spriteBatch);
        _selectLevelButton.Draw(spriteBatch);
        _creditButton.Draw(spriteBatch);
        _exitButton.Draw(spriteBatch);

        _header.Draw(spriteBatch);


        _mouseCursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        base.CurrentMusic("TitleMusic");
        _mousePointer.Update(gameTime, MousePosition);
        _linker.Update(gameTime);

        _mouseCursor.Update(gameTime);

        _startButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _settingsButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _selectLevelButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _exitButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _creditButton.Update(gameTime, _mouseCursor.Hitbox[0]);

        _header.Update(gameTime);
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