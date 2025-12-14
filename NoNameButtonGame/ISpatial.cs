using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public interface ISpatial
{
    public Vector2 GetPosition();
    public Vector2 GetSize();
}