using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

class Cursor : GameObject, IHitbox
{
    private Rectangle[] textureHitbox;
    private Rectangle[] ingameHitbox;
    private Vector2 scale;

    public Rectangle[] Hitbox => ingameHitbox;

    public Cursor(Vector2 position, Vector2 size)
    {
        Size = size;
        Position = position;
        var textureMapping = Mapping.GetMappingFromCache<Cursor>();
        ImageLocation = new Rectangle(0, 0, 0, 0);
        FrameSize = textureMapping.ImageSize;
        Texture = textureMapping.Texture;
        DrawColor = Color.White;
        textureHitbox = textureMapping.Hitboxes;
        ingameHitbox = new Rectangle[textureHitbox.Length];
        scale = new Vector2(size.X / FrameSize.X, size.Y / FrameSize.Y);
        for (int i = 0; i < textureMapping.Hitboxes.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (base.Position.X + (textureMapping.Hitboxes[i].X * scale.X)),
                (int) (base.Position.Y + (textureMapping.Hitboxes[i].Y * scale.Y)), (int) (textureMapping.Hitboxes[i].Width * scale.X),
                (int) (textureMapping.Hitboxes[i].Height * scale.Y));
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateHitbox();
    }

    private void UpdateHitbox()
    {
        scale = new Vector2(Size.X / FrameSize.X, Size.Y / FrameSize.Y);
        for (int i = 0; i < textureHitbox.Length; i++)
        {
            ingameHitbox[i] = new Rectangle((int) (Position.X + (textureHitbox[i].X * scale.X)),
                (int) (Position.Y + (textureHitbox[i].Y * scale.Y)), (int) (textureHitbox[i].Width * scale.X),
                (int) (textureHitbox[i].Height * scale.Y));
        }
    }
}