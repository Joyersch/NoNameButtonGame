using Microsoft.Xna.Framework;

namespace NoNameButtonGame;

public class ScaleProvider
{
    private readonly Scene _scene;

    public float Scale => _scene.Display.Scale;

    public Vector2 ComplexeScale => _scene.Display.ComplexScale;

    public ScaleProvider(Scene scene)
    {
        _scene = scene;
    }
}