using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NoNameButtonGame.LevelSystem;

public class StartScreen : SampleLevel
{
    private readonly TextButton _startButton;
    private readonly TextButton _settingsButton;
    private readonly TextButton _selectLevelButton;
    private readonly TextButton _exitButton;
    private readonly TextButton _creditButton;
    private readonly Cursor _mouseCursor;
    private readonly PositionListener _linker;
    private readonly Text _header;

    public event Action<object> StartClicked;
    public event Action<object> SelectClicked;
    public event Action<object> SettingsClicked;

    public event Action<object> CreditsClicked;
    public event Action<object> ExitClicked;

    public StartScreen(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        Name = "Start Menu";

        const int startPositionY = -(64 * 2 + 32);
        int x = -304;

        _startButton = new TextButton(new Vector2(x, startPositionY), "Start");
        _startButton.Click += StartButtonPressed;

        _selectLevelButton = new TextButton(new Vector2(x, startPositionY + 64), "Select Level");
        _selectLevelButton.Click += SelectButtonPressed;

        _settingsButton = new TextButton(new Vector2(x, startPositionY + 64 * 2), "Settings");
        _settingsButton.Click += SettingsButtonPressed;

        _creditButton = new TextButton(new Vector2(x, startPositionY + 64 * 3), "Credits");
        _creditButton.Click += CreditButtonPressed;

        _exitButton = new TextButton(new Vector2(x, startPositionY + 64 * 4), "Exit");
        _exitButton.Click += ExitButtonPressed;

        _header = new Text("NoNameButtonGame", Vector2.Zero, 2.5F, 1);
        _header.ChangePosition(-_header.Rectangle.Size.ToVector2() / 2 +
                               new Vector2(TextButton.DefaultSize.X, -TextButton.DefaultSize.Y) / 2);

        _mouseCursor = new Cursor();
        _linker = new PositionListener();
        _linker.Add(_mouse, _mouseCursor);
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