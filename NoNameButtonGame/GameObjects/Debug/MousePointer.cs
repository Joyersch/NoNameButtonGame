using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.Debug;

public class MousePointer : GameObject, IMoveable
{
    private readonly bool _draw;

    public new static Texture2D DefaultTexture;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(6, 6),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 6, 6)
        }
    };

    public MousePointer() : this(Vector2.Zero, Vector2.Zero)
    {
    }

    public MousePointer(Vector2 position, Vector2 size) : this(position, size, false)
    {
    }

    public MousePointer(Vector2 position, Vector2 size, bool draw) : base(position, size, DefaultTexture,
        DefaultMapping)
    {
        this._draw = draw;
    }

    public void Update(GameTime gameTime, Vector2 mousePosition)
    {
        Position = mousePosition;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!_draw)
            return;

        spriteBatch.Draw(Texture, new Rectangle((int) Position.X - 3, (int) Position.Y - 3, 6, 6),
            DrawColor);
        base.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
        => Position;

    public void Move(Vector2 newPosition)
        => Position = newPosition;
}