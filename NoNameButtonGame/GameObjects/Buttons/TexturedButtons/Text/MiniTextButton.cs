using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class MiniTextButton : TextButton
{
    public MiniTextButton(Vector2 position, string Name, string Text) : this(position, DefaultSize, Name, Text, DefaultTextSize)
    {
    }
    
    public MiniTextButton(Vector2 position, float scale, string Name, string Text) : this(position, DefaultSize * scale, Name, Text, DefaultTextSize * scale)
    {
    }
    
    public MiniTextButton(Vector2 position, Vector2 Size, string Name, string Text, Vector2 TextSize) : base(position, Size, Name, Text, TextSize)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<MiniTextButton>();
    }

    public new static Vector2 DefaultSize => new Vector2(64, 32);
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
}