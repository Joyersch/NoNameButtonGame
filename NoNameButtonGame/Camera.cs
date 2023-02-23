using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame;

public class Camera : IMoveable
{
    public Matrix CameraMatrix { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; }
    public Rectangle Rectangle { get; private set; }
    
    public float Zoom = 2f;

    public Camera(Vector2 position, Vector2 size)
    {
        Size = size;
        Position = position;
    }

    public void Update()
    {
        UpdateMatrix();
        Rectangle = new Rectangle(Position.ToPoint(), (Size / Zoom).ToPoint());
    }
    private void UpdateMatrix()
    {
        var position = Matrix.CreateTranslation(-Position.X, -Position.Y, 0);
        var offset = Matrix.CreateTranslation(Size.X / 2, Size.Y / 2, 0);
        CameraMatrix = position * Matrix.CreateScale(Zoom) * offset;
    }

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 newPosition)
    {
        Position = newPosition;
        return true;
    }
}