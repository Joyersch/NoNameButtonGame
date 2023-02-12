using Microsoft.Xna.Framework;
using NoNameButtonGame.Cache;

namespace NoNameButtonGame.GameObjects.Buttons;

public class SquareTextButton : TextButton
{
    public SquareTextButton(Vector2 position, string Name, string Text) : this(position, DefaultSize, Name, Text, DefaultTextSize)
    {
    }
    public SquareTextButton(Vector2 position,float scale, string Name, string Text) : this(position, DefaultSize * scale, Name, Text, DefaultTextSize * scale)
    {
    }
    public SquareTextButton(Vector2 position, Vector2 size, string Name, string Text, Vector2 TextSize) : base(position, size, Name, Text, TextSize)
    {
    }

    public SquareTextButton(Vector2 position, Vector2 size, string name, string text) : base(position, size, name, text)
    {
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<SquareTextButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public new static Vector2 DefaultSize => new Vector2(32, 32);
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
}