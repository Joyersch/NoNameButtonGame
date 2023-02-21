using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Text;

public class SquareTextButton : TextButton
{
    public SquareTextButton(Vector2 position, string name, string text) : this(position, DefaultSize, name, text, DefaultTextSize)
    {
    }
    public SquareTextButton(Vector2 position,float scale, string name, string text) : this(position, DefaultSize * scale, name, text, DefaultTextSize * scale)
    {
    }
    public SquareTextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize) : base(position, size, name, text, textSize)
    {
    }

    public SquareTextButton(Vector2 position, Vector2 size, string name, string text) : base(position, size, name, text)
    {
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<SquareTextButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public new static Vector2 DefaultSize => new Vector2(32, 32);
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
}