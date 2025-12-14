using Microsoft.Xna.Framework;
using IUpdateable = Joyersch.Monogame.IUpdateable;

namespace NoNameButtonGame.Collision;

public sealed class HitboxProvider : IHitbox, IUpdateable
{
    private readonly ISpatial _object;
    private Rectangle[] _hitbox;
    private Rectangle[] _baseHitbox;
    private Vector2 _scale;
    public Rectangle[] Hitbox => _hitbox;

    public HitboxProvider(ISpatial @object, Rectangle[] hitbox, Vector2 scale)
    {
        _object = @object;
        _baseHitbox = hitbox;
        _hitbox = new Rectangle[hitbox.Length];
        _scale = scale;
        Calcutate();
    }

    public void Update(GameTime gameTime)
    {
        Calcutate();
    }

    public void Calcutate()
    {
        for (int i = 0; i < _baseHitbox.Length; i++)
        {
            _hitbox[i] = CalculateInGameHitbox(_object.GetPosition(), _baseHitbox[i], _scale);
        }
    }

    private Rectangle CalculateInGameHitbox(Vector2 position, Rectangle hitbox, Vector2 scale)
        => new((int)(position.X + hitbox.X * scale.X)
            , (int)(position.Y + hitbox.Y * scale.Y)
            , (int)(hitbox.Width * scale.X)
            , (int)(hitbox.Height * scale.Y));


    public void SetScale(Vector2 scale)
        => _scale = scale;
}