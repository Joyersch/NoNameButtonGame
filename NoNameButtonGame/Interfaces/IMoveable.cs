using Microsoft.Xna.Framework;
namespace NoNameButtonGame.Interfaces
{
    interface IMoveable
    {
        public Vector2 GetPosition();
        public bool Move(Vector2 Direction);
    }
}
