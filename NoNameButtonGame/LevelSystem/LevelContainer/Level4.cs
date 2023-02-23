using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level4 : SampleLevel
{
    private readonly Cursor _mouseCursor;
    private readonly TextBuilder _infoAboutButton;
    private readonly TextBuilder _infoAboutButton2;
    private readonly MousePointer _mousePointer;
    private readonly PositionListener _linker;
    private readonly HoldButtonAddon _counterButtonAddon;

    public Level4(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 4 - Tutorial 3 - Button Addon: Hold";
        var stateButton = new TextButton(-EmptyButton.DefaultSize / 2, string.Empty, "Finish Level");
        _infoAboutButton = new TextBuilder("This button has a timer", Vector2.Zero);
        _infoAboutButton.ChangePosition(new Vector2(-_infoAboutButton.Rectangle.Width / 2F, -64));
        _infoAboutButton2 =
            new TextBuilder("Press the button to lower the timer and when it hits 0 you win!", Vector2.Zero);
        _infoAboutButton2.ChangePosition(new Vector2(-_infoAboutButton2.Rectangle.Width / 2F,
            64 - _infoAboutButton2.Rectangle.Height));
        _mouseCursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();
        _linker = new PositionListener();
        _linker.Add(_mousePointer, _mouseCursor);
        _counterButtonAddon = new HoldButtonAddon(stateButton, 3000F);
        _counterButtonAddon.TimerReachedZero += Finish;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mousePointer.Update(gameTime, MousePosition);
        _linker.Update(gameTime);
        _mouseCursor.Update(gameTime);
        _counterButtonAddon.Update(gameTime, _mouseCursor.Hitbox[0]);
        _infoAboutButton.Update(gameTime);
        _infoAboutButton2.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _counterButtonAddon.Draw(spriteBatch);
        _infoAboutButton.Draw(spriteBatch);
        _infoAboutButton2.Draw(spriteBatch);
        _mouseCursor.Draw(spriteBatch);
    }
}