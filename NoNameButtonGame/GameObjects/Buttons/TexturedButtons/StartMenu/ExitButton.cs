using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class ExitButton : EmptyButton
{
    public ExitButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<ExitButton>();
    }
}