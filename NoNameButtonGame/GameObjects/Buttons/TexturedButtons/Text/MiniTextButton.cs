using Microsoft.Xna.Framework;

namespace NoNameButtonGame.GameObjects.Buttons.TexturedButtons.Text;

public class MiniTextButton : TextButton
{
    public MiniTextButton(Vector2 position, string name, string text) : this(position, DefaultSize, name, text, DefaultTextSize)
    {
    }
    
    public MiniTextButton(Vector2 position, float scale, string name, string text) : this(position, DefaultSize * scale, name, text, DefaultTextSize * scale)
    {
    }
    
    public MiniTextButton(Vector2 position, Vector2 size, string name, string text, Vector2 textSize) : base(position, size, name, text, textSize)
    {
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<MiniTextButton>();
        clickEffect = Globals.SoundEffects.GetEffect("ButtonSound");
    }

    public new static Vector2 DefaultSize => new Vector2(64, 32);
    public new static Vector2 DefaultTextSize => new Vector2(16, 16);
}