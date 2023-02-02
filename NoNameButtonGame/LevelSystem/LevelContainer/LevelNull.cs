using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.LevelSystem.LevelContainer;

class LevelNull : SampleLevel
{
    readonly EmptyButton failButton;
    readonly Cursor mouseCursor;
    readonly TextBuilder Info;

    public LevelNull(int defaultWidth, int defaultHeight, Vector2 window, Random rand) : base(defaultWidth,
        defaultHeight, window, rand)
    {
        failButton = new FailButton(new Vector2(-64, -32), new Vector2(128, 64))
        {
            DrawColor = Color.White,
        };
        failButton.ClickEventHandler += Fail;
        mouseCursor = new Cursor(new Vector2(0, 0), new Vector2(7, 10));
        Name = "Level ??? End";
        Info = new TextBuilder("This is the end!", new Vector2(-116, -64), new Vector2(16, 16), null, 0);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Info.Draw(spriteBatch);
        failButton.Draw(spriteBatch);
        mouseCursor.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        mouseCursor.Update(gameTime);
        base.Update(gameTime);
        mouseCursor.Position = mousePosition - mouseCursor.Size / 2;
        failButton.Update(gameTime, mouseCursor.Hitbox[0]);
        Info.Update(gameTime);
    }
}