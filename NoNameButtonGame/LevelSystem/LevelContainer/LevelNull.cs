using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

internal class LevelNull : SampleLevel
{
    private readonly TextButton _failButton;
    private readonly Cursor _mouseCursor;
    private readonly Text _info;

    public LevelNull(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        _failButton = new TextButton(new Vector2(-64, -32), "end", "Restart")
        {
            DrawColor = Color.White,
        };
        _failButton.Click += Fail;
        _mouseCursor = new Cursor(new Vector2(0, 0));
        Name = "Level 404";
        _info = new Text("Unknown level requested [404]", Vector2.Zero);
        _info.Move(-_info.Rectangle.Size.ToVector2() / 2F + new Vector2(0,-64));
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
        _mouseCursor.Position = _mouseCursor.Position - _mouseCursor.Size / 2;
        _failButton.Update(gameTime, _mouseCursor.Hitbox[0]);
        _info.Update(gameTime);
    }
}