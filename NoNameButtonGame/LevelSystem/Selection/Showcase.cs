using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.LevelSystem.Selection;

public class Showcase : IManageable, IMoveable, IScaleable
{
    private Vector2 _position;
    public float Scale => _baseScale * _extendedScale;

    private float _baseScale = 1f;
    private float _extendedScale = 1f;
    

    public static Texture2D[] Texture = new Texture2D[11];
    private static readonly Vector2 BaseSize = new(16, 9);

    public LevelFactory.LevelType Level { get; private set; }
    public Rectangle Rectangle => new(_position.ToPoint(), (BaseSize * Scale).ToPoint());

    public Showcase(LevelFactory.LevelType level, float scale = 20F) : this(Vector2.Zero, level, scale)
    {
    }

    private Showcase(Vector2 position, LevelFactory.LevelType level, float scale = 20F)
    {
        _position = position;
        Level = level;
        _baseScale = scale;
    }

    public void Update(GameTime gameTime)
    {
        // Nothing to do
    }

    public void Draw(SpriteBatch spriteBatch)
        => spriteBatch.Draw(Texture[(int)Level], Rectangle, null, Color.White);

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => Scale * BaseSize;

    public void Move(Vector2 newPosition)
        => _position = newPosition;

    public void ChangeLevel(LevelFactory.LevelType type)
        => Level = type;
    
    public void SetScale(ScaleProvider provider)
    {
        _extendedScale = provider.Scale;
    }
}