using System;
using System.Collections.Generic;
using System.Text;
using NoNameButtonGame.Input;
using NoNameButtonGame.Camera;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Cache;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level3 : SampleLevel
{
    private readonly TextButton _stateButton;
    private readonly Cursor _mouseCursor;
    private readonly TextBuilder _infoAboutButton;
    private readonly TextBuilder _infoAboutButton2;
    private readonly MousePointer _mousePointer;
    private readonly GameObjectLinker _linker;
    private readonly CounterButtonAddon _counterButtonAddon;

    public Level3(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 3 - Tutorial 2 - Button Addon: Counter";
        _stateButton = new TextButton(-EmptyButton.DefaultSize / 2, string.Empty,
            Letter.ReverseParse(Letter.Character.AmongUsBean).ToString());
        _infoAboutButton = new TextBuilder("This button has a counter", Vector2.Zero);
        _infoAboutButton.ChangePosition(new Vector2(-_infoAboutButton.Rectangle.Width / 2, -64));
        _infoAboutButton2 =
            new TextBuilder("Press the button lower the counter and when it hits 0 you win!", Vector2.Zero);
        _infoAboutButton2.ChangePosition(new Vector2(-_infoAboutButton2.Rectangle.Width / 2,
            64 - _infoAboutButton2.Rectangle.Height));
        _mouseCursor = new Cursor(Vector2.One);
        _mousePointer = new MousePointer();
        _linker = new GameObjectLinker();
        _linker.Add(_mousePointer, _mouseCursor);
        _counterButtonAddon = new CounterButtonAddon(_stateButton, 5);
        _counterButtonAddon.StateReachedZero += Finish;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mousePointer.Update(gameTime, mousePosition);
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