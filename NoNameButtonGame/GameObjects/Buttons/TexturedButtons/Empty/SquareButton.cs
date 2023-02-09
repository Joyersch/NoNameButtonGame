using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Empty;

public class SquareButton : EmptyButton
{
    public SquareButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public SquareButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<SquareButton>();
    }

    public new static Vector2 DefaultSize => new Vector2(64, 32);
}