using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class Level1 : SampleLevel
{
    private readonly TextButton _startButton;
    private readonly Cursor _mouseCursor;
    private readonly Text _infoText;
    private readonly PositionListener _positionListener;
    private readonly MousePointer _mouse;

    public Level1(int defaultWidth, int defaultHeight, Vector2 window, Random random) : base(defaultWidth,
        defaultHeight, window, random)
    {
        Name = "Level 1 - Click the Button!";
        _startButton = new TextButton("Start");
        _startButton.Move(-TextButton.DefaultSize / 2);
        _startButton.Click += Finish;

        _infoText = new Text("How hard can it be?");
        _infoText.Move(Vector2.Zero -
                       new Vector2(_infoText.Rectangle.Width,
                           _infoText.Rectangle.Height + TextButton.DefaultSize.Y * 2) / 2);
        
        _mouseCursor = new Cursor();
        _mouse = new MousePointer();
        
        _positionListener = new PositionListener();
        _positionListener.Add(_mouse, _mouseCursor);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _mouse.Update(gameTime, MousePosition);
        _positionListener.Update(gameTime);
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