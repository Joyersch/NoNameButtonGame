using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.LogicObjects.Linker;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level1 : SampleLevel
{
    private readonly TextButton _startButton;
    private readonly Cursor _mouseCursor;
    private readonly TextBuilder _infoText;
    private readonly GameObjectLinker _gameObjectLinker;
    private readonly MousePointer _mouse;

    public Level1(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth, defaultHeight,
        window, rand)
    {
        _startButton = new TextButton(-TextButton.DefaultSize / 2, "Start", "Start");
        _startButton.ClickEventHandler += Finish;
        _mouseCursor = new Cursor(new Vector2(0, 0));
        _mouse = new MousePointer();
        Name = "Level 1 - Click the Button!";
        _infoText = new TextBuilder("How hard can it be?", new Vector2(-100, -64));
        _infoText.ChangePosition(Vector2.Zero -
                                new Vector2(_infoText.Rectangle.Width,
                                    _infoText.Rectangle.Height + TextButton.DefaultSize.Y * 2) / 2);
        _gameObjectLinker = new GameObjectLinker();
        _gameObjectLinker.Add(_mouse, _mouseCursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mouse.Update(gameTime, MousePosition);
        _gameObjectLinker.Update(gameTime);
        _mouseCursor.Update(gameTime);
        _startButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _infoText.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _infoText.Draw(spriteBatch);
        _startButton.Draw(spriteBatch);
        _mouseCursor.Draw(spriteBatch);
    }
}