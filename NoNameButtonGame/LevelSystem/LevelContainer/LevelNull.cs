using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class LevelNull : SampleLevel
{
    private readonly TextButton _failButton;
    private readonly Cursor _mouseCursor;
    private readonly TextBuilder _info;

    public LevelNull(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        _failButton = new TextButton(new Vector2(-64, -32), new Vector2(128, 64), "end", "Restart")
        {
            DrawColor = Color.White,
        };
        _failButton.ClickEventHandler += Fail;
        _mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Name = "Level ??? End";
        _info = new TextBuilder("This is the end!", new Vector2(-116, -64), new Vector2(16, 16), 0);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _info.Draw(spriteBatch);
        _failButton.Draw(spriteBatch);
        _mouseCursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        _mouseCursor.Update(gameTime);
        base.Update(gameTime);
        _mouseCursor.Position = MousePosition - _mouseCursor.Size / 2;
        _failButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _info.Update(gameTime);
    }
}