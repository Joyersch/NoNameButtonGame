using Microsoft.Xna.Framework;
using NoNameButtonGame.Hitboxes;

namespace NoNameButtonGame.GameObjects.Buttons;

public class SquareTextButton : TextButton
{
    public SquareTextButton(Vector2 position, string Name, string Text) : this(position, DefaultSize, Name, Text, DefaultTextSize)
    {
    }
    public SquareTextButton(Vector2 position,float scale, string Name, string Text) : this(position, DefaultSize * scale, Name, Text, DefaultTextSize * scale)
    {
    }
    public SquareTextButton(Vector2 position, Vector2 Size, string Name, string Text, Vector2 TextSize) : base(position, Size, Name, Text, TextSize)
    {
    }

    public SquareTextButton(Vector2 position, Vector2 size, string name, string text) : base(position, size, name, text)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<SquareTextButton>();
    }

    public new static Vector2 DefaultSize => new Vector2(32, 32);
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
}