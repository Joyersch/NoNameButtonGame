using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class CameraClass
{
    public Matrix CameraMatrix { get; private set; }
    
    // ToDo: readable & changeable by level
    public readonly float Zoom = 2f;
    private readonly Vector2 _screen;

    public CameraClass(Vector2 screen)
        => _screen = screen;

    public void Update(Vector2 targetPos, Vector2 targetSize)
    {
        var position =
            Matrix.CreateTranslation(-targetPos.X - (targetSize.X / 2), -targetPos.Y - (targetSize.Y / 2), 0);

        var offset = Matrix.CreateTranslation(_screen.X / 2, _screen.Y / 2, 0);

        CameraMatrix = position * Matrix.CreateScale(Zoom) * offset;
    }
}