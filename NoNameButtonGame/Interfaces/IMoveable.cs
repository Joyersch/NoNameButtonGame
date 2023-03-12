using Microsoft.Xna.Framework;
namespace NoNameButtonGame.Interfaces;

public interface IMoveable
{
    public Vector2 GetPosition();
    public Vector2 GetSize();
    public void Move(Vector2 newPosition);
}