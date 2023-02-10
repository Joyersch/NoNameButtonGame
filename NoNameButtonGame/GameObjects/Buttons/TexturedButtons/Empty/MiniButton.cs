using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Empty;

public class MiniButton : EmptyButton
{
    public MiniButton(Vector2 position) : this(position, DefaultSize)
    {
    }

    public MiniButton(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public MiniButton(Vector2 position, Vector2 size) : base(position, size)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<MiniButton>();
    }

    public new static Vector2 DefaultSize => new Vector2(64, 32);
}