using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public interface IMoveable : ISpatial
{

    public void Move(Vector2 newPosition);
}